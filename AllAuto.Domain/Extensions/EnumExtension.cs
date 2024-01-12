using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace AllAuto.Domain.Extensions
{
    public static class EnumExtension
    {
        public static string GetDisplayName(this System.Enum enumValue)
        {
            try
            {
                string result = enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                ?.GetName() ?? "Неопределен";

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine (ex.ToString ());

                return "НУЛЛ";
            }
            
        }
    }
}
