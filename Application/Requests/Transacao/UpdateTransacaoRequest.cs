using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Transacao
{
    public class UpdateTransacaoRequest
    {
        [StringLength(400, ErrorMessage = "A descrição deve conter no máximo 400 caracteres.")]
        public string? Descricao { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O valor deve ser maior que 0.")]
        public decimal? Valor { get; set; }

        [EnumDataType(typeof(TransacoesTipoEnum), ErrorMessage = "O tipo informado é inválido.")]
        public TransacoesTipoEnum? Tipo { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O Id da categoria deve ser maior que 0.")]
        public int? CategoriaId { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "O Id da pessoa deve ser maior que 0.")]
        public int? PessoaId { get; set; }
    }
}
