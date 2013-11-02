using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Api
{
    public class VitalsAndReadingsController : ApiController
    {
        static IG.Analyitcs.WeightComparer comparer = new IG.Analyitcs.WeightComparer();

        private const string DateFormat = "dd MMM yyyy";

        private static readonly dynamic[] _vitalsAndReadings =  {
            new {
                Id=1,
                MeasurementType= "Blood pressure",
                Date= DateTime.ParseExact("02 Aug 2010",DateFormat,CultureInfo.InvariantCulture),
                Time= "1720",
                Systolic= "130",
                Diastolic= "76",
                Comments= "BP taken lying down "
            },
            new
            {
                MeasurementType= "Body weight",
                Date= DateTime.ParseExact("02 Apr 2010",DateFormat,CultureInfo.InvariantCulture),
                Time= "1720",
                BodyWeight= "246",
                DifferenceToCohort = comparer.GetDifferenceToCohort(246,250.0),
                Measure= "Pounds",
                Comments= "Talk to provider about weight management program at next visit  "           
            }
        };



        // GET api/appointment
        public IEnumerable<dynamic> Get()
        {
            return _vitalsAndReadings;
        }

        // GET api/appointment/5
        public dynamic Get(int id)
        {
            return Array.Find(_vitalsAndReadings, a => a.Id == id);
        }

        // POST api/appointment
        public void Post([FromBody]dynamic value)
        {
            _vitalsAndReadings.ToList().Add(value);
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
                _vitalsAndReadings.ToList().Remove(historyRecord);
            }
        }
    }
}
