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
        public ActionResult Get()
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
        public ActionResult Get(int idEstado)
        {
            var result = _municipioService.GetAll().Where(m => m.Estado.Id == idEstado)
                .Select(m => new MunicipioDto
                {
                    Id = m.Id,
                    Nome = m.Nome,                   
                });

            return Ok(result);
        }
    }
}
