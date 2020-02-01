using System;
namespace PsybersGestureRecognizer
{
    public class Drawn
    {
        public Vector[] Vectors = null;            
        public string Name = "";                 // gesture type
        private const int SAMPLING_RESOLUTION = 32;

        
        
        
        public Drawn(Vector[] vectors, string drawnName = "")
        {
            this.Name = drawnName;

            
            this.Vectors = Scale(vectors);
            this.Vectors = TranslateTo(Vectors, Centroid(Vectors));
            this.Vectors = Resample(Vectors, SAMPLING_RESOLUTION);
        }

        

        
        
        
        private Vector[] Scale(Vector[] vectors)
        {
            float minx = float.MaxValue, miny = float.MaxValue, maxx = float.MinValue, maxy = float.MinValue;
            for (int i = 0; i < vectors.Length; i++)
            {
                if (minx > vectors[i].X) minx = vectors[i].X;
                if (miny > vectors[i].Y) miny = vectors[i].Y;
                if (maxx < vectors[i].X) maxx = vectors[i].X;
                if (maxy < vectors[i].Y) maxy = vectors[i].Y;
            }

            Vector[] newVectors = new Vector[vectors.Length];
            float scale = Math.Max(maxx - minx, maxy - miny);
            for (int i = 0; i < vectors.Length; i++)
                newVectors[i] = new Vector((vectors[i].X - minx) / scale, (vectors[i].Y - miny) / scale, vectors[i].StrokeNum);
            return newVectors;
        }
        #region gesture pre-processing steps: scale normalization, translation to origin, and resampling

        

        private Vector[] TranslateTo(Vector[] vectors, Vector p)
        {
            Vector[] newVectors = new Vector[vectors.Length];
            for (int i = 0; i < vectors.Length; i++)
                newVectors[i] = new Vector(vectors[i].X - p.X, vectors[i].Y - p.Y, vectors[i].StrokeNum);
            return newVectors;
        }

        
       
        
        private Vector Centroid(Vector[] vectors)
        {
            float cx = 0, cy = 0;
            for (int i = 0; i < vectors.Length; i++)
            {
                cx += vectors[i].X;
                cy += vectors[i].Y;
            }
            return new Vector(cx / vectors.Length, cy / vectors.Length, 0);
        }

        
       
        
        public Vector[] Resample(Vector[] vectors, int n)
        {
            Vector[] newVectors = new Vector[n];
            newVectors[0] = new Vector(vectors[0].X, vectors[0].Y, vectors[0].StrokeNum);
            int numVectors = 1;

            float I = PathLength(vectors) / (n - 1); 
            float D = 0;
            for (int i = 1; i < vectors.Length; i++)
            {
                if (vectors[i].StrokeNum == vectors[i - 1].StrokeNum)
                {
                    float d = Calculator.EuclideanDistance(vectors[i - 1], vectors[i]);
                    if (D + d >= I)
                    {
                        Vector firstVector = vectors[i - 1];
                        while (D + d >= I)
                        {
                            
                            float t = Math.Min(Math.Max((I - D) / d, 0.0f), 1.0f);
                            if (float.IsNaN(t)) t = 0.5f;
                                newVectors[numVectors++] = new Vector(
                                (1.0f - t) * firstVector.X + t * vectors[i].X,
                                (1.0f - t) * firstVector.Y + t * vectors[i].Y,
                                vectors[i].StrokeNum
                            );

                            
                            d = D + d - I;
                            D = 0;
                            firstVector = newVectors[numVectors - 1];
                        }
                        D = d;
                    }
                    else D += d;
                }
            }

            if (numVectors == n - 1) 
                newVectors[numVectors++] = new Vector(Vectors[vectors.Length - 1].X, Vectors[vectors.Length - 1].Y, Vectors[vectors.Length - 1].StrokeNum);
            return newVectors;
        }

        
        
        
        private float PathLength(Vector[] vectors)
        {
            float length = 0;
            for (int i = 1; i < vectors.Length; i++)
                if (vectors[i].StrokeNum == vectors[i - 1].StrokeNum)
                    length += Calculator.EuclideanDistance(vectors[i - 1], vectors[i]);
            return length;
        }

#endregion
    }
}
