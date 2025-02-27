using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class IndexModel : PageModel
{
    public List<Prodotto> Prodotti { get; set; } = new();

    public async Task OnGet()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/prodotti.json");
        if (System.IO.File.Exists(filePath))
        {
            var json = await System.IO.File.ReadAllTextAsync(filePath);
            Prodotti = JsonConvert.DeserializeObject<List<Prodotto>>(json) ?? new List<Prodotto>();
        }
    }
}
