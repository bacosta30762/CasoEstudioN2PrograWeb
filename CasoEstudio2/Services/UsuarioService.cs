using CasoEstudio2.Models;
using Microsoft.EntityFrameworkCore;

namespace CasoEstudio2.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly Caso2DbContext _context;

        public UsuarioService(Caso2DbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> CrearUsuario(Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<List<Usuario>> ObtenerUsuariosPorRol(string rol)
        {
            var usuarios = await _context.Usuarios
                .Where(u => u.Rol.Nombre == rol)
                .ToListAsync();
            return usuarios;
        }

        public async Task<Usuario> AsignarRol(int usuarioId, int rolId)
        {
            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null) return null;

            usuario.RolId = rolId;
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario> AutenticarUsuario(string correo, string password)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Correo == correo && u.Contraseña == password);
            return usuario;
        }

        public async Task<List<Usuario>> ObtenerTodosUsuarios()
        {
            return await _context.Usuarios
                .Include(u => u.Rol)
                .ToListAsync();
        }

    }
}

