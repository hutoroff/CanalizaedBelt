using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;

namespace CanalizaedBelt
{
    class Field
    {
        int grid_step;
        List<Paricle> Paricle;
        int kX;
        int kY;
        int amnt;
        int[,] wall;
        Random rand = new Random();
        
        int task = -1;  //0 - однотипные частицы; 1 - смешанные частицы; 2 - смешанные с обгоном; -1 - ошибка
        //public int countC1, countL1, countT1, countC2, countL2, countT2;

        public Field(int _w, int _h, int _g, int _amnt, int _task)
        {
            grid_step = _g;
            kX = _w / _g;
            kY = _h / _g;
            amnt = _amnt;
            Paricle = new List<Paricle>();
            wall = new int[kX, kY];
            task = _task;
            float per;
            if (task == 0)
                per = 0.3f;
            else
                per = 2;
            //countC1 = countL1 = countT1 = countC2 = countL2 = countT2 = 0;


            for (var i = 0; i < amnt; ++i)
            {
                Paricle part;
                bool f;
                do
                {
                    part = new Paricle(kY,per);

                    f = Paricle.Any(p => p.X == part.X && p.Y == part.Y);
                } while (f);
                Paricle.Add(part);
            }

            //создание стен
            for (var i = 0; i < kX; i++)
            {
                wall[i, 0] = 1;
                wall[i, kY - 1] = 1;
            }
        }

        private Point NextPos(Paricle p)
        {
            var v = new Point(p.X, p.Y);
            
            var oldV = new Point(p.X, p.Y);
            if (p.Go())
            {
                if (task == 2 && p.type == 1)
                {
                    if(rand.NextDouble()>=0.75)
                        v.Y = v.Y + Convert.ToInt32(Math.Pow(-1, rand.Next(1, 3)));
                }
                if (feelWall(v))
                    v.Y = oldV.Y;
                v.X += 1;
            }
            if (feelEnd(v))
                v.X = 0;

            return v;
        }

        private bool feelWall(Point parts)
        {                   //true при попадании в стену
            try
            {
                if (wall[(int)parts.X, (int)parts.Y] == 1)
                    return true;
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private bool feelEnd(Point parts)
        {
            if (parts.X == kX)
                return true;
            return false;
        }
        public void Move()
        {
            //countL1 = countC1;
            //countT1 += countC1;
            //countC1 = 0;


            //countT2 = countC2 + countL2;

            foreach (var t in Paricle)
            {
                Point v = NextPos(t);
                if (v.Y == -1)
                    v.Y = 0;
                else if (v.Y == kY)
                    v.Y = kY - 1;

                var f = Paricle.All(t1 => v.Y != t1.Y || v.X != t1.X);
                if (!f) continue;
                t.X = (int)v.X;
                t.Y = (int)v.Y;
            }
        }

        public void Draw(Graphics g)
        {
            foreach (var p in Paricle)
            {
                Brush b;
                if(p.type == 0)
                    b = new SolidBrush(Color.Blue);
                else
                    b = new SolidBrush(Color.Red);
                g.FillEllipse(b, p.X * grid_step + 2, p.Y * grid_step + 2, grid_step - 5, grid_step - 5);

            }
            for (int i = 0; i < kX; i++)
            {
                for (int j = 0; j < kY; j++)
                {
                    if (wall[i, j] == 1)
                    {
                        g.FillRectangle(Brushes.Black, i * grid_step, j * grid_step, grid_step, grid_step);
                    }
                }

            }
        }
    }
}
