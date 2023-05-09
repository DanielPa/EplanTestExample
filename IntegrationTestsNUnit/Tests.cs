using System.Threading;
using NUnit.Framework;

namespace IntegrationTestsNUnit
{
  [TestFixture("2.9", "Electric P8", "API")]
  [TestFixture("2023", "Electric P8", "API")]
  [SingleThreaded]
  [Apartment(ApartmentState.STA)]
  public class Tests
  {
    private readonly string _version;
    private readonly string _variant;
    private readonly string _systemConfiguration;
    private EplanApplicationWrapper _app;

    public Tests(string version, string variant, string systemConfiguration)
    {
      _version = version;
      _variant = variant;
      _systemConfiguration = systemConfiguration;
    }

    [OneTimeSetUp]
    public void StartEplan()
    {
      _app = new EplanApplicationWrapper(_version, _variant, _systemConfiguration);
    }

    [OneTimeTearDown]
    public void EndEplan()
    {
      _app?.Exit();
    }

    [Test]
    public void ClassFromBaseAssemblyTest()
    {
      var pathVar = "$(MD_SCRIPTS)";
      var path = Eplan.EplApi.Base.PathMap.SubstitutePath(pathVar);
      Assert.AreNotEqual(path, pathVar);
    }

    [Test]
    public void ClassFromApplicationFrameworkAssemblyTest()
    {
      var actionManager = new Eplan.EplApi.ApplicationFramework.ActionManager();
      Assert.NotNull(actionManager);
    }

    [Test]
    public void ClassFromGuiAssemblyTest()
    {
      var mruList = new Eplan.EplApi.Gui.MRUList();
      var projects = mruList.GetProjects();
      Assert.IsNotEmpty(projects);
    }

    [Test]
    public void ClassFromHEServicesAssemblyTest()
    {
      var projectManagement = new Eplan.EplApi.HEServices.ProjectManagement();
      Assert.DoesNotThrow(() =>
                            projectManagement.LoadDirectory(
                              @"\\Mac\Home\Documents\EPLAN\2.9\Data\Projekte\ibKastl", true));
    }

    [Test]
    public void ClassFromEServicesAssemblyTest()
    {
      var position = new Eplan.EplApi.EServices.Ged.Position("0/0");
      Assert.NotNull(position);
    }

    [Test]
    public void ClassFromMasterDataAssemblyTest()
    {
      var partsManagement = new Eplan.EplApi.MasterData.MDPartsManagement();
      var db = partsManagement.OpenDatabase();
      Assert.IsNotEmpty(db.Parts);
    }

    [Test]
    public void ClassFromDataModelAssemblyTest()
    {
      var projectManagement = new Eplan.EplApi.DataModel.ProjectManager {LockProjectByDefault = false};
      Assert.NotNull(projectManagement);
    }
  }
}