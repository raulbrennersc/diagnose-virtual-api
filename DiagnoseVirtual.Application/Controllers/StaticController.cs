using DiagnoseVirtual.Domain.Dtos;
using DiagnoseVirtual.Domain.Entities;
using DiagnoseVirtual.Infra.Data.Context;
using DiagnoseVirtual.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiagnoseVirtual.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StaticController : ControllerBase
    {
        private readonly BaseService<Estado> _estadoService;
        private readonly BaseService<Municipio> _municipioService;
        private readonly PsqlContext _context = new PsqlContext();

        public StaticController()
        {
            _estadoService = new BaseService<Estado>(_context);
            _municipioService = new BaseService<Municipio>(_context);
        }

        [HttpGet]
        [Route("Estado")]
        [ProducesResponseType(typeof(EstadoDto), StatusCodes.Status200OK)]
        public ActionResult GetEstados()
        {
            var result = _estadoService.GetAll()
                .Select(e => new EstadoDto
                {
                    Id = e.Id,
                    Nome = e.Nome,
                    Sigla = e.Sigla
                });

            return Ok(result);
        }

        [HttpGet]
        [Route("Estado/{idEstado}/Municipios")]
        [ProducesResponseType(typeof(List<EstadoDto>), StatusCodes.Status200OK)]
        public ActionResult GetMunicipios(int idEstado)
        {
            var result = _municipioService.GetAll().Where(m => m.Estado.Id == idEstado)
                .Select(m => new MunicipioDto
                {
                    Id = m.Id,
                    Nome = m.Nome,                   
                });

            return Ok(result);
        }

        [HttpGet]
        [Route("Culturas")]
        [ProducesResponseType(typeof(List<Cultura>), StatusCodes.Status200OK)]
        public ActionResult GetCulturas(int idEstado)
        {
            var result = new BaseService<Cultura>(_context).GetAll()
                .Select(m => new 
                {
                    m.Id,
                    m.Nome,
                });

            return Ok(result);
        }

        [HttpGet]
        [Route("EtapasFazenda")]
        [ProducesResponseType(typeof(List<EtapaFazenda>), StatusCodes.Status200OK)]
        public ActionResult GetEtapasFazenda()
        {
            var result = new BaseService<EtapaFazenda>(_context).GetAll()
                .Select(m => new
                {
                    m.Id,
                    m.Nome,
                });

            return Ok(result);
        }
    }
}
