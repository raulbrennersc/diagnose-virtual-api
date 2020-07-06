using System;
using System.Collections.Generic;
using System.Text;

namespace DiagnoseVirtual.Domain.Exceptions
{
    public class InvalidTypeException : SystemException
    {
        public InvalidTypeException(string message) : base(message) { }
    }
}
