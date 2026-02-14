using System.ComponentModel.DataAnnotations;

namespace PryTrabajadoresPrueba.Application.DTOs
{
    public class TrabajadorDTO
    {
        public string? IdTrb { get; set; };

        [Required(ErrorMessage = "Los Nombres es obligatorio")]
        public string Nombres { get; set; }  = null!;

        [Required(ErrorMessage = "Los Apellidos es obligatorio")]
        public string Apellidos { get; set; } = null!;

        [Required(ErrorMessage = "El Tipo de Documento es obligatorio")]
        public string TipoDocumento { get; set; } = null!;

        [Required(ErrorMessage = "El Numero de Documento es obligatorio")]
        public string NumeroDocumento { get; set; } = null!;

        [Required(ErrorMessage = "El Sexo es obligatorio")]
        public string Sexo { get; set; } = null!;

        [Required(ErrorMessage = "La Fecha de Nacimiento es obligatorio")]       
        public DateTime FechaNacimiento { get; set; }

        // URL de la foto
        public string? FotoUrl { get; set; }  

        public string Direccion { get; set; } = null!;
    }

}