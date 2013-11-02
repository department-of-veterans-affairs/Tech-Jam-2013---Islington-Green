using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Api
{
    public class HealthCareProviderController : ApiController
    {

        private const string DateFormat = "dd MMM yyyy";

        private static readonly dynamic[] _healthCareProvider =  {
            new {
                Id=1,
                ProviderName= "John Doe",
                TypeofProvider= "Primary",
                OtherClinicianInformation=  "",
                PhoneNumber= "000-000-0000" ,   
                Ext= "1234",
                Email= "provider@institution.org",
                Comments= "Dr. Doe can be reached on the weekend if needed by leaving a message with the clinic. "
            }
        };

        // GET api/appointment
        public IEnumerable<dynamic> Get()
        {
            return _healthCareProvider;
        }

        // GET api/appointment/5
        public dynamic Get(int id)
        {
            return Array.Find(_healthCareProvider, a => a.Id == id);
        }

        // POST api/appointment
        public void Post([FromBody]dynamic value)
        {
            _healthCareProvider.ToList().Add(value);
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
                _healthCareProvider.ToList().Remove(historyRecord);
            }
        }




    }
}
