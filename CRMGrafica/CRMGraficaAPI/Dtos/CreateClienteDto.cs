using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CRMGraficaAPI.Models;

namespace CRMGraficaAPI.Dtos
{
    public class CreateClienteDto
    {
        [Required(ErrorMessage = "O Nome é obrigatório.")]
        public string Nome { get; set; } // Nome do cliente

        [Required(ErrorMessage = "O Contato é obrigatório.")]
        public string Contato { get; set; } // Pode ser Telefone, Email, etc.
        public int FrequenciaCompra { get; set; } = 0; // Inicializa com 0, será atualizado posteriormente
        public TipoCliente Tipo { get; set; } = TipoCliente.Novo; // Inicializa como Novo, será atualizado posteriormente
    }
}