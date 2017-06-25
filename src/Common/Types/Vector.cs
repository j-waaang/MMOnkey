namespace JYW.ThesisMMO.Common.Types {

    using System;

    public struct Vector {
        public const float TOLERANCE = 0.000001f;

        public static Vector Zero;

        public float X { get; set; }

        public float Y { get; set; }

        public float Z { get; set; }

        public Vector(float x, float y, float z) : this() {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector(float x, float z) : this() {
            X = x;
            Y = 0;
            Z = z;
        }

        public Vector(Vector v) : this() {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        public static Vector operator +(Vector a, Vector b) {
            return new Vector { X = a.X + b.X, Y = a.Y + b.Y, Z = a.Z + b.Z };
        }

        public static Vector operator /(Vector a, int b) {
            return new Vector { X = a.X / b, Y = a.Y / b, Z = a.Z / b };
        }

        public static Vector operator *(Vector a, float b) {
            return new Vector { X = a.X * b, Y = a.Y * b, Z = a.Z * b };
        }

        public static Vector operator *(Vector a, int b) {
            return new Vector { X = a.X * b, Y = a.Y * b, Z = a.Z * b };
        }

        public static Vector operator -(Vector a, Vector b) {
            return new Vector { X = a.X - b.X, Y = a.Y - b.Y, Z = a.Z - b.Z };
        }

        public static Vector operator -(Vector a) {
            return new Vector { X = -a.X, Y = -a.Y, Z = -a.Z };
        }

        public static Vector Max(Vector value1, Vector value2) {
            return new Vector { X = Math.Max(value1.X, value2.X), Y = Math.Max(value1.Y, value2.Y), Z = Math.Max(value1.Z, value2.Z) };
        }

        public static Vector Min(Vector value1, Vector value2) {
            return new Vector { X = Math.Min(value1.X, value2.X), Y = Math.Min(value1.Y, value2.Y), Z = Math.Min(value1.Z, value2.Z) };
        }

        public static float Dot(Vector a, Vector b) {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        public override string ToString() {
            return string.Format("{0}({1:0.00}, {2:0.00}, {3:0.00})", "V", X, Y, Z);
        }

        public Vector Normalized {
            get {
                var length = Length;
                return new Vector(X / Length, Y / Length, Z / Length);
            }
        }

        public bool IsZero {
            get { return Math.Abs(this.X) < TOLERANCE && Math.Abs(this.Y) < TOLERANCE && Math.Abs(this.Z) < TOLERANCE; }
        }

        public float Length {
            get { return (float)Math.Sqrt(X * X + Y * Y + Z * Z); }
        }

        public static float Distance(Vector value1, Vector value2) {
            return (float)Math.Sqrt(
                (value1.X - value2.X) * (value1.X - value2.X) +
                (value1.Y - value2.Y) * (value1.Y - value2.Y) +
                (value1.Z - value2.Z) * (value1.Z - value2.Z));
        }
    }
}
