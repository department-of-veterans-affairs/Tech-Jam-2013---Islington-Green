using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Api
{
    public class HealthInsuranceController : ApiController
    {
        private const string DateFormat = "dd MMM yyyy";

        private static readonly dynamic[] _healthInsuranceProvider =  {
            new {
                Id=1,
                HealthInsuranceCompany= "My Health Insurance Company",
                PrimaryInsuranceProvider= "Yes",
                IDNumber= "0001234 ",                     
                GroupNumber= "0000000",
                Insured= "One Mhvveteran",
                StartDate= "01 Jan 2000 "   ,             
                StopDate= "",
                PreApprovalPhoneNumber=              "000-000-0003",
                HealthInsuranceCompanyPhoneNumber=  "000-000-0004",
                Comments= "Need to get pre-authorization for special services. "
            }
        };

        // GET api/appointment
        public IEnumerable<dynamic> Get()
        {
            return _healthInsuranceProvider;
        }

        // GET api/appointment/5
        public dynamic Get(int id)
        {
            return Array.Find(_healthInsuranceProvider, a => a.Id == id);
        }

        // POST api/appointment
        public void Post([FromBody]dynamic value)
        {
            _healthInsuranceProvider.ToList().Add(value);
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
                _healthInsuranceProvider.ToList().Remove(historyRecord);
            }
        }
    }
}
