
using DiagnoseVirtual.Application.Helpers;
using DiagnoseVirtual.Domain.Dtos;
using DiagnoseVirtual.Domain.Entities;
using DiagnoseVirtual.Domain.Enums;
using DiagnoseVirtual.Infra.Data.Context;
using DiagnoseVirtual.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetTopologySuite.Features;
using NetTopologySuite.Geometries;
using NetTopologySuite.IO;
using NetTopologySuite.IO.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;

        public FazendasController(IWebHostEnvironment hostingEnvironment, IHttpClientFactory httpClientFactory, IConfiguration config)
        {
            _hostingEnvironment = hostingEnvironment;
            _fazendaService = new BaseService<Fazenda>(_context);
            _usuarioService = new UsuarioService(_context);
            _localizacaoService = new BaseService<LocalizacaoFazenda>(_context);
            _dadosFazendaService = new BaseService<DadosFazenda>(_context);
            _httpClientFactory = httpClientFactory;
            _config = config;
        }

        [HttpPost]
        [Route("UploadGeometrias")]
        [ProducesResponseType(typeof(List<Geometry>), StatusCodes.Status200OK)]
        public ActionResult ValidarLocalizacao(IFormFile file)
        {
            var path = _hostingEnvironment.ContentRootPath;
            try
            {
                var result = GeoFileHelper.ReadFile(file, path);
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

        [HttpPost]
        [Route("ExcluirFazenda/{idFazenda}")]
        public ActionResult Excluir(int idFazenda)
        {
            var fazenda = _fazendaService.Get(idFazenda);
            if (fazenda == null)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            if (!fazenda.Ativa)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            fazenda.Ativa = false;

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
        [ProducesResponseType(typeof(List<FazendaMinDto>), StatusCodes.Status200OK)]
        public ActionResult Get()
        {
            var idUsuario = HttpContext.User.FindFirst("IdUsuario").Value;
            var fazendas = _fazendaService.GetAll().Where(f => f.Usuario.Id == int.Parse(idUsuario) && f.Ativa).ToList();

            var result = fazendas.Select(f => new FazendaMinDto(f)).OrderByDescending(f => f.Id);

            return Ok(result);
        }

        [HttpGet("{idFazenda}")]
        [ProducesResponseType(typeof(FazendaMinDto), StatusCodes.Status200OK)]
        public ActionResult Get(int idFazenda)
        {
            var fazenda = _fazendaService.Get(idFazenda);
            if (fazenda == null)
            {
                return NotFound(Constants.ERR_FAZENDA_NAO_ENCONTRADA);
            }

            return Ok(new FazendaMinDto(fazenda));
        }

        [HttpGet("{idFazenda}/Completa")]
        [ProducesResponseType(typeof(FazendaDto), StatusCodes.Status200OK)]
        public ActionResult GetCompleta(int idFazenda)
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
        [ProducesResponseType(typeof(Polygon), StatusCodes.Status200OK)]
        public ActionResult GetLocalizacaoGeoFazenda(int idFazenda)
        {
            var fazenda = _fazendaService.Get(idFazenda);
            if (fazenda == null || fazenda.Demarcacao == null)
            {
                return NotFound(Constants.ERR_DEMARCACAO_FAZENDA_NAO_ENCONTRADA);
            }

            return Ok(fazenda.Demarcacao);
        }

        [HttpGet]
        [Route("{idFazenda}/Lavouras")]
        [ProducesResponseType(typeof(List<LavouraMinDto>), StatusCodes.Status200OK)]
        public ActionResult GetLavourasFazenda(int idFazenda)
        {
            var fazenda = _fazendaService.Get(idFazenda);
            if (fazenda == null)
            {
                return NotFound(Constants.ERR_LAVOURAS_FAZENDA_NAO_ENCONTRADA);
            }

            return Ok(fazenda.Lavouras.Select(l => new LavouraMinDto(l)));
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
        [ProducesResponseType(typeof(FazendaMinDto), StatusCodes.Status200OK)]
        public ActionResult PostLocalizacaoFazenda(LocalizacaoFazendaDto localizacao)
        {
            if (localizacao == null)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            var idUsuario = HttpContext.User.FindFirst("IdUsuario").Value;
            var usuario = _usuarioService.Get(int.Parse(idUsuario));

            var etapaDados = new BaseService<EtapaFazenda>(_context).Get((int)EEtapaFazenda.DadosFazenda);
            var fazendaBd = new Fazenda
            {
                Etapa = etapaDados,
                Usuario = usuario,
                Ativa = true,
            };

            var municipio = new BaseService<Municipio>(_context).Get(localizacao.IdMunicipio);
            if (municipio == null)
            {
                return BadRequest("O municipio indicado nao existe");
            }

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
                    return Ok(new FazendaMinDto(fazendaBd));
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
            var culturaBd = new BaseService<Cultura>(_context).Get(dadosFazenda?.IdCultura ?? 0);
            if (dadosFazenda == null || fazendaBd == null || fazendaBd.Concluida || culturaBd == null)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            var etapaDemarcacao = new BaseService<EtapaFazenda>(_context).Get((int)EEtapaFazenda.Demarcacao);
            fazendaBd.Etapa = etapaDemarcacao;
            var dadosFazendaBd = new DadosFazenda
            {
                AreaTotal = dadosFazenda.AreaTotal,
                Cultura = culturaBd,
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
        public async Task<ActionResult> PostLocalizacaoGeoFazenda(Polygon demarcacao, int idFazenda)
        {
            var fazendaBd = _fazendaService.Get(idFazenda);
            if (demarcacao == null || fazendaBd == null || fazendaBd.Concluida)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            var etapaConclusao = new BaseService<EtapaFazenda>(_context).Get((int)EEtapaFazenda.Confirmacao);
            fazendaBd.Etapa = etapaConclusao;
            fazendaBd.Demarcacao = demarcacao;

            var body = new List<PdiDto>
            {
                new PdiDto{
                    usr = "app",
                    pw = "pdi2020",
                    layer = "fazenda",
                    cod = fazendaBd.Id.ToString(),
                    geometria = new GeometriaDtoCorreto(fazendaBd.Demarcacao),
                }
            };
            var jsonBody = JsonConvert.SerializeObject(body);

            var client = _httpClientFactory.CreateClient();
            var url = _config.GetSection("AppSettings:UrlPdi").Value + "/insert";

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _fazendaService.Put(fazendaBd);
                    transaction.Commit();
                    return Ok();
                    //var req = await client.PostAsync(url, new StringContent(jsonBody, Encoding.UTF8, "application/json"));
                    //if (req.IsSuccessStatusCode)
                    //{
                    //    return Ok();
                    //}
                    //throw new Exception("Erro ao cadastrar geometria.");
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
                }
            }
        }

        [HttpPut]
        [Route("LocalizacaoFazenda/{idFazenda}")]
        public ActionResult PutLocalizacaoFazenda(LocalizacaoFazendaDto localizacao, int idFazenda)
        {
            var fazendaBd = _fazendaService.Get(idFazenda);
            if (localizacao == null || fazendaBd == null)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            var municipio = new BaseService<Municipio>(_context).Get(localizacao.IdMunicipio);
            if(municipio == null)
            {
                return BadRequest("O municipio indicado nao existe");
            }

            var localizacaoBd = fazendaBd.LocalizacaoFazenda;

            localizacaoBd.Telefone = localizacao.Telefone;
            localizacaoBd.Email = localizacao.Email;
            localizacaoBd.Gerente = localizacao.Gerente;
            localizacaoBd.Nome = localizacao.Nome;
            localizacaoBd.Proprietario = localizacao.Proprietario;
            localizacaoBd.Municipio = municipio;
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
            var culturaBd = new BaseService<Cultura>(_context).Get(dadosFazenda.IdCultura);
            if (dadosFazenda == null || fazendaBd == null || culturaBd == null)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            var dadosFazendaBd = fazendaBd.DadosFazenda;

            dadosFazendaBd.AreaTotal = dadosFazenda.AreaTotal;
            dadosFazendaBd.Cultura = culturaBd;
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
        public async Task<ActionResult> PutLocalizacaoGeoFazenda(Polygon demarcacao, int idFazenda)
        {
            var fazendaBd = _fazendaService.Get(idFazenda);
            if (demarcacao == null || fazendaBd == null)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            fazendaBd.Demarcacao = demarcacao;

            var body = new List<PdiDto>
            {
                new PdiDto{
                    usr = "app",
                    pw = "pdi2020",
                    layer = "fazenda",
                    cod = 6.ToString(),
                    geometria = new GeometriaDtoCorreto(fazendaBd.Demarcacao),
                }
            };
            var jsonBody = JsonConvert.SerializeObject(body);

            var client = _httpClientFactory.CreateClient();
            var url = _config.GetSection("AppSettings:UrlPdi").Value + "/insert";

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _fazendaService.Put(fazendaBd);
                    var req = await client.PostAsync(url, new StringContent(jsonBody, Encoding.UTF8, "application/json"));
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
