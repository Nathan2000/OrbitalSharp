using OrbitalSharp.Wpf.Controls;
using OrbitalSharp.Wpf.Services;
using OrbitalSharp.Wpf.ViewModel;
using System.Windows.Input;

namespace OrbitalSharp.Wpf.Commands
{
    public class NewOrbitWithAltitudeCommand(MainViewModel parent, IDialogService dialogService) : ICommand
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
                Title = "New Orbit with Altitude",
            };
            vm.AddDouble("altitude", "Altitude (km)", 400.0, "");
            vm.AddBody("body", "Orbital Body", Body.Earth, "");
            if (dialogService.ShowDialog(vm))
            {
                var altitude = vm.GetDouble("altitude");
                var body = vm.GetBody("body")!;
                var orbit = KeplerianElements.WithAltitude(altitude * 1000, body);
                parent.Orbit = orbit;
                parent.Refresh();
            }
        }
    }
}
