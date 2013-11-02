using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Api
{
    public class ImmunizationsController : ApiController
    {
        private const string DateFormat = "dd MMM yyyy";

        private static readonly dynamic[] _immunization =  {
            new {
                Id=1,
                Immunization= "Measles + Rubella (German Measles)",
                Other= "none ",
                Method= "Injection",
                DateReceived= DateTime.ParseExact("01 Mar 1950",DateFormat,CultureInfo.InvariantCulture),
                Reactions= "Pain",
            }
        };



        // GET api/appointment
        public IEnumerable<dynamic> Get()
        {
            return _immunization;
        }

        // GET api/appointment/5
        public dynamic Get(int id)
        {
            return Array.Find(_immunization, a => a.Id == id);
        }

        // POST api/appointment
        public void Post([FromBody]dynamic value)
        {
            _immunization.ToList().Add(value);
        }

        // PUT api/appointment/5
        public void Put(int id, [FromBody]dynamic value)
        {
            var historyRecord = this.Get(id);
            if (historyRecord != null)
            {
                historyRecord = value;
            }
        }

        // DELETE api/appointment/5
        public void Delete(int id)
        {
            var historyRecord = this.Get(id);
            if (historyRecord != null)
            {
                _immunization.ToList().Remove(historyRecord);
            }
        }
    }
}
