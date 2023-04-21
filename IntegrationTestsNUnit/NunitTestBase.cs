using NUnit.Framework;

namespace IntegrationTestsNUnit
{
  [TestFixture, SingleThreaded]
  public class NunitTestBase
  {
    private const string VERSION = "2.9";
    private const string VARIANT = "Electric P8";
    
    private EplanApplicationWrapper _app;

    [OneTimeSetUp]
    public void StartEplan()
    {
      _app = new EplanApplicationWrapper(VERSION, VARIANT);
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
                              @"\\Mac\Home\Documents\EPLAN\2.9.4\Dev\Data\Projekte\ibKastl", true));
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