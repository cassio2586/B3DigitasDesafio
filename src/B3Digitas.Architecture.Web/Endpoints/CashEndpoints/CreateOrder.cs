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

    public CreateOrder(ICreateOrderValuesService service, ILogger<CreateOrder> logger, global::AutoMapper.IMapper mapper)
    {
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
            
        
            var result = await _createOrderValuesService.Add(new B3Digitas.Architecture.Core.OrderBookAggregate.Order());

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