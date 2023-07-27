using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using B3Digitas.Architecture.Web.Endpoints.CashEndpoints;
using Bitstamp.Client.Websocket;
using Bitstamp.Client.Websocket.Channels;
using Bitstamp.Client.Websocket.Client;
using Bitstamp.Client.Websocket.Communicator;
using Bitstamp.Client.Websocket.Requests;
using Bitstamp.Client.Websocket.Responses.Books;
using OrderBookSide = Bitstamp.Client.Websocket.Responses.Books.OrderBookSide;

namespace B3Digitas.Architecture.ServiceCrawler
{
    internal class Program
    {
        private static readonly ManualResetEvent ExitEvent = new ManualResetEvent(false);
        
        private static async Task Main(string[] args)
        {
            InitLogging();

            AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnProcessExit;
            AssemblyLoadContext.Default.Unloading += DefaultOnUnloading;
            Console.CancelKeyPress += ConsoleOnCancelKeyPress;

            var url = BitstampValues.ApiWebsocketUrl;
            using (var communicator = new BitstampWebsocketCommunicator(url))
            {
                communicator.Name = "Bitstamp-1";

                using (var client = new BitstampWebsocketClient(communicator))
                {
                    SubscribeToStreams(client);

                    communicator.ReconnectionHappened.Subscribe(async type =>
                    {
                        await SendSubscriptionRequests(client);
                    });

                    await communicator.Start();

                    ExitEvent.WaitOne();
                }
            }


        }

        private static Task SendSubscriptionRequests(BitstampWebsocketClient client)
        {
            client.Send(new SubscribeRequest("btcusd", Channel.OrderBook));
            client.Send(new SubscribeRequest("ethusd", Channel.OrderBook));
            return Task.CompletedTask;
        }

        private static void SubscribeToStreams(BitstampWebsocketClient client)
        {
            client.Streams.ErrorStream.Subscribe(x =>
                Console.WriteLine($"Error received, message: {x?.Message}"));

            client.Streams.SubscriptionSucceededStream.Subscribe(x =>
            {
                Console.WriteLine($"Subscribed to {x?.Symbol} {x?.Channel}");
            });
            
            client.Streams.UnsubscriptionSucceededStream.Subscribe(x =>
            {
                Console.WriteLine($"Unsubscribed from {x?.Symbol} {x?.Channel}");
            });


            client.Streams.OrderBookStream.Subscribe(async x =>
            {
                await SendToApi(x);
            });

            client.Streams.OrdersStream.Subscribe(x =>
            {
            });

            client.Streams.OrderBookDetailStream.Subscribe(x =>
            {
            });

            client.Streams.OrderBookDiffStream.Subscribe(x =>
            {
            });

            
            client.Streams.HeartbeatStream.Subscribe(x =>
                Console.WriteLine($"Heartbeat received, product: {x?.Channel}, seq: {x?.Event}"));

            client.Streams.TickerStream.Subscribe(x =>
            {
                Console.WriteLine($"Trade executed [{x.Symbol}] {x.Data?.Side} price: {x.Data?.Price} size: {x.Data?.Amount}");
            });
        }

        private static async Task SendToApi(OrderBookResponse x)
        {
            try{
                var orderBook = new CreateOrderBookRequest
                {
                    Symbol = x.Symbol,
                    Microtimestamp = x.Data.Microtimestamp,
                    Timestamp = x.Data.Timestamp,
                    BookLevels = new List<BookLevelRequest>()
                };

                foreach (var ask in x.Data.Asks)
                {
                    orderBook.BookLevels.Add(new BookLevelRequest()
                        { BookType = 1, Amount = ask.Amount, OrderId = ask.OrderId, Price = ask.Price, SideRequest = MapEnumSide(ask.Side)  });
                }

                foreach (var bid in x.Data.Bids)
                {
                    orderBook.BookLevels.Add(new BookLevelRequest()
                        { BookType = 2, Amount = bid.Amount, OrderId = bid.OrderId, Price = bid.Price, SideRequest = MapEnumSide(bid.Side) });
                }

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(orderBook);
                var data = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");

                var url = "http://localhost:57679/OrderBook";
                using var client = new HttpClient();

                var response = await client.PostAsync(url, data);

                string result = response.Content.ReadAsStringAsync().Result;
                Console.WriteLine(result);
            }catch{}
        }

        private static OrderBookSideRequest MapEnumSide(OrderBookSide side)
        {
            if (side == OrderBookSide.Ask)
                return OrderBookSideRequest.Ask;
            if (side == OrderBookSide.Bid)
                return OrderBookSideRequest.Bid;
            return OrderBookSideRequest.Undefined;
        }

        private static void InitLogging()
        {
            var executingDir = Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location);
            var logPath = Path.Combine(executingDir, "logs", "verbose.log");
            
        }

        private static void CurrentDomainOnProcessExit(object sender, EventArgs eventArgs)
        {
            Console.WriteLine("Exiting process");
            ExitEvent.Set();
        }

        private static void DefaultOnUnloading(AssemblyLoadContext assemblyLoadContext)
        {
            Console.WriteLine("Unloading process");
            ExitEvent.Set();
        }

        private static void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Canceling process");
            e.Cancel = true;
            ExitEvent.Set();
        }
    }
}