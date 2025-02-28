using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Globalization;

public class IndexModel : PageModel
{
    public List<Prodotto> Prodotti { get; set; } = new();
    public string? Ambiente { get; set; }
    public string? fileLocal { get; set; }
    public string? fileImage { get; set; }
    public string? ultimoAggiornamento { get; set; }
    public string? formatoValuta { get; set; }
    public string? fusoOrario { get; set; }

    public async Task OnGet()
    {
        fileLocal = Environment.GetEnvironmentVariable("PRODOTTI_JSON_PATH") ?? "./wwwroot/data/prodotti.json";
        fileImage = Environment.GetEnvironmentVariable("PRODOTTI_APP_PATH") ?? "Sconosciuto";
        Ambiente = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";
        ultimoAggiornamento = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        formatoValuta = Environment.GetEnvironmentVariable("DOTNET_CURRENCY") ?? "EUR";
        fusoOrario = Environment.GetEnvironmentVariable("TZ") ?? "Europe/Rome";
        
        var cultura = new CultureInfo("it-IT")
        {
            NumberFormat = { CurrencySymbol = formatoValuta }
        };
        CultureInfo.DefaultThreadCurrentCulture = cultura;
        CultureInfo.DefaultThreadCurrentUICulture = cultura;

        Console.WriteLine($"PRODOTTI_JSON_PATH: {fileImage}");
        Console.WriteLine($"PRODOTTI_APP_PATH: {fileLocal}");
        Console.WriteLine($"DOTNET_ENVIRONMENT: {Ambiente}");
        Console.WriteLine($"Ultimo aggiornamento: {ultimoAggiornamento}");
        Console.WriteLine($"DOTNET_CURRENCY: {formatoValuta}");
        Console.WriteLine($"TZ: {fusoOrario}");

        if (System.IO.File.Exists(fileLocal))
        {
            var json = await System.IO.File.ReadAllTextAsync(fileLocal);
            Prodotti = JsonConvert.DeserializeObject<List<Prodotto>>(json) ?? new List<Prodotto>();
        }
    }
}