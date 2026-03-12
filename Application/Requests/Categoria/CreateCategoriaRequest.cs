using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Categoria
{
    public class CreateCategoriaRequest
    {
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        [StringLength(400, ErrorMessage = "A descrição deve conter no máximo 400 caracteres.")]
        public string Descricao { get; set; } = null!;

        [Required(ErrorMessage = "A finalidade é obrigatória.")]
        [EnumDataType(typeof(CategoriaFinalidadeEnum), ErrorMessage = "A finalidade informada é inválida.")]
        public CategoriaFinalidadeEnum Finalidade { get; set; }
    }
}
