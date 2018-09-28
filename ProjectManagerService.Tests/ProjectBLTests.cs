using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using ProjectManagerDL;
using ProjectManagerEntity;
using Moq;

namespace ProjectManagerService.Tests
{
    [TestFixture]
    public class ProjectBLTests
    {
        private IProjectDataLayer _mockRepository;
        private List<ProjectEntity> _projects;

        [SetUp]
        public void Initialize()
        {
            var repository = new Mock<IProjectDataLayer>();
            _projects = new List<ProjectEntity>()
                        {
                            new ProjectEntity { ProjectId = 1, ProjectName = "Development project", TasksCount = 10, Completed = 6, StartDate = "10/01/2018", EndDate = "10/31/2018", Priority = 5, ProjectManagerId = 1235467, ProjectManagerFullName = "Uthaya Kumar" },
                            new ProjectEntity { ProjectId = 2, ProjectName = "Testing Project", TasksCount = 8, Completed = 0, StartDate = "09/01/2018", EndDate = "09/30/2018", Priority = 6, ProjectManagerId = 5645878, ProjectManagerFullName = "Pavan Krishna" },
                            new ProjectEntity { ProjectId = 3, ProjectName = "Support Project", TasksCount = 5, Completed = 3, StartDate = "11/01/2018", EndDate = "11/30/2018", Priority = 7, ProjectManagerId = 8796556, ProjectManagerFullName = "Selva Ganesh" }
                        };

            // Get All
            repository.Setup(r => r.GetAllProjects()).Returns(_projects);

            // Insert Project
            repository.Setup(r => r.AddProject(It.IsAny<ProjectEntity>()))
                .Callback((ProjectEntity p) => _projects.Add(p));

            // Update Project
            repository.Setup(r => r.UpdateProject(It.IsAny<ProjectEntity>())).Callback(
                (ProjectEntity target) =>
                {
                    var original = _projects.Where(
                        q => q.ProjectId == target.ProjectId).Single();

                    original.ProjectName = target.ProjectName;
                    original.Priority = target.Priority;
                    original.ProjectManagerId = target.ProjectManagerId;
                    original.StartDate = target.StartDate;
                    original.EndDate = target.EndDate;
                });

            // Delete Project
            repository.Setup(r => r.SuspendProject(It.IsAny<int>()))
                .Callback((int projectId) => _projects.Remove(GetProjectById(projectId)));

            _mockRepository = repository.Object;
        }

        [Test]
        public void Get_All_Projects()
        {
            List<ProjectEntity> projects = _mockRepository.GetAllProjects();

            Assert.IsTrue(projects.Count() == 3);
            Assert.IsTrue(projects.ElementAt(0).ProjectName == "Development project");
            Assert.IsTrue(projects.ElementAt(0).StartDate == "10/01/2018");
            Assert.IsTrue(projects.ElementAt(1).Priority == 6);
            Assert.IsTrue(projects.ElementAt(1).ProjectManagerFullName == "Pavan Krishna");
            Assert.IsTrue(projects.ElementAt(2).TasksCount == 5);
            Assert.IsTrue(projects.ElementAt(2).Completed == 3);
        }

        [Test]
        public void Add_Project()
        {
            var projectId = _projects.Count() + 1;
            var project = new ProjectEntity
            {
                ProjectId = projectId,
                ProjectName = "Enhancement project",
                StartDate = "01/01/2019",
                EndDate = "01/31/2019",
                Priority = 15,
                ProjectManagerId = 1234567
            };

            _mockRepository.AddProject(project);
            Assert.IsTrue(_projects.Count() == 4);
            ProjectEntity testProject = GetProjectById(projectId);
            Assert.IsNotNull(testProject);
            Assert.AreSame(testProject.GetType(), typeof(ProjectEntity));
            Assert.AreEqual(project.ProjectName, testProject.ProjectName);
            Assert.AreEqual(project.StartDate, testProject.StartDate);
            Assert.AreEqual(project.Priority, testProject.Priority);
            Assert.AreEqual(project.ProjectManagerId, testProject.ProjectManagerId);
        }

        [Test]
        public void Update_Project()
        {
            var projectId = 2;
            var project = new ProjectEntity
            {
                ProjectId = projectId,
                ProjectName = "Enhancement project",
                StartDate = "01/01/2019",
                EndDate = "01/31/2019",
                Priority = 15,
                ProjectManagerId = 1234567
            };

            _mockRepository.UpdateProject(project);

            var updatedProject = GetProjectById(projectId);
            Assert.IsTrue(updatedProject.ProjectName == "Enhancement project");
            Assert.IsTrue(updatedProject.Priority == 15);
            Assert.IsTrue(updatedProject.ProjectManagerId == 1234567);
        }

        [Test]
        public void Suspend_Project()
        {
            var projectId = 3;

            _mockRepository.SuspendProject(projectId);

            var deletedProject = GetProjectById(projectId);
            Assert.IsNull(deletedProject);
        }

        [TearDown]
        public void CleanUp()
        {
            _projects.Clear();
        }

        private ProjectEntity GetProjectById(int projectId)
        {
            return _projects.Where(x => x.ProjectId == projectId).Select(y => y).SingleOrDefault();
        }
    }
}
