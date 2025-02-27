using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

public class AggiungiProdottoModel : PageModel
{
    [BindProperty]
    public Prodotto NuovoProdotto { get; set; } = new();

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPost()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/prodotti.json");
        List<Prodotto> prodotti = new();

        if (System.IO.File.Exists(filePath))
        {
            var json = await System.IO.File.ReadAllTextAsync(filePath);
            prodotti = JsonConvert.DeserializeObject<List<Prodotto>>(json) ?? new List<Prodotto>();
        }

        prodotti.Add(NuovoProdotto);
        await System.IO.File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(prodotti, Formatting.Indented));

        return RedirectToPage("Index");
    }
}