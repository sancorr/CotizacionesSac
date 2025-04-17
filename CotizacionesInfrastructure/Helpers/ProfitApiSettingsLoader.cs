using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json.Linq;

namespace CotizacionesInfrastructure.Helpers
{
    public static class ProfitApiSettingsLoader
    {
        public static string LoadToken()
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            var json = File.ReadAllText(jsonPath);
            var jObject = JObject.Parse(json);
            return jObject["ProfitApi"]?["Token"]?.ToString();
        }
    }
}
