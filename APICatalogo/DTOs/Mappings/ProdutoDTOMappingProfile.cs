using APICatalogo.Models;
using AutoMapper;

namespace APICatalogo.DTOs.Mappings
{
    public class ProdutoDTOMappingProfile : Profile
    {
        public ProdutoDTOMappingProfile()
        {
            CreateMap<Produto, ProdutoDTO>().ReverseMap(); // Mapeando entidade Produto para ProdutoDTO
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
        }
    }
}
