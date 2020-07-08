using DiagnoseVirtual.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DiagnoseVirtual.Application.Helpers
{
    internal class MResult
    {
        public string Message { get; set; }
        public object Result { get; set; }
    }

    public class HttpResponseHelper
    {

        internal static ObjectResult Create(HttpStatusCode status, string message, object result = null)
        {

            if (result == null)
            {
                result = new { };
            }
            else if (result is IEnumerable)
            {
                throw new InvalidTypeException("A propriedade Result não pode ser uma lista ou array de items");
            }

            var mResult = new MResult { Message = message, Result = result };

            return new ObjectResult(mResult)
            {
                StatusCode = (int)status
            };

        }
    }
}
