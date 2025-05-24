using HMSSoft.Core.Entities;
using HMSSoft.Repositories;
using HMSSoft.Repositories.Data;
using Microsoft.EntityFrameworkCore;

namespace HMSSoft.Tests.Repositories
{
    public class AppointmentRepositoryTests
    {
        private HMSSoftDbContext _context = null!;
        private AppointmentRepository _repository = null!;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<HMSSoftDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new HMSSoftDbContext(options);
            _repository = new AppointmentRepository(_context);
        }
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }


        [Test]
        public async Task AddAsync_ShouldAddAppointment()
        {
            var appointment = new Appointment { DoctorId = 1, PatientId = 1, Date = DateTime.Now };

            var result = await _repository.AddAsync(appointment);

            Assert.That(result, Is.Not.Null);
            Assert.That(_context.Appointments.Count(), Is.EqualTo(1));
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveAppointment()
        {
            var appointment = new Appointment { DoctorId = 1, PatientId = 1, Date = DateTime.Now };
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            var result = await _repository.DeleteAsync(appointment.Id);

            var exists = await _context.Appointments.FindAsync(appointment.Id);
            Assert.That(exists, Is.Null, "Appointment Does not  exist in the database.");

        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAppointments()
        {
            var doctor = new Doctor { Name = "Dr. Smith" };
            var patient = new Patient { Name = "John Doe",Gender="Male",PhoneNumber="1223" };
            _context.Doctors.Add(doctor);
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            _context.Appointments.Add(new Appointment
            {
                DoctorId = doctor.Id,
                PatientId = patient.Id,
                Date = DateTime.Now
            });
            await _context.SaveChangesAsync();

            var result = await _repository.GetAllAsync();

            Assert.That(result.Count(), Is.GreaterThan(0));
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnCorrectAppointment()
        {
            var doctor = new Doctor { Name = "Dr. Smith" };
            var patient = new Patient { Name = "John Doe" ,Gender="Male",PhoneNumber="1234"};
            _context.Doctors.Add(doctor);
            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            var appointment = new Appointment
            {
                DoctorId = doctor.Id,
                PatientId = patient.Id,
                Date = DateTime.Now
            };
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            var result = await _repository.GetByIdAsync(appointment.Id);

            Assert.That(result, Is.Not.Null);
            Assert.That(result!.Id, Is.EqualTo(appointment.Id));
        }


        [Test]
        public async Task GetByDoctorIdAsync_ShouldReturnMatchingAppointments()
        {
            _context.Appointments.AddRange(
                new Appointment { DoctorId = 1, PatientId = 1, Date = DateTime.Now },
                new Appointment { DoctorId = 2, PatientId = 1, Date = DateTime.Now }
            );
            await _context.SaveChangesAsync();

            var result = await _repository.GetByDoctorIdAsync(1);


            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task GetByPatientIdAsync_ShouldReturnMatchingAppointments()
        {
            _context.Appointments.AddRange(
                new Appointment { DoctorId = 1, PatientId = 1, Date = DateTime.Now },
                new Appointment { DoctorId = 2, PatientId = 2, Date = DateTime.Now }
            );
            await _context.SaveChangesAsync();

            var result = await _repository.GetByPatientIdAsync(1);

            Assert.That( result,Is.Not.Null);
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateAppointment()
        {
            var appointment = new Appointment { DoctorId = 1, PatientId = 1,Date = DateTime.Now };
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            appointment.DoctorId = 2;

            var result = await _repository.UpdateAsync(appointment);

            Assert.That(result.DoctorId, Is.EqualTo(2));
        }
    }
}
