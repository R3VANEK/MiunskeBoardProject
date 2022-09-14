using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace MiunskeBoardProject.classes
{
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

                if (connector.PinsParameters == null || connector.PinsParameters.Count == 0)
                    throw new JsonSerializationException("źle sformatowana właściwość 'pins-parameters' w jednym z konektorów | plik : " + this.fileName);

                foreach(PinsParameter pin in connector.PinsParameters)
                {
                    int try_parse_output = -1;
                    int.TryParse(pin.CanBits, out try_parse_output);

                    
                    if (pin.CanAddress == null || pin.CanAddress <= 0)
                        throw new JsonSerializationException("źle sformatowana właściwość 'can-address' w jednym z pinów " + connector.Name +" | plik : " + this.fileName);

                    if (!(pin.Type == "value" || pin.Type == "boolean"))
                        throw new JsonSerializationException("niedozwolona wartość 'type' w jednym z pinów (poprawne: 'value', 'boolean') " + connector.Name + " | plik : " + this.fileName);

                    if (pin.Pin == null || pin.Pin <= 0)
                        throw new JsonSerializationException("niedozwolona wartość 'pin' w jednym z pinów (wartość mniejsza lub równa 0 lub większa niż piny w konektorze) " + connector.Name + " | plik : " + this.fileName);

                    if (pin.CanBits == null || pin.CanBits == "" || !Regex.IsMatch(pin.CanBits, @"(^[0-9]{1,2}$)|(^[0-9]{1,2}[-][0-9]{1,2}$)"))
                        throw new JsonSerializationException("źle sformatowana właściwość 'can-bits' w jednym z pinów  " + connector.Name + " | plik : " + this.fileName);

                    if (pin.Type == "boolean" && pin.CanBits.Contains("-"))
                        throw new JsonSerializationException("Pin logiczny w " + connector.Name + " zawiera w konfiguracji wartość " + pin.CanBits + " proszę podać jednocyfrową wartość value" + " | plik : " + this.fileName);
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
