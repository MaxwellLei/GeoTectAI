using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoTectAI.Helpers
{
    class DataPreprocessor
    {
        // 定义均值和标准差
        private double[] means = { 52.8939759464757, 1.4765308066867124, 15.388898103041601, 8.578292278615434, 5.794326090623952, 0.15310470466417467, 1.4972500801388087, 3.0909884222401764, 0.30986517367693206, 25.2698946892969, 51.0867560102278, 6.190920265210042, 25.149146019635392, 5.338070620025492, 1.598690321457403, 5.063783653154329, 0.7726635621953953, 4.430067952627033, 0.8671172087951509, 2.377704569641364, 0.3417672429293879, 2.1541685899567002, 0.3235257288287453 };
        private double[] stdDevs = { 7.430855244586278, 1.0194632237561156, 2.142326892561034, 2.9265864839758167, 3.1211224786620124, 0.04867273258962311, 1.1229773987514735, 0.9369400030426253, 0.22386642478833008, 19.107908665653202, 36.418192065543685, 4.063222410621932, 15.181793761308583, 2.6364086821045167, 0.7929715365111596, 2.1897101156618417, 0.2965782029559937, 1.579363429982624, 0.29523882483198133, 0.7808299471049734, 0.11328000394603001, 0.7059487214372515, 0.10838217912716112 };

        public List<float[]> NormalizeData(List<float[]> rawData)
        {
            List<float[]> normalizedData = new List<float[]>();

            foreach (float[] row in rawData)
            {
                float[] normalizedRow = new float[row.Length];
                for (int i = 0; i < row.Length; i++)
                {
                    // 标准化处理：(值 - 均值) / 标准差
                    normalizedRow[i] = (float)((row[i] - means[i]) / stdDevs[i]);
                }
                normalizedData.Add(normalizedRow);
            }

            return normalizedData;
        }

        public float[] NormalizeFeatures(float[] features)
        {
            float[] normalizedFeatures = new float[features.Length];
            for (int i = 0; i < features.Length; i++)
            {
                normalizedFeatures[i] = (float)((features[i] - means[i]) / stdDevs[i]);
            }
            return normalizedFeatures;
        }
    }
}
