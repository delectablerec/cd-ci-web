using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class IndexModel : PageModel
{
    public List<Prodotto> Prodotti { get; set; } = new();
    public string? Environment { get; set; }
    public string? filePath { get; set; }

    public async Task OnGet()
    {
        filePath = Environment.GetEnvironmentVariable("PRODOTTI_JSON_PATH") ?? "wwwroot/data/prodotti.json";
        // var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/prodotti.json");
        Environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

        if (System.IO.File.Exists(filePath))
        {
            var json = await System.IO.File.ReadAllTextAsync(filePath);
            Prodotti = JsonConvert.DeserializeObject<List<Prodotto>>(json) ?? new List<Prodotto>();
        }
    }
}