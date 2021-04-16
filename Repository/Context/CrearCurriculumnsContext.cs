namespace Repository
{
    using System;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata;
    using Models;

    public partial class CrearCurriculumnsContext : DbContext
    {
        public CrearCurriculumnsContext()
        {
        }

        public CrearCurriculumnsContext(DbContextOptions<CrearCurriculumnsContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Users> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.Iduser);

                entity.ToTable("usuarios");

                entity.Property(e => e.Iduser).HasColumnName("idusuario");

                entity.Property(e => e.Password).HasColumnName("contrasena");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("correo");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasColumnName("usuario");

                entity.Property(e => e.TokenConfirmation)
                    .IsRequired()
                    .HasColumnName("tokenconfirmacion");

                entity.Property(e => e.IsConfirmed)
                    .IsRequired()
                    .HasColumnName("esconfirmado");

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasColumnName("estado");

                entity.Property(e => e.LastLogin).HasColumnName("ultimologin");

                entity.Property(e => e.CreateDate)
                    .IsRequired()
                    .HasColumnName("fechacreacion");

                entity.Property(e => e.UpdateDate).HasColumnName("fechaactualizacion");

                entity.Property(e => e.TokenChangePassword).HasColumnName("tokencambiocontrasena");

                entity.Property(e => e.IsConfirmedChange).HasColumnName("esconfirmadocambio");

                entity.Property(e => e.TokenLogin).HasColumnName("tokensesion");

            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
