﻿using DiagnoseVirtual.Application.Helpers;
using DiagnoseVirtual.Domain.Dtos;
using DiagnoseVirtual.Domain.Entities;
using DiagnoseVirtual.Infra.Data.Context;
using DiagnoseVirtual.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace DiagnoseVirtual.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FazendasController : ControllerBase
    {
        private readonly BaseService<Fazenda> _fazendaService;
        private readonly UsuarioService _usuarioService;
        private readonly BaseService<LocalizacaoFazenda> _localizacaoService;
        private readonly BaseService<DadosFazenda> _dadosFazendaService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly PsqlContext _context = new PsqlContext();

        public FazendasController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _fazendaService = new BaseService<Fazenda>(_context);
            _usuarioService = new UsuarioService(_context);
            _localizacaoService = new BaseService<LocalizacaoFazenda>(_context);
            _dadosFazendaService = new BaseService<DadosFazenda>(_context);
        }

        [HttpPost]
        [Route("UploadGeometrias")]
        [ProducesResponseType(typeof(List<GeometriaDto>), StatusCodes.Status200OK)]
        public ActionResult ValidarLocalizacao(IFormFile file)
        {
            var path = _hostingEnvironment.ContentRootPath;
            try
            {
                var result = GeoFileHelper.ReadFile(file, path).Select(g => new GeometriaDto(g));
                return Ok(result);
            }
            catch
            {
                return BadRequest("Arquivo inválido");
            }
        }

        [HttpPost]
        [Route("ConclusaoFazenda/{idFazenda}")]
        public ActionResult Concluir(int idFazenda)
        {
            var fazenda = _fazendaService.Get(idFazenda);
            if (fazenda == null)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            if (fazenda.Concluida)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            fazenda.Concluida = true;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _fazendaService.Put(fazenda);
                    transaction.Commit();
                    return Ok();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
                }
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<FazendaDto>), StatusCodes.Status200OK)]
        public ActionResult Get()
        {
            var idUsuario = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var fazendas = _fazendaService.GetAll().Where(f => f.Usuario.Id == int.Parse(idUsuario)).ToList();

            var result = fazendas.Select(f => new FazendaDto(f));

            return Ok(result);
        }

        [HttpGet("{idFazenda}")]
        [ProducesResponseType(typeof(FazendaDto), StatusCodes.Status200OK)]
        public ActionResult Get(int idFazenda)
        {
            var fazenda = _fazendaService.Get(idFazenda);
            if (fazenda == null)
            {
                return NotFound(Constants.ERR_FAZENDA_NAO_ENCONTRADA);
            }

            return Ok(new FazendaDto(fazenda));
        }

        [HttpGet]
        [Route("LocalizacaoFazenda/{idFazenda}")]
        [ProducesResponseType(typeof(LocalizacaoFazendaDto), StatusCodes.Status200OK)]
        public ActionResult GetLocalizacaoFazenda(int idFazenda)
        {
            var fazenda = _fazendaService.Get(idFazenda);
            if (fazenda == null || fazenda.LocalizacaoFazenda == null)
            {
                return NotFound(Constants.ERR_LOCALIZACAO_FAZENDA_NAO_ENCONTRADA);
            }

            return Ok(new LocalizacaoFazendaDto(fazenda.LocalizacaoFazenda));
        }

        [HttpGet]
        [Route("DadosFazenda/{idFazenda}")]
        [ProducesResponseType(typeof(DadosFazendaDto), StatusCodes.Status200OK)]
        public ActionResult GetDadosFazenda(int idFazenda)
        {
            var fazenda = _fazendaService.Get(idFazenda);
            if (fazenda == null || fazenda.DadosFazenda == null)
            {
                return NotFound(Constants.ERR_DADOS_FAZENDA_NAO_ENCONTRADOS);
            }

            return Ok(new DadosFazendaDto(fazenda.DadosFazenda));
        }

        [HttpGet]
        [Route("LocalizacaoGeoFazenda/{idFazenda}")]
        [ProducesResponseType(typeof(DemarcacaoDto), StatusCodes.Status200OK)]
        public ActionResult GetLocalizacaoGeoFazenda(int idFazenda)
        {
            var fazenda = _fazendaService.Get(idFazenda);
            if (fazenda == null || fazenda.Demarcacao == null)
            {
                return NotFound(Constants.ERR_DEMARCACAO_FAZENDA_NAO_ENCONTRADA);
            }

            var demarcacao = new DemarcacaoDto();
            demarcacao.Geometrias = new List<GeometriaDto>();
            demarcacao.Geometrias.Add(new GeometriaDto(fazenda.Demarcacao));

            return Ok(demarcacao);
        }

        [HttpGet]
        [Route("LavourasFazenda/{idFazenda}")]
        [ProducesResponseType(typeof(List<LavouraDto>), StatusCodes.Status200OK)]
        public ActionResult GetLavourasFazenda(int idFazenda)
        {
            var fazenda = _fazendaService.Get(idFazenda);
            if (fazenda == null)
            {
                return NotFound(Constants.ERR_LAVOURAS_FAZENDA_NAO_ENCONTRADA);
            }

            return Ok(fazenda.Lavouras.Select(l => new LavouraDto(l)));
        }

        [HttpGet]
        [Route("MonitoramentosFazenda/{idFazenda}")]
        [ProducesResponseType(typeof(List<MonitoramentoDetailDto>), StatusCodes.Status200OK)]
        public ActionResult GetMonitoramentosFazenda(int idFazenda)
        {
            var fazenda = _fazendaService.Get(idFazenda);
            if (fazenda == null)
            {
                return NotFound(Constants.ERR_LAVOURAS_FAZENDA_NAO_ENCONTRADA);
            }

            return Ok(fazenda.Monitoramentos.Select(m => new MonitoramentoDetailDto(m)));
        }

        [HttpPost]
        [Route("LocalizacaoFazenda")]
        [ProducesResponseType(typeof(FazendaDto), StatusCodes.Status200OK)]
        public ActionResult PostLocalizacaoFazenda(LocalizacaoFazendaDto localizacao)
        {
            if (localizacao == null)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            var idUsuario = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var usuario = _usuarioService.Get(int.Parse(idUsuario));

            var fazendaBd = new Fazenda
            {
                Usuario = usuario,
                Ativa = true,
            };

            var municipio = new BaseService<Municipio>(_context).Get(localizacao.IdMunicipio);

            var localizacaoBd = new LocalizacaoFazenda
            {
                Email = localizacao.Email,
                Telefone = localizacao.Telefone,
                Gerente = localizacao.Gerente,
                Nome = localizacao.Nome,
                Proprietario = localizacao.Proprietario,
                Municipio = municipio,
                PontoReferencia = localizacao.PontoReferencia,
                Fazenda = fazendaBd
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _fazendaService.Post(fazendaBd);
                    _localizacaoService.Post(localizacaoBd);
                    transaction.Commit();
                    return Ok(new FazendaDto { Id = fazendaBd.Id });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
                }
            }
        }

        [HttpPost]
        [Route("DadosFazenda/{idFazenda}")]
        public ActionResult PostDadosFazenda(DadosFazendaDto dadosFazenda, int idFazenda)
        {
            var fazendaBd = _fazendaService.Get(idFazenda);
            if (dadosFazenda == null || fazendaBd == null || fazendaBd.Concluida)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            var dadosFazendaBd = new DadosFazenda
            {
                AreaTotal = dadosFazenda.AreaTotal,
                Cultura = dadosFazenda.Cultura,
                QuantidadeLavouras = dadosFazenda.QuantidadeLavouras,
                Fazenda = fazendaBd
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _dadosFazendaService.Post(dadosFazendaBd);
                    transaction.Commit();
                    return Ok();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
                }
            }
        }

        [HttpPost]
        [Route("LocalizacaoGeoFazenda/{idFazenda}")]
        public ActionResult PostLocalizacaoGeoFazenda(DemarcacaoDto demarcacaoDto, int idFazenda)
        {
            var fazendaBd = _fazendaService.Get(idFazenda);
            if (demarcacaoDto == null || demarcacaoDto.Geometrias == null || !demarcacaoDto.Geometrias.Any() || fazendaBd == null || fazendaBd.Demarcacao != null || fazendaBd.Concluida)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            var factory = Geometry.DefaultFactory;
            var polygons = demarcacaoDto.Geometrias
                .Select(g => factory.CreatePolygon(g.Coordinates.Select(c => new Coordinate(c[0], c[1])).ToArray())).ToList();
            var geometrias = polygons.Select(p => factory.CreateGeometry(p));

            fazendaBd.Demarcacao = geometrias.FirstOrDefault();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _fazendaService.Put(fazendaBd);
                    transaction.Commit();
                    return Ok();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
                }
            }
        }

        [HttpPut]
        [Route("LocalizacaoFazenda/idFazenda")]
        public ActionResult PutLocalizacaoFazenda(LocalizacaoFazendaDto localizacao, int idFazenda)
        {
            var fazendaBd = _fazendaService.Get(idFazenda);
            if (localizacao == null || fazendaBd == null || !fazendaBd.Concluida)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            var localizacaoBd = fazendaBd.LocalizacaoFazenda;

            localizacaoBd.Telefone = localizacao.Telefone;
            localizacaoBd.Email = localizacao.Email;
            localizacaoBd.Gerente = localizacao.Gerente;
            localizacaoBd.Nome = localizacao.Nome;
            localizacaoBd.Proprietario = localizacao.Proprietario;
            localizacaoBd.Municipio = new Municipio { Id = localizacao.IdMunicipio };
            localizacaoBd.PontoReferencia = localizacao.PontoReferencia;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _localizacaoService.Put(localizacaoBd);
                    transaction.Commit();
                    return Ok();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
                }
            }
        }

        [HttpPut]
        [Route("DadosFazenda/{idFazenda}")]
        public ActionResult PutDadosFazenda(DadosFazendaDto dadosFazenda, int idFazenda)
        {
            var fazendaBd = _fazendaService.Get(idFazenda);
            if (dadosFazenda == null || fazendaBd == null || !fazendaBd.Concluida)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            var dadosFazendaBd = fazendaBd.DadosFazenda;

            dadosFazendaBd.AreaTotal = dadosFazenda.AreaTotal;
            dadosFazendaBd.Cultura = dadosFazenda.Cultura;
            dadosFazendaBd.QuantidadeLavouras = dadosFazenda.QuantidadeLavouras;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _dadosFazendaService.Put(dadosFazendaBd);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
                }
            }
        }

        [HttpPut]
        [Route("LocalizacaoGeoFazenda/{idFazenda}")]
        public ActionResult PutLocalizacaoGeoFazenda(DemarcacaoDto demarcacaoDto, int idFazenda)
        {
            var fazendaBd = _fazendaService.Get(idFazenda);
            if (demarcacaoDto == null || demarcacaoDto.Geometrias == null || !demarcacaoDto.Geometrias.Any() || fazendaBd == null || !fazendaBd.Concluida)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            var factory = Geometry.DefaultFactory;
            var polygons = demarcacaoDto.Geometrias
                .Select(g => factory.CreatePolygon(g.Coordinates.Select(c => new Coordinate(c[0], c[1])).ToArray())).ToList();
            var geometrias = polygons.Select(p => factory.CreateGeometry(p));

            fazendaBd.Demarcacao = geometrias.FirstOrDefault();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _fazendaService.Put(fazendaBd);
                    return Ok();
                }
                catch (Exception ex)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
                }
            }
        }
    }
}
