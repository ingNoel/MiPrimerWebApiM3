using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiPrimerWebApiM3.Services
{
    public class ClaseB : IClaseB
    {
        private ILogger<ClaseB> logger;
        public ClaseB(ILogger<ClaseB> logger)
        {
            this.logger = logger;
        }
        public void HacerAlgo()
        {
            logger.LogInformation("Ejecutando el metodo " + nameof(HacerAlgo));
        }
    }
    public class ClaseB2 : IClaseB
    {
        public void HacerAlgo()
        {

        }
    }
}
