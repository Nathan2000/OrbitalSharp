using OrbitalSharp.Wpf.Controls;
using OrbitalSharp.Wpf.Services;
using OrbitalSharp.Wpf.ViewModel;
using System.Windows.Input;

namespace OrbitalSharp.Wpf.Commands
{
    public class NewOrbitWithApsideRadiiCommand(MainViewModel parent, IDialogService dialogService) : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            var vm = new NewOrbitViewModel
            {
                Title = "New Orbit with Apside Radii"
            };
            vm.AddDouble("radius1", "Radius 1 (km)", 400.0 + Body.Earth.MeanRadius / 1000, "");
            vm.AddDouble("radius2", "Radius 2 (km)", 300.0 + Body.Earth.MeanRadius / 1000, "");
            vm.AddBody("body", "Orbital Body", Body.Earth, "");
            if (dialogService.ShowDialog(vm))
            {
                var radius1 = vm.GetDouble("radius1");
                var radius2 = vm.GetDouble("radius2");
                var body = vm.GetBody("body")!;
                var orbit = KeplerianElements.WithApsideRadii(radius1 * 1000, radius2 * 1000, body);
                parent.Orbit = orbit;
                parent.Refresh();
            }
        }
    }
}
