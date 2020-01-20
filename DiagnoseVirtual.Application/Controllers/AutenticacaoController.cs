using DiagnoseVirtual.Domain.Entities;
using DiagnoseVirtual.Service.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiagnoseVirtual.Application.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutenticacaoController : ControllerBase
    {
        private BaseService<Usuario> sUsuario = new BaseService<Usuario>();

    }
}
