using Microsoft.EntityFrameworkCore;

namespace CasoEstudio2.Models
{
    public class Caso2DbContext : DbContext
    {
        public Caso2DbContext(DbContextOptions<Caso2DbContext> options) : base(options)
        {
        }

        public DbSet<Evento> Eventos { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Inscripcion>()
                .HasKey(e => new { e.UsuarioId, e.EventoId });
        }
    }
}
