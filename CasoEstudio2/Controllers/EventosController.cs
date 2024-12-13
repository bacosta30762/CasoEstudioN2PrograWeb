using CasoEstudio2.Models;
using CasoEstudio2.Services;
using Microsoft.AspNetCore.Mvc;

namespace CasoEstudio2.Controllers
{
    public class EventosController : Controller
    {
        private readonly IEventoService _eventoService;
        private readonly ICategoriaService _categoriaService;
        private readonly IInscripcionService _inscripcionService;

        public EventosController(IEventoService eventoService, ICategoriaService categoriaService, IInscripcionService inscripcionService)
        {
            _eventoService = eventoService;
            _categoriaService = categoriaService;
            _inscripcionService = inscripcionService;
        }

        // Listado de eventos
        public async Task<IActionResult> Index()
        {
            var eventos = await _eventoService.ObtenerEventosAsync();
            return View(eventos);
        }

        // Crear evento (vista de formulario)
        public async Task<IActionResult> CrearAsync()
        {
            var categorias = await _categoriaService.ObtenerCategoriasActivasAsync();


            var evento = new Evento
            {
                Fecha = DateTime.Now
            };

            ViewBag.Categorias = categorias;
            return View(evento);
        }

        // Crear evento
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(Evento evento)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _eventoService.CrearEventoAsync(evento);
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }

            return View(evento);
        }

        // Modificar evento (vista de formulario)
        public async Task<IActionResult> Editar(int id)
        {
            var categorias = await _categoriaService.ObtenerCategoriasActivasAsync();
            var evento = await _eventoService.ObtenerEventoPorIdAsync(id);
            if (evento == null)
                return NotFound();
            ViewBag.Categorias = categorias;            
            return View(evento);
        }

        // Modificar evento 
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Evento evento)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _eventoService.ActualizarEventoAsync(evento);
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
            }
            return View(evento);
        }

        // Eliminar evento
        public async Task<IActionResult> Eliminar(int id)
        {
            var evento = await _eventoService.ObtenerEventoPorIdAsync(id);
            if (evento == null)
                return NotFound();

            await _eventoService.EliminarEventoAsync(id);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Inscribir(int id)
        {
            var resultadoInscribir = await _inscripcionService.AgregarInscripcionAsync(id);
            if (resultadoInscribir.Exitoso == false)
            {
                TempData["Mensaje"] = resultadoInscribir.Mensaje;
                return RedirectToAction(nameof(Index));
            }

            TempData["Mensaje"] = "Se inscribió correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}

