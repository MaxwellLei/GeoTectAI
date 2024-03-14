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
            { 1, "Continental Flood Basalts" },
            { 2, "Convergent Margins" },
            { 3, "Intracontinental Volcanics" },
            { 4, "Ocean Islands" },
            { 5, "Rift Volcanics" },
            { 6, "Seamounts" }
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
