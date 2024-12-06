using APICatalogo.Context;
using APICatalogo.Models;
using APICatalogo.Pagination;
using X.PagedList;

namespace APICatalogo.Repositories
{
    public class ProdutoRepository : Repository<Produto> , IProdutoRepository
    {
        public ProdutoRepository(AppDbContext context) :base(context)
        {
        }

        public async Task<IPagedList<Produto>> GetProdutosAsync(ProdutosParameters produtosParameters)
        {
            var produtos = await GetAllAsync();
            var produtosOrdenados = produtos.OrderBy(p => p.ProdutoId).AsQueryable();
            var resultado = await produtosOrdenados.ToPagedListAsync(produtosParameters.PageNumber,produtosParameters.PageSize);
            return resultado;
        }

        public async  Task<IPagedList<Produto>> GetProdutosFiltroPrecoAsync(ProdutosFiltroPreco produtosFiltroParameters)
        {
            var produtos = await GetAllAsync();
            if(produtosFiltroParameters.Preco.HasValue && !string.IsNullOrEmpty(produtosFiltroParameters.PrecoCriteiro))
            {
                if (produtosFiltroParameters.PrecoCriteiro.Equals("maior", StringComparison.OrdinalIgnoreCase))
                    produtos = produtos.Where(p => p.Preco > produtosFiltroParameters.Preco.Value).OrderBy(p => p.Preco);
                else if (produtosFiltroParameters.PrecoCriteiro.Equals("menor", StringComparison.OrdinalIgnoreCase))
                    produtos = produtos.Where(p => p.Preco < produtosFiltroParameters.Preco.Value).OrderBy(p => p.Preco);
                else if (produtosFiltroParameters.PrecoCriteiro.Equals("igual", StringComparison.OrdinalIgnoreCase))
                    produtos = produtos.Where(p => p.Preco == produtosFiltroParameters.Preco.Value).OrderBy(p => p.Preco);
            }
            var produtosFiltrados = await produtos.ToPagedListAsync(produtosFiltroParameters.PageNumber, produtosFiltroParameters.PageSize);
            return produtosFiltrados;
        }

        public async Task<IEnumerable<Produto>> GetProdutosPorCategoriaAsync(int id)
        {
            var result = await GetAllAsync();
            var produtosCategoria = result.Where(c => c.CategoriaId == id);
            return produtosCategoria;
        }
    }
}
