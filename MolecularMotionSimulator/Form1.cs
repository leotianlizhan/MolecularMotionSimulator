using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MolecularMotionSimulator
{
    public partial class Form1 : Form
    {
        const int NUMBER_OF_MOLECULES = 20;
        const int DIAMETER = 51;
        const int RADIUS = DIAMETER / 2;
        Random r = new Random();
        struct molecule {
            public int x;
            public int y;
            public int xVelocity;
            public int yVelocity;
        };
        List<molecule> takenPositions = new List<molecule>();
        molecule[] molecules = new molecule[NUMBER_OF_MOLECULES];
        public Form1()
        {
            InitializeComponent();
            //this.ResizeRedraw = true;
            this.DoubleBuffered = true;
            //initialize molecular position and velocity
            bool hasIntersected = false;
            for (int i = 0; i < NUMBER_OF_MOLECULES; i++)
            {
                molecules[i].x = r.Next(60, 400);
                molecules[i].y = r.Next(60, 400);
                do
                {
                    hasIntersected = false;
                    foreach (molecule j in takenPositions)
                    {
                        if (Math.Sqrt((molecules[i].x - j.x) * (molecules[i].x - j.x) + (molecules[i].y - j.y) * (molecules[i].y - j.y)) < DIAMETER + 5)
                        {
                            molecules[i].x = r.Next(60, 400);
                            molecules[i].y = r.Next(60, 400);
                            hasIntersected = true;
                            break;
                        }
                    }
                } while (hasIntersected);
                do
                {
                    molecules[i].xVelocity = r.Next(-1, 2);
                    molecules[i].yVelocity = r.Next(-1, 2);
                } while (Math.Abs(molecules[i].xVelocity) + Math.Abs(molecules[i].yVelocity) == 0);
                //molecules[i].xVelocity = 0;
                //molecules[i].yVelocity = 0;
                takenPositions.Add(molecules[i]);
            }
            tmr.Enabled = true;
        }
        void DiffusionMain()
        {
            for (int i = 0; i < NUMBER_OF_MOLECULES; i++)
            {
                for (int j = i; j < NUMBER_OF_MOLECULES; j++)
                {
                    int dx = molecules[i].x - molecules[j].x;
                    int dy = molecules[i].y - molecules[j].y;
                    double d = Math.Sqrt(dx * dx + dy * dy);
                    if (d <= DIAMETER)
                    {
                        molecules[i].xVelocity *= -1; molecules[i].yVelocity *= -1;
                        molecules[j].xVelocity *= -1; molecules[j].yVelocity *= -1;
                    }
                }
                if (molecules[i].x - RADIUS == 0 || molecules[i].x + RADIUS == ClientSize.Width)
                    molecules[i].xVelocity *= -1;
                if (molecules[i].y - RADIUS == 0 || molecules[i].y + RADIUS == ClientSize.Height)
                    molecules[i].yVelocity *= -1;
                molecules[i].x += molecules[i].xVelocity;
                molecules[i].y += molecules[i].yVelocity;
            }
            Refresh();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Pen p = new Pen(Color.Black, 2);
            for (int i = 0; i < NUMBER_OF_MOLECULES; i++)
            {
                e.Graphics.DrawEllipse(p, molecules[i].x - 30, molecules[i].y - 30, DIAMETER, DIAMETER);
            }
        }

        private void tmr_Tick(object sender, EventArgs e)
        {
            DiffusionMain();
        }

        private void btnTerminate_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
