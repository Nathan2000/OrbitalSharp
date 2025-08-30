using System.Numerics;
using System.Text;

namespace OrbitalSharp
{
    public class KeplerianElements
    {
        private static readonly DateTime J2000 = new (2000, 1, 1, 12, 0, 0, DateTimeKind.Utc);

        private double _a;
        private double _e;
        private double _i;
        private double _raan;
        private double _argPe;
        private double _M0;

        private double _M;
        private Body _body;
        private DateTime _refEpoch;
        private double _t;

        public KeplerianElements(
            double a,
            double e = 0.0,
            double i = 0.0,
            double raan = 0.0,
            double argPe = 0.0,
            double M0 = 0.0,
            Body? body = null,
            DateTime? refEpoch = null)
        {
            _a = a;
            _e = e;
            _i = i;
            _raan = raan;
            _argPe = argPe;
            _M0 = M0;

            _M = M0;
            _body = body ?? throw new ArgumentNullException(nameof(body));
            _refEpoch = refEpoch ?? J2000;

            _t = 0.0;
        }

        /// <summary>
        /// Eccentricity.
        /// </summary>
        public double e
        {
            get => _e;
            set => _e = value;
        }

        /// <summary>
        /// Inclination [rad].
        /// </summary>
        public double i
        {
            get => _i;
            set => _i = value;
        }

        /// <summary>
        /// Right Ascension of the Ascending Node [rad].
        /// </summary>
        public double raan
        {
            get => _raan;
            set => _raan = value;
        }

        /// <summary>
        /// Argument of Periapsis [rad].
        /// </summary>
        public double argPe
        {
            get => _argPe;
            set => _argPe = value;
        }

        /// <summary>
        /// Mean anomaly at refEpoch [rad].
        /// </summary>
        public double M0
        {
            get => _M0;
            set => _M0 = value;
        }

        public Body body
        {
            get => _body;
        }

        /// <summary>
        /// Current epoch calculated from time since refEpoch (DateTime).
        /// </summary>
        public DateTime epoch
        {
            get
            {
                return _refEpoch.AddSeconds(t);
            }
            set
            {
                double t = (value - _refEpoch).TotalSeconds;
                _M = _M0 + n * _t;
                _M = _M % (2 * Math.PI);
                _t = t;
            }
        }

        /// <summary>
        /// Time since refEpoch [seconds].
        /// </summary>
        public double t
        {
            get
            {
                return _t;
            }
            set
            {
                _M = _M0 + n * value;
                _M = _M % (2 * Math.PI);
                _t = value;
            }
        }

        /// <summary>
        /// Mean Anomaly [rad].
        /// </summary>
        public double M
        {
            get
            {
                return _M;
            }
            set
            {
                _M = value % (2 * Math.PI);
            }
        }

        /// <summary>
        /// Eccentric Anomaly [rad].
        /// </summary>
        public double E
        {
            get
            {
                return Utils.EccentricAnomalyFromMean(_e, _M);
            }
            set
            {
                _M = Utils.MeanAnomalyFromEccentric(_e, value);
            }
        }

        /// <summary>
        /// True Anomaly [rad].
        /// </summary>
        public double f
        {
            get
            {
                return Utils.TrueAnomalyFromMean(_e, _M);
            }
            set
            {
                _M = Utils.MeanAnomalyFromTrue(_e, value);
            }
        }

        /// <summary>
        /// Semimajor axis.
        /// </summary>
        public double a
        {
            get => _a;
            set
            {
                _a = value;
                _M0 = (M - n * _t) % (2 * Math.PI);
            }
        }

        /// <summary>
        /// Position vector [m].
        /// </summary>
        public Vector3 r
        {
            get
            {
                return (float)Utils.OrbitRadius(_a, _e, f) * U;
            }
        }

        /// <summary>
        /// Velocity vector [m/s].
        /// </summary>
        public Vector3 v
        {
            get
            {
                double rDot = Math.Sqrt(_body.Mu / _a) * (_e * Math.Sin(f)) / Math.Sqrt(1 - Math.Pow(_e, 2));
                double rfDot = Math.Sqrt(_body.Mu / _a) * (1 + _e * Math.Cos(f)) / Math.Sqrt(1 - Math.Pow(_e, 2));
                return (float)rDot * U + (float)rfDot * V;
            }
            set
            {
                KeplerianElements elements = Utils.ElementsFromStateVector(r, value, _body);
                _a = elements.a;
                _e = elements.e;
                _i = elements.i;
                _raan = elements.raan;
                _argPe = elements.argPe;
                f = elements.f;

                _M0 = (M - n * _t) % (2 * Math.PI);
            }
        }

        /// <summary>
        /// Mean motion [rad/s].
        /// </summary>
        public double n
        {
            get
            {
                return Math.Sqrt(_body.Mu / Math.Pow(_a, 3));
            }
            set
            {
                _a = Math.Pow(_body.Mu / Math.Pow(value, 2), 1.0 / 3.0);
            }
        }

        /// <summary>
        /// Period [s].
        /// </summary>
        public double T
        {
            get
            {
                return 2 * Math.PI / n;
            }
            set
            {
                _a = Math.Pow(_body.Mu * Math.Pow(value, 2) / (4 * Math.Pow(Math.PI, 2)), 1.0 / 3.0);
            }
        }

        public double ApocenterRadius
        {
            get => _a * (1 + _e);
        }

        public double PericenterRadius
        {
            get => _a * (1 - _e);
        }

        public double ApocenterAltitude
        {
            get => Utils.AltitudeFromRadius(ApocenterRadius, _body);
        }

        public double PericenterAltitude
        {
            get => Utils.AltitudeFromRadius(PericenterRadius, _body);
        }

        /// <summary>
        /// Radial direction unit vector.
        /// </summary>
        public Vector3 U
        {
            get
            {
                double u = _argPe + f;
                double sinu = Math.Sin(u);
                double cosu = Math.Cos(u);
                double sinraan = Math.Sin(_raan);
                double cosraan = Math.Cos(_raan);
                double cosi = Math.Cos(_i);
                return new Vector3(
                    (float)(cosu * cosraan - sinu * sinraan * cosi),
                    (float)(cosu * sinraan + sinu * cosraan * cosi),
                    (float)(sinu * Math.Sin(_i))
                );
            }
        }

        /// <summary>
        /// Transversal in-flight direction unit vector.
        /// </summary>
        public Vector3 V
        {
            get
            {
                double u = _argPe + f;

                double sinu = Math.Sin(u);
                double cosu = Math.Cos(u);
                double sinraan = Math.Sin(_raan);
                double cosraan = Math.Cos(_raan);
                double cosi = Math.Cos(_i);
                return new Vector3(
                    (float)(-sinu * cosraan - cosu * sinraan * cosi),
                    (float)(-sinu * sinraan + cosu * cosraan * cosi),
                    (float)(cosu * Math.Sin(_i))
                );
            }
        }

        /// <summary>
        /// Out-of-plane direction unit vector.
        /// </summary>
        public Vector3 W
        {
            get
            {
                double sini = Math.Sin(_i);
                return new Vector3(
                    (float)(Math.Sin(_raan) * sini),
                    (float)(-Math.Cos(_raan) * sini),
                    (float)(Math.Cos(_i)));
            }
        }

        public (Vector3 U, Vector3 V, Vector3 W) UVW
        {
            get
            {
                return Utils.UVWFromElements(_i, _raan, _argPe, f);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendLine($"Semi-major axis (a): {_a} m");
            sb.AppendLine($"Eccentricity (e): {_e}");
            sb.AppendLine($"Inclination (i): {Utils.RadiansToDegrees(_i)}");
            sb.AppendLine($"Right Ascension of Ascending Node (RAAN): {Utils.RadiansToDegrees(_raan)}");
            sb.AppendLine($"Argument of Periapsis (argPe): {Utils.RadiansToDegrees(_argPe)}");
            sb.AppendLine($"Mean Anomaly at Epoch (M0): {Utils.RadiansToDegrees(_M0)}");
            sb.AppendLine($"Period (T): {T}");
            sb.AppendLine($"Reference Epoch: {_refEpoch}");
            sb.AppendLine($"    Mean Anomaly (M): {Utils.RadiansToDegrees(_M)}");
            sb.AppendLine($"    Time (t): {_t}");
            sb.AppendLine($"    Epoch: {epoch}");
            return sb.ToString();
        }

        #region Static Methods

        public static KeplerianElements WithAltitude(
            double altitude,
            Body body,
            double e = 0.0,
            double i = 0.0,
            double raan = 0.0,
            double argPe = 0.0,
            double M0 = 0.0,
            DateTime? refEpoch = null)
        {
            var r = Utils.RadiusFromAltitude(altitude, body);
            double a = r * (1 + e * Math.Cos(Utils.TrueAnomalyFromMean(e, M0))) / (1 - Math.Pow(e, 2));
            return new KeplerianElements(a, e, i, raan, argPe, M0, body, refEpoch);
        }

        public static KeplerianElements WithPeriod(
            double period,
            Body body,
            double e = 0.0,
            double i = 0.0,
            double raan = 0.0,
            double argPe = 0.0,
            double M0 = 0.0,
            DateTime? refEpoch = null)
        {
            var ke = new KeplerianElements(0, e, i, raan, argPe, M0, body, refEpoch);
            ke.T = period;
            return ke;
        }

        public static KeplerianElements WithApsideAltitudes(
            double alt1,
            double alt2,
            Body body,
            double i = 0.0,
            double raan = 0.0,
            double argPe = 0.0,
            double M0 = 0.0,
            DateTime? refEpoch = null)
        {
            List<double> altitudes = [alt1, alt2];
            altitudes.Sort();

            double pericenterAltitude = altitudes[0];
            double apocenterAltitude = altitudes[1];

            double apocenterRadius = Utils.RadiusFromAltitude(apocenterAltitude, body);
            double pericenterRadius = Utils.RadiusFromAltitude(pericenterAltitude, body);

            (double a, double e) = Utils.ElementsForApsides(apocenterRadius, pericenterRadius);
            return new KeplerianElements(a, e, i, raan, argPe, M0, body, refEpoch);
        }

        public static KeplerianElements WithApsideRadii(
            double radius1,
            double radius2,
            Body body,
            double i = 0.0,
            double raan = 0.0,
            double argPe = 0.0,
            double M0 = 0.0,
            DateTime? refEpoch = null)
        {
            List<double> radii = [radius1, radius2];
            radii.Sort();
            double pericenterRadius = radii[0];
            double apocenterRadius = radii[1];

            (double a, double e) = Utils.ElementsForApsides(apocenterRadius, pericenterRadius);
            return new KeplerianElements(a, e, i, raan, argPe, M0, body, refEpoch);
        }

        public static KeplerianElements FromStateVectors(
            Vector3 r,
            Vector3 v,
            Body body,
            DateTime? refEpoch = null)
        {
            var elements = Utils.ElementsFromStateVector(r, v, body);
            var value = new KeplerianElements(
                elements.a,
                elements.e,
                elements.i,
                elements.raan,
                elements.argPe,
                Utils.MeanAnomalyFromTrue(elements.e, elements.f),
                body,
                refEpoch);
            value._M0 = (value.M - value.n * value.t) % 2 * Math.PI;
            return value;
        }

        #endregion
    }
}
