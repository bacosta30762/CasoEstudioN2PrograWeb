using CasoEstudio2.Models;
using Microsoft.EntityFrameworkCore;

namespace CasoEstudio2.Seed
{
    public class RoleSeeder
    {

        private readonly Caso2DbContext _context;

        public RoleSeeder(Caso2DbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            if (!_context.Usuarios.Any(u => u.Rol.Nombre == "Administrador"))
            {
                var adminRol = _context.Roles.First(r => r.Nombre == "Administrador");
                _context.Usuarios.Add(new Usuario
                {
                    NombreUsuario = "Administrador",
                    NombreCompleto = "Admin",
                    Correo = "admin@admin.com",
                    Telefono = "88888888",
                    Contraseña = "Admin123!",
                    RolId = adminRol.Id
                });
                await _context.SaveChangesAsync();
            }
        }
    }
}

