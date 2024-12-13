namespace CasoEstudio2.Models
{
    public class Inscripcion
    {
        public int EventoId { get; set; }
        public int UsuarioId { get; set; }
        public Evento? Evento { get; set; }
        public Usuario? Usuario { get; set; }
    }
}
