using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CRMGraficaAPI.Dtos
{
    public class CreatePedidoDto
    {
        [Required(ErrorMessage = "O serviço solicitado é obrigatório.")]
        public string ServicoSolicitado { get; set; }

        public string? Descricao { get; set; } // Descrição é opcional

        [Range(0.01, double.MaxValue, ErrorMessage = "O valor do pedido deve ser positivo.")]
        public decimal Valor { get; set; }

        [Required(ErrorMessage = "O ID do cliente é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID do cliente inválido.")]
        public int ClienteId { get; set; }
    }
}