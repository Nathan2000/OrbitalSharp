using OrbitalSharp.Wpf.Controls;
using OrbitalSharp.Wpf.Services;
using OrbitalSharp.Wpf.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace OrbitalSharp.Wpf.Commands
{
    public class NewOrbitFromStateVectorsCommand(MainViewModel parent, IDialogService dialogService) : ICommand
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
                Title = "New Orbit from State Vectors"
            };
            vm.AddDouble("rx", "Position X (km)", 0.0, "");
            vm.AddDouble("ry", "Position Y (km)", 0.0, "");
            vm.AddDouble("rz", "Position Z (km)", 0.0, "");
            vm.AddDouble("vx", "Velocity X (m/s)", 0.0, "");
            vm.AddDouble("vy", "Velocity Y (m/s)", 0.0, "");
            vm.AddDouble("vz", "Velocity Z (m/s)", 0.0, "");
            vm.AddBody("body", "Orbital Body", Body.Earth, "");
            if (dialogService.ShowDialog(vm))
            {
                var rx = vm.GetDouble("rx") * 1000;
                var ry = vm.GetDouble("ry") * 1000;
                var rz = vm.GetDouble("rz") * 1000;
                var vx = vm.GetDouble("vx");
                var vy = vm.GetDouble("vy");
                var vz = vm.GetDouble("vz");
                var r = new Vector3((float)rx, (float)ry, (float)rz);
                var v = new Vector3((float)vx, (float)vy, (float)vz);
                var body = vm.GetBody("body")!;
                var orbit = KeplerianElements.FromStateVectors(r, v, body);
                parent.Orbit = orbit;
                parent.Refresh();
            }
        }
    }
}
