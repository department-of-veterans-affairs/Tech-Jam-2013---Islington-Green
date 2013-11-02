using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Api
{
    public class MilitaryHealthHistoryController : ApiController
    {
        private const string DateFormat = "dd MMM yyyy";

        private static readonly dynamic[] _militaryHealthHistory =  {
            new {
                Id=1,
                EventTitle= "Exposure to Burning Chemicals",
                EventDate= DateTime.ParseExact("01 Jan 2007",DateFormat,CultureInfo.InvariantCulture),
                ServiceBranch= "Army"     ,                        
                Rank= "COL",
                Exposures= "Yes",
                LocationofService= "Overseas   "  ,       
                OnboardShip= "No",
                MilitaryOccupationalSpecialty= "Intelligence Officer",
                Assignment= "1st Recon ",
                ExposuresType= "Exposed to burning chemicals ",
                MilitaryServiceDescription= "Unit was in charge of security "
            }
        };

        // GET api/appointment
        public IEnumerable<dynamic> Get()
        {
            return _militaryHealthHistory;
        }

        // GET api/appointment/5
        public dynamic Get(int id)
        {
            return Array.Find(_militaryHealthHistory, a => a.Id == id);
        }

        // POST api/appointment
        public void Post([FromBody]dynamic value)
        {
            _militaryHealthHistory.ToList().Add(value);
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
                _militaryHealthHistory.ToList().Remove(historyRecord);
            }
        }


    }
}
