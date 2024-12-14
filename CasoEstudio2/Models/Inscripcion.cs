using System.ComponentModel.DataAnnotations;

namespace CasoEstudio2.Models
{
    public class Inscripcion

    {
        [Key]
        public int InscripcionId { get; set; }
        public int EventoId { get; set; }
        public int UsuarioId { get; set; }
        public Evento? Evento { get; set; }
        public Usuario? Usuario { get; set; }

        public Asistencia? Asistencia { get; set; }



    }
}
