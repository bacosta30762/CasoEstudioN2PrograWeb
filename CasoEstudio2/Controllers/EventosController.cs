using CasoEstudio2.Models;
using CasoEstudio2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            var categorias = await _categoriaService.ObtenerCategoriasActivasAsync() ?? Enumerable.Empty<SelectListItem>();

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
            if (evento.Fecha < DateTime.Now)
            {
                ModelState.AddModelError(string.Empty, "La fecha del evento debe ser futura.");
            }
            if (evento.Duracion <= 0)
            {
                ModelState.AddModelError(string.Empty, "La duración del evento debe ser positiva.");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    // Validar superposición de horarios
                    var eventosExistentes = await _eventoService.ObtenerEventosAsync();
                    bool horarioSuperpuesto = eventosExistentes.Any(e =>
                        e.Fecha.Date == evento.Fecha.Date &&
                        e.Fecha <= evento.Fecha.AddMinutes(evento.Duracion) &&
                        evento.Fecha <= e.Fecha.AddMinutes(e.Duracion));

                    if (horarioSuperpuesto)
                    {
                        ModelState.AddModelError(string.Empty, "El evento se superpone con otro en el mismo horario.");
                        return View(evento);
                    }

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
            var evento = await _eventoService.ObtenerEventoPorIdAsync(id);
            if (evento == null)
            {
                return NotFound();
            }

            // Esperar el resultado de inscripciones (se asume que retorna una lista)
            var inscripciones = await _inscripcionService.ObtenerInscripcionesPorEventoAsync(id);
            if (inscripciones?.Count() >= evento.CupoMaximo) // Verificar Count() para una colección
            {
                TempData["Mensaje"] = "El evento ya ha alcanzado el cupo máximo.";
                return RedirectToAction(nameof(Index));
            }

            // Intentar agregar inscripción
            var resultadoInscribir = await _inscripcionService.AgregarInscripcionAsync(id);
            if (resultadoInscribir == null || !resultadoInscribir.Exitoso)
            {
                TempData["Mensaje"] = resultadoInscribir?.Mensaje ?? "Ocurrió un error inesperado.";
                return RedirectToAction(nameof(Index));
            }

            TempData["Mensaje"] = "Se inscribió correctamente.";
            return RedirectToAction(nameof(Index));
        }

    }
}

