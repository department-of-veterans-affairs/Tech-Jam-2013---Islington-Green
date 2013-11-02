using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Api
{
    public class MedicalEventsController : ApiController
    {
        private const string DateFormat = "dd MMM yyyy";

        private static readonly dynamic[] _medicalEvents =  {
            new {
                Id=1,
                MedicalEvent=   "Colonoscopy",
                StartDate=      DateTime.ParseExact("18 Mar 2000",DateFormat,CultureInfo.InvariantCulture),
                StopDate=       DateTime.ParseExact("18 Mar 2000",DateFormat,CultureInfo.InvariantCulture),
                Response= "Colonoscopy when well" 
            }
        };




        // GET api/appointment
        public IEnumerable<dynamic> Get()
        {
            return _medicalEvents;
        }

        // GET api/appointment/5
        public dynamic Get(int id)
        {
            return Array.Find(_medicalEvents, a => a.Id == id);
        }

        // POST api/appointment
        public void Post([FromBody]dynamic value)
        {
            _medicalEvents.ToList().Add(value);
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
                _medicalEvents.ToList().Remove(historyRecord);
            }
        }
    }
}
