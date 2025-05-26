using HMSSoft.Core.DTO_s;
using HMSSoft.Core.Entities;
using HMSSoft.Core.Repositories;
using Moq;
using UnitTest;

namespace HMSSoft.Services.Tests
{
    [TestFixture]
    public class AppointmentServiceTests
    {
        private Mock<IAppointmentRepository> _mockRepo;
        private AppointmentService _sut;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IAppointmentRepository>();
            _sut = new AppointmentService(_mockRepo.Object);
        }

        [Test]
        public async Task GetAllAsync_ShouldReturnsListOfAppointmentDtos()
        {
            //Arrange
            var testappoitments = MockData.GetSampleAppointments();

            _mockRepo.Setup(repo => repo.GetAllAsync()).
                ReturnsAsync(testappoitments);

            //Act
            var result = await _sut.GetAllAsync();
            //Assert
            Assert.That(result[0].PatientName, Is.EqualTo("John Doe"));
        }

        [Test]
        public async Task GetAllAsync_ReturnsEmptyList_IfNoAppointmentsExist()
        {
            //Arrange
            _mockRepo.Setup(r => r.GetAllAsync()).
                ReturnsAsync(new List<Appointment>());
            //ACt
            var result = await _sut.GetAllAsync();
            //Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetByIdAsync_ShouldReturnsAppointmentDto()
        {
            //Arrange
            var testAppointment = MockData.GetSampleAppointments().Single();
           
            _mockRepo.Setup(r => r.GetByIdAsync(1)).
                ReturnsAsync(testAppointment);
            //Act
            var result = await _sut.GetByIdAsync(1);
            //Assert
            Assert.That(result.Reason, Is.EqualTo("Checkup"));
        }

        [Test]
        public async Task GetByIdAsync_ReturnsNull_IfNotExists()
        {
            //Arrange
            _mockRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).
                ReturnsAsync((Appointment)null);
            //Act
            var result = await _sut.GetByIdAsync(999);
            //Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task AddAsync_ShouldReturnsAddedAppointmentDto()
        {
            //Arrange
            var inputDto = MockData.GetSampleAppointmentDto();

            var savedAppointment =MockData.GetSampleAppointments().Single();

            _mockRepo.Setup(r => r.AddAsync(It.IsAny<Appointment>()))
                    .ReturnsAsync(savedAppointment);
            //Act
            var result = await _sut.AddAsync(inputDto);
            //Assert
            Assert.That(result.PatientName, Is.EqualTo("John Doe"));
        }

        [Test]
        public async Task UpdateAsync_ShouldReturnsUpdatedDto()
        {
            //Arrange
            var inputDto = MockData.GetSampleAppointmentDto();

            var updatedAppointment = MockData.GetSampleAppointments().First().WithReason("Updated Checkup");
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Appointment>()))
                    .ReturnsAsync(updatedAppointment);
            //Act
            var result = await _sut.UpdateAsync(inputDto);
            //Assert
            Assert.That(result.Reason, Is.EqualTo("Updated Checkup"));
        }

        [Test]
        public void UpdateAsync_WhenRepositoryThrows_PropagatesException()
        {
            var inputDto = new AppointmentDto { Id = 999 };
            _mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Appointment>()))
                    .ThrowsAsync(new KeyNotFoundException());

            Assert.ThrowsAsync<KeyNotFoundException>(() => _sut.UpdateAsync(inputDto));
        }

        [Test]
        public async Task DeleteAsync_ShouldReturnsDeletedDto()
        {
            //Arrange
            var deletedAppointment = MockData.GetSampleAppointments().Single();

            _mockRepo.Setup(r => r.DeleteAsync(1)).
                ReturnsAsync(deletedAppointment);
            //Act
            var result = await _sut.DeleteAsync(1);
            //Assert
            Assert.That(result.DoctorName,Is.EqualTo("Dr. Smith"));
        }

        [Test]
        public async Task DeleteAsync_ReturnsNull_WhenNotExists()
        {
            //Arrange
            _mockRepo.Setup(r => r.DeleteAsync(It.IsAny<int>())).
                ReturnsAsync((Appointment)null);
            //Act
            var result = await _sut.DeleteAsync(999);
            //Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public async Task GetByDoctorIdAsync_ShouldReturnsAppointmentDtos()
        {
            //Arrange
            var testAppointments = MockData.GetSampleAppointments();

            _mockRepo.Setup(r => r.GetByDoctorIdAsync(1)).
                ReturnsAsync(testAppointments);
            //Act
            var result = await _sut.GetByDoctorIdAsync(1);
            //Assert
            Assert.That(result[0].DoctorName, Is.EqualTo("Dr. Smith"));

        }
        [Test]
        public async Task GetByPatientIdAsync_ShouldReturnsAppointmentDtos()
        {
            //Arrange
            var testAppointments = MockData.GetSampleAppointments();

            _mockRepo.Setup(r => r.GetByPatientIdAsync(testAppointments[0].PatientId)).
                ReturnsAsync(testAppointments);
            //Act
            var result = await _sut.GetByPatientIdAsync(1);
            //Assert
            Assert.That(result[0].DoctorName, Is.EqualTo("Dr. Smith"));
        }
    }
}