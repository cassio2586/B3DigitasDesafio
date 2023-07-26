using Ardalis.Result;
using B3Digitas.Architecture.Core.Interfaces;
using B3Digitas.Architecture.Core.OrderBookAggregate;
using B3Digitas.Architecture.SharedKernel.CustomExceptions;
using FastEndpoints;

namespace B3Digitas.Architecture.Web.Endpoints.CashEndpoints;

public class CreateOrder : Endpoint<CreateOrderRequest>
{
    private readonly ICreateOrderValuesService _createOrderValuesService;
    private readonly ILogger<CreateOrder> _logger;
    private readonly global::AutoMapper.IMapper _mapper;
    private readonly IGetBookValuesService _getBookValuesService;

    public CreateOrder(IGetBookValuesService getBookValuesService,ICreateOrderValuesService service, ILogger<CreateOrder> logger, global::AutoMapper.IMapper mapper)
    {
        _getBookValuesService = getBookValuesService;
        _createOrderValuesService = service;
        _logger = logger;
        _mapper = mapper;
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
                var obj = _getBookValuesService.GetBySymbol(request.Symbol);
                var bookLevels =  obj.FirstOrDefault()?.BookLevels;
                
                if (request.Side == "C")
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
                else
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
            }

            var orderInsert = new B3Digitas.Architecture.Core.OrderBookAggregate.Order()
            {
                Symbol = request.Symbol,
                Amount = request.Amount,
                Side = request.Side,
                BestPrice = (decimal)bookLevelAdded.Sum(x => x.Price) * (decimal)bookLevelAdded.Sum(x => x.Amount),
                Guid = Guid.NewGuid().ToString("N")
            };
            
            await _createOrderValuesService.Add(orderInsert);
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
}