using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;
using System.Globalization;

using System.Drawing;
using System.Drawing.Imaging;
using Pb_scientifique;

namespace LectureImage
{
    class Program
    {

        static void Main(string[] args)
        {
            //*********************************NOIR ET BLANC*********************************
            //MyImage coco = new MyImage("coco.bmp");
            //MyImage NbCoco = MyImage.ConversionNB(coco);
            //NbCoco.From_Image_To_File("coco.bmp");

            //*********************************ROTATION**************************************
            //MyImage coco = new MyImage("coco.bmp");
            //MyImage RotaCoco = MyImage.Rotation(180, coco);
            //RotaCoco.From_Image_To_File("coco.bmp");

            //******************************AGRANDISSEMENT**********************************
            //MyImage coco = new MyImage("coco.bmp");
            //MyImage CocoAgrr = MyImage.Agrandir(coco, 2);
            //CocoAgrr.From_Image_To_File("coco.bmp");

            //****************************DETECTION DE CONTOUR*******************************
            //MyImage coco = new MyImage("coco.bmp");
            //MyImage CocoContour = MyImage.Contour(coco);
            //CocoContour.From_Image_To_File("coco.bmp");

            //**************************RENFORCEMENT DES BORDS*******************************


            //**************************FLOU*************************************************
            MyImage coco = new MyImage("coco.bmp");
            MyImage CocoFlou = MyImage.Flou(coco);
            CocoFlou.From_Image_To_File("coco.bmp");
        }


    }
}
