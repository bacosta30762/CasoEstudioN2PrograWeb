using CasoEstudio2.Models;

namespace CasoEstudio2.Services
{
    public interface IInscripcionService
    {
        Task<Resultado> AgregarInscripcionAsync(int idevento);
        Task<List<Inscripcion>> ObtenerInscripcionesAsync();
        Task<List<Inscripcion>> ObtenerInscripcionesPorEventoAsync(int id);
    }
}