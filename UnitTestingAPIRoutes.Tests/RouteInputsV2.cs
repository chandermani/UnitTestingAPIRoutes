using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnitTestingAPIRoutes.Controllers.Api;
using Xunit;

namespace UnitTestingAPIRoutes.Tests
{
    public class RouteInputsV2
    {
        private const string BASEURL = "http://localhost/api/v2";
        public static TheoryData<RouteTheoryInput> UserEndpoints = new TheoryData<RouteTheoryInput>()
        {
            new RouteTheoryInput() {Endpoint = $"{BASEURL}/users", ControllerType = typeof(UsersController), ActionName = "Get", ParameterNames = new string[] { } },
            new RouteTheoryInput() {Endpoint = $"{BASEURL}/users/1", ControllerType = typeof(UsersController), ActionName = "Get", ParameterNames = new string[] { "id" } },
            new RouteTheoryInput() {Endpoint = $"{BASEURL}/users", ControllerType = typeof(UsersController), ActionName = "Post", ParameterNames = new string[] { "user" }, HttpMethod=HttpMethod.Post },
            new RouteTheoryInput() {Endpoint = $"{BASEURL}/users/2", ControllerType = typeof(UsersController), ActionName = "Put", ParameterNames = new string[] { "user", "id"}, HttpMethod=HttpMethod.Put  },
            new RouteTheoryInput() {Endpoint = $"{BASEURL}/users/3", ControllerType = typeof(UsersController), ActionName = "Delete", ParameterNames = new string[] { "id" }, HttpMethod=HttpMethod.Delete  },

            new RouteTheoryInput() {Endpoint = $"{BASEURL}/users/special", ControllerType = typeof(UsersController), ActionName = "GetSpecial", ParameterNames = new string[] { } },
        };
    }
}
