using System.Globalization;
using System.Reflection;
using System.Runtime.Serialization;

namespace Domain.Extensions
{
    public static class EnumExtensions
    {
        public static string Value(this Enum e)
        {
            var enumMember =
                e.GetType()
                 .GetMember(e.ToString())[0]
                 .GetCustomAttribute<EnumMemberAttribute>()?
                 .Value;

            if (string.IsNullOrWhiteSpace(enumMember))
                return e.ToString();

            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(enumMember.ToLower());
        }
    }
}
