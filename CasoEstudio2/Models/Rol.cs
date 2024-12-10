using System.ComponentModel.DataAnnotations;

namespace CasoEstudio2.Models
{
    public class Rol
    {

        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; }

    }
}
