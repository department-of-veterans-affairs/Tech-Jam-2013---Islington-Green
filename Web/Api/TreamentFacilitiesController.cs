using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Api
{
    public class TreamentFacilitiesController : ApiController
    {
        private const string DateFormat = "dd MMM yyyy";

        private static readonly dynamic[] _treatmentFacilities =  {
            new {
                Id=1,
                FacilityName= "Anywhere VA Medical Center",
                FacilityType= "VA     "       ,
                VAHomeFacility= "Yes",
                PhoneNumber= "000-000-0001   ",
                Ext= "1234",
                MailingAddress= "123 VA Drive",
                MailingAddress2= "Suite 4",
                MailingCity= "Anywhere",
                MailingState= "DC",
                MailingCountry= "United States",
                MailingProvince=  "",
                MailingZipPostalCode= "00001",
                Comments= "Contact clinic when calling to make my appointments." 
            }
        };




        // GET api/appointment
        public IEnumerable<dynamic> Get()
        {
            return _treatmentFacilities;
        }

        // GET api/appointment/5
        public dynamic Get(int id)
        {
            return Array.Find(_treatmentFacilities, a => a.Id == id);
        }

        // POST api/appointment
        public void Post([FromBody]dynamic value)
        {
            _treatmentFacilities.ToList().Add(value);
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
                _treatmentFacilities.ToList().Remove(historyRecord);
            }
        }
    }
}
