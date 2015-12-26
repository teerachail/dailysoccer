using ApiApp.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ApiApp.Controllers
{
    /// <summary>
    /// The default api controller from the template
    /// </summary>
    public class ValuesController : ApiController
    {
        private IDemoRepository repo;

        /// <summary>
        /// The ValuesController constructor
        /// </summary>
        /// <param name="repo"></param>
        public ValuesController(IDemoRepository repo)
        {
            this.repo = repo;
        }

        /// <summary>
        /// List all values
        /// </summary>
        /// <returns>The values</returns>
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { this.repo.GetEmail("my-user-id"), "value1", "value2" };
        }

        /// <summary>
        /// Get the specified value
        /// </summary>
        /// <param name="id">The ref id</param>
        /// <returns>The specified value</returns>
        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// Post a new value
        /// </summary>
        /// <param name="value">The new value</param>
        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        /// <summary>
        /// Update the modified value
        /// </summary>
        /// <param name="id">The ref id</param>
        /// <param name="value">The value to be updated</param>
        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        /// <summary>
        /// Delete the specified value
        /// </summary>
        /// <param name="id">The ref id</param>
        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
