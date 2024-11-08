using System.ComponentModel.DataAnnotations;

namespace APICatalogo.DTOs
{
    public class CategoriaDTO
    {
        public int CategoriaId { get; set; }
        [Required] // Nome não pode ser nulo 
        [StringLength(80)] // Nome Pode consumir no máximo 80 bits
        public string? Nome { get; set; }
        [Required] // imagemUrl não pode ser nulo   
        [StringLength(300)] // ImagemUrl pode consumir no máximo 300 bits
        public string? ImagemUrl { get; set; }
    }
}
