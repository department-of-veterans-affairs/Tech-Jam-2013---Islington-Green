using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IG.Data
{
    public class Customer
    {
        public string Id { get; set; }

        public Demographics Demographics { get; set; }
        public List<HealthCareProvider> HealthCareProviders { get; set; }
        public List<TreatmentFacility> TreatmentFacilities { get; set; }
        public List<Insurance> HealthInsurance { get; set; }
        public List<Reminder> WellnessReminders { get; set; }
        public List<Appointment> Appointments { get; set; }
        public List<Medication> MedicationHistory { get; set; }
        public List<MedicationsOrSupplement> MedicationsAndSupplements { get; set; }
        public List<Allergy> Allergies { get; set; }
        public List<AllergiesOrAdverseReaction> AllergiesAndAdverseReactions { get; set; }
        public List<MedicalEvent> MedicalEvents { get; set; }
        public List<Immunization> Immunizations { get; set; }
        public VaLabResult VaLabResults { get; set; }
        public LabAndTest LabAndTests { get; set; }
        public List<VitalsAndReading> VitalsAndReadings { get; set; }
        public List<FamilyHealthHistory> FamilyHealthHistory { get; set; }
        public List<MilitaryHealthHistory> MilitaryHealthHistory { get; set; }
    }

    public class Demographics
    {
        public string FirstName { get; set; }
        public string MiddleIntial { get; set; }
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }
    }

    public class HealthCareProvider
    {
        //Source: Self-Entered

        //Provider Name: John Doe
        //Type of Provider: Primary
        //Other Clinician Information:  
        //Phone Number: 000-000-0000    Ext: 1234
        //Email: provider@institution.org
        //Comments: Dr. Doe can be reached on the weekend if needed by leaving a message 
        //with the clinic. 
    }

    public class TreatmentFacility
    {
        //Source: Self-Entered

        //Facility Name: Anywhere VA Medical Center
        //Facility Type: VA            VA Home Facility: Yes
        //Phone Number: 000-000-0001   Ext: 1234
        //Mailing Address: 123 VA Drive
        //Mailing Address2: Suite 4
        //Mailing City: Anywhere
        //Mailing State: DC
        //Mailing Country: United States
        //Mailing Province:  
        //Mailing Zip/Postal Code: 00001
        //Comments: Contact clinic when calling to make my appointments. 
    }

    public class Insurance
    {
    }

    public class Reminder
    {
    }

    public class Appointment 
    {
        public Guid Id { get; set; }
        public DateTime AppointmentDateTime { get; set; }
        public String LocationDescription { get; set; }
        public String StatusDescription { get; set; }
        public String ClinicDescription { get; set; }
        public String PhoneNumber { get; set; }
        public String AppointmentTypeDescription { get; set; }
    }

    public class Medication
    {
        public Guid Id { get; set; }
        public String MedicalationDescription { get; set; }
        public String Instructions { get; set; }
        public String Status { get; set; }
        public Int32 RefillsRemaining { get; set; }
        public DateTime LastFilledDate { get; set; }
        public DateTime InitiallyOrderedOn { get; set; }
        public Int32 Quantity { get; set; }
        public Int32 DaysSupply { get; set; }
        public String PharmacyName { get; set; }
        public String PrescriptionNumber { get; set; }
    }

    public class MedicationsOrSupplement
    {
    }

    public class Allergy
    {
    }

    public class AllergiesOrAdverseReaction
    {
    }

    public class MedicalEvent
    {
    }

    public class Immunization
    {
    }

    public class VaLabResult
    {
        public List<VaLabTest> VaLabTests { get; set; }
        public List<VaTest> VaTests { get; set; }
    }

    public class VaLabTest
    {
    }

    public class VaTest
    {
    }

    public class LabAndTest
    {
        public List<Test> Tests { get; set; }
    }

    public class Test
    {
    }

    public class VitalsAndReading 
    { 
    }

    public class FamilyHealthHistory 
    { 
    }

    public class MilitaryHealthHistory 
    { 
    }


    // TODO: Do This last (perhaps as a separate
    public class DODMILITARYSERVICEINFORMATION 
    { 
    }

}
