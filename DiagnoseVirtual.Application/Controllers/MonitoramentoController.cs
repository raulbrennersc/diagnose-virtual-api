using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using DiagnoseVirtual.Application.Helpers;
using DiagnoseVirtual.Domain.Dtos;
using DiagnoseVirtual.Domain.Entities;
using DiagnoseVirtual.Infra.Data.Context;
using DiagnoseVirtual.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetTopologySuite.Geometries;

namespace DiagnoseVirtual.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class MonitoramentoController : ControllerBase
    {
        private readonly BaseService<Fazenda> _fazendaService;
        private readonly UsuarioService _usuarioService;
        private readonly BaseService<Monitoramento> _monitoramentoService;
        private readonly BaseService<ProblemaMonitoramento> _problemaMonitoramentoService;
        private readonly BaseService<UploadMonitoramento> _uploadMonitoramentoService;
        private readonly IWebHostEnvironment _webHostingEnvironment;

        // private readonly BaseService<LocalizacaoFazenda> _localizacaoService;
        // private readonly BaseService<DadosFazenda> _dadosFazendaService;
        private readonly PsqlContext _context = new PsqlContext();

        public MonitoramentoController(IWebHostEnvironment hostingEnvironment)
        {
            _fazendaService = new BaseService<Fazenda>(_context);
            _usuarioService = new UsuarioService(_context);
            _monitoramentoService = new BaseService<Monitoramento>(_context);
            _problemaMonitoramentoService = new BaseService<ProblemaMonitoramento>(_context);
            _uploadMonitoramentoService = new BaseService<UploadMonitoramento>(_context);
            _webHostingEnvironment = hostingEnvironment;
            // _localizacaoService = new BaseService<LocalizacaoFazenda>(_context);
            // _dadosFazendaService = new BaseService<DadosFazenda>(_context);
        }


        [HttpGet]
        public ActionResult Get()
        {
            var idUsuario = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var fazendas = _fazendaService.GetAll().Where(f => f.Usuario.Id == Int32.Parse(idUsuario)).ToList();

            var result = new List<MonitoramentoToListDto>();
            foreach (var fazenda in fazendas)
            {
                result.AddRange(fazenda.Monitoramentos.Select(m => new MonitoramentoToListDto(m)));
            }

            return Ok(result);
        }

        [HttpGet("{idMonitoramento}")]
        public ActionResult Get(int idMonitoramento)
        {
            var monitoramento = _monitoramentoService.Get(idMonitoramento);
            if (monitoramento == null)
                return NotFound(Constants.ERR_MONITORAMENTO_NAO_ENCONTRADO);

            return Ok(new MonitoramentoDetailDto(monitoramento));
        }

        [HttpPost]
        [Route("{idFazenda}")]
        public ActionResult Concluir(MonitoramentoPostDto monitoramentoDto, int idFazenda)
        {
            var fazenda = _fazendaService.Get(idFazenda);
            if (fazenda == null)
                return BadRequest(Constants.ERR_REQ_INVALIDA);

            if (fazenda.Concluida)
                return BadRequest(Constants.ERR_REQ_INVALIDA);

            var monitoramento = new Monitoramento { Fazenda = fazenda, DataMonitoramento = DateTime.Now };
            
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _monitoramentoService.Post(monitoramento);
                    _problemaMonitoramentoService.Post(MontarProblemas(monitoramentoDto, monitoramento));
                    _uploadMonitoramentoService.Post(MontarUploads(monitoramentoDto, monitoramento));
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

        private List<ProblemaMonitoramento> MontarProblemas(MonitoramentoPostDto monitoramentoDto, Monitoramento monitoramento){
            var problemasMonitoramento = new List<ProblemaMonitoramento>();
            var factory = Geometry.DefaultFactory;
            foreach (var problemaDto in monitoramento.Problemas)
            {
                var ponto = factory.CreatePoint(new Coordinate(problemaDto.Ponto.Coordinate.X, problemaDto.Ponto.Coordinate.Y));
                var problema = new ProblemaMonitoramento
                {
                    Descricao = problemaDto.Descricao,
                    Monitoramento = monitoramento,
                    Recomendacao = problemaDto.Recomendacao,
                    Ponto = ponto,
                };
                problemasMonitoramento.Add(problema);
            }
            return problemasMonitoramento;
        }

        private List<UploadMonitoramento> MontarUploads(MonitoramentoPostDto monitoramentoDto, Monitoramento monitoramento){
            var uploads = new List<UploadMonitoramento>();
            var basePath = _webHostingEnvironment.ContentRootPath;
            var caminho = $"/arquivos/fazendas/{monitoramento.Fazenda.Id}/monitoramentos/{monitoramento.Id}/uploads_monitoramento/";
            var caminhoFinal = Path.Combine(basePath, caminho);
            if(monitoramentoDto.Uploads.Any())
                Directory.CreateDirectory(caminhoFinal);

            foreach (var uploadDto in monitoramentoDto.Uploads)
            {
                var nomeArquivo = uploadDto.FileName;
                var finalNomeArquivo = "";
                var count = 0;
                while (uploads.Any(u => u.NomeArquivo == (nomeArquivo + finalNomeArquivo)))
                {
                    count++;
                    finalNomeArquivo = $"({count})";
                }

                nomeArquivo += finalNomeArquivo;

                var filePath = Path.Combine(caminhoFinal, nomeArquivo);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    uploadDto.CopyTo(fileStream);
                }
                var upload = new UploadMonitoramento
                {
                    Monitoramento = monitoramento,
                    NomeArquivo = uploadDto.FileName,
                    Url = filePath,
                };
            }
            return uploads;
        }


        [HttpPost]
        [Route("teste")]
        public ActionResult Teste(IFormFile file)
        {
            return Ok(file);
        }

    }
}