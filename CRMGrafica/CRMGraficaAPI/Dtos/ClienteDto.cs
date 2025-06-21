using System;
using CRMGraficaAPI.Models;

namespace CRMGraficaAPI.Dtos
{
    public class ClienteDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Contato { get; set; }
        public TipoCliente Tipo { get; set; }
        public int FrequenciaCompra { get; set; }
    }
}

