using CasoEstudio2.Models;
using CasoEstudio2.Services;
using Microsoft.EntityFrameworkCore;

namespace CasoEstudio2.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly Caso2DbContext _context;
        private readonly IUsuarioService _usuarioService;
        private readonly IInscripcionService _inscripcionService;
        private readonly IEventoService _eventoService;

        public DashboardService(Caso2DbContext context,
                                IUsuarioService usuarioService,
                                IInscripcionService inscripcionService,
                                IEventoService eventoService)
        {
            _context = context;
            _usuarioService = usuarioService;
            _inscripcionService = inscripcionService;
            _eventoService = eventoService;
        }

        // Obtener el total de eventos creados
        public async Task<int> ObtenerTotalEventosAsync()
        {
            return await _context.Eventos.CountAsync();
        }

        // Obtener el total de usuarios activos (sin necesidad de agregar campo 'Activo' en la base)
        public async Task<int> ObtenerTotalUsuariosActivosAsync()
        {
            var usuarios = await _usuarioService.ObtenerTodosUsuarios();  // Obtener todos los usuarios
            return usuarios.Count();  // Contar el total de usuarios (considerando todos como activos)
        }

        // Obtener el número de asistentes registrados en el mes actual
        public async Task<int> ObtenerAsistentesPorMesAsync()
        {
            var primerDiaMesActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var ultimoDiaMesActual = primerDiaMesActual.AddMonths(1).AddDays(-1); // Último día del mes actual

            // Filtrar inscripciones basadas en las fechas del mes actual
            var inscripciones = await _context.Inscripciones
                .Include(i => i.Evento) // Incluimos la relación con el evento
                .Where(i => i.Evento.Fecha >= primerDiaMesActual && i.Evento.Fecha <= ultimoDiaMesActual)
                .CountAsync();  // Contar las inscripciones

            return inscripciones;
        }

        // Obtener el top 5 de eventos más populares (por cantidad de asistentes)
        public async Task<List<Evento>> ObtenerTopEventosPopularesAsync()
        {
            // Obtener los eventos con la mayor cantidad de inscripciones
            var eventosPopulares = await _context.Eventos
                .Select(e => new
                {
                    Evento = e,
                    Asistentes = _context.Inscripciones.Count(i => i.EventoId == e.Id)
                })
                .OrderByDescending(e => e.Asistentes)  // Ordenar por cantidad de asistentes
                .Take(5)  // Tomar los primeros 5 eventos
                .Select(e => e.Evento)  // Seleccionar solo el evento
                .ToListAsync();

            return eventosPopulares;
        }

        
    }
}
