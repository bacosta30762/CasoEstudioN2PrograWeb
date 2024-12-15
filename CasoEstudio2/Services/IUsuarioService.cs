using CasoEstudio2.Models;

namespace CasoEstudio2.Services
{
    public interface IUsuarioService
    {
        Task<Usuario> CrearUsuario(Usuario usuario);
        Task<List<Usuario>> ObtenerUsuariosPorRol(string rol);
        Task<Usuario> AsignarRol(int usuarioId, int rolId);
        Task<Usuario> AutenticarUsuario(string correo, string password);
        Task<List<Usuario>> ObtenerTodosUsuarios();
    }
}
