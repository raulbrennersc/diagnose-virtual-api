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
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;

namespace DiagnoseVirtual.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MonitoramentosController : ControllerBase
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

        public MonitoramentosController(IWebHostEnvironment hostingEnvironment)
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
        [ProducesResponseType(typeof(List<MonitoramentoToListDto>), StatusCodes.Status200OK)]
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
        [ProducesResponseType(typeof(MonitoramentoDetailDto), StatusCodes.Status200OK)]
        public ActionResult Get(int idMonitoramento)
        {
            var monitoramento = _monitoramentoService.Get(idMonitoramento);
            if (monitoramento == null)
            {
                return NotFound(Constants.ERR_MONITORAMENTO_NAO_ENCONTRADO);
            }

            return Ok(new MonitoramentoDetailDto(monitoramento));
        }

        [HttpDelete("{idMonitoramento}")]
        public ActionResult Delete(int idMonitoramento)
        {
            var monitoramento = _monitoramentoService.Get(idMonitoramento);
            if (monitoramento == null)
            {
                return NotFound(Constants.ERR_MONITORAMENTO_NAO_ENCONTRADO);
            }
            using(var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    monitoramento.Ativo = false;
                    _monitoramentoService.Put(monitoramento);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
                }
            }


            return Ok(new MonitoramentoDetailDto(monitoramento));
        }

        [HttpPost("Filtrar/")]
        [ProducesResponseType(typeof(MonitoramentoDetailDto), StatusCodes.Status200OK)]
        public ActionResult Consultar(FiltroDto filtro)
        {
            if (filtro.IdFazenda == 0 || filtro.Data == DateTime.MinValue)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            var fazenda = _fazendaService.Get(filtro.IdFazenda);
            var monitoramento = fazenda.Monitoramentos
                .FirstOrDefault(m => m.DataMonitoramento.Date == filtro.Data.Date);
            if (monitoramento == null)
            {
                return NotFound(Constants.ERR_MONITORAMENTO_NAO_ENCONTRADO);
            }

            return Ok(new MonitoramentoDetailDto(monitoramento));
        }

        [HttpPost]
        public ActionResult Post(MonitoramentoPostDto monitoramentoDto)
        {
            var fazenda = _fazendaService.Get(monitoramentoDto.IdFazenda);
            if (fazenda == null)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            if (!fazenda.Concluida)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            var monitoramento = new Monitoramento { Fazenda = fazenda, DataMonitoramento = DateTime.Now };

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    _monitoramentoService.Post(monitoramento);
                    var problemas = MontarProblemas(monitoramentoDto, monitoramento);
                    var uploads = MontarUploads(monitoramentoDto, monitoramento);
                    _problemaMonitoramentoService.Post(problemas);
                    _uploadMonitoramentoService.Post(uploads);
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

        [HttpPut("{idMonitoramento}")]
        public ActionResult Put(MonitoramentoPostDto monitoramentoDto, int idMonitoramento)
        {
            var monitoramento = _monitoramentoService.Get(idMonitoramento);
            if (monitoramento == null)
            {
                return BadRequest(Constants.ERR_REQ_INVALIDA);
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var idsProblemas = monitoramento.Problemas.Select(p => p.Id).ToList();
                    foreach (var idProblema in idsProblemas)
                    {
                        _problemaMonitoramentoService.Delete(idProblema);
                    }
                    var idsUploads = monitoramento.Uploads.Select(u => u.Id).ToList();
                    foreach (var idUpload in idsUploads)
                    {
                        _uploadMonitoramentoService.Delete(idUpload);
                    }

                    monitoramento.DataMonitoramento = DateTime.Now;
                    _monitoramentoService.Put(monitoramento);
                    var problemas = MontarProblemas(monitoramentoDto, monitoramento);
                    var uploads = MontarUploads(monitoramentoDto, monitoramento);
                    _problemaMonitoramentoService.Post(problemas);
                    _uploadMonitoramentoService.Post(uploads);
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

        private List<ProblemaMonitoramento> MontarProblemas(MonitoramentoPostDto monitoramentoDto, Monitoramento monitoramento)
        {
            var problemasMonitoramento = new List<ProblemaMonitoramento>();
            var factory = Geometry.DefaultFactory;
            foreach (var problemaDto in monitoramentoDto.Problemas)
            {
                var ponto = factory.CreatePoint(new Coordinate(problemaDto.Ponto.Coordinates[0][0], problemaDto.Ponto.Coordinates[0][1]));
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

        private List<UploadMonitoramento> MontarUploads(MonitoramentoPostDto monitoramentoDto, Monitoramento monitoramento)
        {
            var uploads = new List<UploadMonitoramento>();
            var basePath = _webHostingEnvironment.ContentRootPath;
            var caminho = $"files\\fazendas\\{monitoramento.Fazenda.Id}\\monitoramentos\\{monitoramento.Id}\\uploads_monitoramento\\";
            var caminhoFinal = Path.Combine(basePath, caminho);
            var arquivosExistentes = new List<string>();
            if (monitoramentoDto.Uploads.Any())
            {
                Directory.CreateDirectory(caminhoFinal);
                arquivosExistentes = Directory.GetFiles(caminhoFinal).ToList();
                arquivosExistentes = arquivosExistentes.Select(x => x.Split("\\").LastOrDefault()).ToList();
            }

            foreach (var uploadDto in monitoramentoDto.Uploads)
            {
                try
                {
                    var nomeArquivo = uploadDto.FileName;
                    var finalNomeArquivo = "";
                    var count = 0;
                    while (ExisteArquivo(uploads, arquivosExistentes, nomeArquivo, finalNomeArquivo))
                    {
                        count++;
                        finalNomeArquivo = $"({count})";
                    }

                    var extensao = "." + nomeArquivo.Split(".").Last();
                    var nomeSimplificado = nomeArquivo.Replace(extensao, "");
                    nomeArquivo = nomeSimplificado + finalNomeArquivo + extensao;

                    var filePath = Path.Combine(caminhoFinal, nomeArquivo);
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        uploadDto.CopyTo(fileStream);
                    }
                    var upload = new UploadMonitoramento
                    {
                        Monitoramento = monitoramento,
                        NomeArquivo = nomeArquivo,
                        Url = filePath.Replace(basePath, "18.229.125.193"),
                    };
                    uploads.Add(upload);
                }
                catch
                {
                    throw new Exception($"Erro ao tentar salvar o arquivo {uploadDto.FileName}");
                }
            }
            return uploads;
        }

        private bool ExisteArquivo(List<UploadMonitoramento> uploads, List<string> arquivosExistentes, string nomeArquivo, string finalNomeArquivo)
        {
            var extensao = "." + nomeArquivo.Split(".").Last();
            var nomeSimplificado = nomeArquivo.Replace(extensao, "");
            var nomeArquivoModificado = nomeSimplificado + finalNomeArquivo + extensao;


            return uploads.Any(u => u.NomeArquivo == nomeArquivoModificado)
                || arquivosExistentes.Any(a => a == nomeArquivoModificado);
        }

        [HttpPost]
        [Route("teste")]
        public ActionResult Teste([FromForm]MonitoramentoPostDto monitoramento)
        {
            return Ok();
        }

    }
}