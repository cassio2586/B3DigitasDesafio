using B3Digitas.Cripto.Core.Interfaces;
using B3Digitas.Cripto.Core.OrderBookAggregate;
using B3Digitas.Cripto.Core.OrderBookAggregate.Enums;
using B3Digitas.Cripto.SharedKernel.CustomExceptions;
using FastEndpoints;
using Order = B3Digitas.Cripto.Core.OrderBookAggregate.Order;

namespace B3Digitas.Cripto.Web.Endpoints.CashEndpoints;

public class CreateOrder : Endpoint<CreateOrderRequest>
{
    private readonly ICreateOrderService _createOrderService;
    private readonly ILogger<CreateOrder> _logger;
    private readonly IGetOrderBookService _getOrderBookService;

    public CreateOrder(IGetOrderBookService getOrderBookService,ICreateOrderService service, ILogger<CreateOrder> logger)
    {
        _getOrderBookService = getOrderBookService;
        _createOrderService = service;
        _logger = logger;
    }

    public override void Configure()
    {
        Post(CreateOrderRequest.Route);
        AllowAnonymous();
        Options(x => x
            .WithTags("OrderEndpoints"));
    }

    public override async Task HandleAsync(
        CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var bookLevelAdded = new List<BookLevel>();
            
            if (request.Symbol != null)
            {
                var orderBookBySymbol = _getOrderBookService.GetBySymbol(request.Symbol);
                var bookLevels =  orderBookBySymbol.FirstOrDefault()?.BookLevels;
                
                if (request.Side == "C")
                {
                    BuyOrder(request, bookLevels, bookLevelAdded);
                }
                else
                {
                    SellOrder(request, bookLevels, bookLevelAdded);
                }
            }

            var orderInsert = await PersistOrder(request, bookLevelAdded);
            
            var result = new CreateOrderResponse()
            {
                Symbol = orderInsert.Symbol,
                Amount = orderInsert.Amount,
                Side = orderInsert.Side,
                BestPrice = orderInsert.BestPrice,
                BookLevels = bookLevelAdded, Guid = orderInsert.Guid
            };
            
            await SendAsync(result,
                cancellation: cancellationToken);
        }
        catch (InvalidDataException)
        {
            _logger.LogCritical("Invalid Description or TransactionType argument");
            await SendErrorsAsync(400, cancellationToken);
        }
        catch (NegativeAmountException)
        {
            _logger.LogCritical("Invalid amount negative argument");
            await SendErrorsAsync(400, cancellationToken);
        }

    }

    private async Task<Core.OrderBookAggregate.Order> PersistOrder(CreateOrderRequest request, List<BookLevel> bookLevelAdded)
    {
        var orderInsert = new Core.OrderBookAggregate.Order()
        {
            Symbol = request.Symbol,
            Amount = request.Amount,
            Side = request.Side,
            BestPrice = (decimal)bookLevelAdded.Sum(x => x.Price) * (decimal)bookLevelAdded.Sum(x => x.Amount),
            Guid = Guid.NewGuid().ToString("N")
        };

        await _createOrderService.Add(orderInsert);
        return orderInsert;
    }

    private static void SellOrder(CreateOrderRequest request, List<BookLevel>? bookLevels, List<BookLevel> bookLevelAdded)
    {
        if (bookLevels != null)
        {
            var bids = bookLevels.Where(x => x.Side == OrderBookSide.Bid).OrderByDescending(x => x.Price);

            foreach (var bid in bids)
            {
                if ((decimal)bid.Amount > request.Amount)
                {
                    bookLevelAdded.Add(bid);
                }
            }
        }
    }

    private static void BuyOrder(CreateOrderRequest request, List<BookLevel>? bookLevels, List<BookLevel> bookLevelAdded)
    {
        if (bookLevels != null)
        {
            var asks = bookLevels.Where(x => x.Side == OrderBookSide.Ask).OrderByDescending(x => x.Price);

            foreach (var ask in asks)
            {
                if ((decimal)ask.Amount > request.Amount)
                {
                    bookLevelAdded.Add(ask);
                }
            }
        }
    }
}