using HMSSoft.Core.DTO_s;
using HMSSoft.Core.Entities;
using HMSSoft.Core.Repositories;
using Moq;

namespace HMSSoft.Services.Tests
{
    [TestFixture]
    public class DepartmentServiceTest
    {
        private Mock<IDepartmentRepository> _mockRepo;
        private DepartmentService _service;

        [SetUp]
        public void Setup()
        {
            _mockRepo = new Mock<IDepartmentRepository>();
            _service = new DepartmentService(_mockRepo.Object);
        }
        [Test]
        public async Task GetAllAsync_ShouldReturnListOfDepartmentDtos()
        {
            var testdepartments = new List<Department>
            {
              new Department{
              Id = 1,
              Name="Pathology"
              },
              new Department
              {
                  Id = 2,
                  Name="Urology",
              }
            };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(testdepartments);

            var result = await _service.GetAllAsync();

            Assert.That(result.Count, Is.EqualTo(2));
        }
        [Test]
        public async Task GetAllAsync_IfListisEmpty_ReturnsEmptyList()
        {
            var testdepartments = new List<Department>();


            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(testdepartments);

            var result = await _service.GetAllAsync();

            Assert.That(result, Is.Empty);
        }
        [Test]
        public async Task GetByIdAsync_WhenDepartmentExists_ShouldReturnDepartmentDto()
        {
            var department = new Department { Id = 1, Name = "Cardiology" };
            _mockRepo.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(department);

            var result = await _service.GetByIdAsync(1);

            Assert.That(result, Is.Not.Null);
        }
        [Test]
        public async Task GetByIdAsync_WhenDepartmentDoesNotExists_ReturnNull()
        {

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Department)null);

            var result = await _service.GetByIdAsync(2);

            Assert.That(result, Is.Null);
        }
        [Test]
        public async Task AddDepartmentAsync_CallsRepositoryAdd_WithCorrectDepartment()
        {
            var dto = new DepartmentDto { Name = "Pediatrics" };

            await _service.AddDepartmentAsync(dto);

            _mockRepo.Verify(r => r.AddAsync(It.Is<Department>(d => d.Name == "Pediatrics")));
        }
        [Test]
        public async Task UpdateDepartmentAsync_WhenDepartmentExists_UpdatesName()
        {
            var existingdept = new Department { Id = 1, Name = "OldName" };
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existingdept);

            await _service.UpdateDepartmentAsync(1, new DepartmentDto { Name = "NewName" });

            Assert.That(existingdept.Name, Is.EqualTo("NewName"));
        }
        [Test]
        public async Task UpdateDepartmentAsync_WhenDepartmentDoesNotExist_DoesNotCallUpdate()
        {
            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Department)null);

            Assert.DoesNotThrowAsync(async () =>
                await _service.UpdateDepartmentAsync(1, new DepartmentDto { Name = "Pathology" })
            );
        }
        [Test]
        public async Task DeleteDepartmentAsync_CallsRepositoryDelete()
        {
            await _service.DeleteDepartmentAsync(1);

            _mockRepo.Verify(r => r.DeleteAsync(1));
        }
        [Test]
        public async Task GetDepartmentWithDoctorsAsync_WhenDoctorsExist_IncludesThemInDto()
        {
            var department = new Department
            {
                Id = 1,
                Name = "Cardiology",
                Doctors = new List<Doctor> { new Doctor { Name = "Dr. Smith" } }
            };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(department);

            var result = await _service.GetDepartmentWithDoctorsAsync(1);

            Assert.That(result.Doctors.First().Name, Is.EqualTo("Dr. Smith"));
        }
        [Test]
        public async Task GetDepartmentWithDoctorsAsync_WhenDoctorsDoesNotExist_ReturnsEmptyList()
        {
            var department = new Department
            {
                Id = 1,
                Name = "Cardiology",
                Doctors = null
            };

            _mockRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(department);

            var result = await _service.GetDepartmentWithDoctorsAsync(1);

            Assert.That(result.Doctors, Is.Null); 
        }
    }
}
