using System;
using System.Collections.Generic;
using UnityEngine;
namespace PsybersGestureRecognizer
{
    public class VectorRcnzr
    {
        
        public static RcnzdGesture GetType(Drawn candidate, Drawn[] trainingSet)
        {
            float minDistance = float.MaxValue;
            string gestureType = "";
            foreach (Drawn template in trainingSet)
            {
                float dist = RangeMatch(candidate.Vectors, template.Vectors);
                if (dist <= minDistance)
                {
                    minDistance = dist;
                    gestureType = template.Name;
                }
            }

            return gestureType == "" ? new RcnzdGesture() { GestureType = "No match", PercentMatch = 0.0f } : new RcnzdGesture() { GestureType = gestureType,PercentMatch = Mathf.Max((minDistance - 2.0f) / -2.0f, 0.0f) };
        }

        
        private static float RangeMatch(Vector[] vector1, Vector[] vector2)
        {
            int n = vector1.Length; 
            float eps = 0.5f;       
            int step = (int)Math.Floor(Math.Pow(n, 1.0f - eps));
            float minDistance = float.MaxValue;
            for (int i = 0; i < n; i += step)
            {
                float dist1 = RangeDistance(vector1, vector2, i);   
                float dist2 = RangeDistance(vector2, vector1, i);   
                minDistance = Math.Min(minDistance, Math.Min(dist1, dist2));
            }
            return minDistance;
        }

       
        private static float RangeDistance(Vector[] vector1, Vector[] vector2, int startNum)
        {
            int n = vector1.Length;       
            bool[] matched = new bool[n]; 
            Array.Clear(matched, 0, n);   

            float sum = 0;  
            int i = startNum;
            do
            {
                int index = -1;
                float minDistance = float.MaxValue;
                for (int j = 0; j < n; j++)
                    if (!matched[j])
                    {
                        float dist = Calculator.SqrEuclideanDistance(vector1[i], vector2[j]);  
                        if (dist < minDistance)
                        {
                            minDistance = dist;
                            index = j;
                        }
                    }
                matched[index] = true; 
                float weight = 1.0f - ((i - startNum + n) % n) / (1.0f * n);
                sum += weight * minDistance; 
                i = (i + 1) % n;
            } while (i != startNum);
            return sum;
        }
    }
}
