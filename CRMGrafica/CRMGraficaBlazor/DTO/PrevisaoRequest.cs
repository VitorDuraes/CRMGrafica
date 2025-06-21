using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CRMGraficaBlazor.DTO
{
    public class PrevisaoRequest
    {
        public List<double> Historico { get; set; }
        public string DataBase { get; set; }
        public int QuantidadeMeses { get; set; }
    }
}