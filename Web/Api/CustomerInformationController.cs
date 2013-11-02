using IG.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using MongoDB.Bson;

namespace Web.Api
{
    using MongoDB.Bson;

    public class CustomerInformationController : ApiController
    {

        private const string DateFormat = "dd MMM yyyy";

        private static readonly dynamic[] _customerInformation =  {
            new {
                Id=1,
                FirstName= "ONE",
                MiddleInitial= "A",
                LastName= "MHVVETERAN",
                Suffix= "",
                Alias= "MHVVET",
                RelationshiptoVA= "Patient, Veteran, Employee",
                Gender= "Male",
                BloodType= "AB+"       ,  
                OrganDonor= "Yes",
                DateofBirth= DateTime.ParseExact("01 Mar 1948",DateFormat,CultureInfo.InvariantCulture),
                MaritalStatus= "Married",
                CurrentOccupation= "Truck Driver",
                MailingorDestinationAddress= "123 Anywhere Road",
                MailingorDestinationAddress2= "Apartment 123",
                MailingorDestinationCity= "Anywhere",
                MailingorDestinationState= "DC",
                MailingorDestinationCountry= "United States",
                MailingorDestinationProvince= "",
                MailingorDestinationZipPostalCode= "00001",
                HomePhoneNumber= "000-010-0101",
                WorkPhoneNumber= "000-020-0202",
                PagerNumber= "000-030-0303",
                CellPhoneNumber= "000-040-0404",
                FAXNumber= "000-050-0505",
                EmailAddress= "mhvveteran@emailaddress.com",
                PreferredMethodofContact= "Email",
                EmergencyContact=Enumerable.Empty<dynamic>()
            }
        };

        private static readonly dynamic[] _emergencyContact =  {
            new {
            Id = 1,
            ContactFirstName= "Two",
            ContactLastName= "MHVVeteran",
            Relationship= "",
            HomePhoneNumber= "000-010-0101",
            WorkPhoneNumber= "000-060-0606" , 
            Extension= "",
            CellPhoneNumber= "000-070-0707",
            AddressLine1= "123 Anywhere Road",
            AddressLine2= "Apartment 123",
            City= "Anywhere",
            State= "DC",
            Country= "United States",
            Province= "",
            ZipPostCode= "00001",
            EmailAddress= "twomhvveteran@domain.com"
            }
        };


        public IEnumerable<dynamic> Get()
        {

            //var collection = MongoHelper.Current.Database.GetCollection("customers");
            return _customerInformation;
        }

        public dynamic Get(int id)
        {
            //Swap out 0 for the Id - once the Id becomes a Guid
            var collection = MongoHelper.Current.Database.GetCollection("customers");
            var firstElement = collection.FindAll().ElementAt(0);
            return firstElement.Where(e => e.Name == "Demographics").ToJson();

            return _customerInformation.FirstOrDefault(c => c.Id == id);
        }

        public void Post([FromBody]dynamic value)
        {
            _customerInformation.ToList().Add(value);
        }

        public void Put(int id, [FromBody]dynamic value)
        {
            var historyRecord = this.Get(id);
            if (historyRecord != null)
            {
                historyRecord = value;
            }
        }

        public void Delete(int id)
        {
            var historyRecord = this.Get(id);
            if (historyRecord != null)
            {
                _customerInformation.ToList().Remove(historyRecord);
            }
        }
    }
}
