using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Transacao
{
    public class CreateTransacaoRequest
    {
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(400, ErrorMessage = "A descrição deve conter no máximo 400 caracteres.")]
        public string Descricao { get; set; } = null!;

        [Required(ErrorMessage = "O valor é obrigatório.")]
        [Range(0, double.MaxValue, ErrorMessage = "O valor deve ser maior que 0.")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O tipo é obrigatório.")]
        [EnumDataType(typeof(TransacoesTipoEnum), ErrorMessage = "O tipo informado é inválido.")]
        public string Tipo { get; set; } = null!;

        [Required(ErrorMessage = "A categoria é obrigatória.")]
        [Range(0, double.MaxValue, ErrorMessage = "O Id da categoria deve ser maior que 0.")]
        public int CategoriaId { get; set; }

        [Required(ErrorMessage = "A pessoa é obrigatória.")]
        [Range(0, double.MaxValue, ErrorMessage = "O Id da pessoa deve ser maior que 0.")]
        public int PessoaId { get; set; }
    }
}
