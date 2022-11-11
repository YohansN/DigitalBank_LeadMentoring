using DigitalBankApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DigitalBankApi.Data
{
    public class Context : DbContext
    {

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-BKEG8HP;Initial Catalog=DigitalBankApi;Integrated Security=True");
        }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<ContaBancaria> ContaBancaria { get; set; }
        public DbSet<Transacao> Transacao { get; set; }
    }
}
