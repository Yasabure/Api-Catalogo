using APICatalogo.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace APICatalogo.DTOs
{
    public class ProdutoDTO
    {
        public int ProdutoId { get; set; }
        [Required]  //Nome não pode ser nulo
        [StringLength(80)] // tamanho máximo de 80 bits

        public string? Nome { get; set; }

        [Required] //Descrição não pode ser nulo
        [StringLength(300)] // Tamanho máximo de 300 bits

        public string? Descricao { get; set; }

        [Required] //Preco não pode ser nulo
        public decimal Preco { get; set; }
        [Required] //ImagemUrl não pode ser nulo
        [StringLength(300)] //tamanho máximo de 300 bits
        public string? ImagemUrl { get; set; }

        public int CategoriaId { get; set; }

    }
}

