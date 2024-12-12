using APICatalogo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Context; // Esáço para definir a string de conexão com o Banco De dados. Para isso precisamos ir para o appsetings.json

public class AppDbContext : IdentityDbContext<ApplicationUser>  // Herda 
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { 
            
    }

    public DbSet<Categoria>? Categorias { get; set; }
    public DbSet<Produto>? Produtos { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
