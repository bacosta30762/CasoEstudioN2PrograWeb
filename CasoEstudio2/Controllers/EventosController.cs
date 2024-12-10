using CasoEstudio2.Models;
using CasoEstudio2.Services;
using Microsoft.AspNetCore.Mvc;

namespace CasoEstudio2.Controllers
{
    public class EventosController : Controller
    {
        private readonly IEventoService _eventoService;

        public EventosController(IEventoService eventoService)
        {
            _eventoService = eventoService;
        }

        // Listado de eventos
        public async Task<IActionResult> Index()
        {
            var eventos = await _eventoService.ObtenerEventosAsync();
            return View(eventos);
        }

        // Crear evento (vista de formulario)
        public IActionResult Crear()
        {
            var evento = new Evento
            {
                Fecha = DateTime.Now
            };
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
            var evento = await _eventoService.ObtenerEventoPorIdAsync(id);
            if (evento == null)
                return NotFound();
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
    }
}

