namespace CasoEstudio2.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; } =string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public DateTime FechaRegistro { get; set; }
        public string UsuarioRegistro { get; set; } = string.Empty;

    }
}
