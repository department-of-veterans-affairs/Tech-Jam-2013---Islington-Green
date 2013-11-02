using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace Web.Api
{
    public class DrugsController : ApiController
    {
        private const string DateFormat = "dd MMM yyyy";
        static IG.Analyitcs.DrugEstimator estimator = new IG.Analyitcs.DrugEstimator();

        private static readonly dynamic[] _doctorPrescribed =  {
            new {
            Id = 1,
            Medication="AMLODIPINE BESYLATE 10MG TAB",
            Instructions="TAKE ONE TABLET BY MOUTH TAKE ONE-HALF TABLET FOR 1 DAY --AVOID GRAPEFRUIT JUICE--",
            Status= "Active",
            RefillsRemaining= "3",
            LastFilledOn=DateTime.ParseExact("20 Aug 2010",DateFormat,CultureInfo.InvariantCulture),
            InitiallyOrderedOn=DateTime.ParseExact("13 Aug 2010",DateFormat,CultureInfo.InvariantCulture),
            Quantity= "45",
            DaysSupply= "90",
            Pharmacy= "DAYTON",
            PrescriptionNumber= "2718953",
            ProjectedRunOutDate=estimator.GetDateDrugsAreEmpty(DateTime.ParseExact("20 Aug 2010",DateFormat,CultureInfo.InvariantCulture),90),
            }
        };

        private static readonly dynamic[] _selfPrescribed =  {
            new {
            Id = 1,
            Category="RX Medication",
            DrugName= "Aspirin EC",
            PrescriptionNumber= "010101B",
            Strength= "81mg",
            Dose= "1 tab",
            Frequency= "daily",
            StartDate= DateTime.ParseExact("15 Jun 2005",DateFormat,CultureInfo.InvariantCulture),
            StopDate= "",
            PharmacyName= "My Local Drugstore",              
            PharmacyPhone="000-010-0000",
            ReasonForTaking="Daily regimen for heart health", 
            Comments=""
            }
        };

        internal struct Drugs
        {
            public IEnumerable<dynamic> DoctorPrescribed;
            public IEnumerable<dynamic> SelfPrescribed;
        }

        private readonly Drugs _drugs = new Drugs {
            DoctorPrescribed = _doctorPrescribed,
            SelfPrescribed = _selfPrescribed
        };



        public HttpResponseMessage Get()
        {
            return Request.CreateResponse(HttpStatusCode.OK, _drugs);
        }

        public dynamic Get(int id)
        {
            return Array.Find(_doctorPrescribed, d => d.Id == id) ?? Array.Find(_selfPrescribed, s => s.Id == id);
        }

        public void PostDoctorPrescribed([FromBody]string value)
        {
            _doctorPrescribed.ToList().Add(value);        
        }

        public void PostSelfPrescribed([FromBody]string value)
        {
            _selfPrescribed.ToList().Add(value);
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
                _doctorPrescribed.ToList().Remove(doctorPrescribed);
            }
        }

        public void DeleteSelfPrescribed(int id)
        {
            var selfPrescribed = this.Get(id);
            if (selfPrescribed != null)
            {
                _selfPrescribed.ToList().Remove(selfPrescribed);
            }
        }
    }


}
