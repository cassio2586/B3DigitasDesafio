using Ardalis.Result;
using B3Digitas.Architecture.Core.Interfaces;
using B3Digitas.Architecture.Core.OrderBookAggregate;
using B3Digitas.Architecture.SharedKernel.CustomExceptions;
using FastEndpoints;

namespace B3Digitas.Architecture.Web.Endpoints.CashEndpoints;

public class CreateOrderBook : Endpoint<CreateOrderBookRequest>
{
    private readonly ICreateBookValuesService _createBookValuesService;
    private readonly ILogger<CreateOrderBook> _logger;
    private readonly global::AutoMapper.IMapper _mapper;

    public CreateOrderBook(ICreateBookValuesService service, ILogger<CreateOrderBook> logger, global::AutoMapper.IMapper mapper)
    {
        _createBookValuesService = service;
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
            var result = await _createBookValuesService.Add(_mapper.Map<OrderBook>(request));

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