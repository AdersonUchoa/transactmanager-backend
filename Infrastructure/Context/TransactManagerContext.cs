using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Context;

public partial class TransactManagerContext : DbContext
{
    public TransactManagerContext(DbContextOptions<TransactManagerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categorias { get; set; }

    public virtual DbSet<Pessoa> Pessoas { get; set; }

    public virtual DbSet<Transacao> Transacoes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categorias_pkey");

            entity.ToTable("categorias");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(400)
                .HasColumnName("descricao");
            entity.Property(e => e.Finalidade)
                .HasConversion(new EnumToStringConverter<CategoriaFinalidadeEnum>())
                .HasMaxLength(100)
                .HasColumnName("finalidade");
        });

        modelBuilder.Entity<Pessoa>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("pessoas_pkey");

            entity.ToTable("pessoas");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Idade).HasColumnName("idade");
            entity.Property(e => e.Nome)
                .HasMaxLength(200)
                .HasColumnName("nome");
        });

        modelBuilder.Entity<Transacao>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("transacoes_pkey");

            entity.ToTable("transacoes");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CategoriaId).HasColumnName("categoria_id");
            entity.Property(e => e.Descricao)
                .HasMaxLength(400)
                .HasColumnName("descricao");
            entity.Property(e => e.PessoaId).HasColumnName("pessoa_id");
            entity.Property(e => e.Tipo)
                .HasConversion(new EnumToStringConverter<TransacoesTipoEnum>())
                .HasMaxLength(100)
                .HasColumnName("tipo");
            entity.Property(e => e.Valor)
                .HasPrecision(12, 2)
                .HasColumnName("valor");

            entity.HasOne(d => d.Categoria).WithMany(p => p.Transacoes)
                .HasForeignKey(d => d.CategoriaId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("transacoes_categoria_id_fkey");

            entity.HasOne(d => d.Pessoa).WithMany(p => p.Transacoes)
                .HasForeignKey(d => d.PessoaId)
                .HasConstraintName("transacoes_pessoa_id_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
