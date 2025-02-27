using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

public class DeleteProdottoModel : PageModel
{
    [BindProperty]
    public int ProdottoId { get; set; }

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

    public async Task<IActionResult> OnPost()
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/data/prodotti.json");
        if (System.IO.File.Exists(filePath))
        {
            var json = await System.IO.File.ReadAllTextAsync(filePath);
            var prodotti = JsonConvert.DeserializeObject<List<Prodotto>>(json) ?? new List<Prodotto>();
            prodotti = prodotti.Where(p => p.Id != ProdottoId).ToList();
            await System.IO.File.WriteAllTextAsync(filePath, JsonConvert.SerializeObject(prodotti, Formatting.Indented));
        }
        return RedirectToPage("Index");
    }
}