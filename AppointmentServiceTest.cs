using HMSSoft.Core.DTO_s;
using HMSSoft.Core.Entities;
using HMSSoft.Core.Repositories;
using Moq;

namespace HMSSoft.Services.Tests
{
    [TestFixture]
    public class AppointmentServiceTests
    {
        private Mock<IAppointmentRepository> _mockRepo;
        private AppointmentService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IAppointmentRepository>();
            _service = new AppointmentService(_mockRepo.Object);
        }

        [Test]
        public async Task GetAllAsync_ReturnsListOfAppointmentDtos()
        {
            var testappoitments = new List<Appointment>
            {
                new Appointment
                {
                    Id = 1,
                    Reason = "Checkup",
                    Date = DateTime.Now.AddDays(1),
                    PatientId = 101,
                    DoctorId = 201,
                    Patient = new Patient { Id = 1, Name = "Zubair", Gender = "Male", PhoneNumber = "1213" }
                },
                 new Appointment
                {
                    Id = 2,
                    Reason = "Checkup",
                    Date = DateTime.Now.AddDays(1),
                    PatientId = 11,
                    DoctorId = 21,
                    Patient = new Patient { Id = 1, Name = "Zubair", Gender = "Male", PhoneNumber = "1213" }
                },
            };

            _mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(testappoitments);

            var result = await _service.GetAllAsync();

            Assert.That(result.Count, Is.EqualTo(2));
        }

        [Test]
        public async Task GetAllAsync_WhenNoAppointments_ReturnsEmptyList()
        {
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Appointment>());

            var result = await _service.GetAllAsync();

            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetByIdAsync_WhenExists_ReturnsAppointmentDto()
        {
            var testAppointment = new Appointment
            {
                Id = 1,
                Reason = "Checkup",
                Date = DateTime.Now,
                PatientId = 1,
                DoctorId = 1
            };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(testAppointment);

            var result = await _service.GetByIdAsync(1);

            Assert.That(result.Reason, Is.EqualTo("Checkup"));
        }

        [Test]
        public async Task GetByIdAsync_WhenNotExists_ReturnsNull()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Appointment)null);

            var result = await _service.GetByIdAsync(999);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task AddAsync_ValidDto_ReturnsNewAppointmentDto()
        {
            var inputDto = new AppointmentDto
            {
                Reason = "Checkup",
                Date = DateTime.Now,
                PatientId = 1,
                DoctorId = 1,
                PatientName = "John"
            };

            var savedAppointment = new Appointment
            {
                Id = 1,
                Reason = "Checkup",
                Date = DateTime.Now,
                PatientId = 1,
                DoctorId = 1
            };

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Appointment>()))
                    .ReturnsAsync(savedAppointment);

            var result = await _service.AddAsync(inputDto);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task UpdateAsync_ValidDto_ReturnsUpdatedDto()
        {
            var inputDto = new AppointmentDto
            {
                Id = 1,
                Reason = "Updated Checkup",
                Date = DateTime.Now,
                PatientId = 1,
                DoctorId = 1
            };

            var updatedAppointment = new Appointment
            {
                Id = 1,
                Reason = "Updated Checkup",
                Date = DateTime.Now,
                PatientId = 1,
                DoctorId = 1
            };

            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Appointment>()))
                    .ReturnsAsync(updatedAppointment);

            var result = await _service.UpdateAsync(inputDto);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Reason, Is.EqualTo("Updated Checkup"));
        }

        [Test]
        public void UpdateAsync_WhenRepositoryThrows_PropagatesException()
        {
            var inputDto = new AppointmentDto { Id = 999 };
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Appointment>()))
                    .ThrowsAsync(new KeyNotFoundException());

            Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(inputDto));
        }

        [Test]
        public async Task DeleteAsync_WhenExists_ReturnsDeletedDto()
        {
            var deletedAppointment = new Appointment
            {
                Id = 1,
                Reason = "Checkup",
                Date = DateTime.Now,
                PatientId = 1,
                DoctorId = 1
            };

            _mockRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(deletedAppointment);

            var result = await _service.DeleteAsync(1);

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public async Task DeleteAsync_WhenNotExists_ReturnsNull()
        {
            _mockRepo.Setup(r => r.DeleteAsync(It.IsAny<int>())).ReturnsAsync((Appointment)null);

            var result = await _service.DeleteAsync(999);

            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetByDoctorIdAsync_WhenExists_ReturnsAppointmentDtos()
        {
            var testAppointments = new List<Appointment>
    {
        new Appointment
        {
            Id = 1,
            Patient = new Patient { Id = 1, Name = "daud",Gender = "Male", PhoneNumber="5456567" },
            Doctor = new Doctor { Id = 1, Name = "Dr. Smith" },
            Reason = "Checkup",
            Date = DateTime.Now,
            PatientId = 1,
            DoctorId = 1
        },
        new Appointment
        {
            Id = 2,
            Patient = new Patient { Id = 2, Name = "Munneb",Gender="Male",PhoneNumber="123" },
            Doctor = new Doctor { Id = 1, Name = "Dr. Smith" },
            Reason = "Follow-up",
            Date = DateTime.Now.AddDays(1),
            PatientId = 2,
            DoctorId = 1
        }
    };

            _mockRepo.Setup(r => r.GetByDoctorIdAsync(1)).ReturnsAsync(testAppointments);

            var result = await _service.GetByDoctorIdAsync(1);

            Assert.That(result.Count, Is.EqualTo(2));

        }
        [Test]
        public async Task GetByPatientIdAsync_WhenExists_ReturnsAppointmentDtos()
        {
            var testAppointments = new List<Appointment>
            {
                new Appointment { Id = 1,
                    Reason = "Checkup", 
                    Date = DateTime.Now,
                    PatientId = 1,
                    DoctorId = 1 },
                new Appointment { Id = 2,
                    Reason = "Follow-up",
                    Date = DateTime.Now.AddDays(1),
                    PatientId = 1, 
                    DoctorId = 1 }
            };

            _mockRepo.Setup(r => r.GetByPatientIdAsync(1)).ReturnsAsync(testAppointments);

            var result = await _service.GetByPatientIdAsync(1);

            Assert.That(result.Count, Is.EqualTo(2));
        }
    }
}