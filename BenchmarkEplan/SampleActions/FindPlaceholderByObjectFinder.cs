using System.Linq;
using Eplan.EplApi.ApplicationFramework;
using Eplan.EplApi.DataModel;
using Eplan.EplApi.DataModel.Graphics;
using Eplan.EplApi.HEServices;

namespace BenchmarkEplan.SampleActions
{
    public class FindPlaceholderByObjectFinder : IEplAction
    {
        public bool OnRegister(ref string Name, ref int Ordinal)
        {
            Name = nameof(FindPlaceholderByObjectFinder);
            Ordinal = 20;
            return true;
        }

        public bool Execute(ActionCallingContext oActionCallingContext)
        {
            var project = new SelectionSet().GetCurrentProject(true); // with lock
            PlaceHolder[] placeHolders = new DMObjectsFinder(project).GetPlacements(null).OfType<PlaceHolder>().ToArray();
            return true;
        }

        public void GetActionProperties(ref ActionProperties actionProperties)
        {
        }
    }
}