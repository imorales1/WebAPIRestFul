using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using WebApiAutores.Entidades;

namespace WebApiAutores
{
    public class ApplicationDbContext: IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Autor> Autores { get; set; }
        public DbSet<Libro> Libros { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }
        public DbSet<AutorLibro> AutorLibro { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Autor>(autor =>
            {
                autor.ToTable("Autores");
                autor.HasKey(p => p.Id);
                autor.Property(p => p.Nombre).IsRequired();
            });

            modelBuilder.Entity<Libro>(libro =>
            {
                libro.ToTable("Libros");
                libro.HasKey(p => p.Id);
                //tarea.HasOne(p => p.categoria).WithMany(p => p.Tareas).HasForeignKey(p => p.CategoriaId);
                //libro.HasOne(p => p.Autor).WithMany(p => p.Libros).HasForeignKey(p => p.AutorId);
                libro.Property(p => p.Titulo).IsRequired();
            });

            modelBuilder.Entity<AutorLibro>()
                .HasKey(al => new { al.AutorId, al.LibroId });
            
        }
    }
}
