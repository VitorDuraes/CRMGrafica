using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace CRMGraficaAPI.Models
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
        public string Nome { get; set; }
        public string Contato { get; set; } // Pode ser Telefone, Email, etc.
        public TipoCliente Tipo { get; set; } // Segmentação automática
        public int FrequenciaCompra { get; set; } // Número de compras, por exemplo
        // Poderia adicionar DataCadastro, DataUltimaCompra para ajudar na segmentação

        [JsonIgnore]
        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
    }
}