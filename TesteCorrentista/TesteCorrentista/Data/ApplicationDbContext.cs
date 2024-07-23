using Microsoft.EntityFrameworkCore;
using TesteCorrentista.Models;
using System.Collections.Generic;
using TesteCorrentista.Models;

namespace SeuProjeto.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Transacao> Transacoes { get; set; }
    }
}

