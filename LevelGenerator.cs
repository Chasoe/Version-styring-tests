using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;

namespace Rts_project_base
{
    class LevelGenerator
    {
        Graphics draws;
        const int tileSizeX = 10;
        const int tileSizeY = 10;
        private static int width = 80;
        private static int height = 60;


        static int[,] multiArray = new int[width, height];

        public LevelGenerator(Graphics draws)
        {
            this.draws = draws;
            Thread GridLoader = new Thread(LoadWorld);
            LoadWorld();
        }

        //Loader grid ind
        public void LoadWorld()
        {
            for (int i = 0; i < width; i++)
            {
                for (int a = 0; a < height; a++)
                {
                    multiArray[i, a] = 0;
                }
            }
            BackGroundDraw();
        }
        
        public void BackGroundDraw()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    switch (multiArray[x, y])
                    {
                        case 0:
                            draws.DrawRectangle(new Pen(Brushes.Green), x*tileSizeX, y*tileSizeY, tileSizeX, tileSizeY);
                            break;
                                
                        case 1:
                            Console.WriteLine("1");
                            break;

                        case 2:
                            Console.WriteLine("2");
                            break;

                        case 3:
                            Console.WriteLine("3");
                            break;

                        case 4:
                            Console.WriteLine("4");
                            break;

                        case 5:
                            Console.WriteLine("5");
                            break;

                        case 6:
                            Console.WriteLine("6");
                            break;
                    }
                }
            }
        }
    }
}
