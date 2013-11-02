using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Web.Api
{
    
    public class AllergyController : ApiController
    {

        private const string DateFormat = "dd MMM yyyy";


        private static readonly dynamic[] _providerEntered =  {
            new {
                Id=1,
                AllergyName= "TRIMETHOPRIM",
                Location="DAYT29",
                DateEntered=DateTime.ParseExact("09 Mar 2011",DateFormat,CultureInfo.InvariantCulture),
                Reaction="",
                AllergyType="DRUG ",
                VADrugClass="ANTI-INFECTIVES,OTHER" ,
                ObservedHistorical="HISTORICAL",
                Comments="The reaction to this allergy was MILD (NO SQUELAE)"
            }
        };

        private static readonly dynamic[] _selfEntered =  {
            new {
                Id = 1,
                AllergyName= "Peanuts",
                Date= DateTime.ParseExact("01 Aug 1980",DateFormat,CultureInfo.InvariantCulture),
                Severity=     "Moderate",
                Diagnosed=    "Yes",
                Reaction= "Hives ",
                Comments= "Avoid peanuts and peanut based foods.  Foods cooked with peanut oil also cause the reaction "
            }
        };

        internal struct Allergies
        {
            public IEnumerable<dynamic> ProviderEntered;
            public IEnumerable<dynamic> SelfEntered;
        }

        private readonly Allergies _allergies = new Allergies
        {
            ProviderEntered = _providerEntered,
            SelfEntered = _selfEntered
        };


        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _allergies);
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
