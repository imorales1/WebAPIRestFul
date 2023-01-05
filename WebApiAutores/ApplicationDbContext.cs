using Microsoft.EntityFrameworkCore;
using System.Threading;
using WebApiAutores.Entidades;

namespace WebApiAutores
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Libro>(libro =>
            {
                libro.ToTable("Libros");
                libro.HasKey(p => p.Id);
                //tarea.HasOne(p => p.categoria).WithMany(p => p.Tareas).HasForeignKey(p => p.CategoriaId);
                libro.HasOne(p => p.Autor).WithMany(p => p.Libros).HasForeignKey(p => p.AutorId);
                libro.Property(p => p.Titulo).IsRequired().HasMaxLength(50);
            });
        }
    }
}
