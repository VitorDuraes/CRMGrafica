using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRMGraficaAPI.Dtos;
using CRMGraficaAPI.Models;

namespace CRMGraficaAPI.Services
{
    public interface IIAService
    {
        List<PrevisaoVendaDto> Prever(PrevisaoRequest request);
    }
}