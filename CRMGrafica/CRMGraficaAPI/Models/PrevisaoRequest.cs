using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMGraficaAPI.Models
{
    public class PrevisaoRequest
    {
        public List<double> Historico { get; set; }
        public string DataBase { get; set; }  // Ex: "2024-12"
        public int QuantidadeMeses { get; set; } = 6;
    }
}