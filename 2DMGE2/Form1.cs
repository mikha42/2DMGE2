using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2DMGE2
{
    public partial class Form1 : Form
    {
        Bitmap img;
        Graphics g;
        int width = 1920;
        int height = 1080;
        Random r;
        int fps = 60;
        Bitmap tx;
        bool fullscreen = false;
        double time = 0;
        Bitmap estimated;

        double S = 1;

        public Form1()
        {
            InitializeComponent();
            img = new Bitmap(width, height);
            g = Graphics.FromImage(img);
            r = new Random();
            tx = new Bitmap(2, 2);
            tx.SetPixel(0, 0, Color.Black);
            tx.SetPixel(1, 0, Color.Red);
            tx.SetPixel(0, 1, Color.Red);
            tx.SetPixel(1, 1, Color.Black);

            fill(Color.Black);
            //createStatic();
            updatePic();
            timer1.Interval = 1000 / fps;
            //openFileDialog1.ShowDialog();
            //estimated = new Bitmap(openFileDialog1.FileName);
            //estimated = new Bitmap(estimated, width, height);
        }

        private int rand(int a, int b = 0)
        {
            if (a > b)
                return r.Next(b, a + 1);
            else
                return r.Next(a, b + 1);
        }
        private int rh()
        {
            return rand(0, height - 1);
        }
        private int rw()
        {
            return rand(0, width - 1);
        }
        private void setPixel(int x, int y, Color c)
        {
            img.SetPixel(x, y, c);
        }
        private Color getPixel(int x, int y)
        {
            return img.GetPixel(x, y);
        }
        private void line(int x1, int y1, int x2, int y2, Color c, float width = 1)
        {
            Pen p = new Pen(new SolidBrush(c), width);
            g.DrawLine(p, x1, y2, x2, y2);
        }
        private void fill(Color c)
        {
            g.FillRectangle(new SolidBrush(c), 0, 0, width, height);
        }
        private void fc(int x, int y, int r, Color c)
        {
            g.FillEllipse(new SolidBrush(c), x - r, y - r, 2 * r, 2 * r);
        }
        private void ft(int[] x, int[] y, Color c)
        {
            PointF[] points = new PointF[x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                points[i].X = x[i];
                points[i].Y = y[i];
            }
            g.FillPolygon(new SolidBrush(c), points);
        }
        private void fcurve(int[] x, int[] y, Color c, float width = 1)
        {
            PointF[] points = new PointF[x.Length];
            for (int i = 0; i < x.Length; i++)
            {
                points[i].X = x[i];
                points[i].Y = y[i];
            }
            g.DrawCurve(new Pen(new SolidBrush(c), width), points);
        }
        private void fe(int x, int y, int rx, int ry, Color c)
        {
            g.FillEllipse(new SolidBrush(c), x - rx, y - ry, 2 * rx, 2 * ry);
        }

        private void updatePic()
        {
            pictureBox1.Image = img;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            /*AviWriter p = new AviWriter();
            img = p.Open("render.avi", 60, width, height);
            for (double i = 1; i < 128; i += 0.1)
            {
                simplify(i);
                p.AddFrame();
            }
            p.Close();*/
            //updatePic();
            timer1.Enabled = !timer1.Enabled;
        }

        private Color rbc()
        {
            int[] val = new int[3] { 0, 0, 0 };
            int a = rand(2);
            int b = rand(1);
            int c = 1 - b;
            val[a] = 255;
            if (b >= a)
                b++;
            if (c >= a)
                c++;
            val[b] = rand(255);
            //val[c] = val[b] / 2;
            return Color.FromArgb(val[0], val[1], val[2]);
        }

        private Color rc()
        {
            return Color.FromArgb(rand(0, 255), rand(0, 255), rand(0, 255));
        }

        private void createStatic()
        {
            for (int i = 0; i < width; i++)
                for (int j = 0; j < height; j++)
                    setPixel(i, j, rc());
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time += 1.0 / (double)fps;
            //fill(Color.Black);
            for (int i = 0; i < 200; i++)
            {
                equis();
                //aqua();
            }
            /*int x = rw();
            int y = rh();
            int r = rand(3, 14);
            g.FillEllipse(new TextureBrush(estimated), x - r, y - r, x + r, y + r);*/
            //fill(Color.FromArgb(30, Color.Black));
            //createStatic();
            updatePic();
        }
        private int bw(int a, int b, int c)
        {
            if (a > b)
                return a;
            else if (b > c)
                return c;
            else
                return b;
        }
        private void simplify(double p)
        {
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    img.SetPixel(x, y, round(estimated.GetPixel(x, y), p));
        }
        private Color round(Color c, double p)
        {
            return Color.FromArgb(round(c.R, p), round(c.G, p), round(c.B, p));
        }
        private Color Noise(Color c, int a)
        {
            return Color.FromArgb(
                bw(0, c.R + rand(-a, a), 255),
                bw(0, c.G + rand(-a, a), 255),
                bw(0, c.B + rand(-a, a), 255)
                );
        }
        private void equis()
        {
            int a = (int)Math.Floor(time);
            int cx = rw();
            int cy = rh();
            int r = 100;
            int p = 7;
            int[] px = new int[p];
            int[] py = new int[p];
            for (int i = 0; i < p; i++)
            {
                px[i] = cx + rand(-r / 2, r / 2);
                py[i] = cy + rand(-r / 2, r / 2);
            }
            ft(px, py,// estimated.GetPixel(cx, cy)
                rc()
                /*Color.FromArgb(
                (int)(127.5 * Math.Sin(cx / 300) + 127.5),
                (int)(Math.Sqrt(cx * cx + cy * cy) + time * 200) % 255,
                (int)Math.Abs(127 - cx * Math.Sin(time / 3) + cy * Math.Cos(time / 3)) % 255
                )*/
                //,1
                );
        }
        private int round(int a, double div)
        {
            return (int)Math.Floor(Math.Floor((double)a / div) * div);
        }
        private void aqua()
        {
            int a = (int)Math.Floor(time);
            int px = rw();
            int py = rh();
            int size = rand(10);
            //Color c = estimated.GetPixel(px, py);
            fc(px, py, size + 3,// rand(1, 50),
                                rbc()
                                //new Color[] { Color.Black, Color.Red }[rand(0,1)]);
                                //Color.FromArgb(13 - size, c)
                                  //Color.FromArgb(22 - 2 * size, round(c.R, 14), round(c.G, 14), round(c.B, 14))
                                  /*Color.FromArgb(
                                           //30 - size,
                                           (int)Math.Floor(107.5 * Math.Sin(2 * (double)(Math.Tan(time * 0.05 * 0.3) * px + py * Math.Tan(time * 0.05 * 0.3) + 200 * time * 0.3) * (2 * Math.PI) / 1000) + 127.5) + rand(-20, 20),
                                           (int)Math.Floor(107.5 * Math.Sin((double)py * (2 * Math.PI) / 1000) + 127.5) + rand(-20, 20),
                                           (int)Math.Floor(107.5 * Math.Sin((time * 0.3 * 1000 + (double)Math.Sqrt((px - width / 4) * (px - width / 4) + (py - height / 2) * (py - height / 2))) * (2 * Math.PI) / 1000) + 127.5) + rand(-20, 20)
                                           )*/
                                           ); 
        }
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case 'a':
                    break;
                case 'f':
                    if (fullscreen)
                    {
                        FormBorderStyle = FormBorderStyle.Sizable;
                        WindowState = FormWindowState.Normal;
                    }
                    else
                    {
                        FormBorderStyle = FormBorderStyle.None;
                        WindowState = FormWindowState.Maximized;
                    }
                    fullscreen = !fullscreen;
                    break;
            }
        }
    }
}
