using CasoEstudio2.Models;
using CasoEstudio2.Services;
using Microsoft.AspNetCore.Mvc;

namespace CasoEstudio2.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var categorias = await _categoriaService.ObtenerCategoriasAsync();
            return View(categorias);
        }
        public IActionResult Crear()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CrearAsync(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _categoriaService.AgregarCategoriaAsync(categoria);
                    return RedirectToAction("Index");
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(categoria);
        }
    }
}
