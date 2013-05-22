using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CanalizaedBelt
{
    class Paricle
    {
        public float p;
        public int X;
        public int Y;
        public int type;
        public bool isCnt1, isCnt2;

        public Paricle(int height, float _p = 2)
        {
            var rand = new Random();
            if (_p == 2)
            {
                do
                {
                    p = (float)(rand).NextDouble();
                } while (p < 0.35);
            }
            else
            {
                p = _p;
            }
            type = rand.Next(2);
            X = rand.Next(5);
            Y = rand.Next(height / 3) + height / 3;
            isCnt1 = isCnt2 = false;
        }

        public bool Go()
        {
            var rand = new Random();
            var m = (float)rand.NextDouble();
            return p >= m;
        }
    }
}
