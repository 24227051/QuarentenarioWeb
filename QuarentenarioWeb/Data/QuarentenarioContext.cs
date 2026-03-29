using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Data;

public partial class QuarentenarioContext : DbContext
{
    public QuarentenarioContext()
    {
    }

    public QuarentenarioContext(DbContextOptions<QuarentenarioContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Analise> Analises { get; set; }

    public virtual DbSet<AnaliseDetalhe> AnaliseDetalhes { get; set; }

    public virtual DbSet<Anexo> Anexos { get; set; }

    public virtual DbSet<AspNetRole> AspNetRoles { get; set; }

    public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetUser> AspNetUsers { get; set; }

    public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }

    public virtual DbSet<Importacao> Importacaos { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<Pai> Pais { get; set; }

    public virtual DbSet<Patogeno> Patogenos { get; set; }

    public virtual DbSet<TipoControle> TipoControles { get; set; }

    public virtual DbSet<TipoPatogeno> TipoPatogenos { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=ConnectionStrings:QuarentanarioDB");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Analise>(entity =>
        {
            entity.ToTable("Analise");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataInicio)
                .HasDefaultValueSql("(getdate())", "DF_Analise_DataInicio")
                .HasColumnType("datetime")
                .HasColumnName("dataInicio");
            entity.Property(e => e.DataTermino)
                .HasColumnType("datetime")
                .HasColumnName("dataTermino");
            entity.Property(e => e.Descricao)
                .IsUnicode(false)
                .HasColumnName("descricao");
            entity.Property(e => e.Finalizada).HasColumnName("finalizada");
            entity.Property(e => e.IdMaterial).HasColumnName("idMaterial");
            entity.Property(e => e.IdPais).HasColumnName("idPais");
            entity.Property(e => e.Positivo).HasColumnName("positivo");

            entity.HasOne(d => d.IdMaterialNavigation).WithMany(p => p.Analises)
                .HasForeignKey(d => d.IdMaterial)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Analise_Material");

            entity.HasOne(d => d.IdPaisNavigation).WithMany(p => p.Analises)
                .HasForeignKey(d => d.IdPais)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Analise_Pais");
        });

        modelBuilder.Entity<AnaliseDetalhe>(entity =>
        {
            entity.ToTable("AnaliseDetalhe");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.DataInicio)
                .HasDefaultValueSql("(getdate())", "DF_AnaliseDetalhe_DataInicio")
                .HasColumnType("datetime")
                .HasColumnName("dataInicio");
            entity.Property(e => e.DataTermino)
                .HasColumnType("datetime")
                .HasColumnName("dataTermino");
            entity.Property(e => e.Descricao)
                .IsUnicode(false)
                .HasColumnName("descricao");
            entity.Property(e => e.Finalizada).HasColumnName("finalizada");
            entity.Property(e => e.IdAnalise).HasColumnName("idAnalise");
            entity.Property(e => e.IdPatogeno).HasColumnName("idPatogeno");
            entity.Property(e => e.Positivo).HasColumnName("positivo");

            entity.HasOne(d => d.IdAnaliseNavigation).WithMany(p => p.AnaliseDetalhes)
                .HasForeignKey(d => d.IdAnalise)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AnaliseDetalhe_Analise");

            entity.HasOne(d => d.IdPatogenoNavigation).WithMany(p => p.AnaliseDetalhes)
                .HasForeignKey(d => d.IdPatogeno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AnaliseDetalhe_Patogeno");
        });

        modelBuilder.Entity<Anexo>(entity =>
        {
            entity.ToTable("Anexo");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdAnalise).HasColumnName("idAnalise");
            entity.Property(e => e.IdAnaliseDetalhe).HasColumnName("idAnaliseDetalhe");
            entity.Property(e => e.NomeArmazenado)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("nomeArmazenado");
            entity.Property(e => e.NomeArquivo)
                .HasMaxLength(500)
                .IsUnicode(false)
                .HasColumnName("nomeArquivo");
            entity.Property(e => e.TipoConteudo)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("tipoConteudo");

            entity.HasOne(d => d.IdAnaliseNavigation).WithMany(p => p.Anexos)
                .HasForeignKey(d => d.IdAnalise)
                .HasConstraintName("FK_Anexo_Analise");

            entity.HasOne(d => d.IdAnaliseDetalheNavigation).WithMany(p => p.Anexos)
                .HasForeignKey(d => d.IdAnaliseDetalhe)
                .HasConstraintName("FK_Anexo_AnaliseDetalhe");
        });

        modelBuilder.Entity<AspNetRole>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetRoleClaim>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetUser>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasMany(d => d.Roles).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRole",
                    r => r.HasOne<AspNetRole>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUser>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.ToTable("AspNetUserRoles");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<AspNetUserClaim>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.ProviderKey).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserToken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.Property(e => e.LoginProvider).HasMaxLength(128);
            entity.Property(e => e.Name).HasMaxLength(128);

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<Importacao>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Importacao");

            entity.Property(e => e.Material).HasMaxLength(50);
            entity.Property(e => e.Patogeno).HasMaxLength(100);
            entity.Property(e => e.TipoControle).HasMaxLength(50);
            entity.Property(e => e.TipoPatageno).HasMaxLength(50);
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.ToTable("Material");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nome");

            entity.HasMany(d => d.IdPatogenos).WithMany(p => p.IdMaterials)
                .UsingEntity<Dictionary<string, object>>(
                    "MaterialPatogeno",
                    r => r.HasOne<Patogeno>().WithMany()
                        .HasForeignKey("IdPatogeno")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_MaterialPatogeno_Patogeno"),
                    l => l.HasOne<Material>().WithMany()
                        .HasForeignKey("IdMaterial")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_MaterialPatogeno_Material"),
                    j =>
                    {
                        j.HasKey("IdMaterial", "IdPatogeno");
                        j.ToTable("MaterialPatogeno");
                        j.IndexerProperty<int>("IdMaterial").HasColumnName("idMaterial");
                        j.IndexerProperty<int>("IdPatogeno").HasColumnName("idPatogeno");
                    });
        });

        modelBuilder.Entity<Pai>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<Patogeno>(entity =>
        {
            entity.ToTable("Patogeno");

            entity.HasIndex(e => new { e.Nome, e.IdTipoPatogeno, e.IdTipoControle }, "UX_Patogeno_Nome_IdTipoPatogeno_IdTipoControle").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IdTipoControle).HasColumnName("idTipoControle");
            entity.Property(e => e.IdTipoPatogeno).HasColumnName("idTipoPatogeno");
            entity.Property(e => e.Nome)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("nome");

            entity.HasOne(d => d.IdTipoControleNavigation).WithMany(p => p.Patogenos)
                .HasForeignKey(d => d.IdTipoControle)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Patogeno_TipoControle");

            entity.HasOne(d => d.IdTipoPatogenoNavigation).WithMany(p => p.Patogenos)
                .HasForeignKey(d => d.IdTipoPatogeno)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Patogeno_TipoPatogeno");
        });

        modelBuilder.Entity<TipoControle>(entity =>
        {
            entity.ToTable("TipoControle");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<TipoPatogeno>(entity =>
        {
            entity.ToTable("TipoPatogeno");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Nome)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("nome");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
