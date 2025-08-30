using OrbitalSharp.Wpf.Controls;
using OrbitalSharp.Wpf.Services;
using OrbitalSharp.Wpf.ViewModel;
using System.Windows.Input;

namespace OrbitalSharp.Wpf.Commands
{
    public class NewOrbitWithPeriodCommand(MainViewModel parent, IDialogService dialogService) : ICommand
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
                Title = "New Orbit with Period"
            };
            vm.AddDouble("period", "Orbital Period (seconds)", 90.0 * 60.0, "");
            vm.AddBody("body", "Orbital Body", Body.Earth, "");
            if (dialogService.ShowDialog(vm))
            {
                var period = vm.GetDouble("period");
                var body = vm.GetBody("body")!;
                var orbit = KeplerianElements.WithPeriod(period, body);
                parent.Orbit = orbit;
                parent.Refresh();
            }
        }
    }
}
