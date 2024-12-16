using CasoEstudio2.Models;
using Microsoft.EntityFrameworkCore;

namespace CasoEstudio2.Services
{
    public class InscripcionService : IInscripcionService
    {
        private readonly Caso2DbContext _context;

        public InscripcionService(Caso2DbContext context)
        {
            _context = context;
        }

        public async Task<List<Inscripcion>> ObtenerInscripcionesAsync()
        {
            return await _context.Inscripciones.ToListAsync();
        }

        public async Task<Resultado> AgregarInscripcionAsync(int idevento)
        {
            var usuarioAutenticado = new Usuario { Id = 1 }; // Suponiendo un usuario autenticado con id = 1
            var evento = await _context.Eventos.FindAsync(idevento);
            if (evento == null)
            {
                return new Resultado { Exitoso = false, Mensaje = "No se encontró el evento." };
            }
            if (evento.Fecha < DateTime.Now)
            {
                return new Resultado { Exitoso = false, Mensaje = "El evento ya pasó." };
            }
            var cantidadInscripciones = await _context.Inscripciones
                .Where(i => i.EventoId == evento.Id)
                .CountAsync();
            if (cantidadInscripciones >= evento.CupoMaximo)
            {
                return new Resultado { Exitoso = false, Mensaje = "El evento ya está lleno." };
            }

            var elUsuarioEstaDisponibleParaElEvento = await VerificarDisponibilidadEventoAsync(evento);

            if (!elUsuarioEstaDisponibleParaElEvento)
            {
                return new Resultado { Exitoso = false, Mensaje = "Ya se encuentra registrado a un evento." };
            }

            var inscripcion = new Inscripcion
            {
                EventoId = evento.Id,
                UsuarioId = usuarioAutenticado.Id,
            };

            _context.Inscripciones.Add(inscripcion);
            await _context.SaveChangesAsync();

            return new Resultado { Exitoso = true };
        }

        public async Task<List<Inscripcion>> ObtenerInscripcionesPorEventoAsync(int id)
        {
            // Retorna todas las inscripciones para el evento con el Id dado
            return await _context.Inscripciones
                .Where(i => i.EventoId == id)
                .ToListAsync();
        }

        private async Task<bool> VerificarDisponibilidadEventoAsync(Evento evento)
        {
            var usuarioAutenticado = new Usuario { Id = 1 }; // Suponiendo un usuario autenticado con id = 1
            var eventosUsuario = await _context.Inscripciones
                .Include(i => i.Evento)
                .Where(i => i.UsuarioId == usuarioAutenticado.Id)
                .Select(i => i.Evento)
                .ToListAsync();

            foreach (var eventoUsuario in eventosUsuario)
            {
                var eventoInicio = evento.Fecha.Add(evento.Hora);
                var eventoFin = eventoInicio.AddMinutes(evento.Duracion);

                var eventoUsuarioInicio = eventoUsuario.Fecha.Add(eventoUsuario.Hora);
                var eventoUsuarioFin = eventoUsuarioInicio.AddMinutes(eventoUsuario.Duracion);

                if (eventoInicio < eventoUsuarioFin && eventoFin > eventoUsuarioInicio)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
