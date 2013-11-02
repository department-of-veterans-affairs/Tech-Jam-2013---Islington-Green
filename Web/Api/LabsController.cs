using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Api
{
    public class LabsController : ApiController
    {
        private const string DateFormat = "dd MMM yyyy";

        private static readonly dynamic[] _providerEntered =  {
            new {
                Id=1,
                TestName= "COAGULATION SURFACE INDUCED.FACTOR SUBSTITUTION~IMMEDIATELY AFTER ADDITION OF NORMAL PLASMA",
                Result= "13.2 Low",
                Units= "SECONDS",
                ReferenceRange= "(59.0-104.0)",
                Interpretation= "Current normal range = 20.3 -35.8 seconds." +
                "Normalrangeprior to 7/5/02 was 25.3 - 40.4 sec." +
                "Normalrangeprior to 2/26/98 was 26.1 - 43.0 sec." +
                "Therapeuticrangeprior to 7/5/02 was 67.0 - 118.0 sec." +
                "herapeuticrangeprior to 2/26/98 was 72.0 - 126.0 sec." ,
                PerformingLocation= "DAYTON, OH VAMC 4100 W. THIRD STREET , DAYTON, OH 45428 ",
                Status= "Final"
            }
        };

        private static readonly dynamic[] _selfEntered =  {
            new {
                Id = 1,
                TestName= "Blood Test",
                Date= DateTime.ParseExact("06 Jun 2010",DateFormat,CultureInfo.InvariantCulture),
                Locationperformed= "Community Center",
                Provider= "Red Cross Blood Drive",
                Results= "Was not able to donate blood because iron was low ",
                Comments= "Will ask doctor at next visit "
            }
        };

        internal struct Labs
        {
            public IEnumerable<dynamic> ProviderEntered;
            public IEnumerable<dynamic> SelfEntered;
        }

        private readonly Labs _labs = new Labs
        {
            ProviderEntered = _providerEntered,
            SelfEntered = _selfEntered
        };


        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _labs);
        }

        public dynamic Get(int id)
        {
            return Array.Find(_providerEntered, d => d.Id == id) ?? Array.Find(_selfEntered, s => s.Id == id);
        }

        public void PostDoctorPrescribed([FromBody]string value)
        {
            _providerEntered.ToList().Add(value);
        }

        public void PostSelfPrescribed([FromBody]string value)
        {
            _selfEntered.ToList().Add(value);
        }

        public void PutDoctorPrescribed(int id, [FromBody]string value)
        {
            var doctorPrescribed = this.Get(id);
            if (doctorPrescribed != null)
            {
                doctorPrescribed = value;
            }
        }

        public void PutSelfPrescribed(int id, [FromBody]string value)
        {
            var selfPrescribed = this.Get(id);
            if (selfPrescribed != null)
            {
                selfPrescribed = value;
            }
        }

        public void DeleteDoctorPrescribed(int id)
        {
            var doctorPrescribed = this.Get(id);
            if (doctorPrescribed != null)
            {
                _providerEntered.ToList().Remove(doctorPrescribed);
            }
        }

        public void DeleteSelfPrescribed(int id)
        {
            var selfPrescribed = this.Get(id);
            if (selfPrescribed != null)
            {
                _selfEntered.ToList().Remove(selfPrescribed);
            }
        }

    
    }
}
