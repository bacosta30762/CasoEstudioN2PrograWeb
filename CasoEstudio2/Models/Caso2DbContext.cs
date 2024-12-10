using Microsoft.EntityFrameworkCore;

namespace CasoEstudio2.Models
{
    public class Caso2DbContext : DbContext
    {
        public DbSet<Evento> Eventos { get; set; }
        public Caso2DbContext(DbContextOptions<Caso2DbContext> options) : base(options)
        {
        }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
    }
}
