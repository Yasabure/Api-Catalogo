using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace APICatalogo.Models;

public class Produto
{ // Classe Anêmica(Sem Comportamento )
    [Key] // Seta ProdutoId como chave primária
    public int ProdutoId { get; set; }
    [Required]  //Nome não pode ser nulo
    [StringLength(80)] // tamanho máximo de 80 bits
    public string?  Nome { get; set; }
    [Required] //Descrição não pode ser nulo
    [StringLength(300)] // Tamanho máximo de 300 bits
    public string?  Descricao { get; set; }
    [Required] //Preco não pode ser nulo
    [Column(TypeName = "decimal(10,2)")] // Pode armazenar até 10 números antes da casa decimal, e apenas 2 após
    public decimal Preco { get; set; }
    [Required] //ImagemUrl não pode ser nulo
    [StringLength(300)] //tamanho máximo de 300 bits
    public string? ImagemUrl { get; set; }
    public float Estoque { get; set; }
    public DateTime DataCadastro { get; set; }

    public int CategoriaId { get; set; }
    [JsonIgnore] // Prop não será exibida no Put e Post
    public Categoria? Categoria { get; set; }
}
