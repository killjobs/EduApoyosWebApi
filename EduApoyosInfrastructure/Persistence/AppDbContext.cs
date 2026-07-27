using EduApoyosDomain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduApoyosInfrastructure.Persistence
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }
        public DbSet<UsuarioToken> UsuarioTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.ToTable("Usuarios");

                entity.HasKey(e => e.Id);
                entity.Property(e => e.NombreCompleto)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(e => e.CorreoElectronico)
                    .IsRequired()
                    .HasMaxLength(150);
                entity.HasIndex(e => e.CorreoElectronico)
                    .IsUnique();
                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(500);
                entity.Property(e => e.Rol)
                    .IsRequired();
                entity.Property(e => e.FechaRegistro)
                    .HasColumnType("datetime2")
                    .IsRequired();
            });
            modelBuilder.Entity<UsuarioToken>(entity =>
            {
                entity.ToTable("UsuariosToken");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.JwtId)
                      .IsRequired()
                      .HasMaxLength(200);
                entity.Property(x => x.Activo)
                      .IsRequired();
                entity.Property(e => e.FechaCreacion)
                    .HasColumnType("datetime2");
                entity.Property(e => e.FechaExpiracion)
                    .HasColumnType("datetime2");
                entity.HasOne(x => x.Usuario)
                      .WithMany(x => x.Tokens)
                      .HasForeignKey(x => x.UsuarioId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Estudiante>(entity =>
            {
                entity.ToTable("Estudiantes");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NumeroDocumento)
                    .IsRequired()
                    .HasMaxLength(20);
                entity.Property(e => e.TipoDocumento)
                    .IsRequired()
                    .HasMaxLength(20);
                entity.Property(e => e.ProgramaAcademico)
                    .IsRequired()
                    .HasMaxLength(200);
                entity.Property(e => e.Semestre)
                    .IsRequired();
                entity.HasOne(e => e.Usuario)
                    .WithOne(u => u.Estudiante)
                    .HasForeignKey<Estudiante>(e => e.UsuarioId);
                entity.HasIndex(e => e.UsuarioId)
                    .IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
