using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Domain.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CategoriaFinalidadeEnum
    {
        [EnumMember(Value = "DESPESA")] Despesa,
        [EnumMember(Value = "RECEITA")] Receita,
        [EnumMember(Value = "AMBAS")] Ambas
    }
}
