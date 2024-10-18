using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace APICatalogo.Models;

public class Categoria
{
    public Categoria()
    {
        Produtos = new Collection<Produto>();
    }
    [Key] //Categoriza CategoriaId como Chave Primaria (é uma redundancia, pois Id já é tido como uma
    public int CategoriaId { get; set; }
    [Required] // Nome não pode ser nulo 
    [StringLength(80)] // Nome Pode consumir no máximo 80 bits
    public string? Nome { get; set; }
    [Required] // imagemUrl não pode ser nulo   
    [StringLength(300)] // ImagemUrl pode consumir no máximo 300 bits
    public string?  ImagemUrl { get; set; }

    public ICollection<Produto>? Produtos { get; set; } // Métodod para realizar a conexão no banco de dados entre Categoria e produtoi
}
