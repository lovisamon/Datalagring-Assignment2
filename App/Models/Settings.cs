using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Collections.Generic;

namespace App.Models
{
    public class Settings
    {
        [JsonProperty(propertyName: "statuses")]
        public List<string> Statuses { get; set; }

        [JsonProperty(propertyName: "maxitemscount")]
        public int MaxItemsCount { get; set; }
    }
}
