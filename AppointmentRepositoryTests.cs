using HMSSoft.Core.Entities;
using HMSSoft.Repositories;
using HMSSoft.Repositories.Data;
using Microsoft.EntityFrameworkCore;
using UnitTest;

namespace HMSSoft.Tests.Repositories
{
    public class AppointmentRepositoryTests
    {
        private HMSSoftDbContext _context = null!;
        private AppointmentRepository _sut = null!;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<HMSSoftDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new HMSSoftDbContext(options);
            _sut = new AppointmentRepository(_context);
        }
        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }


        [Test]
        public async Task AddAsync_ShouldAddAppointment()
        {
            //Arrange 
            var appointment = MockData.GetSampleAppointments().Single();

            //Act
            var result = await _sut.AddAsync(appointment);

            //Assert
            Assert.That(result.PatientId, Is.EqualTo(1));
        }

        [Test]
        public async Task DeleteAsync_ShouldRemoveAppointment()
        {
            //Arrange
            var appointment = MockData.GetSampleAppointments().Single();

            _context.Appointments.Add(appointment);

            await _context.SaveChangesAsync();

            //Act
            var result = await _sut.DeleteAsync(appointment.Id);

            //Assert
            var exists = await _context.Appointments.FindAsync(appointment.Id);
            Assert.That(exists, Is.Null, "Appointment Does not  exist in the database.");

        }

        [Test]
        public async Task GetAllAsync_ShouldReturnAppointments()
        {
            //Arrange
            var doctor = new Doctor { Name = "Dr. Smith" };
            var patient = new Patient { Name = "John Doe", Gender = "Male", PhoneNumber = "1223" };
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
            //ACt
            var result = await _sut.GetAllAsync();
            //Assert
            Assert.That(result[0].Doctor.Name, Is.EqualTo("Dr. Smith"));
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnCorrectAppointment()
        {
            //Arrange
            var appointment = MockData.GetSampleAppointments().Single();

            _context.Appointments.Add(appointment);

            await _context.SaveChangesAsync();

            //Act
            var result = await _sut.GetByIdAsync(appointment.Id);

            //Assert
            Assert.That(result!.Id, Is.EqualTo(appointment.Id));
        }


        [Test]
        public async Task GetByDoctorIdAsync_ShouldReturnMatchingAppointments()
        {
            //Arrange
            _context.Appointments.AddRange(
                MockData.GetSampleAppointments()
            );
            await _context.SaveChangesAsync();

            //Act
            var result = await _sut.GetByDoctorIdAsync(1);

            //Assert
            Assert.That(result[0].DoctorId, Is.EqualTo(1));
        }

        [Test]
        public async Task GetByPatientIdAsync_ShouldReturnMatchingAppointments()
        {
            //Arrange
            _context.Appointments.AddRange(
               MockData.GetSampleAppointments().Single()

            );
            await _context.SaveChangesAsync();

            //Act
            var result = await _sut.GetByPatientIdAsync(1);

            //Assert
            Assert.That(result[0].PatientId,Is.EqualTo(1));
        }

        [Test]
        public async Task UpdateAsync_ShouldUpdateAppointment()
        {
            //Arrange
            var appointment = MockData.GetSampleAppointments().Single();

            _context.Appointments.Add(appointment);

            await _context.SaveChangesAsync();

            appointment.DoctorId = 2;

            //Act
            var result = await _sut.UpdateAsync(appointment);

            //Assert
            Assert.That(result.DoctorId, Is.EqualTo(2));
        }
    }
}