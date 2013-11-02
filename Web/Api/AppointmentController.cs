using IG.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MongoDB.Bson;
using Newtonsoft.Json.Linq;
using WebGrease.Css.Extensions;

namespace Web.Api
{
    public class AppointmentController : ApiController
    {
        private const string DateFormat = "dd MMM yyyy @ HHmm";

        private readonly dynamic[] _appointments =  {
                                         new  {
                                             Id = 1,
                                        DateTime=DateTime.ParseExact("13 Oct 2011 @ 1600",DateFormat,CultureInfo.InvariantCulture),
                                        Location="DAYT29 TEST LAB",
                                        Status="NOT APPLICABLE",
                                        Clinic="C&P CHRISTIE",
                                        PhoneNumber="3929",
                                        Type="Compensation and Pension Appointment"
                                        },
                                         new {
                                             Id = 2,
                                        DateTime=DateTime.ParseExact("07 Sep 2011 @ 1100",DateFormat,CultureInfo.InvariantCulture),
                                        Location="DAYT29 TEST LAB",
                                        Status="NOT APPLICABLE",
                                        Clinic="TELEPHONE CALLS/GERIATRICS",
                                        PhoneNumber="3742"
                                        },
                                         new {
                                             Id = 3,
                                        DateTime=DateTime.ParseExact("27 Jul 2011 @ 1400",DateFormat,CultureInfo.InvariantCulture),
                                        Location="DAYT29 TEST LAB",
                                        Status="NOT APPLICABLE",
                                        Clinic="DIABETIC-BURKE",
                                        PhoneNumber="800-123-1234",
                                        Note="This appointment has pre-appointment activity scheduled: Lab=27 Jul 2011 @ 1000"
                                        },
                                         new {
                                             Id = 4,
                                        DateTime=DateTime.ParseExact("15 Jun 2011 @ 1300",DateFormat,CultureInfo.InvariantCulture),
                                        Location="DAYT29 TEST LAB",
                                        Status="NOT APPLICABLE",
                                        Clinic="DIABETIC-BURKE",
                                        PhoneNumber="800-123-1234",
                                        Note="This appointment has pre-appointment activity scheduled: Lab=15 Jun 2011 @ 0800, EKG=15 Jun 2011 @ 1030, X-Ray: 15 Jun 2011 @ 0900"
                                        },
                                         new {
                                             Id = 5,
                                        DateTime=DateTime.ParseExact("03 May 2011 @ 1100",DateFormat,CultureInfo.InvariantCulture),
                                        Location="DAYT29 TEST LAB",
                                        Status="NOT APPLICABLE",
                                        Clinic="TELEPHONE CALLS/GERIATRICS",
                                        PhoneNumber="3742",
                                        Note="This appointment has pre-appointment activity scheduled: X-Ray: 03 May 2011 @ 0800"
                                        },
                                         new {
                                             Id = 6,
                                        DateTime=DateTime.ParseExact("15 Mar 2011 @ 1000",DateFormat,CultureInfo.InvariantCulture),
                                        Location="MIDDLETOWN",
                                        Status="CANCELLED",
                                        Clinic="MD/DENTAL FOSTER",
                                        PhoneNumber="2449",
                                        Note="This appointment has pre-appointment activity scheduled: Lab=15 Mar 2011 @ 0930"
                                        },
                                         new {
                                             Id = 7,
                                        DateTime=DateTime.ParseExact("06 Jan 2011 @ 1000",DateFormat,CultureInfo.InvariantCulture),
                                        Location="DAYT29 TEST LAB",
                                        Status="NOT APPLICABLE",
                                        Clinic="PRP JOHNSON,C (GRP)",
                                        PhoneNumber="2188",
                                        Note="This appointment has pre-appointment activity scheduled: Lab=06 Jan 2011 @ 0900, EKG=06 Jan 2011 @ 0930",
                                        },
                                         new {
                                             Id = 1,
                                        DateTime=DateTime.ParseExact("03 Jan 2011 @ 1300",DateFormat,CultureInfo.InvariantCulture),
                                        Location="DAYT29 TEST LAB",
                                        Status="NOT APPLICABLE",
                                        Clinic="PRP JOHNSON,C (GRP)",
                                        PhoneNumber="2188"
                                        },
                                         new {
                                             Id = 8,
                                        DateTime=DateTime.ParseExact("29 Dec 2010 @ 1000",DateFormat,CultureInfo.InvariantCulture),
                                        Location="MIDDLETOWN",
                                        Status="NOT APPLICABLE",
                                        Clinic="MD/PODIATRY NGUYEN (Follow-Up)",
                                        PhoneNumber="5416"
                                        },
                                         new {
                                             Id = 9,
                                        DateTime=DateTime.ParseExact("30 Aug 2010 @ 1400",DateFormat,CultureInfo.InvariantCulture),
                                        Location="DAYT29 TEST LAB",
                                        Status="APPOINTMENT KEPT",
                                        Clinic="TELEPHONE CALLS/GERIATRICS",
                                        PhoneNumber="3742",
                                        },
                                         new {
                                             Id = 10,
                                        DateTime=DateTime.ParseExact("14 Jul 2010 @ 0900",DateFormat,CultureInfo.InvariantCulture),
                                        Location="MIDDLETOWN",
                                        Status="NOT APPLICABLE",
                                        Clinic="MD/PODIATRY NGUYEN (Follow-Up)",
                                        PhoneNumber="5416"
                                        }
       };

        // GET api/appointment
        public IEnumerable<dynamic> Get()
        {
            var collection = MongoHelper.Current.Database.GetCollection("customers");
            var firstElement = collection.FindAll().ElementAt(0);
            var appt = firstElement.Where(e => e.Name == "VaAppointments").First().Value;
            var past = appt["PastAppointments"].AsBsonArray;
            var items = ((JToken.Parse(appt.ToString()).AsEnumerable<dynamic>().ToList().Last() as JProperty).ToList<dynamic>()
                    .First() as JArray).ToList<dynamic>();

            items.ForEach(item => item.DateTime.Value = DateTime.ParseExact(item.DateTime.Value, DateFormat, CultureInfo.InvariantCulture).ToString());
            return items;
            //return _appointments;
        }

        public dynamic Get(int id)
        {
            return _appointments.Where(a => a.Id == id);


            //Swap out 0 for the Id - once the Id becomes a Guid
            //var collection = MongoHelper.Current.Database.GetCollection("customers");
            //var firstElement = collection.FindAll().ElementAt(0);
            //var secondElement = firstElement.Where(e => e.Name == "VaAppointments").FirstOrDefault().Value;
            //return secondElement.;
        }

        // POST api/appointment
        public void Post([FromBody]dynamic value)
        {
            _appointments.ToList().Add(value);
        }

        // PUT api/appointment/5
        public void Put(int id, [FromBody]dynamic value)
        {
            var appointment = this.Get(id);
            if (appointment != null)
            {
                appointment = value;
            }
        }

        // DELETE api/appointment/5
        public void Delete(int id)
        {
            var appointment = this.Get(id);
            if (appointment != null)
            {
                _appointments.ToList().Remove(appointment);
            }
        }
    }
}
