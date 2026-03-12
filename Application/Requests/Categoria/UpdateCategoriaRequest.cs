using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.Requests.Categoria
{
    public class UpdateCategoriaRequest
    {
        [StringLength(400, ErrorMessage = "A descrição deve conter no máximo 400 caracteres.")]
        public string? Descricao { get; set; }

        [EnumDataType(typeof(TransacoesTipoEnum), ErrorMessage = "A finalidade informada é inválida.")]
        public string? Finalidade { get; set; }
    }
}
