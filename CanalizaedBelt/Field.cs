using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;

namespace CanalizaedBelt
{
    class Field
    {
        int grid_step;                  //шаг сетки;
        List<Paricle> Paricle;          //список частиц;
        int kX;                         //размер поля по X
        int kY;                         //размер поля по Y
        int amnt;                       //количество частиц на поле;
        int[,] wall;                    //границы стен;
        int[] counter;                  //количество частиц прошедших в ряду за единицу времени (20 тактов);
        float[] intens;                 //интенсивности для рядов       
        int ticker;                     //счетчик тиков таймера для контроля за единицей времени;
        Random rand = new Random();     //генератор случайных чисел;
        
        int task = -1;  //0 - однотипные частицы; 1 - смешанные частицы; 2 - смешанные с обгоном; -1 - ошибка;

        public Field(int _w, int _h, int _g, int _amnt, int _task)
        {
            grid_step = _g;
            kX = _w / _g;
            kY = _h / _g;
            amnt = _amnt;
            ticker = 0;
            Paricle = new List<Paricle>();
            wall = new int[kX, kY];
            intens = new float[kY-2];
            counter = new int[kY-2];
            task = _task;
            float per;
            if (task == 0)
                per = 0.3f;
            else
                per = 2;

            for(int i = 0; i < kY-2; i++)
                counter[i] = 0;

            for (int i = 0; i < kY-2; i++)
                intens[i] = 0;
            
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
                    if(rand.NextDouble() >= 0.75)     //смещение по Y с вероятностью 25%
                        v.Y = v.Y + Convert.ToInt32(Math.Pow(-1, rand.Next(1, 3)));
                }
                if (feelWall(v))
                    v.Y = oldV.Y;
                v.X += 1;
            }
            if (feelEnd(v))
                v.X = 0;

            if (oldV.X < kX/2 && v.X >= kX / 2 && v.Y != 0 && v.Y != kY)
                counter[v.Y-1]++;

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

            if(ticker == 19)
            {
                ticker = 0;
                for (var i = 0; i < kY - 2; i++)
                    counter[i] = 0;
                for (var i = 0; i < kY - 2; i++)
                    intens[i] = 0;
            }
            ticker++;
            if(ticker==19)
            {
                for (var i = 0; i < kY-2; i++)
                    intens[i] = intensity(counter[i], kY - 2);
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

        public Point getSize()
        {
            Point tmp = new Point(kX,kY);
            return tmp;
        }

        public Point getSizeInSquares()
        {
            Point tmp = new Point(kX*grid_step, kY*grid_step);
            return tmp;
        }

        private float M(int[] x, int size)
        {
            float M=0;
            for(int i=0; i < size; i++)
                M+=x[i];
            return M/size;
        }

        private float M(float x, int size)
        {
            return x / size;
        }

        private float intensity(float x, int size)
        {
            return M(x,20) / 20;
        }

        private float intensity(int[] x, int size)
        {
            return M(x, size) / 20;
        }

        public float[] getIntensity()
        {
            return intens;
        }

        public int getTicker()
        {
            return ticker;
        }
    }
}
