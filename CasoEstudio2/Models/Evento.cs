using System.ComponentModel.DataAnnotations;

namespace CasoEstudio2.Models
{
    public class Evento
    {
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime Fecha { get; set; }

        [Required]
        [DataType(DataType.Time)]
        public TimeSpan Hora { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "La duración debe ser un valor positivo.")]
        public int Duracion { get; set; } // Poner la duración en minutos

        [Required]
        public string Ubicacion { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El cupo máximo debe ser mayor a 0.")]
        public int CupoMaximo { get; set; }

        [Required]
        public int CategoriaId { get; set; }

        public DateTime FechaRegistro { get; set; }
        public string? UsuarioRegistro { get; set; }

        public List<Inscripcion>? Inscripciones { get; set; } = new List<Inscripcion>();

    }
}
