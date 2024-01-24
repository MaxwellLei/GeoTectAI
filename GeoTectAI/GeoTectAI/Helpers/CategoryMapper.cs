using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoTectAI.Models
{
    public class CategoryMapper
    {
        private static readonly Dictionary<int, string> CategoryMapping = new Dictionary<int, string>
        {
            { 0, "Archean Cratons (incl. Greenstone Belts)" },
            { 1, "Complex Volcanic Settings" },
            { 2, "Continental Flood Basalts" },
            { 3, "Convergent Margins" },
            { 4, "Intracontinental Volcanics" },
            { 5, "Ocean Islands" },
            { 6, "Rift Volcanics" },
            { 7, "Seamounts" }
        };

        public static string GetCategoryName(int categoryCode)
        {
            if (CategoryMapping.TryGetValue(categoryCode, out var categoryName))
            {
                return categoryName;
            }
            else
            {
                return "Unknown Category";
            }
        }
    }
}
