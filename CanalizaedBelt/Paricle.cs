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
                type = rand.Next(2);
                if (type == 0)
                    p = 0.3f;
                else
                    p = 0.6f;
            }
            else
            {
                type = 0;
                p = _p;
            }
            
            X = rand.Next(20);
            Y = rand.Next(height-2) + 1;
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
