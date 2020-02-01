using System;

namespace PsybersGestureRecognizer
{
    public class Vector
    {
        //x and y positions
        public float X, Y;
        // the stroke number which is change in direction.
        public int StrokeNum;

        public Vector(float x, float y, int strokeNum)
        {
            this.X = x;
            this.Y = y;
            this.StrokeNum = strokeNum;
        }
    }
}
