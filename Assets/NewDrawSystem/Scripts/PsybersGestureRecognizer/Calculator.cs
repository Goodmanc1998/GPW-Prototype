using System;

namespace PsybersGestureRecognizer
{
    public class Calculator
    {
        // Squared Euclidean Distance between two vectors in 2D
        public static float SqrEuclideanDistance(Vector a, Vector b)
        {
            return (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
        }

        //  Euclidean Distance between two vectors in 2D
        public static float EuclideanDistance(Vector a, Vector b)
        {
            return (float)Math.Sqrt(SqrEuclideanDistance(a, b));
        }
    }
}
