using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReadExcelPOC.Models
{
    public class City
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonIgnore]
        public string CountryName { get; set; }

        [MaxLength(255)]
        public string GEOID { get; set; }

        [JsonIgnore]
        public string RKSTCode { get; set; }

        [JsonIgnore]
        public string CityName { get; set; }

        [MaxLength(255)]
        public string StandardName { get; set; }

        [MaxLength(255)]
        public string Alias { get; set; }

        [JsonIgnore]
        public string Status { get; set; }
    }
}
