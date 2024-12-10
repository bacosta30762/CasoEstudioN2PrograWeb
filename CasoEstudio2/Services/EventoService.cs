using CasoEstudio2.Models;
using Microsoft.EntityFrameworkCore;

namespace CasoEstudio2.Services
{
    public class EventoService : IEventoService
    {
        private readonly Caso2DbContext _context;

        public EventoService(Caso2DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Evento>> ObtenerEventosAsync()
        {
            return await _context.Eventos.ToListAsync();
        }

        public async Task<Evento> ObtenerEventoPorIdAsync(int id)
        {
            return await _context.Eventos.FindAsync(id);
        }

        public async Task CrearEventoAsync(Evento evento)
        {
            // Validaciones
            if (evento.Fecha < DateTime.Now.Date)
                throw new InvalidOperationException("La fecha no puede ser en el pasado.");

            if (evento.Duracion <= 0)
                throw new InvalidOperationException("La duración debe ser positiva.");

            if (evento.CupoMaximo <= 0)
                throw new InvalidOperationException("El cupo máximo debe ser mayor a 0.");

            evento.FechaRegistro = DateTime.Now;
            evento.UsuarioRegistro = "UsuarioActual"; // Esto debe ser gestionado por la sesión del usuario 
            //evento.UsuarioRegistro = User.Identity.Name ?? "UsuarioDesconocido";

            _context.Eventos.Add(evento);
            await _context.SaveChangesAsync();
        }

        public async Task ActualizarEventoAsync(Evento evento)
        {
            var eventoExistente = await _context.Eventos.FindAsync(evento.Id);
            if (eventoExistente == null)
                throw new InvalidOperationException("El evento no existe.");

            // Validaciones
            if (evento.Fecha < DateTime.Now.Date)
                throw new InvalidOperationException("La fecha no puede ser en el pasado.");

            if (evento.Duracion <= 0)
                throw new InvalidOperationException("La duración debe ser positiva.");

            if (evento.CupoMaximo <= 0)
                throw new InvalidOperationException("El cupo máximo debe ser mayor a 0.");

            eventoExistente.Titulo = evento.Titulo;
            eventoExistente.Descripcion = evento.Descripcion;
            eventoExistente.CategoriaId = evento.CategoriaId;
            eventoExistente.Fecha = evento.Fecha;
            eventoExistente.Hora = evento.Hora;
            eventoExistente.Duracion = evento.Duracion;
            eventoExistente.Ubicacion = evento.Ubicacion;
            eventoExistente.CupoMaximo = evento.CupoMaximo;

            await _context.SaveChangesAsync();
        }

        public async Task EliminarEventoAsync(int id)
        {
            var evento = await _context.Eventos.FindAsync(id);
            if (evento == null)
                throw new InvalidOperationException("El evento no existe.");

            _context.Eventos.Remove(evento);
            await _context.SaveChangesAsync();
        }

    }
}
