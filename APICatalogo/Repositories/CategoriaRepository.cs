using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using Microsoft.AspNetCore.Http.HttpResults;
using X.PagedList;

namespace APICatalogo.Repositories
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        public CategoriaRepository(AppDbContext context) : base(context)
        { }
        public async Task<IPagedList<Categoria>> GetCategoriasAsync(CategoriasParameters categoriasParameters)
        {
            var categorias = await GetAllAsync();
            var categoriasOrdenadas = categorias.OrderBy(p => p.CategoriaId).AsQueryable();
            var resultado = await categoriasOrdenadas.ToPagedListAsync(categoriasParameters.PageNumber, categoriasParameters.PageSize);
            return resultado;
        }

        public async Task<IPagedList<Categoria>> GetCategoriasNomeAsync(CategoriasFiltroNome categoriasParameters)
        {
            var categorias = await GetAllAsync();
            if (!string.IsNullOrEmpty(categoriasParameters.Nome))
                categorias = categorias.Where(p => p.Nome.Contains(categoriasParameters.Nome));

            var categoriasFiltradas = await categorias.ToPagedListAsync(categoriasParameters.PageNumber, categoriasParameters.PageSize);
            return categoriasFiltradas;

        }

    }
}

