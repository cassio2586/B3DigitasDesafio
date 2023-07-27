using B3Digitas.Architecture.Web.Endpoints.CashEndpoints;
using B3Digitas.Architecture.Core.Interfaces;
using B3Digitas.Architecture.SharedKernel.CustomExceptions;
using FastEndpoints;

namespace B3Digitas.Architecture.Web.CashEndpoints;


public class GetAllOrderBookBySymbol : Endpoint<GetAllOrderBookBySymbolRequest, GetAllOrderBookBySymbolResponse>
{
    private readonly IGetBookValuesService _getBookValuesService;
    private readonly ILogger<GetAllOrderBookBySymbol> _logger;

    public GetAllOrderBookBySymbol(IGetBookValuesService getBookValuesService, ILogger<GetAllOrderBookBySymbol> logger)
    {
        _getBookValuesService = getBookValuesService;
        _logger = logger;
    }

    public override void Configure()
    {
        Get(GetAllOrderBookBySymbolRequest.Route);
        AllowAnonymous();
        Options(x => x
            .WithTags("OrderBookEndpoints"));
    }

    public override async Task HandleAsync(GetAllOrderBookBySymbolRequest request,
        CancellationToken cancellationToken)
    {
        try
        {   
            if (request.Symbol != null)
            {
                var obj = _getBookValuesService.GetBySymbol(request.Symbol);
                var result = new GetAllOrderBookBySymbolResponse(obj);
                result.AvgPriceLastFiveSeconds = obj.FirstOrDefault()!.FiveSecondAvgPrice;
                await SendAsync(result,
                    cancellation: cancellationToken);
            }
        }
        catch (NoDataReportException)
        {
            _logger.LogCritical("Invalid Date Parameter");
            await SendNoContentAsync(cancellationToken);
        }
        catch (InvalidDataException)
        {
            await SendErrorsAsync(400,cancellationToken);
        }
    }
}