using OrbitalSharp.Plotting;
using OrbitalSharp.Wpf.Commands;
using OrbitalSharp.Wpf.Controls;
using OrbitalSharp.Wpf.Services;
using ScottPlot.WPF;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace OrbitalSharp.Wpf.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private DateTime lastTickTime;

        public double SemimajorAxis
        {
            get => Orbit.a / 1000;
            set
            {
                Orbit.a = value * 1000;
                OnPropertyChanged();
                Refresh();
            }
        }

        public double Eccentricity
        {
            get => Orbit.e;
            set
            {
                Orbit.e = value;
                OnPropertyChanged();
                Refresh();
            }
        }

        public double Inclination
        {
            get => Orbit.i * 180 / Math.PI;
            set
            {
                Orbit.i = value * Math.PI / 180;
                OnPropertyChanged();
                Refresh();
            }
        }

        public double RAAN
        {
            get => Orbit.raan * 180 / Math.PI;
            set
            {
                Orbit.raan = value * Math.PI / 180;
                OnPropertyChanged();
                Refresh();
            }
        }

        public double ArgumentOfPeriapsis
        {
            get => Orbit.argPe * 180 / Math.PI;
            set
            {
                Orbit.argPe = value * Math.PI / 180;
                OnPropertyChanged();
                Refresh();
            }
        }

        public double MeanAnomaly
        {
            get => Orbit.M * 180 / Math.PI;
            set
            {
                Orbit.M = value * Math.PI / 180;
                OnPropertyChanged();
                Refresh(false);
            }
        }

        private double _timeScale = 1.0;
        public double TimeScale
        {
            get => _timeScale;
            set
            {
                _timeScale = value;
                OnPropertyChanged();
            }
        }
        public double LogTimeScale
        {
            get => Math.Log10(_timeScale);
            set
            {
                TimeScale = Math.Pow(10, value);
                OnPropertyChanged();
                OnPropertyChanged(nameof(TimeScale));
            }
        }

        public string FormattedSimTime => TimeSpan.FromSeconds(Orbit.t).ToString(@"hh\:mm\:ss");

        public ICommand NewOrbitWithAltitude { get; set; }
        public ICommand NewOrbitWithPeriod { get; set; }
        public ICommand NewOrbitWithApsideAltitudes { get; set; }
        public ICommand NewOrbitWithApsideRadii { get; set; }
        public ICommand NewOrbitFromStateVectors { get; set; }

        public ICommand OperationPropagateAnomalyTo { get; set; }
        public ICommand OperationSetApocenterRadiusTo { get; set; }

        public WpfPlot PlotControl { get; }
        
        public KeplerianElements Orbit { get; internal set; }

        private readonly ScottOrbitPlotter _plotter;

        public event PropertyChangedEventHandler? PropertyChanged;

        public MainViewModel(IDialogService dialogService)
        {
            PlotControl = new WpfPlot();
            Orbit = KeplerianElements.WithPeriod(90 * 60, body: Body.Earth);
            _plotter = new ScottOrbitPlotter(PlotControl.Plot);

            NewOrbitWithAltitude = new NewOrbitWithAltitudeCommand(this, dialogService);
            NewOrbitWithPeriod = new NewOrbitWithPeriodCommand(this, dialogService);
            NewOrbitWithApsideAltitudes = new NewOrbitWithApsideAltitudesCommand(this, dialogService);
            NewOrbitWithApsideRadii = new NewOrbitWithApsideRadiiCommand(this, dialogService);
            NewOrbitFromStateVectors = new NewOrbitFromStateVectorsCommand(this, dialogService);

            OperationPropagateAnomalyTo = new OperationPropagateAnomalyToCommand(this, dialogService);
            OperationSetApocenterRadiusTo = new OperationSetApocenterRadiusToCommand(this, dialogService);

            Refresh();

            SetupAnimation();
        }

        private void SetupAnimation()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
                return;

            lastTickTime = DateTime.UtcNow;
            CompositionTarget.Rendering += OnRenderFrame;
        }

        private void OnRenderFrame(object? sender, EventArgs e)
        {
            DateTime now = DateTime.UtcNow;
            double deltaTime = (now - lastTickTime).TotalSeconds;
            lastTickTime = now;

            Orbit.t += deltaTime * TimeScale;
            OnPropertyChanged(nameof(FormattedSimTime));
            Refresh(false);
        }

        public void Refresh(bool firstTime = true)
        {
            _plotter.Plot(Orbit, "Orbit", firstTime);
            PlotControl.Refresh();
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
