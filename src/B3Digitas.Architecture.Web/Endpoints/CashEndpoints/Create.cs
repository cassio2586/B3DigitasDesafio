using Ardalis.Result;
using B3Digitas.Architecture.Core.Interfaces;
using B3Digitas.Architecture.Core.OrderBookAggregate;
using B3Digitas.Architecture.SharedKernel.CustomExceptions;
using FastEndpoints;

namespace B3Digitas.Architecture.Web.Endpoints.CashEndpoints;

public class Create : Endpoint<CreateOrderBookRequest>
{
    private readonly ICreateBookValuesService _createBookValuesService;
    private readonly ILogger<Create> _logger;
    private readonly global::AutoMapper.IMapper _mapper;

    public Create(ICreateBookValuesService service, ILogger<Create> logger, global::AutoMapper.IMapper mapper)
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
            /*
            if (request. is null)
                throw new InvalidDataException();
            if (request.Amount < 0)
                throw new NegativeAmountException();
            if(!Enum.IsDefined(typeof(TransactionTypeEnum), request.TransactionType))
                throw new InvalidDataException();
            */

            var x = _mapper.Map<OrderBook>(request);
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