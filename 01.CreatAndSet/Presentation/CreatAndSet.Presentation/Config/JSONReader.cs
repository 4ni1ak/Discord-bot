using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Config
{
    public class JSONReader
    {
        public string Token { get; set; }
        public string Prefix { get; set; }
        public async Task ReadJSON()
        {
            using (StreamReader reader = new StreamReader("config.json"))
            {
                string json = await reader.ReadToEndAsync();
                JsonStructure data = JsonConvert.DeserializeObject<JsonStructure>(json);
                Token = data.Token;
                Prefix = data.Prefix;
            }
        }
    }
    internal sealed class JsonStructure
    {
        public string Token { get; set; }
        public string Prefix { get; set; }


    }
}
