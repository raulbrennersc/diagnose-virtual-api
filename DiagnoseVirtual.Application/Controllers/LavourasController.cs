using DiagnoseVirtual.Domain.Dtos;
using DiagnoseVirtual.Domain.Entities;
using DiagnoseVirtual.Infra.Data.Context;
using DiagnoseVirtual.Service.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace DiagnoseVirtual.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LavourasController : ControllerBase
    {
        private readonly BaseService<Lavoura> _lavouraService;
        private readonly BaseService<DadosLavoura> _dadosLavouraService;
        private readonly BaseService<Fazenda> _fazendaService;
        private readonly BaseService<Talhao> _talhaoService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly PsqlContext _context = new PsqlContext();

        public LavourasController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            _lavouraService = new BaseService<Lavoura>(_context);
            _dadosLavouraService = new BaseService<DadosLavoura>(_context);
            _fazendaService = new BaseService<Fazenda>(_context);
            _talhaoService = new BaseService<Talhao>(_context);
        }

        [HttpPost]
        [Route("ConclusaoLavoura/{idLavoura}")]
        public ActionResult PostConcluirLavoura(int idLavoura)
        {
            var lavouraBd = _lavouraService.Get(idLavoura);
            if (lavouraBd == null || lavouraBd.Concluida)
                return BadRequest(Constants.ERR_REQ_INVALIDA);

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

        [HttpGet("{idLavoura}")]
        public ActionResult Get(int idLavoura)
        {
            var lavoura = _lavouraService.Get(idLavoura);
            if (lavoura == null)
                return NotFound(Constants.ERR_LAVOURA_NAO_ENCONTRADA);
            return Ok(new LavouraDto(lavoura));
        }

        [HttpGet]
        [Route("DadosLavoura/{idLavoura}")]
        public ActionResult GetDadosLavoura(int idLavoura)
        {
            var lavoura = _lavouraService.Get(idLavoura);
            if (lavoura == null || lavoura.DadosLavoura == null)
                return BadRequest(Constants.ERR_DADOS_LAVOURA_NAO_ENCONTRADOS);

            return Ok(new DadosLavouraDto(lavoura.DadosLavoura));
        }

        [HttpGet]
        [Route("DemarcacaoLavoura/{idLavoura}")]
        public ActionResult GetDemarcacaoLavoura(int idLavoura)
        {
            var lavoura = _lavouraService.Get(idLavoura);
            if (lavoura == null || lavoura.Demarcacao == null)
                return BadRequest(Constants.ERR_DEMARCACAO_LAVOURA_NAO_ENCONTRADA);

            return Ok(new GeometriaDto(lavoura.Demarcacao));
        }

        [HttpGet]
        [Route("TalhoesLavoura/{idLavoura}")]
        public ActionResult GetTalhoesLavoura(int idLavoura)
        {
            var lavoura = _lavouraService.Get(idLavoura);
            if (lavoura == null || lavoura.Talhoes == null)
                return BadRequest(Constants.ERR_TALHOES_LAVOURA_NAO_ENCONTRADOS);

            return Ok(lavoura.Talhoes.Select(t => new GeometriaDto(t.Geometria)));
        }

        [HttpPost]
        [Route("DadosLavoura/{idFazenda}")]
        public ActionResult PostDadosLavoura(DadosLavouraDto dadosLavoura, int idFazenda)
        {
            var fazendaBd = _fazendaService.Get(idFazenda);
            if (dadosLavoura == null || fazendaBd == null || !fazendaBd.Concluida)
                return BadRequest(Constants.ERR_REQ_INVALIDA);

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
        public ActionResult PostDemarcacaoLavoura(GeometriaDto geometriaDemarcacao, int idLavoura)
        {
            var lavouraBd = _lavouraService.Get(idLavoura);
            if (geometriaDemarcacao == null || lavouraBd == null || lavouraBd.Concluida)
                return BadRequest(Constants.ERR_REQ_INVALIDA);

            var factory = Geometry.DefaultFactory;
            var polygon = factory.CreatePolygon(geometriaDemarcacao.Coordinates.Select(c => new Coordinate(c[0], c[1])).ToArray());
            var demarcacaoBd = factory.CreateGeometry(polygon);

            lavouraBd.Demarcacao = demarcacaoBd;

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
        public ActionResult PostTalhoesLavoura(List<GeometriaDto> talhoes, int idLavoura)
        {
            var lavouraBd = _lavouraService.Get(idLavoura);
            if (talhoes == null || !talhoes.Any() || lavouraBd == null || lavouraBd.Concluida)
                return BadRequest(Constants.ERR_REQ_INVALIDA);

            var factory = Geometry.DefaultFactory;
            var polygons = talhoes
                .Select(g => factory.CreatePolygon(g.Coordinates.Select(c => new Coordinate(c[0], c[1])).ToArray())).ToList();
            var talhoesBd = polygons.Select(p => factory.CreateGeometry(p))
                .Select(g => new Talhao { Geometria = g, Lavoura = lavouraBd }).ToList();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var talhao in lavouraBd.Talhoes)
                    {
                        _talhaoService.Delete(talhao.Id);
                    }

                    foreach (var talhao in talhoesBd)
                    {
                        _talhaoService.Post(talhao);
                    }

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
            if (dadosLavoura == null || !lavouraBd.Concluida)
                return BadRequest(Constants.ERR_REQ_INVALIDA);

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
        public ActionResult PutDemarcacaoLavoura(GeometriaDto geometriaDemarcacao, int idLavoura)
        {
            var lavouraBd = _lavouraService.Get(idLavoura);
            if (geometriaDemarcacao == null || lavouraBd == null || !lavouraBd.Concluida)
                return BadRequest(Constants.ERR_REQ_INVALIDA);

            var factory = Geometry.DefaultFactory;
            var polygon = factory.CreatePolygon(geometriaDemarcacao.Coordinates.Select(c => new Coordinate(c[0], c[1])).ToArray());
            lavouraBd.Demarcacao = factory.CreateGeometry(polygon);

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
        public ActionResult PutTalhoesLavoura(List<GeometriaDto> talhoes, int idLavoura)
        {
            var lavouraBd = _lavouraService.Get(idLavoura);
            if (talhoes == null || !talhoes.Any() || lavouraBd == null || lavouraBd.Talhoes == null)
                return BadRequest(Constants.ERR_REQ_INVALIDA);

            var factory = Geometry.DefaultFactory;
            var polygons = talhoes
                .Select(g => factory.CreatePolygon(g.Coordinates.Select(c => new Coordinate(c[0], c[1])).ToArray())).ToList();
            var talhoesBd = polygons.Select(p => factory.CreateGeometry(p))
                .Select(g => new Talhao { Geometria = g, Lavoura = lavouraBd }).ToList();

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    foreach (var talhao in talhoesBd)
                    {
                        _talhaoService.Post(talhao);
                    }

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
