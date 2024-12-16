using CasoEstudio2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CasoEstudio2.Services
{
    public interface IDashboardService
    {
        Task<int> ObtenerTotalEventosAsync();
        Task<int> ObtenerTotalUsuariosActivosAsync();
        Task<int> ObtenerAsistentesPorMesAsync();
        Task<List<Evento>> ObtenerTopEventosPopularesAsync();
    }

}
