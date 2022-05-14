using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiunskeBoardProject.classes
{
    public class ConnectorJSON
    {
        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("pins")]
        public int pins { get; set; }

        [JsonProperty("can-address")]
        public int CanAddress { get; set; }

        [JsonProperty("pins-parameters")]
        public List<PinsParameterJSON> PinsParameters { get; set; }
    }

    public class PinsParameterJSON
    {
        [JsonProperty("pin")]
        public int pin { get; set; }

        [JsonProperty("can-bit")]
        public int CanBit { get; set; }
    }

    public class RootJson
    {
        [JsonProperty("connectors")]
        public List<ConnectorJSON> connectors { get; set; }
    }
}
