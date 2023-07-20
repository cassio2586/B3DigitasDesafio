using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using B3Digitas.Architecture.Core.Interfaces;
using B3Digitas.Architecture.Core.OrderBookAggregate;
using B3Digitas.Architecture.Core.Services;
using B3Digitas.Architecture.Infrastructure.Data;
using B3Digitas.Architecture.Web.Endpoints.CashEndpoints;
using Bitstamp.Client.Websocket;
using Bitstamp.Client.Websocket.Channels;
using Bitstamp.Client.Websocket.Client;
using Bitstamp.Client.Websocket.Communicator;
using Bitstamp.Client.Websocket.Requests;
using Bitstamp.Client.Websocket.Responses.Books;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Serilog.Events;

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

            Console.WriteLine("|=======================|");
            Console.WriteLine("|    BITSTAMP CLIENT    |");
            Console.WriteLine("|=======================|");
            Console.WriteLine();
            
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
            return Task.CompletedTask;
            //client.Send(new SubscribeRequest("btceur", Channel.OrderBook));

            //client.Send(new SubscribeRequest("btcusd", Channel.OrderBookDetail));
            //client.Send(new SubscribeRequest("btceur", Channel.OrderBookDetail));

            //client.Send(new SubscribeRequest("btcusd", Channel.OrderBookDiff));
            //client.Send(new SubscribeRequest("btceur", Channel.OrderBookDiff));

            //client.Send(new SubscribeRequest("btcusd", Channel.Ticker));
            //client.Send(new SubscribeRequest("btceur", Channel.Ticker));
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
                Console.WriteLine($"Order book L2 [{x.Symbol}]");
                Console.WriteLine($"    {x.Data?.Asks.FirstOrDefault()?.Price} " +
                                  $"{x.Data?.Asks.FirstOrDefault()?.Amount ?? 0} " +
                                  $"{x.Data?.Asks.FirstOrDefault()?.Side} " +
                                  $"({x.Data?.Asks?.Length})");
                Console.WriteLine($"    {x.Data?.Bids.FirstOrDefault()?.Price} " +
                                  $"{x.Data?.Bids.FirstOrDefault()?.Amount ?? 0} " +
                                  $"{x.Data?.Bids.FirstOrDefault()?.Side} " +
                                  $"({x.Data?.Bids?.Length})");

                
            });

            client.Streams.OrdersStream.Subscribe(x =>
            {
                //Log.Information($"{x.Data} {x.Data.Asks[0]} {x.Data.EventBids[0]}");
                //Log.Information($"{x.Symbol} {x.Data.Channel} {x.Data.Amount}");
            });

            client.Streams.OrderBookDetailStream.Subscribe(x =>
            {
                
                //await SendToApi(x);

                Console.WriteLine($"Order book L3 [{x.Symbol}]");
                Console.WriteLine($"    {x.Data?.Asks.FirstOrDefault()?.Price} " +
                                  $"{x.Data?.Asks.FirstOrDefault()?.Amount ?? 0} " +
                                  $"{x.Data?.Asks.FirstOrDefault()?.Side} " +
                                  $"({x.Data?.Asks?.Length}) " +
                                  $"id: {x.Data?.Asks?.FirstOrDefault()?.OrderId}");
                Console.WriteLine($"    {x.Data?.Bids.FirstOrDefault()?.Price} " +
                                  $"{x.Data?.Bids.FirstOrDefault()?.Amount ?? 0} " +
                                  $"{x.Data?.Bids.FirstOrDefault()?.Side} " +
                                  $"({x.Data?.Bids?.Length}) " +
                                  $"id: {x.Data?.Bids?.FirstOrDefault()?.OrderId}");
            });

            client.Streams.OrderBookDiffStream.Subscribe(x =>
            {
                Console.WriteLine($"Order book L2 diffs [{x.Symbol}]");
                Console.WriteLine($"    updates {x.Data?.Asks.Count(y => y.Amount > 0)} " +
                                  $"deletes {x.Data?.Asks.Count(y => y.Amount <= 0)}  " +
                                  $"{x.Data?.Asks.FirstOrDefault()?.Side} " +
                                  $"({x.Data?.Asks?.Length}) ");
                Console.WriteLine($"    updates {x.Data?.Bids.Count(y => y.Amount > 0)} " +
                                  $"deletes {x.Data?.Bids.Count(y => y.Amount <= 0)}  " +
                                  $"{x.Data?.Bids.FirstOrDefault()?.Side} " +
                                  $"({x.Data?.Bids?.Length}) ");
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
            var orderBook = new CreateOrderBookRequest();
            orderBook.Microtimestamp = x.Data.Microtimestamp;
            orderBook.Timestamp = x.Data.Timestamp;
            orderBook.Asks = new List<BookLevelRequest>();
            orderBook.Bids = new List<BookLevelRequest>();
            
            foreach (var ask in x.Data.Asks)
            {
                orderBook.Asks.Add(new BookLevelRequest()
                    { Amount = ask.Amount, OrderId = ask.OrderId, Price = ask.Price, SideRequest = OrderBookSideRequest.Ask });
            }

            foreach (var bid in x.Data.Bids)
            {
                orderBook.Asks.Add(new BookLevelRequest()
                    { Amount = bid.Amount, OrderId = bid.OrderId, Price = bid.Price, SideRequest = OrderBookSideRequest.Bid });
            }

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(orderBook);
            var data = new System.Net.Http.StringContent(json, Encoding.UTF8, "application/json");

            var url = "http://localhost:57679/OrderBook";
            using var client = new HttpClient();

            var response = await client.PostAsync(url, data);

            string result = response.Content.ReadAsStringAsync().Result;
            Console.WriteLine(result);
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