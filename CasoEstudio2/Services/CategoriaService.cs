using CasoEstudio2.Models;
using Microsoft.EntityFrameworkCore;

namespace CasoEstudio2.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly Caso2DbContext _context;

        public CategoriaService(Caso2DbContext context)
        {
            _context = context;
        }
        public async Task<List<Categoria>> ObtenerCategoriasAsync()
        {
            return await _context.Categorias.ToListAsync();
        }
        public async Task AgregarCategoriaAsync(Categoria categoria)
        {
            categoria.FechaRegistro = DateTime.Now;
            categoria.UsuarioRegistro = "UsuarioActual";
            _context.Categorias.Add(categoria);
            await _context.SaveChangesAsync();
        }
        public async Task EditarCategoriaAsync(Categoria categoria)
        {
            var categoriaExistente = await _context.Categorias.FindAsync(categoria.Id);
            if (categoriaExistente == null)
                throw new InvalidOperationException("La categoria no existe.");
            categoriaExistente.Nombre = categoria.Nombre;
            categoriaExistente.Descripcion = categoria.Descripcion;
            categoriaExistente.Estado = categoria.Estado;
            await _context.SaveChangesAsync();
        }
        public async Task EliminarCategoriaAsync(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
                throw new InvalidOperationException("La categoria no existe.");
            _context.Categorias.Remove(categoria);
            await _context.SaveChangesAsync();
        }
        public async Task<Categoria?> ObtenerCategoriaPorIdAsync(int id)
        {
            return await _context.Categorias.FindAsync(id);
        }
    }
}
