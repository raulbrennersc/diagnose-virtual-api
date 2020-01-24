using DiagnoseVirtual.Domain.Dtos;
using DiagnoseVirtual.Domain.Entities;
using DiagnoseVirtual.Service.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiagnoseVirtual.Application.Controllers
{
    public class LavourasController : ControllerBase
    {
        private readonly BaseService<Lavoura> _lavouraService = new BaseService<Lavoura>();
        private readonly BaseService<DadosLavoura> _dadosLavouraService = new BaseService<DadosLavoura>();
        private readonly BaseService<Fazenda> _fazendaService = new BaseService<Fazenda>();
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly BaseService<Geometria> _geometriaService = new BaseService<Geometria>();


        public LavourasController(IWebHostEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        [Route("ConclusaoLavoura/{idLavoura}")]
        public ActionResult PostConcluirLavoura(int idLavoura)
        {
            var lavouraBd = _lavouraService.Get(idLavoura);

            if (lavouraBd == null)
                return BadRequest();

            lavouraBd.Concluida = true;

            _lavouraService.Put(lavouraBd);
            var response = new LavouraDto(lavouraBd);

            return Ok(response);
        }

        [HttpGet("{idLavoura}")]
        public ActionResult Get(int idLavoura)
        {
            var lavoura = _lavouraService.Get(idLavoura);

            var response = new LavouraDto(lavoura);

            return Ok(response);
        }

        [HttpGet]
        [Route("DadosLavoura/{idLavoura}")]
        public ActionResult GetDadosLavoura(int idLavoura)
        {
            var lavoura = _lavouraService.Get(idLavoura);
            if (lavoura == null || lavoura.DadosLavoura == null)
                return NotFound();

            return Ok(new DadosLavouraDto(lavoura.DadosLavoura));
        }

        [HttpGet]
        [Route("DemarcacaoLavoura/{idLavoura}")]
        public ActionResult GetDemarcacaoLavoura(int idLavoura)
        {
            var lavoura = _lavouraService.Get(idLavoura);
            if (lavoura == null || lavoura.Demarcacao == null)
                return NotFound();

            return Ok(new GeometriaDto(lavoura.Demarcacao));
        }

        [HttpGet]
        [Route("TalhoesLavoura/{idLavoura}")]
        public ActionResult GetTalhoesLavoura(int idLavoura)
        {
            var lavoura = _lavouraService.Get(idLavoura);
            if (lavoura == null || lavoura.Talhoes == null || !lavoura.Talhoes.Any())
                return NotFound();

            return Ok(lavoura.Talhoes.Select(t => new GeometriaDto(t.Geometria)));
        }

        [HttpPost]
        [Route("DadosLavoura/{idFazenda}")]
        public ActionResult PostDadosLavoura(DadosLavouraDto dadosLavoura, int idFazenda)
        {
            if (dadosLavoura == null)
                return BadRequest();

            var fazendaBd = _fazendaService.Get(idFazenda);
            if (fazendaBd == null)
                return BadRequest();

            var dadosLavouraBd = new DadosLavoura
            {
                Cultivar = dadosLavoura.Cultivar,
                EspacamentoHorizontal = dadosLavoura.EspacamentoHorizontal,
                EspacamentoVertical = dadosLavoura.EspacamentoVertical,
                MesAnoPlantio = dadosLavoura.MesAnoPlantio,
                Nome = dadosLavoura.Nome,
                NumeroPlantas = dadosLavoura.NumeroPlantas,
                Observacoes = dadosLavoura.Observacoes,
            };

            _dadosLavouraService.Post(dadosLavouraBd);
            var lavouraBd = new Lavoura
            {
                DadosLavoura = dadosLavouraBd
            };

            _lavouraService.Post(lavouraBd);

            if (fazendaBd.Lavouras == null)
                fazendaBd.Lavouras = new List<Lavoura>();
            fazendaBd.Lavouras.Add(lavouraBd);

            _fazendaService.Put(fazendaBd);

            var response = new LavouraDto(lavouraBd);

            return Ok(response);
        }

        [HttpPost]
        [Route("DemarcacaoLavoura/{idLavoura}")]
        public ActionResult PostDemarcacaoLavoura(GeometriaDto geometriaDemarcacao, int idLavoura)
        {
            if (geometriaDemarcacao == null)
                return BadRequest();

            var lavouraBd = _lavouraService.Get(idLavoura);

            if (lavouraBd == null)
                return BadRequest();

            var factory = Geometry.DefaultFactory;
            var polygon = factory.CreatePolygon(geometriaDemarcacao.Coordinates.Select(c => new Coordinate(c[0], c[1])).ToArray());
            var demarcacaoBd = factory.CreateGeometry(polygon);

            lavouraBd.Demarcacao = demarcacaoBd;

            _lavouraService.Put(lavouraBd);
            var response = new LavouraDto(lavouraBd);

            return Ok(response);
        }

        [HttpPost]
        [Route("TalhoesLavoura/{idLavoura}")]
        public ActionResult PostTalhoesLavoura(List<GeometriaDto> talhoes, int idLavoura)
        {
            if (talhoes == null || !talhoes.Any())
                return BadRequest();

            var lavouraBd = _lavouraService.Get(idLavoura);

            if (lavouraBd == null)
                return BadRequest();

            var factory = Geometry.DefaultFactory;
            var polygons = talhoes
                .Select(g => (Polygon)factory.CreatePolygon(g.Coordinates.Select(c => new Coordinate(c[0], c[1])).ToArray())).ToList();
            var geometrias = polygons.Select(p => factory.CreateGeometry(p))
                .Select(g => new Talhao { Geometria = g }).ToList();

            lavouraBd.Talhoes = geometrias;

            _lavouraService.Put(lavouraBd);
            var response = new LavouraDto(lavouraBd);

            return Ok(response);
        }

        [HttpPut]
        [Route("DadosLavoura/{idLavoura}")]
        public ActionResult PutDadosLavoura(DadosLavouraDto dadosLavoura, int idLavoura)
        {
            if (dadosLavoura == null)
                return BadRequest();

            var lavouraBd = _lavouraService.Get(idLavoura);
            if (lavouraBd == null && lavouraBd.DadosLavoura == null)
                return BadRequest();

            lavouraBd.DadosLavoura.Cultivar = dadosLavoura.Cultivar;
            lavouraBd.DadosLavoura.EspacamentoHorizontal = dadosLavoura.EspacamentoHorizontal;
            lavouraBd.DadosLavoura.EspacamentoVertical = dadosLavoura.EspacamentoVertical;
            lavouraBd.DadosLavoura.MesAnoPlantio = dadosLavoura.MesAnoPlantio;
            lavouraBd.DadosLavoura.Nome = dadosLavoura.Nome;
            lavouraBd.DadosLavoura.NumeroPlantas = dadosLavoura.NumeroPlantas;
            lavouraBd.DadosLavoura.Observacoes = dadosLavoura.Observacoes;

            _dadosLavouraService.Put(lavouraBd.DadosLavoura);

            var response = new LavouraDto(lavouraBd);

            return Ok(response);
        }

        [HttpPut]
        [Route("DemarcacaoLavoura/{idLavoura}")]
        public ActionResult PutDemarcacaoLavoura(GeometriaDto geometriaDemarcacao, int idLavoura)
        {
            if (geometriaDemarcacao == null)
                return BadRequest();

            var lavouraBd = _lavouraService.Get(idLavoura);

            if (lavouraBd == null || lavouraBd.Demarcacao == null)
                return BadRequest();

            var factory = Geometry.DefaultFactory;
            var polygon = factory.CreatePolygon(geometriaDemarcacao.Coordinates.Select(c => new Coordinate(c[0], c[1])).ToArray());
            lavouraBd.Demarcacao = factory.CreateGeometry(polygon);

            _lavouraService.Put(lavouraBd);
            var response = new LavouraDto(lavouraBd);

            return Ok(response);
        }

        [HttpPut]
        [Route("TalhoesLavoura/{idLavoura}")]
        public ActionResult PutTalhoesLavoura(List<GeometriaDto> talhoes, int idLavoura)
        {
            if (talhoes == null || !talhoes.Any())
                return BadRequest();

            var lavouraBd = _lavouraService.Get(idLavoura);

            if (lavouraBd == null || lavouraBd.Talhoes == null)
                return BadRequest();

            var factory = Geometry.DefaultFactory;
            var polygons = talhoes
                .Select(g => (Polygon)factory.CreatePolygon(g.Coordinates.Select(c => new Coordinate(c[0], c[1])).ToArray())).ToList();
            var geometrias = polygons.Select(p => (Geometry)factory.CreateGeometry(p))
                .Select(g => new Talhao { Geometria = g }).ToList();

            lavouraBd.Talhoes = geometrias;

            _lavouraService.Post(lavouraBd);
            var response = new LavouraDto(lavouraBd);

            return Ok(response);
        }
    }
}
