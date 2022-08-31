using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MiunskeBoardProject.classes
{

    public class Root
    {
        [JsonProperty("connectors")]
        public List<Connector> Connectors { get; set; }
    }

    public class Connector
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("pins")]
        public int Pins { get; set; }

        [JsonProperty("can-addresses")]
        public List<int> CanAddresses { get; set; }

        [JsonProperty("pins-parameters")]
        public List<PinsParameter> PinsParameters { get; set; }

        public List<System.Windows.Controls.UserControl> PinsXAMLObjects { get; set; }

    }

    public class PinsParameter
    {
        [JsonProperty("pin")]
        public int Pin { get; set; }

        [JsonProperty("can-address")]
        public int CanAddress { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("can-bits")]
        public string CanBits { get; set; }

    }



    class JsonParser
    {
        private string fileName { get; }

        private Root _configInfo { get; set; }




        public JsonParser(string fileName)
        {
            this.fileName = fileName;

           
            string raw = File.ReadAllText("../../../connector-configurations/" + fileName);
            Root config = JsonConvert.DeserializeObject<Root>(raw);

            if (config.Connectors == null)
                throw new JsonSerializationException("źle sformatowana właściwość 'connectors' | plik : " + this.fileName);


            foreach (Connector connector in config.Connectors)
            {
                if (connector.Name == null || connector.Name == "")
                    throw new JsonSerializationException("źle sformatowana właściwość 'name' w jednym z konektorów | plik : " + this.fileName);

                if (connector.CanAddresses.Count == null || connector.CanAddresses.Count == 0)
                    throw new JsonSerializationException("źle sformatowana właściwość 'can-addresses' w jednym z konektorów | plik : " + this.fileName);

                if (connector.Pins == null || connector.Pins <= 0)
                    throw new JsonSerializationException("źle sformatowana właściwość 'pins' w jednym z konektorów | plik : " + this.fileName);

                if (connector.PinsParameters == null || connector.PinsParameters.Count == 0)
                    throw new JsonSerializationException("źle sformatowana właściwość 'pins-parameters' w jednym z konektorów | plik : " + this.fileName);

                foreach(PinsParameter pin in connector.PinsParameters)
                {
                    int try_parse_output = -1;
                    int.TryParse(pin.CanBits, out try_parse_output);

                    //sprawdzanie typów?
                    if (pin.CanAddress == null || pin.CanAddress <= 0)
                        throw new JsonSerializationException("źle sformatowana właściwość 'can-address' w jednym z pinów " + connector.Name +" | plik : " + this.fileName);

                    if (!(pin.Type == "value" || pin.Type == "boolean"))
                        throw new JsonSerializationException("niedozwolona wartość 'type' w jednym z pinów (poprawne: 'value', 'boolean') " + connector.Name + " | plik : " + this.fileName);

                    if (pin.Pin == null || pin.Pin <= 0 || pin.Pin > connector.Pins)
                        throw new JsonSerializationException("niedozwolona wartość 'pin' w jednym z pinów (wartość mniejsza lub równa 0 lub większa niż piny w konektorze) " + connector.Name + " | plik : " + this.fileName);

                    if (pin.CanBits == null || pin.CanBits == "" || !Regex.IsMatch(pin.CanBits, @"(^[0-9]{1,2}$)|(^[0-9]{1,2}[-][0-9]{1,2}$)"))
                        throw new JsonSerializationException("źle sformatowana właściwość 'can-bits' w jednym z pinów  " + connector.Name + " | plik : " + this.fileName);
                }
            }

            this._configInfo = config;
            
            
        }


        public Root getConfigInfo()
        {
            return this._configInfo;
        }
    }



    


}
