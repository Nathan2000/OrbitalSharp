using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OrbitalSharp
{
    public static class Utils
    {
        public static double DegreesToRadians(double degrees) => degrees * Math.PI / 180.0;
        public static double RadiansToDegrees(double radians) => radians * 180.0 / Math.PI;

        public static double EccentricAnomalyFromMean(double e, double M, double tolerance = 1e-6)
        {
            // Normalize M to the range [0, 2π)
            M = M % (2 * Math.PI);
            if (M < 0)
                M += 2 * Math.PI;

            double E0 = M; // Initial guess
            double dE = tolerance + 1;
            int count = 0;
            while (dE > tolerance)
            {
                double t1 = Math.Cos(E0);
                double t2 = -1 + e * t1;
                double t3 = Math.Sin(E0);
                double t4 = e * t3;
                double t5 = -E0 + t4 + M;
                double t6 = t5 / (0.5 * t5 * t4 / t2 + t2);
                double E = E0 - t5 / ((0.5 * t3 - (1 / 6) * t1 * t6) * e * t6 + t2);
                dE = Math.Abs(E - E0);
                E0 = E;
                count++;

                if (count > 100)
                    throw new Exception("Eccentric anomaly calculation did not converge.");
            }
            return E0;
        }

        public static double EccentricAnomalyFromTrue(double e, double f)
        {
            double E = Math.Atan2(Math.Sqrt(1 - Math.Pow(e, 2)) * Math.Sin(f), e + Math.Cos(f));
            E = E % (2 * Math.PI);
            if (E < 0)
                E += 2 * Math.PI;
            return E;
        }

        public static double MeanAnomalyFromEccentric(double e, double E)
        {
            return E - e * Math.Sin(E);
        }

        public static double MeanAnomalyFromTrue(double e, double f)
        {
            double E = EccentricAnomalyFromTrue(e, f);
            return MeanAnomalyFromEccentric(e, E);
        }

        public static double TrueAnomalyFromEccentric(double e, double E)
        {
            return 2 * Math.Atan2(Math.Sqrt(1 + e) * Math.Sin(E / 2), Math.Sqrt(1 - e) * Math.Cos(E / 2));
        }

        public static double TrueAnomalyFromMean(double e, double M)
        {
            double E = EccentricAnomalyFromMean(e, M);
            return TrueAnomalyFromEccentric(e, E);
        }

        public static double OrbitRadius(double a, double e, double f)
        {
            return a * (1 - Math.Pow(e, 2)) / (1 + e * Math.Cos(f));
        }

        public static (double a, double e) ElementsForApsides(double ra, double rp)
        {
            double a = (ra + rp) / 2;
            double e = (ra - rp) / (ra + rp);
            return (a, e);
        }

        public static double RadiusFromAltitude(double altitude, Body body)
        {
            if (body == null)
                throw new ArgumentNullException(nameof(body), "Body cannot be null.");
            return body.MeanRadius + altitude;
        }

        public static double AltitudeFromRadius(double radius, Body body)
        {
            if (body == null)
                throw new ArgumentNullException(nameof(body), "Body cannot be null.");
            return radius - body.MeanRadius;
        }

        public static Vector3 AngularMomentum(Vector3 position, Vector3 velocity)
        {
            return Vector3.Cross(position, velocity);
        }

        public static Vector3 NodeVector(Vector3 angularMomentum)
        {
            return Vector3.Cross(new Vector3(0, 0, 1), angularMomentum);
        }

        public static Vector3 EccentricityVector(Vector3 position, Vector3 velocity, double mu)
        {
            var r = position;
            var v = velocity;
            var ev = r * (float)(1 / mu) * (float)(v.LengthSquared() - mu / r.Length()) - Vector3.Dot(r, v) * v;
            return ev;
        }

        public static double SpecificOrbitalEnergy(Vector3 position, Vector3 velocity, double mu)
        {
            return velocity.LengthSquared() / 2 - mu / position.Length();
        }

        public static KeplerianElements ElementsFromStateVector(Vector3 r, Vector3 v, Body body)
        {
            const double SmallNumber = 1e-15;
            var h = AngularMomentum(r, v);
            var n = NodeVector(h);

            var ev = EccentricityVector(r, v, body.Mu);
            var E = SpecificOrbitalEnergy(r, v, body.Mu);

            var a = body.Mu / (2 * E);
            var e = ev.Length();

            double i = Math.Acos(h.Z / h.Length());
            double raan;
            double argPe;
            if (Math.Abs(i) < SmallNumber)
            {
                // For non-inclined orbits, RAAN is undefined; set to zero by convention
                raan = 0.0;
                if (Math.Abs(e) < SmallNumber)
                    // For circular orbits, place periapsis at ascending node by convention
                    argPe = 0.0;
                else
                    // Argument of periapsis is the angle between eccentricity vector its X component
                    argPe = Math.Acos(ev.X / ev.Length());
            }
            else
            {
                // Right ascension of ascending node is the angle between the node vector and its X component
                raan = Math.Acos(n.X / n.Length());
                if (n.Y < 0)
                    raan = 2 * Math.PI - raan;

                // Argument of periapsis is the angle between node and eccentricity vectors
                argPe = Math.Acos(Vector3.Dot(n, ev) / (n.Length() * ev.Length()));
            }

            double f;
            if (Math.Abs(e) < SmallNumber)
            {
                if (Math.Abs(i) < SmallNumber)
                {
                    // True anomaly is the angle between position vector and its X component
                    f = Math.Acos(r.X / r.Length());
                    if (v.X > 0)
                        f = 2 * Math.PI - f;
                }
                else
                {
                    // True anomaly is the angle between node vector and position vector
                    f = Math.Acos(Vector3.Dot(n, r) / (n.Length() * r.Length()));
                    if (Vector3.Dot(n, r) > 0)
                        f = 2 * Math.PI - f;
                }
            }
            else
            {
                if (ev.Z < 0)
                    argPe = 2 * Math.PI - argPe;

                // True anomaly is the angle between eccentricity vector and position vector
                f = Math.Acos(Vector3.Dot(ev, r) / (ev.Length() * r.Length()));
                if (Vector3.Dot(r, v) < 0)
                    f = 2 * Math.PI - f;
            }

            return new KeplerianElements(a, e, i, raan, argPe, f, body);
        }

        public static (Vector3 U, Vector3 V, Vector3 W) UVWFromElements(double i, double raan, double argPe, double f)
        {
            double u = argPe + f;

            double sinu = Math.Sin(u);
            double cosu = Math.Cos(u);
            double sinraan = Math.Sin(raan);
            double cosraan = Math.Cos(raan);
            double sini = Math.Sin(i);
            double cosi = Math.Cos(i);
            var U = new Vector3(
                (float)(cosu * cosraan - sinu * sinraan * cosi),
                (float)(cosu * sinraan + sinu * cosraan * cosi),
                (float)(sinu * sini)
            );
            var V = new Vector3(
                (float)(-sinu * cosraan - cosu * sinraan * cosi),
                (float)(-sinu * sinraan + cosu * cosraan * cosi),
                (float)(cosu * sini)
            );
            var W = new Vector3(
                (float)(sinraan * sini),
                (float)(-cosraan * sini),
                (float)cosi
            );
            return (U, V, W);
        }
    }
}
