using System;
using Eplan.EplApi.Starter;
using Eplan.EplApi.System;

namespace IntegrationTestsNUnit
{
  public class EplanApplicationWrapper
  {
    EplApplication _app;

    public EplanApplicationWrapper()
    {
      var finder = new EplanFinder();
      var binPath = finder.SelectEplanVersion(true);
      var assemblyResolver = new AssemblyResolver();
      assemblyResolver.SetEplanBinPath(binPath);
      var platformBinPath = assemblyResolver.GetPlatformBinPath();
      Environment.CurrentDirectory = platformBinPath;
      assemblyResolver.PinToEplan();
      StartEplan(binPath);
    }
    
    private void StartEplan(string binPath)
    {
      _app = new EplApplication();
      _app.EplanBinFolder = binPath;
      _app.Init("", true, true);
    }

    public void Exit()
    {
      _app?.Exit();
    }
  }
}