using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;


namespace CRMGraficaAPI.Models
{
    public enum StatusPedido
    {
        Orcamento,
        Aprovado,
        Producao,
        Finalizado,
        Entregue,
        Cancelado // Adicionado para cobrir mais casos
    }

    public class Pedido
    {
        public int Id { get; set; }

        [Required]
        public string ServicoSolicitado { get; set; } // Ex: Banner, Cartão, Adesivo

        public string Descricao { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Valor { get; set; }

        public StatusPedido Status { get; set; }

        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataAtualizacao { get; set; }

        public int ClienteId { get; set; }
        [JsonIgnore]
        public virtual Cliente Cliente { get; set; }
    }
}