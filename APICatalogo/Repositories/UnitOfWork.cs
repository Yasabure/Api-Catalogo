using APICatalogo.Context;

namespace APICatalogo.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        public AppDbContext _context;
        private IProdutoRepository? _ProdutoRepository;
        private ICategoriaRepository? _CategoriaRepository;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public IProdutoRepository ProdutoRepository
        {
            get
            {
                return _ProdutoRepository = _ProdutoRepository ?? new ProdutoRepository(_context);    
            }
        }
        public ICategoriaRepository CategoriaRepository
        {
            get
            {
                return _CategoriaRepository = _CategoriaRepository ?? new CategoriaRepository(_context);
            }
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
