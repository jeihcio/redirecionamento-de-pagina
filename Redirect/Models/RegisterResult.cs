using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Redirect.Models
{
    public class RegisterResult
    {
        public String Url { get; set; }

        [JsonProperty(PropertyName = "Quantidade de acessos únicos")]
        public int QuantitySingleAccesses { get; set; }

        [JsonProperty(PropertyName = "Quantidade de acessos no geral")]
        public int QuantityAccessesGeneral { get; set; }
    }
}