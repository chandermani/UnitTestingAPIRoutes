using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace UnitTestingAPIRoutes.Controllers.Api
{
    [RoutePrefix("api")]
    public class UsersController : ApiController
    {
        private List<User> userStore = new List<User>()
        {
            new User() { Id=1, Name="Tom"},
            new User() { Id=2, Name="Jane"},
            new User() { Id=3, Name="Ana"},
            new User() { Id=4, Name="Ruth"},
            new User() { Id=4, Name="John"}

        };
        // GET: api/users
        [Route("v1/users")]
        [Route("v2/users")]
        public IEnumerable<User> Get()
        {
            return userStore;
        }

        // GET: api/users/special
        [Route("v2/users/special")]
        public IEnumerable<User> GetSpecial()
        {
            return userStore.Take(3);
        }

        // GET: api/users/5
        [Route("v1/users/{id:int}")]
        // Uncomment the line below to fix the unit tests.
        //[Route("v2/users/{id:int}")]
        public User Get(int id)
        {
            return userStore.FirstOrDefault(u => u.Id == id);
        }

        // POST: api/users
        [Route("v1/users")]
        [Route("v2/users")]
        public void Post([FromBody]User user)
        {
            userStore.Add(user);
        }

        // PUT: api/users/5
        [Route("v1/users/{id:int}")]
        [Route("v2/users/{id:int}")]
        public void Put(int id, [FromBody]User user)
        {
            var existingUser = userStore.FirstOrDefault(u => u.Id == id);

            if (existingUser != null)
            {
                existingUser.Name = user.Name;
            }
        }

        // DELETE: api/users/5
        [Route("v1/users/{id:int}")]
        [Route("v2/users/{id:int}")]
        public void Delete(int id)
        {
            var existingUser = userStore.FirstOrDefault(u => u.Id == id);
            if (existingUser != null)
            {
                userStore.Remove(existingUser);
            }
        }
    }

    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
