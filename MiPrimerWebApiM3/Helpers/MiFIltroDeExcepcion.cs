using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerWebApiM3.Helpers
{
    public class MiFIltroDeExcepcion : ExceptionFilterAttribute , IExceptionFilter
    {
        //https://www.thecodebuzz.com/exception-filters-in-net-core/
        public override void OnException(ExceptionContext context)
        {
            //Logs your technical exception with stack trace below

            context.Result = new JsonResult("Something went wrong! Internal Server Error.");
        }
    }
}
