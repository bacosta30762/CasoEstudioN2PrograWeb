using Microsoft.EntityFrameworkCore;

namespace CasoEstudio2.Models
{
    public class Caso2DbContext : DbContext
    {
        public Caso2DbContext(DbContextOptions<Caso2DbContext> options) : base(options)
        {
        }
    }
}
