using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace Domain.Entities;

public partial class Transacao
{
    public int Id { get; set; }
    public string Descricao { get; set; } = null!;
    public decimal Valor { get; set; }
    public TransacoesTipoEnum Tipo { get; set; }
    public int CategoriaId { get; set; }
    public int PessoaId { get; set; }
    public virtual Categoria Categoria { get; set; } = null!;
    public virtual Pessoa Pessoa { get; set; } = null!;

    public Transacao() { }

    public Transacao(string descricao, decimal valor, TransacoesTipoEnum tipo, int categoriaId, int pessoaId)
    {
        Descricao = descricao;
        Valor = valor;
        Tipo = tipo;
        CategoriaId = categoriaId;
        PessoaId = pessoaId;
    }

    public void Update(string? newDescricao, decimal? newValor, TransacoesTipoEnum? newTipo, int? newCategoriaId, int? newPessoaId)
    {
        Descricao = newDescricao ?? Descricao;
        Valor = newValor ?? Valor;
        Tipo = newTipo ?? Tipo;
        CategoriaId = newCategoriaId ?? CategoriaId;
        PessoaId = newPessoaId ?? PessoaId;
    }

    public static string? ValidateCreation(Pessoa pessoa, Categoria categoria, TransacoesTipoEnum tipo)
    {
        if(pessoa.Idade <18 && tipo != TransacoesTipoEnum.Despesa)
            return "Pessoas menores de 18 anos só podem criar transações do tipo despesa";

        var categoriaIncompativel = (categoria.Finalidade == CategoriaFinalidadeEnum.Despesa && tipo != TransacoesTipoEnum.Despesa)
            || (categoria.Finalidade == CategoriaFinalidadeEnum.Receita && tipo != TransacoesTipoEnum.Receita);

        if (categoriaIncompativel)
            return "A categoria informada é incompatível com o tipo da transação";

        return null;
    }
}
