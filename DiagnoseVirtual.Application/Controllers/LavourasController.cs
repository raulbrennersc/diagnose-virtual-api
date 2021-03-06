﻿using DiagnoseVirtual.Application.Helpers;
using DiagnoseVirtual.Domain.Dtos;
using DiagnoseVirtual.Domain.Entities;
using DiagnoseVirtual.Domain.Enums;
using DiagnoseVirtual.Infra.Data.Context;
using DiagnoseVirtual.Service.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetTopologySuite.Features;
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
        private readonly PsqlContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public LavourasController(IWebHostEnvironment hostingEnvironment, IHttpClientFactory httpClientFactory, IConfiguration config, PsqlContext context)
        {
            _context = context;
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

        [HttpPost]
        [Route("ExcluirLavoura/{idLavoura}")]
        public ActionResult PostExcluirLavoura(int idLavoura)
        {
            var lavouraBd = _lavouraService.Get(idLavoura);
            if (lavouraBd == null || !lavouraBd.Ativa)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            lavouraBd.Ativa = false;

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
        [ProducesResponseType(typeof(List<LavouraMinDto>), StatusCodes.Status200OK)]
        public ActionResult GetAll()
        {
            var idUsuario = int.Parse(HttpContext.User.FindFirst("IdUsuario").Value);
            var lLavouras = _fazendaService.GetAll().Where(f => f.Usuario.Id == idUsuario && f.Ativa).Select(f => f.Lavouras.Where(l => l.Ativa))
                .ToList();
            var lavouras = new List<Lavoura>();
            foreach (var l1 in lLavouras)
            {
                foreach (var l in l1)
                {
                    lavouras.Add(l);
                }
            }

            return Ok(lavouras.Select(l => new LavouraMinDto(l)));
        }

        [HttpGet("{idLavoura}")]
        [ProducesResponseType(typeof(LavouraMinDto), StatusCodes.Status200OK)]
        public ActionResult Get(int idLavoura)
        {
            var lavoura = _lavouraService.Get(idLavoura);
            if (lavoura == null || !lavoura.Ativa)
            {
                return NotFound(Constants.ERR_LAVOURA_NAO_ENCONTRADA);
            }

            return Ok(new LavouraMinDto(lavoura));
        }

        [HttpGet("{idLavoura}/Completa")]
        [ProducesResponseType(typeof(LavouraMinDto), StatusCodes.Status200OK)]
        public ActionResult GetLavouraCompleta(int idLavoura)
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
        [ProducesResponseType(typeof(Geometry), StatusCodes.Status200OK)]
        public ActionResult GetDemarcacaoLavoura(int idLavoura)
        {
            var lavoura = _lavouraService.Get(idLavoura);
            if (lavoura == null || lavoura.Demarcacao == null)
            {
                return BadRequest(Constants.ERR_DEMARCACAO_LAVOURA_NAO_ENCONTRADA);
            }

            return Ok(lavoura.Demarcacao);
        }

        [HttpGet]
        [Route("TalhoesLavoura/{idLavoura}")]
        [ProducesResponseType(typeof(Geometry[]), StatusCodes.Status200OK)]
        public ActionResult GetTalhoesLavoura(int idLavoura)
        {
            var lavoura = _lavouraService.Get(idLavoura);
            if (lavoura == null || lavoura.Talhoes == null)
            {
                return BadRequest(Constants.ERR_TALHOES_LAVOURA_NAO_ENCONTRADOS);
            }

            return Ok(lavoura.Talhoes);
        }

        [HttpPost]
        [Route("DadosLavoura/{idFazenda}")]
        [ProducesResponseType(typeof(LavouraMinDto), StatusCodes.Status200OK)]
        public ActionResult PostDadosLavoura(DadosLavouraDto dadosLavoura, int idFazenda)
        {
            var fazendaBd = _fazendaService.Get(idFazenda);
            var etapaDados = new BaseService<EtapaLavoura>(_context).Get((int)EEtapaLavoura.DemarcacaoLavoura);
            if (dadosLavoura == null || fazendaBd == null || !fazendaBd.Concluida)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            var lavouraBd = new Lavoura
            {
                Fazenda = fazendaBd,
                Etapa = etapaDados,
            };

            var dadosLavouraBd = new DadosLavoura
            {
                Cultivar = dadosLavoura.Cultivar,
                EspacamentoHorizontal = dadosLavoura.EspacamentoHorizontal,
                EspacamentoVertical = dadosLavoura.EspacamentoVertical,
                MesAnoPlantio = dadosLavoura.MesAnoPlantio,
                Nome = dadosLavoura.Nome,
                AreaTotal = dadosLavoura.AreaTotal,
                NumeroPlantas = dadosLavoura.NumeroPlantas,
                Observacoes = dadosLavoura.Observacoes,
                Lavoura = lavouraBd,
            };

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _lavouraService.Post(lavouraBd);
                    _dadosLavouraService.Post(dadosLavouraBd);
                    transaction.Commit();
                    return Ok(new LavouraMinDto(lavouraBd));
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
        public ActionResult PostDemarcacaoLavoura(Geometry geometriaDemarcacao, int idLavoura)
        {
            var lavouraBd = _lavouraService.Get(idLavoura);
            if (geometriaDemarcacao == null || lavouraBd == null || lavouraBd.Concluida)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            if (!lavouraBd.Fazenda.Demarcacao.Contains(geometriaDemarcacao))
            {
                return BadRequest("A lavoura deve estar dentro da fazenda");
            }


            var etapaDemarcacao= new BaseService<EtapaLavoura>(_context).Get((int)EEtapaLavoura.Talhoes);
            lavouraBd.Etapa = etapaDemarcacao;
            lavouraBd.Demarcacao = geometriaDemarcacao;



            var objReq = PdiHttpReqHelper.PdiTalhaoInsertReq(lavouraBd.Demarcacao, lavouraBd.Fazenda.IdPdi);

            var respostaPdi = HttpRequestHelper.MakeJsonRequest<PdiResponseDto>(_httpClientFactory.CreateClient(), $"{_config.GetSection("AppSettings:UrlPdi").Value}/insert", objReq);
            if (respostaPdi != null)
            {
                lavouraBd.IdPdi = respostaPdi.Feature_Id;
            }

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
        public async Task<ActionResult> PostTalhoesLavoura(FeatureCollection talhoes, int idLavoura)
        {
            var lavouraBd = _lavouraService.Get(idLavoura);
            if (talhoes == null || !talhoes.Any() || lavouraBd == null || lavouraBd.Concluida)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }
            var etapaDemarcacao = new BaseService<EtapaLavoura>(_context).Get((int)EEtapaLavoura.Confirmacao);
            lavouraBd.Etapa = etapaDemarcacao;
            lavouraBd.Talhoes = talhoes.Select(f => f.Geometry).ToArray();
            var geometriaPdi = talhoes.FirstOrDefault().Geometry;
            foreach (var talhao in talhoes)
            {
                if (!lavouraBd.Demarcacao.Contains(talhao.Geometry))
                {
                    return BadRequest("O talhão deve estar dentro da lavoura");
                }
                geometriaPdi = geometriaPdi.Union(talhao.Geometry);
            }

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
        [Route("DadosLavoura/{idLavoura}")]
        public ActionResult PutDadosLavoura(DadosLavouraDto dadosLavoura, int idLavoura)
        {
            var lavouraBd = _lavouraService.Get(idLavoura);
            if (dadosLavoura == null)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            var dadosLavouraBd = lavouraBd.DadosLavoura;
            dadosLavouraBd.Lavoura.Fazenda = _fazendaService.Get(dadosLavoura.IdFazenda);
            dadosLavouraBd.Cultivar = dadosLavoura.Cultivar;
            dadosLavouraBd.EspacamentoHorizontal = dadosLavoura.EspacamentoHorizontal;
            dadosLavouraBd.EspacamentoVertical = dadosLavoura.EspacamentoVertical;
            dadosLavouraBd.MesAnoPlantio = dadosLavoura.MesAnoPlantio;
            dadosLavouraBd.Nome = dadosLavoura.Nome;
            dadosLavouraBd.NumeroPlantas = dadosLavoura.NumeroPlantas;
            dadosLavouraBd.Observacoes = dadosLavoura.Observacoes;
            dadosLavouraBd.AreaTotal = dadosLavoura.AreaTotal;

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
        public ActionResult PutDemarcacaoLavoura(Geometry geometriaDemarcacao, int idLavoura)
        {
            var lavouraBd = _lavouraService.Get(idLavoura);
            if (geometriaDemarcacao == null || lavouraBd == null)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            if (!lavouraBd.Fazenda.Demarcacao.Contains(geometriaDemarcacao))
            {
                return BadRequest("A lavoura deve estar dentro da fazenda");
            }


            lavouraBd.Demarcacao = geometriaDemarcacao;
            var objReq = PdiHttpReqHelper.PdiTalhaoInsertReq(lavouraBd.Demarcacao, lavouraBd.Fazenda.IdPdi);

            var respostaPdi = HttpRequestHelper.MakeJsonRequest<PdiResponseDto>(_httpClientFactory.CreateClient(), $"{_config.GetSection("AppSettings:UrlPdi").Value}/insert", objReq);
            if (respostaPdi != null)
            {
                lavouraBd.IdPdi = respostaPdi.Feature_Id;
            }

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
        public async Task<ActionResult> PutTalhoesLavoura(FeatureCollection talhoes, int idLavoura)
        {
            var lavouraBd = _lavouraService.Get(idLavoura);
            if (talhoes == null || !talhoes.Any() || lavouraBd == null || lavouraBd.Talhoes == null)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            foreach (var talhao in talhoes)
            {
                if (!lavouraBd.Demarcacao.Contains(talhao.Geometry))
                {
                    return BadRequest("O talhão deve estar dentro da lavoura");
                }
            }

            lavouraBd.Talhoes = talhoes.Select(f => f.Geometry).ToArray();
            
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
    }
}
