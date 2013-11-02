using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Api
{
    public class RecoveryPlanController : ApiController
    {
        private const string DateFormat = "dd MMM yyyy";


        // GET api/recoveryplan
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/recoveryplan/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/recoveryplan
        public void Post([FromBody]string value)
        {
        }

        // PUT api/recoveryplan/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/recoveryplan/5
        public void Delete(int id)
        {
        }
    }
}
