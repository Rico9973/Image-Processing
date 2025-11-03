using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pb_scientifique
{
    internal class Pixel
    {
        int rouge;
        public int Rouge
        {
            get
            {
                return rouge;
            }
            set
            {
                if (value > 255) rouge = 255;
                else if (value < 0) rouge = 0;
                else rouge = value;
            }
        }
        int vert;
        public int Vert
        {
            get
            {
                return vert;
            }
            set
            {
                if (value > 255) vert = 255;
                else if (value < 0) vert = 0;
                else vert = value;
            }
        }
        public int bleu;
        public int Bleu
        {
            get
            {
                return bleu;
            }
            set
            {
                if (value > 255) bleu = 255;
                else if (value < 0) bleu = 0;
                else bleu = value;
            }
        }

        public Pixel(int b, int v, int r)
        {
            Rouge = r;
            Vert = v;
            Bleu = b;
        }
    }
}
