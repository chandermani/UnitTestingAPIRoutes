using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestingAPIRoutes.Tests
{
    public class RouteTheoryInput
    {
        public RouteTheoryInput()
        {
            this.ParameterNames = new string[] { };
            this.HttpMethod = HttpMethod.Get;
            this.ResponseStatusCode = HttpStatusCode.OK;
        }

        public string Endpoint { get; set; }
        public Type ControllerType { get; set; }
        public string ActionName { get; set; }
        public string[] ParameterNames { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public HttpStatusCode ResponseStatusCode { get; set; }
    }
}
