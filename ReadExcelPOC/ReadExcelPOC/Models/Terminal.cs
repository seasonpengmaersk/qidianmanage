using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ReadExcelPOC.Models
{
    public class Terminal
    {
        [JsonIgnore]
        public Guid Id { get; set; }

        [JsonProperty("TerminalGEOID")]
        public string TerminalGEOID { get; set; }

        [JsonIgnore]
        [Column("Sub Area")]
        public string SubArea {get;set;}

        [JsonProperty("TerminalRKSTCode")]
        public string TerminalRKSTCode { get; set; }

        [JsonIgnore]
        public string PortRKST { get; set; }

        [JsonIgnore]
        public string PortGEOID  { get; set; }

        [JsonProperty("TerminalName")]
        public string TerminalName { get; set; }
        [JsonIgnore]
        public string PortName { get; set; }
        [JsonIgnore]
        public string ExtendField1 { get; set; }
        [JsonIgnore]
        public string ExtendField2 { get; set; }
        [JsonIgnore]
        public string ExtendField3 { get; set; }
        [JsonIgnore]
        public string ExtendField4 { get; set; }
        [JsonIgnore]
        public string ExtendField5 { get; set; }
        [JsonIgnore]
        public string ExtendField6 { get; set; }
        [JsonIgnore]
        public string ExtendField7 { get; set; }
        [JsonIgnore]
        public string ExtendField8 { get; set; }

    }
}
