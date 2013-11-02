using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Api
{
    public class TreatmentPlanController : ApiController
    {

        private const string DateFormat = "dd MMM yyyy @ HHmm";

        private readonly dynamic[] _appointments =  {
            new {
            Id = 1,
            DateTime=DateTime.ParseExact("13 Oct 2011 @ 1600",DateFormat,CultureInfo.InvariantCulture),
            Location="DAYT29 TEST LAB",
            Status="NOT APPLICABLE",
            Clinic="C&P CHRISTIE",
            PhoneNumber="3929",
            Type="Compensation and Pension Appointment"
            }
        };
        
        
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/treatmentplan/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/treatmentplan
        public void Post([FromBody]string value)
        {
        }

        // PUT api/treatmentplan/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/treatmentplan/5
        public void Delete(int id)
        {
        }
    }
}
