using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Eplan.EplApi.Starter;
using Eplan.EplApi.System;

namespace IntegrationTestsNUnit
{
  public class EplanApplicationWrapper
  {
    private EplApplication _app;

    public EplanApplicationWrapper()
    {
      var finder = new EplanFinder();
      var binPath = finder.SelectEplanVersion(true);
      PinToEplan(binPath);
      StartEplan(binPath, null);
    }

    private static void PinToEplan(string binPath)
    {
      var assemblyResolver = new AssemblyResolver();
      assemblyResolver.SetEplanBinPath(binPath);
      var platformBinPath = assemblyResolver.GetPlatformBinPath();
      Environment.CurrentDirectory = platformBinPath;
      assemblyResolver.PinToEplan();
    }

    public EplanApplicationWrapper(string version, string variant, string systemConfiguration)
    {
      List<EplanData> instancesInstalled = GetInstalledEplanInstances();
      instancesInstalled = instancesInstalled
                           .Where(obj =>
                                    obj.EplanVariant.Equals(variant) &&
                                    obj.EplanVersion.StartsWith(version))
                           .OrderBy(obj => obj.EplanVersion)
                           .ToList();
      if (!instancesInstalled.Any())
      {
        throw new Exception($"EPLAN instance in version {version} not found");
      }

      string binPath = instancesInstalled.First().EplanPath;
      binPath = Path.GetDirectoryName(binPath);
      PinToEplan(binPath);
      StartEplan(binPath, systemConfiguration);
    }

    private static List<EplanData> GetInstalledEplanInstances()
    {
      EplanFinder eplanFinder = new EplanFinder();
      List<EplanData> eplanVersions = new List<EplanData>();
      eplanFinder.GetInstalledEplanVersions(ref eplanVersions, true);
      return eplanVersions;
    }

    private void StartEplan(string binPath, string systemConfiguration)
    {
      _app = new EplApplication();
      _app.EplanBinFolder = binPath;
      if (!string.IsNullOrWhiteSpace(systemConfiguration))
      {
        _app.SystemConfiguration = systemConfiguration;  
      }
      _app.Init("", true, true);
    }

    public void Exit()
    {
      _app?.Exit();
    }
  }
}