using B3Digitas.Cripto.Core.Interfaces;
using B3Digitas.Cripto.SharedKernel.CustomExceptions;
using B3Digitas.Cripto.Web.Endpoints.CashEndpoints;
using FastEndpoints;

namespace B3Digitas.Cripto.Web.CashEndpoints;


public class GetAllOrderBookBySymbol : Endpoint<GetAllOrderBookBySymbolRequest, GetAllOrderBookBySymbolResponse>
{
    private readonly IGetOrderBookService _getOrderBookService;
    private readonly ILogger<GetAllOrderBookBySymbol> _logger;

    public GetAllOrderBookBySymbol(IGetOrderBookService getOrderBookService, ILogger<GetAllOrderBookBySymbol> logger)
    {
        _getOrderBookService = getOrderBookService;
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
                var obj = _getOrderBookService.GetBySymbol(request.Symbol);
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