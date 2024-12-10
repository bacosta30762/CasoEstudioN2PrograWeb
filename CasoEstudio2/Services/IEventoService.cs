using CasoEstudio2.Models;

namespace CasoEstudio2.Services
{
    public interface IEventoService
    {
        Task<IEnumerable<Evento>> ObtenerEventosAsync();
        Task<Evento> ObtenerEventoPorIdAsync(int id);
        Task CrearEventoAsync(Evento evento);
        Task ActualizarEventoAsync(Evento evento);
        Task EliminarEventoAsync(int id);
    }
}
