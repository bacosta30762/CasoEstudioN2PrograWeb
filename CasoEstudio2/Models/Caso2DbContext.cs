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

        public DbSet<Asistencia> Asistencias { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Inscripcion>()
                .HasOne(i => i.Usuario)
                .WithMany(u => u.Inscripciones)
                .HasForeignKey(i => i.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Inscripcion>()
                .HasOne(i => i.Evento)
                .WithMany(e => e.Inscripciones)
                .HasForeignKey(i => i.EventoId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Inscripcion>()
                .HasOne(i => i.Asistencia)
                .WithOne(a => a.Inscripcion)
                .HasForeignKey<Asistencia>(a => a.InscripcionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
