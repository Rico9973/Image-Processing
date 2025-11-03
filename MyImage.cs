using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

namespace Pb_scientifique
{
    internal class MyImage
    {
        public string type;
        public int tailleFichier;
        public int tailleOffset;
        public int largeur;
        public int hauteur;
        public int bitsCouleur;
        public Pixel[,] tabIm;

        public MyImage (string myFile)
        {
            byte[] dansImage = File.ReadAllBytes(myFile);
            if (dansImage[0] == 66 && dansImage[1] == 77)
            {
                type = "BM";
            }
            else throw new Exception("Vérifier que vous avez le bon type de fichier"); 

            tailleFichier = Convertir_Endian_To_Int(dansImage, 2, 4);//definir la taille fichier
            tailleOffset = Convertir_Endian_To_Int(dansImage, 10, 4);//defini la taille de l'Offset
            largeur = Convertir_Endian_To_Int(dansImage, 18, 4);//defini la largeur de l'image
            hauteur = Convertir_Endian_To_Int(dansImage, 22, 4);//defini la hauteur de l'image
            bitsCouleur = Convertir_Endian_To_Int(dansImage, 28, 2); // nous permet de définir le nombre de bit par pixel pour chaque couleur
            Pixel[,] image = new Pixel[hauteur, largeur]; // ici on crée l'image avec un tableau de pixels que l'on remplpi à partir de la 54è donnée
            int comp = 0;
            for(int i = 0; i < hauteur; i++)
            {
                for(int j = 0; j< largeur; j++)
                {
                    image[i, j] = new Pixel(dansImage[54 + comp], dansImage[54 + comp + 1], dansImage[54 + comp + 2]);
                    comp += 3;
                }
            }
            tabIm = image;
        }

        public MyImage (int hauteur, int largeur)
        {
            this.largeur= largeur;
            this.hauteur= hauteur;
            this.type = "BM";
            this.tabIm = new Pixel[hauteur, largeur];
            this.tailleOffset= 54;
            this.tailleFichier = tailleOffset + largeur * hauteur * 3;
            bitsCouleur = 24;

        }
        

        /// <summary>
        /// Cette fonction nous permet de convertir une séquence d’octets au format little endian en entier
        /// </summary>
        /// <param name="dansImage">le tableau qui contient l'ensemble des bytes du fichier</param>
        /// <param name="debut"> Le premier byte auquel les données recherchées commencent </param>
        /// <param name="nbOctet"> le nombres d'octets après sur lesquels continue cette même information</param>
        /// <returns></returns>
        /// 

        public void From_Image_To_File(string file)// transfome une instance MyImage en fichier binaire respectant la structure fichier .bmp
        {
            if (File.Exists("sortie.bmp"))
            {
                // Supprime le fichier
                File.Delete("sortie.bmp");
                File.Copy(file, "sortie.bmp");
                Console.WriteLine("Le fichier a été supprimé et changé avec succès.");
            }

            else
            {
                File.Copy(file, "sortie.bmp");
            }
           
            
            MyImage image = this; //represente l'image qui appel
            byte[] newImage = new byte[14 + 40 + image.tabIm.GetLength(0) * image.tabIm.GetLength(1) * 3];
            newImage[0] = (byte)66;
            newImage[1] = (byte)77;
            Insert(newImage, image.tailleFichier, 2);
            Insert(newImage, image.tailleOffset, 10);
            newImage[14] = (byte)40;
            Insert(newImage, image.tabIm.GetLength(1), 18);
            Insert(newImage, image.tabIm.GetLength(0), 22);
            newImage[26] = (byte)1;
            newImage[27] = (byte)0;
            Insert(newImage, image.bitsCouleur, 28);
            for (int i = 0; i < 6; i++)
            {
                Insert(newImage, 0, 30 + i);
            }

            int k = 0;
            for (int i = 0; i < image.tabIm.GetLength(0); i++)//Transposition de l'image dans mon tableau de Pixel
            {
                for (int j = 0; j < image.tabIm.GetLength(1); j++)
                {
                    newImage[54 + k] = (byte)image.tabIm[i, j].Bleu;
                    newImage[54 + k + 1] = (byte)image.tabIm[i, j].Vert;
                    newImage[54 + k + 2] = (byte)image.tabIm[i, j].Rouge;
                    k += 3;
                }
            }
            File.WriteAllBytes("sortie.bmp", newImage);
        }
        public static void Insert(byte[] tab, int insert, int emplacement)//insert dans un tableau de bits dans le bon emplacement les valeurs convertits de bits ex : taille, type image
        {
            int k = 0;
            byte[] conVert = ConVertir_Int_To_Endian(insert, 4);
            for (int i = 0; i < conVert.Length; i++)
            {
                tab[emplacement + i] = conVert[k];
                k++;
            }
        }
        public static int Convertir_Endian_To_Int(byte[] dansImage, int debut, int nbOctet) 
        {
            int nouveauInt = 0;
            for (int i = debut; i < debut + nbOctet; i++)
            {
                nouveauInt += (int)(dansImage[i] * Math.Pow(256, i - debut));
            }
            return nouveauInt;
        }
        public static byte[] ConVertir_Int_To_Endian(int val, int nbOctet)//convertion int to endian
        {
            byte[] tab = new byte[nbOctet];
            for (int i = nbOctet - 1; i >= 0; i--)
            {
                tab[i] = (byte)(val / Math.Pow(256, i));
                val = (int)(val % Math.Pow(256, i));
            }
            return tab;
        }
        //***********************************************PASSAGE EN NOIR ET BLANC***********************************************

        public static MyImage ConversionNB(MyImage image)
        {
            MyImage image2 = new MyImage(image.hauteur, image.largeur);
            //WIDTH PUIS HEIGHT => PIXEL(X,Y)
            for (int i = 0; i < image2.tabIm.GetLength(0); i++)
                for (int j = 0; j < image2.tabIm.GetLength(1); j++)
                {
                    int gray = (int)(image.tabIm[i,j].Rouge * 0.3 + image.tabIm[i,j].Vert * 0.59 + image.tabIm[i,j].Bleu * 0.11);

                    image2.tabIm[i,j] = new Pixel(gray, gray, gray);

                }

            return image2;
        }


        //***********************************************ROTATION DE L'IMAGE ***********************************************

        public static MyImage Rotation(double degre, MyImage image)
        {
            MyImage image2 = new MyImage(image.hauteur, image.largeur);
            double angle = Math.PI * degre / 180;
            int x;
            int y;
            int centerx = image2.largeur / 2;
            int centery = image2.hauteur / 2;
            for (int i= 0; i < image2.tabIm.GetLength(0); i++)
            {
                for (int j = 0; j < image2.tabIm.GetLength(1); j++)
                {
                    //Formule découlant de la matrice de rotation
                    x = (int)((j - centerx) * Math.Cos(angle) - (i - centery) * Math.Sin(angle) + centerx);
                    y = (int)((j - centerx) * Math.Sin(angle) + (i - centery) * Math.Cos(angle) + centery);
                    if (x >= 0 && x < image2.largeur && y >= 0 && y < image2.hauteur)
                    {
                        
                        
                        image2.tabIm[i, j] = image.tabIm[y, x];

                       
                    }
                    else 
                    {
                        image2.tabIm[i, j] = new Pixel(255, 255, 255); //Remplir avec du blanc si n'est pas dans le champ
                       
                    }
                }
            }
            return image2;
        }


        //*****************************************AGRANDISSEMENT*****************************************************************
        public static MyImage Agrandir(MyImage image, double coeff)
        {
            int y_agrr = (int)(image.hauteur * coeff);
            int x_agrr = (int)(image.largeur * coeff);
            MyImage img_aggrandi = new MyImage(y_agrr, x_agrr);

            for(int i = 0; i<img_aggrandi.tabIm.GetLength(0); i++)
            {
                for(int j =0; j<img_aggrandi.tabIm.GetLength(1); j++)
                {
                    int x = (int)(j / coeff); //Coord dans l'img d'origine
                    int y = (int)(i / coeff);

                    img_aggrandi.tabIm[i, j] = image.tabIm[y, x];
                }
            }

            return img_aggrandi;
        }


        //*****************************************DETECTION DES CONTOURS**************************************************************

        //Laiser les noyaux en float => précision sinon flou ne marchje pas
        public static Pixel[,] Convolution(MyImage image, float[,] noyau_convolution)
        {
            MyImage image2 = new MyImage(image.hauteur, image.largeur);
            for (int i = 0; i < image.hauteur; i++)
            {
                for (int j = 0; j < image.largeur; j++)
                {
                    image2.tabIm[i, j] = new Pixel(255, 255, 255);
                }
            }

            //Parcours sur la matrice sans les cases des contours pour app la matrice de convolution
            for (int i = 1; i < image.hauteur - 1; i++)
            {
                for (int j = 1; j < image.largeur - 1; j++)
                {

                    int sommeR = 0, sommeG = 0, sommeB = 0;
                    for (int k = -1; k <= 1; k++)
                    {
                        for (int l = -1; l <= 1; l++)
                        {
                            sommeR += (byte)(image.tabIm[i + k, j + l].Rouge * noyau_convolution[k + 1, l + 1]);
                            sommeG += (byte)(image.tabIm[i + k, j + l].Vert * noyau_convolution[k + 1, l + 1]);
                            sommeB += (byte)(image.tabIm[i + k, j + l].Bleu * noyau_convolution[k + 1, l + 1]);
                        }
                    }

                    // Verif valeur appartenant à [0, 255]
                    sommeR = Math.Max(0, Math.Min(sommeR, 255));
                    sommeG = Math.Max(0, Math.Min(sommeG, 255));
                    sommeB = Math.Max(0, Math.Min(sommeB, 255));

                    image2.tabIm[i, j] = new Pixel((byte)sommeR, (byte)sommeG, (byte)sommeB);

                }


            }
            return image2.tabIm;
        }
        public static MyImage Contour(MyImage image)
        {
            MyImage image2 = new MyImage(image.hauteur, image.largeur);

            float[,] noyeau_convulation = new float[,] { { -1, -1, -1 }, { -1, 8, -1 }, { -1, -1, -1 } };

            image2.tabIm = MyImage.Convolution(image, noyeau_convulation);
            
            return image2;
        }



        //**************************************FLOU*************************************************

        public static MyImage Flou(MyImage image)
        {
            MyImage image2 = new MyImage(image.hauteur, image.largeur);

            float[,] noyeau_convulation = new float[,] { { 0.0625f, 0.125f, 0.0625f }, { 0.125f, 0.25f, 0.125f}, { 0.0625f, 0.125f, 0.0625f} };

            image2.tabIm = MyImage.Convolution(image, noyeau_convulation);

            return image2;
        }
    }
}
