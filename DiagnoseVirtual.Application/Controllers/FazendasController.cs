using DiagnoseVirtual.Application.Helpers;
using DiagnoseVirtual.Domain.Dtos;
using DiagnoseVirtual.Domain.Entities;
using DiagnoseVirtual.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DiagnoseVirtual.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FazendasController : ControllerBase
    {
        private readonly BaseService<Fazenda> _fazendaService = new BaseService<Fazenda>();
        private readonly UsuarioService _usuarioService = new UsuarioService();
        private readonly BaseService<LocalizacaoFazenda> _localizacaoService = new BaseService<LocalizacaoFazenda>();
        private readonly BaseService<DadosFazenda> _dadosFazendaService = new BaseService<DadosFazenda>();
        private readonly IWebHostEnvironment _hostingEnvironment;

        public FazendasController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }


        [HttpPost]
        [Route("UploadGeometrias")]
        public ActionResult ValidarLocalizacao(IFormFile file)
        {
            var path = _hostingEnvironment.ContentRootPath;
            var result = GeoFileHelper.ReadFile(file, path).Select(g => new GeometriaDto(g));
            return Ok(result);
        }

        [HttpPost]
        [Route("ConclusaoFazenda/{idFazenda}")]
        public ActionResult Concluir(int idFazenda)
        {
            var fazenda = _fazendaService.Get(idFazenda);
            if (fazenda == null)
                return NotFound();

            if (fazenda.Concluida)
                return BadRequest();

            fazenda.Concluida = true;
            _fazendaService.Put(fazenda);

            return Ok(new FazendaDto(fazenda));
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
                return NotFound();

            return Ok(new FazendaDto(fazenda));
        }

        [HttpGet]
        [Route("LocalizacaoFazenda/{idFazenda}")]
        public ActionResult GetLocalizacaoFazenda(int idFazenda)
        {
            var fazenda = _fazendaService.Get(idFazenda);
            if (fazenda == null || fazenda.LocalizacaoFazenda == null)
                return NotFound();

            var localizacao = new LocalizacaoFazendaDto(fazenda.LocalizacaoFazenda);

            return Ok(localizacao);
        }

        [HttpGet]
        [Route("DadosFazenda/{idFazenda}")]
        public ActionResult GetDadosFazenda(int idFazenda)
        {
            var fazenda = _fazendaService.Get(idFazenda);
            if (fazenda == null || fazenda.DadosFazenda == null)
                return NotFound();

            var dadosFazenda = new DadosFazendaDto(fazenda.DadosFazenda);

            return Ok(dadosFazenda);
        }

        [HttpGet]
        [Route("LocalizacaoGeoFazenda/{idFazenda}")]
        public ActionResult GetLocalizacaoGeoFazenda(int idFazenda)
        {
            var fazenda = _fazendaService.Get(idFazenda);
            if (fazenda == null || fazenda.Demarcacao == null)
                return NotFound();

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
            if (fazenda == null || fazenda.Lavouras == null || !fazenda.Lavouras.Any())
                return NotFound();

            var lavouras = fazenda.Lavouras.Select(l => new LavouraDto(l));

            return Ok(lavouras);
        }

        [HttpPost]
        [Route("LocalizacaoFazenda")]
        public ActionResult PostLocalizacaoFazenda(LocalizacaoFazendaDto localizacao)
        {
            var idUsuario = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var usuario = _usuarioService.Get(Int32.Parse(idUsuario));

            var fazendaBd = new Fazenda
            {
                IdUsuario = usuario.Id,
                Ativa = true,
            };

            _fazendaService.Post(fazendaBd);

            var localizacaoBd = new LocalizacaoFazenda
            {
                Contato = localizacao.Contato,
                Gerente = localizacao.Gerente,
                Nome = localizacao.Nome,
                Proprietario = localizacao.Proprietario,
                Municipio = localizacao.Municipio,
                PontoReferencia = localizacao.PontoReferencia,
                Estado = localizacao.Estado,
                IdFazenda = fazendaBd.Id
            };

            _localizacaoService.Post(localizacaoBd);

            var result = new FazendaDto(fazendaBd);

            return Ok(result);
        }

        [HttpPost]
        [Route("DadosFazenda/{idFazenda}")]
        public ActionResult PostDadosFazenda(DadosFazendaDto dadosFazenda, int idFazenda)
        {
            if (dadosFazenda == null || idFazenda == 0)
                return BadRequest();

            var fazendaBd = _fazendaService.Get(idFazenda);
            if (fazendaBd == null)
                return BadRequest();

            var dadosFazendaBd = new DadosFazenda
            {
                AreaTotal = dadosFazenda.AreaTotal,
                Cultura = dadosFazenda.Cultura,
                QuantidadeLavouras = dadosFazenda.QuantidadeLavouras,
                IdFazenda = fazendaBd.Id
            };

            _dadosFazendaService.Post(dadosFazendaBd);

            var result = new FazendaDto(fazendaBd);

            return Ok(result);
        }

        [HttpPost]
        [Route("LocalizacaoGeoFazenda/{idFazenda}")]
        public ActionResult PostLocalizacaoGeoFazenda(DemarcacaoDto demarcacaoDto, int idFazenda)
        {
            if (demarcacaoDto == null || idFazenda == 0)
                return BadRequest();

            var fazendaBd = _fazendaService.Get(idFazenda);
            if (fazendaBd == null)
                return BadRequest();

            var factory = Geometry.DefaultFactory;
            var polygons = demarcacaoDto.Geometrias
                .Select(g => (Polygon)factory.CreatePolygon(g.Coordinates.Select(c => new Coordinate(c[0], c[1])).ToArray())).ToList();
            var geometrias = polygons.Select(p => (Geometry)factory.CreateGeometry(p));

            fazendaBd.Demarcacao = geometrias.FirstOrDefault();
            _fazendaService.Put(fazendaBd);

            var result = new FazendaDto(fazendaBd);

            return Ok(result);
        }

        [HttpPut]
        [Route("LocalizacaoFazenda/idFazenda")]
        public ActionResult PutLocalizacaoFazenda(LocalizacaoFazendaDto localizacao, int idFazenda)
        {
            var fazendaBd = _fazendaService.Get(idFazenda);

            if (fazendaBd == null || fazendaBd.LocalizacaoFazenda == null)
                return BadRequest();

            fazendaBd.LocalizacaoFazenda.Contato = localizacao.Contato;
            fazendaBd.LocalizacaoFazenda.Gerente = localizacao.Gerente;
            fazendaBd.LocalizacaoFazenda.Nome = localizacao.Nome;
            fazendaBd.LocalizacaoFazenda.Proprietario = localizacao.Proprietario;
            fazendaBd.LocalizacaoFazenda.Municipio = localizacao.Municipio;
            fazendaBd.LocalizacaoFazenda.PontoReferencia = localizacao.PontoReferencia;
            fazendaBd.LocalizacaoFazenda.Estado = localizacao.Estado;


            _localizacaoService.Put(fazendaBd.LocalizacaoFazenda);

            var result = new LocalizacaoFazendaDto(fazendaBd.LocalizacaoFazenda);

            return Ok();
        }

        [HttpPut]
        [Route("DadosFazenda/{idFazenda}")]
        public ActionResult PutDadosFazenda(DadosFazendaDto dadosFazenda, int idFazenda)
        {
            if (dadosFazenda == null)
                return BadRequest();

            var fazendaBd = _fazendaService.Get(idFazenda);
            if (fazendaBd == null || fazendaBd.DadosFazenda == null)
                return BadRequest();

            fazendaBd.DadosFazenda.AreaTotal = dadosFazenda.AreaTotal;
            fazendaBd.DadosFazenda.Cultura = dadosFazenda.Cultura;
            fazendaBd.DadosFazenda.QuantidadeLavouras = dadosFazenda.QuantidadeLavouras;

            _dadosFazendaService.Put(fazendaBd.DadosFazenda);
            return Ok();
        }

        [HttpPut]
        [Route("LocalizacaoGeoFazenda/{idFazenda}")]
        public ActionResult PutLocalizacaoGeoFazenda(DemarcacaoDto demarcacaoDto, int idFazenda)
        {
            if (demarcacaoDto == null)
                return BadRequest();

            var fazendaBd = _fazendaService.Get(idFazenda);
            if (fazendaBd == null || fazendaBd.Demarcacao == null)
                return BadRequest();

            var factory = Geometry.DefaultFactory;
            var polygons = demarcacaoDto.Geometrias
                .Select(g => (Polygon)factory.CreatePolygon(g.Coordinates.Select(c => new Coordinate(c[0], c[1])).ToArray())).ToList();
            var geometrias = polygons.Select(p => (Geometry)factory.CreateGeometry(p));

            fazendaBd.Demarcacao = geometrias.FirstOrDefault();
            _fazendaService.Put(fazendaBd);

            return Ok();
        }
    }
}
