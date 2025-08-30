using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbitalSharp
{
    public class Body
    {
        public string Name { get; init; }
        public double Mass { get; init; } // in kg
        public double Mu { get; init; } // in m^3/s^2
        public double MeanRadius { get; init; } // in m
        public double EquatorialRadius { get; init; } // in m
        public double PolarRadius { get; init; } // in m
        public string? PlotColor { get; init; } // in hex format, e.g. "#ff0000"

        public string? AtmosphereColor { get; init; }
        public double AtmosphereThickness { get; init; }

        private string[] _apoapsisNames = [];
        public string[] ApoapsisNames
        {
            get => _apoapsisNames;
            set => _apoapsisNames = value ?? [];
        }
        public string ApoapsisName
        {
            get => _apoapsisNames.Length > 0 ? _apoapsisNames[0] : "apoapsis";
            set => _apoapsisNames = [value];
        }

        private string[] _periapsisNames = [];
        public string[] PeriapsisNames
        {
            get => _periapsisNames;
            set => _periapsisNames = value ?? [];
        }
        public string PeriapsisName
        {
            get => _periapsisNames.Length > 0 ? _periapsisNames[0] : "periapsis";
            set => _periapsisNames = [value];
        }

        public Body(
            string name,
            double mass,
            double mu,
            double meanRadius,
            double equatorialRadius,
            double polarRadius,
            string[]? apoapsisNames = null,
            string[]? periapsisNames = null,
            string? plotColor = null,
            string? atmosphereColor = null,
            double atmosphereThickness = 0)
        {
            Name = name;
            Mass = mass;
            Mu = mu;
            MeanRadius = meanRadius;
            EquatorialRadius = equatorialRadius;
            PolarRadius = polarRadius;
            ApoapsisNames = apoapsisNames ?? [];
            PeriapsisNames = periapsisNames ?? [];
            PlotColor = plotColor;
            AtmosphereColor = atmosphereColor;
            AtmosphereThickness = atmosphereThickness;
        }

        public static readonly Body Mercury = new(
            "Mercury",
            Constants.MercuryMass,
            Constants.MercuryMu,
            Constants.MercuryRadiusMean,
            Constants.MercuryRadiusEquatorial,
            Constants.MercuryRadiusPolar,
            ["aphermion"],
            ["perihermion"],
            "#ffd8b0");
        public static readonly Body Venus = new(
            "Venus",
            Constants.VenusMass,
            Constants.VenusMu,
            Constants.VenusRadiusMean,
            Constants.VenusRadiusEquatorial,
            Constants.VenusRadiusPolar,
            ["apocytherion", "apocytherean", "apokrition"],
            ["pericytherion", "pericytherean", "perikrition"],
            "#d58f41",
            "#f5deb3",
            258.97);
        public static readonly Body Earth = new(
            "Earth",
            Constants.EarthMass,
            Constants.EarthMu,
            Constants.EarthRadiusMean,
            Constants.EarthRadiusEquatorial,
            Constants.EarthRadiusPolar,
            ["apogee"],
            ["perigee"],
            "#4e82ff",
            "#87cefa",
            100.0);
        public static readonly Body Mars = new(
            "Mars",
            Constants.MarsMass,
            Constants.MarsMu,
            Constants.MarsRadiusMean,
            Constants.MarsRadiusEquatorial,
            Constants.MarsRadiusPolar,
            ["apoareion"],
            ["perareion"],
            "#a0522d",
            "#d2b48c",
            99.9);
        public static readonly Body Jupiter = new(
            "Jupiter",
            Constants.JupiterMass,
            Constants.JupiterMu,
            Constants.JupiterRadiusMean,
            Constants.JupiterRadiusEquatorial,
            Constants.JupiterRadiusPolar,
            ["apozene", "apojove"],
            ["perizene", "perijove"],
            "#ff7726",
            "#ff7726",
            317.65);
        public static readonly Body Saturn = new(
            "Saturn",
            Constants.SaturnMass,
            Constants.SaturnMu,
            Constants.SaturnRadiusMean,
            Constants.SaturnRadiusEquatorial,
            Constants.SaturnRadiusPolar,
            ["apokrone", "aposaturnium"],
            ["perikrone", "perisaturnium"],
            "#ffe296",
            "#ffe296",
            700.0);
        public static readonly Body Uranus = new(
            "Uranus",
            Constants.UranusMass,
            Constants.UranusMu,
            Constants.UranusRadiusMean,
            Constants.UranusRadiusEquatorial,
            Constants.UranusRadiusPolar,
            ["apouranion"],
            ["periuranion"],
            "#becaff",
            "#becaff",
            325.88);
        public static readonly Body Neptune = new(
            "Neptune",
            Constants.NeptuneMass,
            Constants.NeptuneMu,
            Constants.NeptuneRadiusMean,
            Constants.NeptuneRadiusEquatorial,
            Constants.NeptuneRadiusPolar,
            ["apoposeidion"],
            ["periposeidion"],
            "#8da4ff",
            "#8da4ff",
            231.76);
    }
}
