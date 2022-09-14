using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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

        public System.Windows.Controls.UserControl XamlControl { get; set; }

    }
}
