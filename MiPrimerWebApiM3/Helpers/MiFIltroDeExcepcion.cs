using Amazon.CodePipeline.Model;
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
            var error = new ErrorDetails()
            {
                Code = Convert.ToString(500),
                Message = "Something went wrong! Internal Server Error."
            };

            //Logs your technical exception with stack trace below

            context.Result = new JsonResult(error);
        }
    }
}
