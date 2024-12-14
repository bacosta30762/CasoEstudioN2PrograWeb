using CasoEstudio2.Models;
using CasoEstudio2.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CasoEstudio2.Controllers
{
    public class AsistenciaController : Controller
    {
        private readonly Caso2DbContext _context;

        public AsistenciaController(Caso2DbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> TomarLista(int eventoId)
        {
            // Log para verificar el valor de eventoId
            Console.WriteLine($"EventoId recibido: {eventoId}");

            var evento = await _context.Eventos
                .Include(e => e.Inscripciones)
                    .ThenInclude(i => i.Usuario)
                .Include(e => e.Inscripciones)
                    .ThenInclude(i => i.Asistencia)
                .FirstOrDefaultAsync(e => e.Id == eventoId);

            if (evento == null)
            {
                return NotFound($"Evento con Id {eventoId} no encontrado.");
            }

            ViewBag.EventoTitulo = evento.Titulo;
            ViewBag.EventoId = eventoId;

            return View(evento.Inscripciones);
        }



        [HttpPost]
        public async Task<IActionResult> TomarLista(int eventoId, List<Inscripcion> inscripciones)
        {
            if (inscripciones == null || inscripciones.Count == 0)
            {
                return BadRequest("No se enviaron datos para actualizar.");
            }

            foreach (var inscripcion in inscripciones)
            {
                var dbInscripcion = await _context.Inscripciones
                    .Include(i => i.Asistencia)
                    .FirstOrDefaultAsync(i => i.InscripcionId == inscripcion.InscripcionId);

                if (dbInscripcion != null)
                {
                    if (dbInscripcion.Asistencia == null)
                    {
                        // Crear nueva asistencia si no existe
                        dbInscripcion.Asistencia = new Asistencia
                        {
                            InscripcionId = dbInscripcion.InscripcionId,
                            Estado = inscripcion.Asistencia.Estado,
                            FechaRegistro = DateTime.Now
                        };
                        _context.Asistencias.Add(dbInscripcion.Asistencia);
                    }
                    else
                    {
                        // Actualizar asistencia existente
                        dbInscripcion.Asistencia.Estado = inscripcion.Asistencia.Estado;
                        dbInscripcion.Asistencia.FechaRegistro = DateTime.Now;
                    }
                }
            }

            // Guardar cambios
            await _context.SaveChangesAsync();

            // Redirigir al detalle del evento
            TempData["Success"] = "Asistencia registrada correctamente.";
            return RedirectToAction("Index", "Eventos");
        }

    }
}
