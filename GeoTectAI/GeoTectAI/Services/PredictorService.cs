using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace GeoTectAI.Services
{
    class PredictorService
    {
        private readonly string _exePath;

        public PredictorService(string exePath)
        {
            _exePath = exePath;
        }

        public async Task<(int, float[])> PredictAsync(string modelPath, float[] features)
        {

            var startInfo = new ProcessStartInfo
            {
                FileName = _exePath,
                Arguments = $"\"{modelPath}\" \"{JsonSerializer.Serialize(features)}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            using (var process = Process.Start(startInfo))
            {
                var commandLine = $"{_exePath} {startInfo.Arguments}";
                Console.WriteLine($"Command line: {commandLine}");

                using (var reader = process.StandardOutput)
                {
                    var result = await reader.ReadToEndAsync();
                    process.WaitForExit();

                    var predictionResult = JsonSerializer.Deserialize<Dictionary<string, object>>(result);
                    var predictedClassJsonElement = (JsonElement)predictionResult["class"];
                    var predictedClass = predictedClassJsonElement.GetInt32();

                    var probabilitiesJsonElement = (JsonElement)predictionResult["probabilities"];
                    var probabilities = probabilitiesJsonElement.EnumerateArray().Select(p => p.GetSingle()).ToArray();

                    return (predictedClass, probabilities);
                }
            }
        }
    }
}
