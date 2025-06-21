using CRMGraficaAPI.Dtos;
using CRMGraficaAPI.Models;
using CRMGraficaAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace CRMGraficaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IAController : ControllerBase
    {
        private readonly IIAService _iaService;
        private readonly ILogger<IAController> _logger;

        public IAController(IIAService iaService, ILogger<IAController> logger)
        {
            _iaService = iaService;
            _logger = logger;
        }

        [HttpPost("PreverVendas")]
        public IActionResult PreverVendas([FromBody] PrevisaoRequest request)
        {
            if (request == null)
            {
                return BadRequest("Request inválido");
            }

            // Log
            var json = JsonSerializer.Serialize(request);
            Console.WriteLine($"Recebido JSON: {json}");

            var resultado = _iaService.Prever(request);
            return Ok(resultado);
        }
    }
}
