using System;

namespace CRMGraficaBlazor.Models
{
    public enum TipoCliente
    {
        Novo,
        Recorrente,
        Inativo
    }

    public class Cliente
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Contato { get; set; } = string.Empty;
        public TipoCliente Tipo { get; set; }
        public int FrequenciaCompra { get; set; }
    }

    public enum StatusPedido
    {
        Orcamento,
        Aprovado,
        Producao,
        Finalizado,
        Entregue,
        Cancelado
    }

    public class Pedido
    {
        public int Id { get; set; }
        public string ServicoSolicitado { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public StatusPedido Status { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public int ClienteId { get; set; }
        public string NomeCliente { get; set; } = string.Empty;
    }

    public class CreateClienteDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Contato { get; set; } = string.Empty;
        public int FrequenciaCompra { get; set; } = 0;
        public TipoCliente Tipo { get; set; } = TipoCliente.Novo;
    }

    public class CreatePedidoDto
    {
        public string ServicoSolicitado { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public decimal Valor { get; set; }
        public int ClienteId { get; set; }
    }
}

