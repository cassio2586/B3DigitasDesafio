using Ardalis.Result;
using B3Digitas.Architecture.Web.Endpoints.CashEndpoints;
using B3Digitas.Architecture.Core.CashAggregate.Enums;
using B3Digitas.Architecture.Core.Interfaces;
using B3Digitas.Architecture.SharedKernel.CustomExceptions;
using FastEndpoints;

namespace CashFlow.Architecture.Web.CashEndpoints;

public class Create : Endpoint<CreateCashRequest>
{
    private readonly ICreateCashService _createCashService;
    private readonly ILogger<Create> _logger;

    public Create(ICreateCashService service, ILogger<Create> logger)
    {
        _createCashService = service;
        _logger = logger;
    }

    public override void Configure()
    {
        Post(CreateCashRequest.Route);
        AllowAnonymous();
        Options(x => x
            .WithTags("CashEndpoints"));
    }

    public override async Task HandleAsync(
        CreateCashRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (request.Description is null)
                throw new InvalidDataException();
            if (request.Amount < 0)
                throw new NegativeAmountException();
            if(!Enum.IsDefined(typeof(TransactionTypeEnum), request.TransactionType))
                throw new InvalidDataException();
            
            var result = await _createCashService.Add(request.Description, request.Amount, request.TransactionType,
                request.DateTimeTransaction);

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