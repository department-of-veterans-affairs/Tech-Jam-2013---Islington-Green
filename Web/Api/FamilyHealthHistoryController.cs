using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Api
{
    public class FamilyHealthHistoryController : ApiController
    {
        private const string DateFormat = "dd MMM yyyy";

        private static readonly dynamic[] _familyHealthHistory =  {
            new {
                Id=1,
                Relationship= "Mother",
                FirstName= "Four",
                LastName= "MHVVeteranMother",
                LivingorDeceased= "Deceased",
                HealthIssue= "Cancer Other",
                HealthIssue2= "Diabetics Type 2",
                HealthIssue3= "Overweight",
                HealthIssue4= "Joint Pain",
                HealthIssue5= "Stroke"
            }
        };

        // GET api/appointment
        public IEnumerable<dynamic> Get()
        {
            return _familyHealthHistory;
        }

        // GET api/appointment/5
        public dynamic Get(int id)
        {
            return Array.Find(_familyHealthHistory, a => a.Id == id);
        }

        // POST api/appointment
        public void Post([FromBody]dynamic value)
        {
            _familyHealthHistory.ToList().Add(value);
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
                _familyHealthHistory.ToList().Remove(historyRecord);
            }
        }

    }
}
