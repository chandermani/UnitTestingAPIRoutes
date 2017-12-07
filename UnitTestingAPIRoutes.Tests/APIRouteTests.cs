using System;
using System.Net.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http;
using System.Collections.ObjectModel;
using Moq;
using FluentAssertions;
using System.Web.Http.Controllers;
using Xunit;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnitTestingAPIRoutes.Controllers.Api;

namespace UnitTestingAPIRoutes.Tests
{
    public class APIRouteTests : IClassFixture<RouteFixture>
    {
        private readonly RouteFixture routeFixture;

        public APIRouteTests(RouteFixture routeFixture)
        {
            this.routeFixture = routeFixture;
        }

        [Theory]
        [MemberData(nameof(RouteInputsV1.UserEndpoints), MemberType = typeof(RouteInputsV1))]
        public void Should_Resolve_V1_Routes_For_Users_API(RouteTheoryInput routeInput)
        {
            Should_Hit_Correct_Controller_Action_On_API_Calls(routeInput);
        }

        [Theory]
        [MemberData(nameof(RouteInputsV2.UserEndpoints), MemberType = typeof(RouteInputsV2))]
        public void Should_Resolve_V2_Routes_For_Users_API(RouteTheoryInput routeInput)
        {
            // The test fails, unless you go and uncomment //[Route("v2/users/{id:int}")] on UsersController.
            Should_Hit_Correct_Controller_Action_On_API_Calls(routeInput);
        }

        /// <summary>
        /// Test to highlight how to test a single route.
        /// </summary>
        [Fact]
        public void Should_Resolve_V1_Get_Users_Route()
        {
            var configuration = new HttpConfiguration();

            WebApiConfig.Register(configuration);

            var controllerTypeResolver = new Mock<IHttpControllerTypeResolver>();

            var controllerTypes = GetAllControllerTypes();


            controllerTypeResolver.Setup(r => r.GetControllerTypes(It.IsAny<IAssembliesResolver>())).Returns(controllerTypes);
            configuration.Services.Replace(typeof(IHttpControllerTypeResolver), controllerTypeResolver.Object);
            configuration.EnsureInitialized();

            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/v1/users");

            var routeData = configuration.Routes.GetRouteData(request);

            request.SetConfiguration(configuration);
            if (routeData != null)
            {
                // For incorrectly formed route url,route data is null. This request may fail later and we can check status code.
                request.SetRouteData(routeData);
            }

            var controllerSelector = configuration.Services.GetHttpControllerSelector();
            var actionSelector = configuration.Services.GetActionSelector();

            //act
            var controllerDescriptor = controllerSelector.SelectController(request);
            var controllerContext = new HttpControllerContext(configuration, routeData, request)
            {
                ControllerDescriptor = controllerDescriptor,
                RequestContext = new HttpRequestContext()
                {
                    Configuration = configuration,
                    RouteData = routeData
                }
            };

            var actionDescriptor = actionSelector.SelectAction(controllerContext);

            // assert
            controllerDescriptor.ControllerType.Should().Be(typeof(UsersController));

            actionDescriptor.ActionName.Should().Be("Get");

            actionDescriptor.GetParameters().Select(p => p.ParameterName).Should().BeEquivalentTo(new string[] { });
        }

        private void Should_Hit_Correct_Controller_Action_On_API_Calls(RouteTheoryInput input)
        {
            // Arrange
            var configuration = routeFixture.Configuration;

            var request = new HttpRequestMessage(input.HttpMethod, input.Endpoint);

            var routeData = configuration.Routes.GetRouteData(request);

            request.SetConfiguration(configuration);
            if (routeData != null)
            {
                // For incorrectly formed route url,route data is null. This request may fail later and we can check status code.
                request.SetRouteData(routeData);
            }

            try
            {
                var controllerSelector = configuration.Services.GetHttpControllerSelector();
                var actionSelector = configuration.Services.GetActionSelector();

                var controllerDescriptor = controllerSelector.SelectController(request);
                var controllerContext = new HttpControllerContext(configuration, routeData, request)
                {
                    ControllerDescriptor = controllerDescriptor,
                    RequestContext = new HttpRequestContext()
                    {
                        Configuration = configuration,
                        RouteData = routeData
                    }
                };

                //act
                var controller = controllerSelector.SelectController(request);
                var action = actionSelector.SelectAction(controllerContext);

                // assert
                controller.ControllerType.Should().Be(input.ControllerType);

                action.ActionName.Should().Be(input.ActionName);

                action.GetParameters().Select(p => p.ParameterName).Should().BeEquivalentTo(input.ParameterNames);
            }
            catch (HttpResponseException ex)
            {
                ex.Response.StatusCode.Should().Be(input.ResponseStatusCode);
            }
        }

        private Collection<Type> GetAllControllerTypes()
        {
            Collection<Type> controllerTypes = new Collection<Type>();
            var baseControllerType = typeof(ApiController);
            Assembly.GetAssembly(typeof(MvcApplication))
                .GetTypes().Where(t => baseControllerType.IsAssignableFrom(t)).ToList().ForEach(t => controllerTypes.Add(t));

            return controllerTypes;
        }

    }

    public class RouteFixture
    {
        public HttpConfiguration Configuration { get; set; }

        public RouteFixture()
        {
            this.Configuration = new HttpConfiguration();
            WebApiConfig.Register(this.Configuration);

            var controllerTypeResolver = new Mock<IHttpControllerTypeResolver>();
            var controllerTypes = GetAllControllerTypes();


            controllerTypeResolver.Setup(r => r.GetControllerTypes(It.IsAny<IAssembliesResolver>())).Returns(controllerTypes);
            this.Configuration.Services.Replace(typeof(IHttpControllerTypeResolver), controllerTypeResolver.Object);
            this.Configuration.EnsureInitialized();
        }


        private Collection<Type> GetAllControllerTypes()
        {
            Collection<Type> controllerTypes = new Collection<Type>();
            var baseControllerType = typeof(ApiController);
            Assembly.GetAssembly(typeof(MvcApplication))
                .GetTypes().Where(t => baseControllerType.IsAssignableFrom(t)).ToList().ForEach(t => controllerTypes.Add(t));

            return controllerTypes;
        }
    }
}