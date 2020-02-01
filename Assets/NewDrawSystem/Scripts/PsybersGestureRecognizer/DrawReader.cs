using System.IO;
using System.Collections.Generic;
using System.Xml;

namespace PsybersGestureRecognizer
{
    public class DrawReader
    {
        
        
        
        public static Drawn ReadDrawnFromXML(string xml)
        {

            XmlTextReader xmlReader = null;
            Drawn drawn = null;

            try
            {

                xmlReader = new XmlTextReader(new StringReader(xml));
                drawn = ReadDrawn(xmlReader);

            }
            finally
            {

                if (xmlReader != null)
                    xmlReader.Close();
            }

            return drawn;
        }

        
        
        public static Drawn ReadDrawnFromFile(string fileName)
        {

            XmlTextReader xmlReader = null;
            Drawn drawn= null;

            try
            {

                xmlReader = new XmlTextReader(File.OpenText(fileName));
                drawn = ReadDrawn(xmlReader);

            }
            finally
            {

                if (xmlReader != null)
                    xmlReader.Close();
            }

            return drawn;
        }

        private static Drawn ReadDrawn(XmlTextReader xmlReader)
        {
            List<Vector> vectors = new List<Vector>();
            int currentStrokeIndex = -1;
            string drawnName = "";
            try
            {
                while (xmlReader.Read())
                {
                    if (xmlReader.NodeType != XmlNodeType.Element) continue;
                    switch (xmlReader.Name)
                    {
                        case "Drawn":
                            drawnName = xmlReader["Name"];
                            if (drawnName.Contains("~")) 
                                drawnName = drawnName.Substring(0, drawnName.LastIndexOf('~'));
                            if (drawnName.Contains("_")) 
                                drawnName = drawnName.Replace('_', ' ');
                            break;
                        case "Stroke":
                            currentStrokeIndex++;
                            break;
                        case "Vector":
                            vectors.Add(new Vector(
                                float.Parse(xmlReader["X"]),
                                float.Parse(xmlReader["Y"]),
                                currentStrokeIndex
                            ));
                            break;
                    }
                }
            }
            finally
            {
                if (xmlReader != null)
                    xmlReader.Close();
            }
            return new Drawn(vectors.ToArray(), drawnName);
        }

        
        
        
        public static void WriteDrawn(PsybersGestureRecognizer.Vector[] vectors, string drawnName, string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" standalone=\"yes\"?>");
                sw.WriteLine("<Drawn Name = \"{0}\">", drawnName);
                int currentStroke = -1;
                for (int i = 0; i < vectors.Length; i++)
                {
                    if (vectors[i].StrokeNum != currentStroke)
                    {
                        if (i > 0)
                            sw.WriteLine("\t</Stroke>");
                        sw.WriteLine("\t<Stroke>");
                        currentStroke = vectors[i].StrokeNum;
                    }

                    sw.WriteLine("\t\t<Vector X = \"{0}\" Y = \"{1}\" T = \"0\" Pressure = \"0\" />",
                        vectors[i].X, vectors[i].Y
                    );
                }
                sw.WriteLine("\t</Stroke>");
                sw.WriteLine("</Drawn>");
            }
        }
    }
}
