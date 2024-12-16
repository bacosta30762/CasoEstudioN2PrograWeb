using CasoEstudio2.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CasoEstudio2.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        // Acción para la vista principal del dashboard
        public async Task<IActionResult> Index()
        {
            // Obtener los datos del dashboard
            var totalEventos = await _dashboardService.ObtenerTotalEventosAsync();
            var totalUsuariosActivos = await _dashboardService.ObtenerTotalUsuariosActivosAsync();
            var asistentesPorMes = await _dashboardService.ObtenerAsistentesPorMesAsync();
            var topEventosPopulares = await _dashboardService.ObtenerTopEventosPopularesAsync();

            // Crear un objeto anónimo o ViewModel para pasar los datos a la vista
            var dashboardData = new
            {
                TotalEventos = totalEventos,
                TotalUsuariosActivos = totalUsuariosActivos,
                AsistentesPorMes = asistentesPorMes,
                TopEventosPopulares = topEventosPopulares
            };

            // Pasar los datos a la vista
            return View(dashboardData);
        }
    }
}
