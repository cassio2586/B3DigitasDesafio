using System.Diagnostics;
using System.Text;
using System.Text.Json.Nodes;
using B3Digitas.Cripto.Web.Endpoints.CashEndpoints;
using CashFlow.Architecture.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CashFlow.Architecture.Web.Controllers;

public class HomeController : Controller
{
  public async Task<IActionResult> Index()
  {
        using (HttpClient client = new HttpClient())
        {
          var listResponse = new List<GetAllOrderBookBySymbolResponse>();

          var url = "http://localhost:57679/GetAllOrderBookBySymbol?Symbol=btcusd";
            
            var btc = await client.GetFromJsonAsync<GetAllOrderBookBySymbolResponse>(url);
            if(btc!=null)
            listResponse.Add(btc);


            url = "http://localhost:57679/GetAllOrderBookBySymbol?Symbol=ethusd";
            var eth = await client.GetFromJsonAsync<GetAllOrderBookBySymbolResponse>(url);
            if(eth!=null)
            listResponse.Add(eth);         
            
            return View(listResponse);
        }

/*
    var url = "https://cassio2586-humble-meme-xr4rj5g4p5whp599-57679.preview.app.github.dev/GetAllOrderBookBySymbol?Symbol=btcusd";
            using var client = new HttpClient();

            var response = await client.GetAsync(url);

            var result = response.Content.ReadFromJsonAsync<GetAllOrderBookBySymbolResponse>().Result;
    */

  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error() => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
