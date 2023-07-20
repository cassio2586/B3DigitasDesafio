/*using Ardalis.ApiEndpoints;
using B3Digitas.Architecture.Web.Endpoints.CashEndpoints;
using B3Digitas.Architecture.Core.Interfaces;
using B3Digitas.Architecture.SharedKernel.CustomExceptions;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace B3Digitas.Architecture.Web.CashEndpoints;


public class GetCashFlowReportDay : Endpoint<GetCashFlowReportDayRequest, GetCashFlowReportDayResponse>
{
    private readonly IGetCashFlowDayReportService _getCashFlowDayReportService;
    private readonly ILogger<GetCashFlowReportDay> _logger;

    public GetCashFlowReportDay(IGetCashFlowDayReportService flowDayReportService, ILogger<GetCashFlowReportDay> logger)
    {
        _getCashFlowDayReportService = flowDayReportService;
        _logger = logger;
    }

    public override void Configure()
    {
        Get(GetCashFlowReportDayRequest.Route);
        AllowAnonymous();
        Options(x => x
            .WithTags("CashEndpoints"));
    }

    public override async Task HandleAsync(GetCashFlowReportDayRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var result =  _getCashFlowDayReportService.Get(request.Day);
            
            await SendAsync(new GetCashFlowReportDayResponse(result.Day, result.Amount),
                cancellation: cancellationToken);
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
}*/