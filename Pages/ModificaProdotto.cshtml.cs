using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class ModificaProdottoModel : PageModel
{
    [BindProperty]
    public Prodotto ProdottoModificato { get; set; } = new();

    public List<Prodotto> Prodotti { get; set; } = new();

    public async Task OnGet(int id)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/prodotti.json");
        if (System.IO.File.Exists(filePath))
        {
            var json = await System.IO.File.ReadAllTextAsync(filePath);
            Prodotti = JsonConvert.DeserializeObject<List<Prodotto>>(json) ?? new List<Prodotto>();
            ProdottoModificato = Prodotti.FirstOrDefault(p => p.Id == id);
        }
    }

    public async Task<IActionResult> OnPost()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/prodotti.json");
        if (System.IO.File.Exists(filePath))
        {
            var json = await System.IO.File.ReadAllTextAsync(filePath);
            var prodotti = JsonConvert.DeserializeObject<List<Prodotto>>(json) ?? new List<Prodotto>();
            var prodotto = prodotti.FirstOrDefault(p => p.Id == ProdottoModificato.Id);
            if (prodotto != null)
            {
                prodotto.Nome = ProdottoModificato.Nome;
                prodotto.Prezzo = ProdottoModificato.Prezzo;
                await System.IO.File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(prodotti, Formatting.Indented));
            }
        }
        return RedirectToPage("Index");
    }
}