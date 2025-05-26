using HMSSoft.Core.DTO_s;
using HMSSoft.Core.Entities;
using System;
using System.Collections.Generic;

namespace UnitTest
{
    public static class MockData
    {
        public static Doctor GetSampleDoctor()
        {
            return new Doctor { Id = 1, Name = "Dr. Smith" };
        }

        public static Patient GetSamplePatient()
        {
            return new Patient { Id = 1, Name = "John Doe", Gender = "Male", PhoneNumber = "1234" };
        }

        public static List<Appointment> GetSampleAppointments()
        {
            // Create sample Doctor and Patient
            var doctor = GetSampleDoctor();
            var patient = GetSamplePatient();

            // Return appointments with embedded Doctor & Patient
            return new List<Appointment>
            {
                new Appointment
                {
                    Reason="Checkup",
                    DoctorId = doctor.Id,
                    Doctor = doctor,  // Include Doctor object
                    PatientId = patient.Id,
                    Patient = patient, // Include Patient object
                    Date = DateTime.Now,


                }
            };
        }
        public static Appointment WithReason(this Appointment source, string reason)
        {
            return new Appointment
            {
                // Copy all properties
                Id = source.Id,
                DoctorId = source.DoctorId,
                Doctor = source.Doctor,
                PatientId = source.PatientId,
                Patient = source.Patient,
                Date = source.Date,

                // Set new reason
                Reason = reason
            };
        }
        public static AppointmentDto GetSampleAppointmentDto()
        {
            return new AppointmentDto
            {
                Id = 1,
                Reason = "Updated Checkup",
                Date = DateTime.Now,
                PatientId = 1,
                DoctorId = 1,
                PatientName = "John Doe",
                DoctorName = "Dr. Smith"
            };
        }
       
    }
}