﻿using DiagnoseVirtual.Domain.Dtos;
using DiagnoseVirtual.Domain.Entities;
using DiagnoseVirtual.Infra.Data.Context;
using DiagnoseVirtual.Service.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DiagnoseVirtual.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LavourasController : ControllerBase
    {
        private readonly BaseService<Lavoura> _lavouraService;
        private readonly BaseService<DadosLavoura> _dadosLavouraService;
        private readonly BaseService<Fazenda> _fazendaService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly PsqlContext _context = new PsqlContext();
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public LavourasController(IWebHostEnvironment hostingEnvironment, IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _hostingEnvironment = hostingEnvironment;
            _lavouraService = new BaseService<Lavoura>(_context);
            _dadosLavouraService = new BaseService<DadosLavoura>(_context);
            _fazendaService = new BaseService<Fazenda>(_context);
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        [HttpPost]
        [Route("ConclusaoLavoura/{idLavoura}")]
        public ActionResult PostConcluirLavoura(int idLavoura)
        {
            var lavouraBd = _lavouraService.Get(idLavoura);
            if (lavouraBd == null || lavouraBd.Concluida)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            lavouraBd.Concluida = true;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _lavouraService.Put(lavouraBd);
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
        [ProducesResponseType(typeof(LavouraMinDto), StatusCodes.Status200OK)]
        public ActionResult GetAll()
        {
            var idUsuario = int.Parse(HttpContext.User.FindFirst("IdUsuario").Value);
            var lavouras = _fazendaService.GetAll().Where(f => f.Usuario.Id == idUsuario).Select(f => f.Lavouras)
                .ToList().Aggregate((result, item) => result.Concat(item).ToList());

            return Ok(lavouras.Select(l => new LavouraMinDto(l)));
        }

        [HttpGet("{idLavoura}")]
        [ProducesResponseType(typeof(LavouraDto), StatusCodes.Status200OK)]
        public ActionResult Get(int idLavoura)
        {
            var lavoura = _lavouraService.Get(idLavoura);
            if (lavoura == null)
            {
                return NotFound(Constants.ERR_LAVOURA_NAO_ENCONTRADA);
            }

            return Ok(new LavouraDto(lavoura));
        }

        [HttpGet]
        [Route("DadosLavoura/{idLavoura}")]
        [ProducesResponseType(typeof(DadosLavouraDto), StatusCodes.Status200OK)]
        public ActionResult GetDadosLavoura(int idLavoura)
        {
            var lavoura = _lavouraService.Get(idLavoura);
            if (lavoura == null || lavoura.DadosLavoura == null)
            {
                return BadRequest(Constants.ERR_DADOS_LAVOURA_NAO_ENCONTRADOS);
            }

            return Ok(new DadosLavouraDto(lavoura.DadosLavoura));
        }

        [HttpGet]
        [Route("DemarcacaoLavoura/{idLavoura}")]
        [ProducesResponseType(typeof(GeometriaDto), StatusCodes.Status200OK)]
        public ActionResult GetDemarcacaoLavoura(int idLavoura)
        {
            var lavoura = _lavouraService.Get(idLavoura);
            if (lavoura == null || lavoura.Demarcacao == null)
            {
                return BadRequest(Constants.ERR_DEMARCACAO_LAVOURA_NAO_ENCONTRADA);
            }

            return Ok(new GeometriaDto(lavoura.Demarcacao));
        }

        [HttpGet]
        [Route("TalhoesLavoura/{idLavoura}")]
        [ProducesResponseType(typeof(List<GeometriaDto>), StatusCodes.Status200OK)]
        public ActionResult GetTalhoesLavoura(int idLavoura)
        {
            var lavoura = _lavouraService.Get(idLavoura);
            if (lavoura == null || lavoura.Talhoes == null)
            {
                return BadRequest(Constants.ERR_TALHOES_LAVOURA_NAO_ENCONTRADOS);
            }

            return Ok(lavoura.Talhoes.Select(t => new GeometriaDto(t)));
        }

        [HttpPost]
        [Route("DadosLavoura/{idFazenda}")]
        [ProducesResponseType(typeof(LavouraDto), StatusCodes.Status200OK)]
        public ActionResult PostDadosLavoura(DadosLavouraDto dadosLavoura, int idFazenda)
        {
            var fazendaBd = _fazendaService.Get(idFazenda);
            if (dadosLavoura == null || fazendaBd == null || !fazendaBd.Concluida)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            var lavouraBd = new Lavoura
            {
                Fazenda = fazendaBd,
            };

            var dadosLavouraBd = new DadosLavoura
            {
                Cultivar = dadosLavoura.Cultivar,
                EspacamentoHorizontal = dadosLavoura.EspacamentoHorizontal,
                EspacamentoVertical = dadosLavoura.EspacamentoVertical,
                MesAnoPlantio = dadosLavoura.Data,
                Nome = dadosLavoura.Nome,
                NumeroPlantas = dadosLavoura.NumeroPlantas,
                Observacoes = dadosLavoura.Observacao,
                Lavoura = lavouraBd,
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _lavouraService.Post(lavouraBd);
                    _dadosLavouraService.Post(dadosLavouraBd);
                    transaction.Commit();
                    return Ok(new LavouraDto { Id = lavouraBd.Id });
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
                }
            }
        }

        [HttpPost]
        [Route("DemarcacaoLavoura/{idLavoura}")]
        public ActionResult PostDemarcacaoLavoura(Polygon geometriaDemarcacao, int idLavoura)
        {
            var lavouraBd = _lavouraService.Get(idLavoura);
            if (geometriaDemarcacao == null || lavouraBd == null || lavouraBd.Concluida)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }
            lavouraBd.Demarcacao = geometriaDemarcacao;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _lavouraService.Put(lavouraBd);
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
        [Route("TalhoesLavoura/{idLavoura}")]
        public async Task<ActionResult> PostTalhoesLavoura(MultiPolygon talhoes, int idLavoura)
        {
            var lavouraBd = _lavouraService.Get(idLavoura);
            if (talhoes == null || !talhoes.Any() || lavouraBd == null || lavouraBd.Concluida)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            lavouraBd.Talhoes = talhoes;
            var geometriaPdi = talhoes;


            var body = new List<PdiDto>
            {
                new PdiDto{
                    usr = "app",
                    pw = "pdi2020",
                    layer = "fazenda",
                    cod = 6.ToString(),
                    geometria = new GeometriaDtoCorreto(geometriaPdi),
                }
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var client = _httpClientFactory.CreateClient();
            var url = _config.GetSection("AppSettings:UrlPdi").Value + "/insert";

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    transaction.Commit();
                    var req = await client.PostAsync(url, new StringContent(jsonBody, Encoding.UTF8, "application/json"));
                    if (req.IsSuccessStatusCode)
                    {
                        return Ok();
                    }

                    throw new Exception("Erro ao cadastrar geometrias.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
                }
            }
        }

        [HttpPut]
        [Route("DadosLavoura/{idLavoura}")]
        public ActionResult PutDadosLavoura(DadosLavouraDto dadosLavoura, int idLavoura)
        {
            var lavouraBd = _lavouraService.Get(idLavoura);
            if (dadosLavoura == null)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            var dadosLavouraBd = lavouraBd.DadosLavoura;

            dadosLavouraBd.Cultivar = dadosLavoura.Cultivar;
            dadosLavouraBd.EspacamentoHorizontal = dadosLavoura.EspacamentoHorizontal;
            dadosLavouraBd.EspacamentoVertical = dadosLavoura.EspacamentoVertical;
            dadosLavouraBd.MesAnoPlantio = dadosLavoura.Data;
            dadosLavouraBd.Nome = dadosLavoura.Nome;
            dadosLavouraBd.NumeroPlantas = dadosLavoura.NumeroPlantas;
            dadosLavouraBd.Observacoes = dadosLavoura.Observacao;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _dadosLavouraService.Put(dadosLavouraBd);
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
        [Route("DemarcacaoLavoura/{idLavoura}")]
        public ActionResult PutDemarcacaoLavoura(Polygon geometriaDemarcacao, int idLavoura)
        {
            var lavouraBd = _lavouraService.Get(idLavoura);
            if (geometriaDemarcacao == null || lavouraBd == null)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            lavouraBd.Demarcacao = geometriaDemarcacao;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _lavouraService.Put(lavouraBd);
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
        [Route("TalhoesLavoura/{idLavoura}")]
        public async Task<ActionResult> PutTalhoesLavoura(MultiPolygon talhoes, int idLavoura)
        {
            var lavouraBd = _lavouraService.Get(idLavoura);
            if (talhoes == null || !talhoes.Any() || lavouraBd == null || lavouraBd.Talhoes == null)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            lavouraBd.Talhoes = talhoes;

            var geometriaPdi = talhoes.FirstOrDefault();
            foreach (var geometria in talhoes)
            {
                geometriaPdi = geometriaPdi.Union(geometria);
            }


            var body = new List<PdiDto>
            {
                new PdiDto{
                    usr = "app",
                    pw = "pdi2020",
                    layer = "fazenda",
                    cod = 6.ToString(),
                    geometria = new GeometriaDtoCorreto(geometriaPdi),
                }
            };

            var jsonBody = JsonConvert.SerializeObject(body);

            var client = _httpClientFactory.CreateClient();
            var url = _config.GetSection("AppSettings:UrlPdi").Value + "/insert";

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var req = await client.PostAsync(url, new StringContent(jsonBody, Encoding.UTF8, "application/json"));
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
    }
}
