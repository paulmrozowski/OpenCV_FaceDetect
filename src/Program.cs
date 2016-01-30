using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using OpenCvSharp;
using OpenCvSharp.CPlusPlus;

namespace OpenCvTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //string classifier = @"C:\opencv\sources\data\haarcascades\haarcascade_frontalface_default.xml";
            string classifier = @"C:\opencv\sources\data\lbpcascades\lbpcascade_frontalface.xml";

            //var cascade = new CascadeClassifier(@"C:\Users\Paul\Downloads\OpenCV\opencv\sources\data\haarcascades\haarcascade_frontalface_default.xml");
            var cascade = new CascadeClassifier(classifier);
            //var video = new VideoCapture(0);
            while (true)
            {               
                using (var mat = new Mat(@"C:\temp\face_img.jpg", LoadMode.Color))
                using (var grey = new Mat())
                {
                    Mat result;
                    result = mat.Clone();
                    
                    Cv2.CvtColor(mat, grey, ColorConversion.BgrToGray, 0);
                    Rect[] faces = cascade.DetectMultiScale(grey, 
                                                            1.10, 
                                                            4, 
                                                            HaarDetectionType.ScaleImage, 
                                                            new Size(80, 80));

                    foreach (Rect face in faces)
                    {
                        var center = new Point
                        {
                            X = (int)(face.X + face.Width * 0.5),
                            Y = (int)(face.Y + face.Height * 0.5)
                        };

                        Cv2.Rectangle(result, face, new Scalar(0, 255, 0), 4);

                        //var axes = new Size
                        //{
                        //    Width = (int)(face.Width * 0.5),
                        //    Height = (int)(face.Height * 0.5)
                        //};
                        
                        //Cv2.Ellipse(result, center, axes, 0, 0, 360, new Scalar(0, 0, 255), 4); 
                    }
                    Cv2.ImShow("Face", result);
                    Cv2.WaitKey(0);
                    Cv2.DestroyAllWindows();
                }
                #region Demo 2
                using (CvCapture camera = CvCapture.FromCamera(0))
                using (CvWindow window = new CvWindow("Capture"))
                {
                    while (CvWindow.WaitKey(50) != 32)
                    {
                        Mat result;
                        IplImage frame = camera.QueryFrame();                        

                        using (var mat = new Mat(frame))
                        using (var grey = new Mat())
                        {
                            result = mat.Clone();
                            Cv2.CvtColor(mat, grey, ColorConversion.BgrToGray, 0);

                            Rect[] faces = cascade.DetectMultiScale(grey, 1.10, 4, HaarDetectionType.ScaleImage, new Size(80, 80));

                            foreach (Rect face in faces)
                            {
                                Cv2.Rectangle(result, face, new Scalar(0, 0, 255), 4);                                
                            }
                        }

                        //window.Image = frame;
                        window.Image = (IplImage)result;
                        
                    }
                }
                #endregion
            }

            Application.Run(new Form1());
        }
    }
}
