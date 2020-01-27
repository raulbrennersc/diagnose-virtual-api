using DiagnoseVirtual.Application.Helpers;
using DiagnoseVirtual.Domain.Dtos;
using DiagnoseVirtual.Domain.Entities;
using DiagnoseVirtual.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using NHibernate;
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
        private readonly ISession _session;

        public FazendasController(IWebHostEnvironment hostingEnvironment, ISession session)
        {
            _hostingEnvironment = hostingEnvironment;
            _usuarioService  = new UsuarioService(session);
            _fazendaService  = new BaseService<Fazenda>(session);
            _localizacaoService  = new BaseService<LocalizacaoFazenda>(session);
            _dadosFazendaService  = new BaseService<DadosFazenda>(session);
            _session = session;
        }

        [HttpPost]
        [Route("UploadGeometrias")]
        public ActionResult ValidarLocalizacao(Microsoft.AspNetCore.Http.IFormFile file)
        {
            var path = _hostingEnvironment.ContentRootPath;
            try
            {
                var result = GeoFileHelper.ReadFile(file, path).Select(g => new GeometriaDto(g));
                return Ok(result);
            }
            catch
            {
                return BadRequest("Arquivo inálido");
            }
        }

        [HttpPost]
        [Route("ConclusaoFazenda/{idFazenda}")]
        public ActionResult Concluir(int idFazenda)
        {
            var fazenda = _fazendaService.Get(idFazenda);
            if (fazenda == null)
                return BadRequest(Constants.ERR_REQ_INVALIDA);

            if (fazenda.Concluida)
                return BadRequest(Constants.ERR_REQ_INVALIDA);

            fazenda.Concluida = true;

            using(var transaction = _session.BeginTransaction())
            {
                try
                {
                    _fazendaService.Put(fazenda);
                    transaction.Commit();
                    return Ok();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
                }
            }
        }

        [HttpGet]
        public ActionResult Get()
        {
            var idUsuario = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var fazendas = _fazendaService.GetAll().Where(f => f.Usuario.Id == Int32.Parse(idUsuario));

            var result = fazendas.Select(f => new FazendaDto(f));

            return Ok(result);
        }

        [HttpGet("{idFazenda}")]
        public ActionResult Get(int idFazenda)
        {
            var fazenda = _fazendaService.Get(idFazenda);
            if (fazenda == null)
                return NotFound(Constants.ERR_FAZENDA_NAO_ENCONTRADA);

            return Ok(new FazendaDto(fazenda));
        }

        [HttpGet]
        [Route("LocalizacaoFazenda/{idFazenda}")]
        public ActionResult GetLocalizacaoFazenda(int idFazenda)
        {
            var fazenda = _fazendaService.Get(idFazenda);
            if (fazenda == null || fazenda.LocalizacaoFazenda == null)
                return NotFound(Constants.ERR_LOCALIZACAO_FAZENDA_NAO_ENCONTRADA);

            return Ok(new LocalizacaoFazendaDto(fazenda.LocalizacaoFazenda));
        }

        [HttpGet]
        [Route("DadosFazenda/{idFazenda}")]
        public ActionResult GetDadosFazenda(int idFazenda)
        {
            var fazenda = _fazendaService.Get(idFazenda);
            if (fazenda == null || fazenda.DadosFazenda == null)
                return NotFound(Constants.ERR_DADOS_FAZENDA_NAO_ENCONTRADOS);

            return Ok(new DadosFazendaDto(fazenda.DadosFazenda));
        }

        [HttpGet]
        [Route("LocalizacaoGeoFazenda/{idFazenda}")]
        public ActionResult GetLocalizacaoGeoFazenda(int idFazenda)
        {
            var fazenda = _fazendaService.Get(idFazenda);
            if (fazenda == null || fazenda.Demarcacao == null)
                return NotFound(Constants.ERR_DEMARCACAO_FAZENDA_NAO_ENCONTRADA);

            var demarcacao = new DemarcacaoDto();
            demarcacao.Geometrias = new List<GeometriaDto>();
            demarcacao.Geometrias.Add(new GeometriaDto(fazenda.Demarcacao));

            return Ok(demarcacao);
        }

        [HttpGet]
        [Route("LavourasFazenda/{idFazenda}")]
        public ActionResult GetLavourasFazenda(int idFazenda)
        {
            var fazenda = _fazendaService.Get(idFazenda);
            if (fazenda == null)
                return NotFound(Constants.ERR_LAVOURAS_FAZENDA_NAO_ENCONTRADA);

            return Ok(fazenda.Lavouras.Select(l => new LavouraDto(l)));
        }

        [HttpPost]
        [Route("LocalizacaoFazenda")]
        public ActionResult PostLocalizacaoFazenda(LocalizacaoFazendaDto localizacao)
        {
            if (localizacao == null)
                return BadRequest(Constants.ERR_REQ_INVALIDA);

            var idUsuario = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var usuario = _usuarioService.Get(Int32.Parse(idUsuario));

            var fazendaBd = new Fazenda
            {
                Usuario = usuario,
                Ativa = true,
            };

            var localizacaoBd = new LocalizacaoFazenda
            {
                Contato = localizacao.Contato,
                Gerente = localizacao.Gerente,
                Nome = localizacao.Nome,
                Proprietario = localizacao.Proprietario,
                Municipio = localizacao.Municipio,
                PontoReferencia = localizacao.PontoReferencia,
                Estado = localizacao.Estado,
                Fazenda = fazendaBd
            };

            using (var transaction = _session.BeginTransaction())
            {
                try
                {
                    _fazendaService.Post(fazendaBd);
                    _localizacaoService.Post(localizacaoBd);
                    transaction.Commit();
                    return Ok(new FazendaDto { Id = fazendaBd.Id });
                }
                catch(Exception ex)
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
                return BadRequest(Constants.ERR_REQ_INVALIDA);

            var dadosFazendaBd = new DadosFazenda
            {
                AreaTotal = dadosFazenda.AreaTotal,
                Cultura = dadosFazenda.Cultura,
                QuantidadeLavouras = dadosFazenda.QuantidadeLavouras,
                Fazenda = fazendaBd
            };

            using(var transaction = _session.BeginTransaction())
            {
                try
                {
                    _dadosFazendaService.Post(dadosFazendaBd);
                    transaction.Commit();
                    return Ok();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode((int)HttpStatusCode.InternalServerError,  ex.Message);
                }
            }
        }

        [HttpPost]
        [Route("LocalizacaoGeoFazenda/{idFazenda}")]
        public ActionResult PostLocalizacaoGeoFazenda(DemarcacaoDto demarcacaoDto, int idFazenda)
        {
            var fazendaBd = _fazendaService.Get(idFazenda);
            if (demarcacaoDto == null || demarcacaoDto.Geometrias == null || !demarcacaoDto.Geometrias.Any() || fazendaBd == null || fazendaBd.Concluida)
                return BadRequest(Constants.ERR_REQ_INVALIDA);

            var factory = Geometry.DefaultFactory;
            var polygons = demarcacaoDto.Geometrias
                .Select(g => factory.CreatePolygon(g.Coordinates.Select(c => new Coordinate(c[0], c[1])).ToArray())).ToList();
            var geometrias = polygons.Select(p => factory.CreateGeometry(p));

            fazendaBd.Demarcacao = geometrias.FirstOrDefault();

            using(var transaction = _session.BeginTransaction())
            {
                try
                {
                    _fazendaService.Put(fazendaBd);
                    transaction.Commit();
                    return Ok();
                }
                catch(Exception ex)
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
                return BadRequest(Constants.ERR_REQ_INVALIDA);

            var localizacaoBd = fazendaBd.LocalizacaoFazenda;

            localizacaoBd.Contato = localizacao.Contato;
            localizacaoBd.Gerente = localizacao.Gerente;
            localizacaoBd.Nome = localizacao.Nome;
            localizacaoBd.Proprietario = localizacao.Proprietario;
            localizacaoBd.Municipio = localizacao.Municipio;
            localizacaoBd.PontoReferencia = localizacao.PontoReferencia;
            localizacaoBd.Estado = localizacao.Estado;

            using(var transaction = _session.BeginTransaction())
            {
                try
                {
                    _localizacaoService.Put(localizacaoBd);
                    transaction.Commit();
                    return Ok();
                }
                catch(Exception ex)
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
                return BadRequest(Constants.ERR_REQ_INVALIDA);

            var dadosFazendaBd = fazendaBd.DadosFazenda;

            dadosFazendaBd.AreaTotal = dadosFazenda.AreaTotal;
            dadosFazendaBd.Cultura = dadosFazenda.Cultura;
            dadosFazendaBd.QuantidadeLavouras = dadosFazenda.QuantidadeLavouras;

            using(var transaction = _session.BeginTransaction())
            {
                try
                {
                    _dadosFazendaService.Put(dadosFazendaBd);
                    return Ok();
                }
                catch(Exception ex)
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
                return BadRequest(Constants.ERR_REQ_INVALIDA);

            var factory = Geometry.DefaultFactory;
            var polygons = demarcacaoDto.Geometrias
                .Select(g => factory.CreatePolygon(g.Coordinates.Select(c => new Coordinate(c[0], c[1])).ToArray())).ToList();
            var geometrias = polygons.Select(p => factory.CreateGeometry(p));

            fazendaBd.Demarcacao = geometrias.FirstOrDefault();

            using(var transaction = _session.BeginTransaction())
            {
                try
                {
                    _fazendaService.Put(fazendaBd);
                    return Ok();
                }
                catch(Exception ex)
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
                }
            }
        }
    }
}
