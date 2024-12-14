using System.ComponentModel.DataAnnotations;

namespace CasoEstudio2.Models
{
	public class Asistencia
	{
        [Key]
        public int Id { get; set; }
        public int InscripcionId { get; set; }
        public Inscripcion Inscripcion { get; set; }
        public string Estado { get; set; }
        public DateTime FechaRegistro { get; set; }

    }
}
