using Ardalis.Result;
using B3Digitas.Cripto.Core.Interfaces;
using B3Digitas.Cripto.Core.OrderBookAggregate;
using B3Digitas.Cripto.SharedKernel.CustomExceptions;
using FastEndpoints;

namespace B3Digitas.Cripto.Web.Endpoints.CashEndpoints;

public class CreateOrderBook : Endpoint<CreateOrderBookRequest>
{
    private readonly ICreateOrderBookService _createOrderBookService;
    private readonly ILogger<CreateOrderBook> _logger;
    private readonly global::AutoMapper.IMapper _mapper;

    public CreateOrderBook(ICreateOrderBookService service, ILogger<CreateOrderBook> logger, global::AutoMapper.IMapper mapper)
    {
        _createOrderBookService = service;
        _logger = logger;
        _mapper = mapper;
    }

    public override void Configure()
    {
        Post(CreateOrderBookRequest.Route);
        AllowAnonymous();
        Options(x => x
            .WithTags("OrderBookEndpoints"));
    }

    public override async Task HandleAsync(
        CreateOrderBookRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result = await _createOrderBookService.Add(_mapper.Map<OrderBook>(request));

            if (result.Status == ResultStatus.NotFound)
            {
                await SendNotFoundAsync(cancellationToken);
                return;
            }

            await SendNoContentAsync(cancellationToken);
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