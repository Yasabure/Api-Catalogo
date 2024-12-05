using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.AspNetCore.Http.HttpResults;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        { }
        public async Task<PagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters)
        {
            var categorias = await GetAllAsync();
            var categoriasOrdenadas = categorias.OrderBy(p => p.CategoriaId).AsQueryable();
            var resultado = PagedList<Categoria>.ToPagedList(categoriasOrdenadas, categoriasParameters.PageNumber, categoriasParameters.PageSize);
            return resultado;
        }

        public async Task<PagedList<Categoria>> GetCategoriasNomeAsync(CategoriasFiltroNome categoriasParameters)
        {
            var categorias = await GetAllAsync();
            if (!string.IsNullOrEmpty(categoriasParameters.Nome))
                categorias = categorias.Where(p => p.Nome.Contains(categoriasParameters.Nome));

            var categoriasFiltradas = PagedList<Categoria>.ToPagedList(categorias.AsQueryable(), categoriasParameters.PageNumber, categoriasParameters.PageSize);
            return categoriasFiltradas;

        }

    }
}

