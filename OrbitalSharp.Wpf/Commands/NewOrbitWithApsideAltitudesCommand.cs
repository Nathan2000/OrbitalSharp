using OrbitalSharp.Wpf.Controls;
using OrbitalSharp.Wpf.Services;
using OrbitalSharp.Wpf.ViewModel;
using System.Windows.Input;

namespace OrbitalSharp.Wpf.Commands
{
    public class NewOrbitWithApsideAltitudesCommand(MainViewModel parent, IDialogService dialogService) : ICommand
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
                Title = "New Orbit with Apside Altitudes"
            };
            vm.AddDouble("altitude1", "Altitude 1 (km)", 400.0, "");
            vm.AddDouble("altitude2", "Altitude 2 (km)", 300.0, "");
            vm.AddBody("body", "Orbital Body", Body.Earth, "");
            if (dialogService.ShowDialog(vm) == true)
            {
                var altitude1 = vm.GetDouble("altitude1");
                var altitude2 = vm.GetDouble("altitude2");
                var body = vm.GetBody("body")!;
                var orbit = KeplerianElements.WithApsideAltitudes(altitude1 * 1000, altitude2 * 1000, body);
                parent.Orbit = orbit;
                parent.Refresh();
            }
        }
    }
}
