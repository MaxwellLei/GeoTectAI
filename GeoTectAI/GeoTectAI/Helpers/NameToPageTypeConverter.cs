using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GeoTectAI.Helpers
{
    internal sealed class NameToPageTypeConverter
    {
        private static readonly Type[] PageTypes = Assembly
        .GetExecutingAssembly()
        .GetTypes()
        .Where(t => t.Namespace?.StartsWith("GeoTectAI.Views.Pages") ?? false)
        .ToArray();

        public static Type? Convert(string pageName)
        {
            pageName = pageName.Trim() + "Page";

            return PageTypes.FirstOrDefault(
                singlePageType => singlePageType.Name == pageName);
        }
    }
}
