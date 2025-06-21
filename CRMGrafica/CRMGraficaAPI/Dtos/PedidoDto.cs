using System;
using CRMGraficaAPI.Models;

namespace CRMGraficaAPI.Dtos
{
    public class PedidoDto
    {
        public int Id { get; set; }
        public string ServicoSolicitado { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public StatusPedido Status { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public int ClienteId { get; set; }
        public string NomeCliente { get; set; }
    }
}

