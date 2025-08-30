using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrbitalSharp
{
    public class Constants
    {
        private const int _kilo = 1000;

        public const double G = 6.67428e-11; // m^3 kg^-1 s^-2
        public const double SolarMassParameter = 1.32712440041e20; // m^3 s^-2

        public const double SolarMass = SolarMassParameter / G;
        public const double GeocentricGravitationalConstant = 3.986004415e14;  // m^3 s^-2
        public const double EarthMu = GeocentricGravitationalConstant; // m^3 s^-2

        public const double EarthMass = GeocentricGravitationalConstant / G; // kg

        public const double MercuryMass = SolarMass / 6.0236e6; // kg
        public const double VenusMass = SolarMass / 4.08523710e5; // kg
        public const double MarsMass = SolarMass / 3.09870359e6; // kg
        public const double JupiterMass = SolarMass / 1.047348644e3; // kg
        public const double SaturnMass = SolarMass / 3.4979018e3; // kg
        public const double UranusMass = SolarMass / 2.290298e4; // kg
        public const double NeptuneMass = SolarMass / 1.941226e4; // kg


        public const double SunRadiusEquatorial = 696000 * _kilo;
        public const double MercuryRadiusEquatorial = 2439.7 * _kilo;
        public const double VenusRadiusEquatorial = 6051.8 * _kilo;
        public const double EarthRadiusEquatorial = 6378.1366 * _kilo;
        public const double MarsRadiusEquatorial = 3396.19 * _kilo;
        public const double JupiterRadiusEquatorial = 71492 * _kilo;
        public const double SaturnRadiusEquatorial = 60268 * _kilo;
        public const double UranusRadiusEquatorial = 25559 * _kilo;
        public const double NeptuneRadiusEquatorial = 24764 * _kilo;

        public const double MercuryMu = MercuryMass * G;
        public const double VenusMu = VenusMass* G;
        public const double MarsMu = MarsMass* G;
        public const double JupiterMu = JupiterMass* G;
        public const double SaturnMu = SaturnMass* G;
        public const double UranusMu = UranusMass* G;
        public const double NeptuneMu = NeptuneMass* G;

        public const double MercuryRadiusPolar = MercuryRadiusEquatorial;
        public const double MercuryRadiusMean = MercuryRadiusEquatorial;
        public const double VenusRadiusPolar = VenusRadiusEquatorial;
        public const double VenusRadiusMean = VenusRadiusEquatorial;

        // The following constants are not from IAU
        public const double EarthRadiusMean = 6371.0 * _kilo;
        public const double EarthRadiusPolar = 6356.8 * _kilo;

        public const double MarsRadiusMean = 3389.5 * _kilo;
        public const double MarsRadiusPolar = 3376.2 * _kilo;

        public const double JupiterRadiusMean = 69911 * _kilo;
        public const double JupiterRadiusPolar = 66854 * _kilo;

        public const double SaturnRadiusMean = 58232 * _kilo;
        public const double SaturnRadiusPolar = 54364 * _kilo;

        public const double UranusRadiusMean = 25362 * _kilo;
        public const double UranusRadiusPolar = 24973 * _kilo;

        public const double NeptuneRadiusMean = 24622 * _kilo;
        public const double NeptuneRadiusPolar = 24341 * _kilo;

        public const double EarthSiderealDay = 23 * 3600 + 56 * 60 + 4.1; // seconds
    }
}
