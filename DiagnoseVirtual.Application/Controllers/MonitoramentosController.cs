using DiagnoseVirtual.Application.Helpers;
using DiagnoseVirtual.Domain.Dtos;
using DiagnoseVirtual.Domain.Entities;
using DiagnoseVirtual.Infra.Data.Context;
using DiagnoseVirtual.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _config;
        private readonly PsqlContext _context;

        public MonitoramentosController(IWebHostEnvironment hostingEnvironment, IHttpClientFactory httpClientFactory, IConfiguration config, PsqlContext context)
        {
            _context = context;
            _fazendaService = new BaseService<Fazenda>(_context);
            _usuarioService = new UsuarioService(_context);
            _monitoramentoService = new BaseService<Monitoramento>(_context);
            _problemaMonitoramentoService = new BaseService<ProblemaMonitoramento>(_context);
            _uploadMonitoramentoService = new BaseService<UploadMonitoramento>(_context);
            _webHostingEnvironment = hostingEnvironment;
            _httpClientFactory = httpClientFactory;
            _config = config;
        }


        [HttpGet]
        [ProducesResponseType(typeof(List<MonitoramentoToListDto>), StatusCodes.Status200OK)]
        public ActionResult Get()
        {
            var idUsuario = HttpContext.User.FindFirst("IdUsuario").Value;
            var fazendas = _fazendaService.GetAll().Where(f => f.Usuario.Id == int.Parse(idUsuario)).ToList();

            var result = new List<MonitoramentoToListDto>();
            foreach (var fazenda in fazendas)
            {
                result.AddRange(fazenda.Monitoramentos.Select(m => new MonitoramentoToListDto(m)));
            }

            return Ok(result);
        }

        [HttpGet("Fazendas")]
        [ProducesResponseType(typeof(List<FazendaMinDto>), StatusCodes.Status200OK)]
        public ActionResult GetFazendasMonitoramento()
        {
            var idUsuario = HttpContext.User.FindFirst("IdUsuario").Value;
            var fazendas = _fazendaService.GetAll().Where(f => f.Ativa && f.Usuario.Id == int.Parse(idUsuario) && f.Lavouras.Any(l => l.Concluida))
                .ToList().Select(f => new FazendaMinDto(f));

            return Ok(fazendas);
        }

        [HttpGet("Fazenda/{idFazenda}")]
        [ProducesResponseType(typeof(MonitoramentoDetailDto), StatusCodes.Status200OK)]
        public ActionResult GetFazenda(int idFazenda)
        {
            var fazenda = _fazendaService.Get(idFazenda);
            if (fazenda == null)
            {
                return NotFound(Constants.ERR_MONITORAMENTO_NAO_ENCONTRADO);
            }

            var urlpdi = "";
            if (!string.IsNullOrEmpty(fazenda.IdPdi))
            {
                var responsePdi = (List<PdiQueryDto>)HttpRequestHelper.MakeJsonRequest<List<PdiQueryDto>>(_httpClientFactory.CreateClient(), $"{_config.GetSection("AppSettings:UrlPdi").Value}/query", PdiHttpReqHelper.PdiQueryReq(fazenda.IdPdi));
                if((responsePdi?.Any() ?? false ) && (responsePdi.FirstOrDefault().Imgs?.Any() ?? false) && (responsePdi.FirstOrDefault().Imgs.FirstOrDefault()?.Images.Any() ?? false) )
                {
                    var images = responsePdi.FirstOrDefault().Imgs.FirstOrDefault().Images;
                    urlpdi = images.FirstOrDefault(i => i.EndsWith(".png") && i.Contains("ndvi") );
                }
            }
            

            var response = new MonitoramentoDetailDto(fazenda)
            {
                UrlPdi = urlpdi
            };

            return Ok(response);
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
                    var uploads = monitoramentoDto.Uploads == null ? null :  MontarUploads(monitoramentoDto, monitoramento);
                    _problemaMonitoramentoService.Post(problemas);
                    if(uploads != null)
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
            foreach (var problemaDto in monitoramentoDto.Problemas)
            {
                var problema = new ProblemaMonitoramento
                {
                    Descricao = problemaDto.Descricao,
                    Monitoramento = monitoramento,
                    Recomendacao = problemaDto.Recomendacao,
                    Ponto = problemaDto.Ponto,
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
                        //Url = filePath.Replace(basePath, "18.229.125.193"),
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

        [HttpPost("upload")]
        [ProducesResponseType(typeof(MonitoramentoDetailDto), StatusCodes.Status200OK)]
        public async Task<IActionResult> Upload(IFormFile upload)
        {
            var memoryStream = new MemoryStream();
            await upload.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            var bytes = memoryStream.ToArray();
            var fileName = upload.FileName;
            var download = File(bytes, "multipart/form-data", fileName);


            return download;
        }

    }
}