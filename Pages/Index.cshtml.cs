using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class IndexModel : PageModel
{
    public List<Prodotto> Prodotti { get; set; } = new();
    public string? Ambiente { get; set; }
    public string? fileLocal { get; set; }
    public string? fileImage { get; set; }
    public string? ultimoAggiornamento { get; set; }

    public async Task OnGet()
    {
        fileLocal = Environment.GetEnvironmentVariable("PRODOTTI_JSON_PATH") ?? "./wwwroot/data/prodotti.json";
        fileImage = Environment.GetEnvironmentVariable("PRODOTTI_APP_PATH") ?? "/app/data/prodotti.json";
        Ambiente = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Development";
        ultimoAggiornamento = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");

        Console.WriteLine($"PRODOTTI_JSON_PATH: {fileImage}");
        Console.WriteLine($"PRODOTTI_APP_PATH: {fileLocal}");
        Console.WriteLine($"DOTNET_ENVIRONMENT: {Ambiente}");
        Console.WriteLine($"Ultimo aggiornamento: {ultimoAggiornamento}");

        if (System.IO.File.Exists(fileLocal))
        {
            var json = await System.IO.File.ReadAllTextAsync(fileLocal);
            Prodotti = JsonConvert.DeserializeObject<List<Prodotto>>(json) ?? new List<Prodotto>();
        }
    }
}