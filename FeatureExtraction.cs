using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using DlibDotNet;
using DlibDotNet.Extensions;
using Dlib = DlibDotNet.Dlib;


// CMP304: Artificial Intelligence  - Lab 2 Example Code

namespace FeatureExtraction
{
    
    // The main program class
    class Program
    {
        static string GetEmotion(string file_name)
        {
            
            if (file_name.Substring(7, 7) == "malcolm")
            {
              
                var temp_emotion = file_name.Substring(15, 2);
                switch (temp_emotion)
                {
                    case "an":
                        return "anger";
                    case "jo":
                        return "happiness";
                    case "sa":
                        return "sadness";
                    case "su":
                        return "suprise";
                    case "fe":
                        return "fear";
                    case "di":
                        return "disgust";
                    case "ne":
                        return "neutral";
                    default:
                        Console.WriteLine("unknown type of emotion: " + temp_emotion);
                        return "unknown";
                }

            }
            else if (file_name.Substring(7, 4) == "mery")
            {
             
                var temp_emotion = file_name.Substring(12, 2);
                switch (temp_emotion)
                {
                    case "an":
                        return "anger";
                    case "jo":
                        return "happiness";
                    case "sa":
                        return "sadness";
                    case "su":
                        return "suprise";
                    case "fe":
                        return "fear";
                    case "di":
                        return "disgust";
                    case "ne":
                        return "neutral";
                    default:
                        Console.WriteLine("unknown type of emotion: " + temp_emotion);
                        return "unknown";
                }
            }
            else
            {
            
                var temp_emotion = file_name.Substring(11, 2);
                switch (temp_emotion)
                {
                    case "an":
                        return "anger";
                    case "ha":
                        return "happiness";
                    case "sa":
                        return "sadness";
                    case "su":
                        return "suprise";
                    case "fe":
                        return "fear";
                    case "di":
                        return "disgust";
                    case "ne":
                        return "neutral";
                    default:
                        Console.WriteLine("unknown type of emotion: "+temp_emotion);
                        return "unknown";
                }
            }
        }

        static Point GetPoint(FullObjectDetection shape, int i)
        {
            return shape.GetPart((uint)i - 1);
        }
        static double GetDistance(Point point1, Point point2)
        {
            double dif_x, dif_y;
            dif_x = (point1.X - point2.X) * (point1.X - point2.X);
            dif_y = (point1.Y - point2.Y) * (point1.Y - point2.Y);
            return Math.Sqrt(dif_x + dif_y);
        }

        static double GetLeftEyebrow(FullObjectDetection shape)
        {
            double sum_dis = 0.0, left_eye_distance;
            left_eye_distance = GetDistance(GetPoint(shape, 40), GetPoint(shape, 22));
            sum_dis += GetDistance(GetPoint(shape, 40), GetPoint(shape, 19)) / left_eye_distance;
            sum_dis += GetDistance(GetPoint(shape, 40), GetPoint(shape, 20)) / left_eye_distance;
            sum_dis += GetDistance(GetPoint(shape, 40), GetPoint(shape, 21)) / left_eye_distance;
            sum_dis += 1;
            return sum_dis;
        }
        static double GetRightEyebrow(FullObjectDetection shape)
        {
            double sum_dis = 0.0, right_eye_distance;
            right_eye_distance = GetDistance(GetPoint(shape, 43), GetPoint(shape, 23));
            sum_dis += GetDistance(GetPoint(shape, 43), GetPoint(shape, 26)) / right_eye_distance;
            sum_dis += GetDistance(GetPoint(shape, 43), GetPoint(shape, 25)) / right_eye_distance;
            sum_dis += GetDistance(GetPoint(shape, 43), GetPoint(shape, 24)) / right_eye_distance;
            sum_dis += 1;
            return sum_dis;
        }

        static double GetLeftLip(FullObjectDetection shape)
        {
            double sum_dis = 0.0, left_lip_distance;
            left_lip_distance = GetDistance(GetPoint(shape, 34), GetPoint(shape, 52));
            sum_dis += GetDistance(GetPoint(shape, 34), GetPoint(shape, 49)) / left_lip_distance;
            sum_dis += GetDistance(GetPoint(shape, 34), GetPoint(shape, 50)) / left_lip_distance;
            sum_dis += GetDistance(GetPoint(shape, 34), GetPoint(shape, 51)) / left_lip_distance;
            return sum_dis;
        }

        static double GetRightLip(FullObjectDetection shape)
        {
            double sum_dis = 0.0, right_lip_distance;
            right_lip_distance = GetDistance(GetPoint(shape, 34), GetPoint(shape, 52));
            sum_dis += GetDistance(GetPoint(shape, 34), GetPoint(shape, 55)) / right_lip_distance;
            sum_dis += GetDistance(GetPoint(shape, 34), GetPoint(shape, 54)) / right_lip_distance;
            sum_dis += GetDistance(GetPoint(shape, 34), GetPoint(shape, 53)) / right_lip_distance;
            return sum_dis;
        }

        static double GetLipWidth(FullObjectDetection shape)
        {
            return GetDistance(GetPoint(shape, 49), GetPoint(shape, 55))/GetDistance(GetPoint(shape, 34), GetPoint(shape, 52));
        }

        static double GetLipHeight(FullObjectDetection shape)
        {
            return GetDistance(GetPoint(shape, 52), GetPoint(shape, 58)) / GetDistance(GetPoint(shape, 34), GetPoint(shape, 52));
        }

 
      
        private const string inputFilePath = "input.jpg";

        // The main program entry point
        static void Main(string[] args)
        {
            bool use_mirror = false;
            // file paths
            string[] files = Directory.GetFiles("images", "*.*", SearchOption.AllDirectories);
            List<FullObjectDetection> shapes = new List<FullObjectDetection>();
            List<string> emotions = new List<string>(); 
            // Set up Dlib Face Detector
            using (var fd = Dlib.GetFrontalFaceDetector())
            // ... and Dlib Shape Detector
            using (var sp = ShapePredictor.Deserialize("shape_predictor_68_face_landmarks.dat"))
            {
                // load input image
                for (int i = 0; i < files.Length; i++)
                {

                    var emotion = GetEmotion(files[i]);
                    var img = Dlib.LoadImage<RgbPixel>(files[i]);

                    // find all faces in the image
                    var faces = fd.Operator(img);
                    // for each face draw over the facial landmarks
                    foreach (var face in faces)
                    {
                        // find the landmark points for this face
                        var shape = sp.Detect(img, face);
                        shapes.Add(shape);
                        emotions.Add(emotion);
                        // draw the landmark points on the image
                     
                        for (var i2 = 0; i2 < shape.Parts; i2++)
                        {
                            var point = shape.GetPart((uint)i2);
                            var rect = new Rectangle(point);
                           
                            if (point == GetPoint(shape, 40) || point == GetPoint(shape, 22))
                            {
                                Dlib.DrawRectangle(img, rect, color: new RgbPixel(0, 255, 0), thickness: 4);
                            }
                            else
                            {
                                Dlib.DrawRectangle(img, rect, color: new RgbPixel(255, 255, 0), thickness: 4);
                            }

                        }
                    }

                    // export the modified image
                    Console.WriteLine(files[i]);
                    Dlib.SaveJpeg(img, "output_" + files[i]);
                    
                }

                string header = "leftEyebrow,rightEyebrow,leftLip,rightLip,lipHeight,lipWidth,emotion\n";
                System.IO.File.WriteAllText(@"feature_vectors.csv", header);
                for (var i =0; i < shapes.Count; i++)
                {
                    var shape = shapes[i];
                    var emotion = emotions[i];
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(@"feature_vectors.csv", true))
                    {
                        file.WriteLine(GetLeftEyebrow(shape) + "," + GetRightEyebrow(shape) + "," +
                            GetLeftLip(shape) + "," + GetRightLip(shape) + "," + GetLipWidth(shape) + "," + GetLipHeight(shape)+
                            "," + emotion);
                        if (use_mirror){
                            file.WriteLine(GetRightEyebrow(shape) + "," + GetLeftEyebrow(shape) + "," +
                            GetRightLip(shape) + "," + GetLeftLip(shape) + "," + GetLipWidth(shape) + "," + GetLipHeight(shape) +
                            "," + emotion);
                            
                        }
                    }
                }

            }
        }

       
    }
}