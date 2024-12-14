using System.ComponentModel.DataAnnotations;

namespace CasoEstudio2.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string NombreUsuario { get; set; }

        [Required]
        [StringLength(200)]
        public string NombreCompleto { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        [Phone]
        public string Telefono { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; }

        // Relación con Rol
        [Required]
        public int RolId { get; set; }
        public Rol Rol { get; set; }

        public List<Inscripcion>? Inscripciones { get; set; } = new List<Inscripcion>();

    }
}
