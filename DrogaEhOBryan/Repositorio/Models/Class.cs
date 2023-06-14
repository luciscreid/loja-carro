using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;


namespace Repositorio.Models
{

    public class CarrosContext : DbContext
    {
        public DbSet<Carro> Carros { get; set; }

        public DbSet<Marca> Marcas { get; set; }

        public string DbPath { get; }

        public CarrosContext()
        {
            var folder = Environment.SpecialFolder.LocalApplicationData;
            var path = Environment.GetFolderPath(folder);
            DbPath = System.IO.Path.Join(path, "carros.db");
        }

        // The following configures EF to create a Sqlite database file in the
        // special "local" folder for your platform.
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer("Data Source=localhost;Initial Catalog=carros;Persist Security Info=True;User ID=dev;Password=Cr.2005")
                .LogTo(x => System.Diagnostics.Debug.WriteLine(x))
                .EnableDetailedErrors(true)
                .EnableSensitiveDataLogging(true);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Carro>()
                .HasOne(c => c.Marca)
                .WithMany(m => m.Carros)
                .HasForeignKey(c => c.MarcaId)
                .HasPrincipalKey(m => m.Id);

            modelBuilder.Entity<Marca>()
                .ToTable("Marca");
            //modelBuilder.Entity<Carro>()
            //    .HasOne(e => e.Marca)
            //    .WithMany(e => e.Carros)
            //    .HasForeignKey(e => e.MarcaId)
            //    .IsRequired();
        }

    }

}

