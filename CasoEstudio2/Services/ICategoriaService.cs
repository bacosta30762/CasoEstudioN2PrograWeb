using CasoEstudio2.Models;

namespace CasoEstudio2.Services
{
    public interface ICategoriaService
    {
        Task AgregarCategoriaAsync(Categoria categoria);
        Task EditarCategoriaAsync(Categoria categoria);
        Task EliminarCategoriaAsync(int id);
        Task<List<Categoria>> ObtenerCategoriasAsync();
        Task<Categoria?> ObtenerCategoriaPorIdAsync(int id);
    }
}