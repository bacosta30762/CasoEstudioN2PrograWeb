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
        public async Task<IActionResult> EditarAsync(int id)
        {
            var categoria = await _categoriaService.ObtenerCategoriaPorIdAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> EditarAsync(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _categoriaService.EditarCategoriaAsync(categoria);
                    return RedirectToAction("Index");
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(categoria);
        }

        public async Task<IActionResult> EliminarAsync(int id)
        {
            var categoria = await _categoriaService.ObtenerCategoriaPorIdAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }


        [HttpPost, ActionName("Eliminar")]
        public async Task<IActionResult> EliminarConfirmadoAsync(int id)
        {
            try
            {
                await _categoriaService.EliminarCategoriaAsync(id);
                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View();
            }

        }
    }
}
