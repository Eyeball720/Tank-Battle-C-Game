using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    public abstract class Chassis
    {
        public const int WIDTH = 4;
        public const int HEIGHT = 3;
        public const int NUM_TANKS = 1;

        public abstract int[,] DisplayTankSprite(float angle);

        /// <summary>
        /// Draws the Barrel/Line from and to the given positions.
        /// </summary>
        /// <param name="graphic">
        /// The Chassis graphic</param>
        /// <param name="X1">
        /// Starting X position (From)</param>
        /// <param name="Y1">
        /// Starting Y position (From)</param>
        /// <param name="X2">
        /// End X position (To)</param>
        /// <param name="Y2">
        /// End X position (To)</param>
        public static void DrawLine(int[,] graphic, int X1, int Y1, int X2, int Y2)
        {
            // Checks if line goes straight up.
            if (X1 == X2)
            {
                // Draws line strigh down (starts from the lower Y position)
                for (int yPos = Math.Min(Y1,Y2); yPos <= Math.Max(Y1, Y2); yPos++)
                {
                    graphic[X1, yPos] = 1;
                }
            }
            else
            {
                // Apply Bresenham's line algorithm and Equation of a Straight Line (y = mx + c)
                double deltaX = X2 - X1;
                double deltaY = Y2 - Y1;
                double deltaErr = deltaY / deltaX;

                double c = -(deltaErr * X1) + Y1;

                // For every X and Y position, within the Max positions respectively
                for (int yPos = Math.Min(Y1, Y2); yPos <= Math.Max(Y1, Y2); yPos++)
                {
                    for (int xPos = Math.Min(X1, X2); xPos <= Math.Max(X1, X2); xPos++)
                    {
                        // Check the current line against the y = mx + c line
                        // If its close enough draw line
                        if (yPos == Math.Round(deltaErr * xPos + c))
                        {
                            graphic[xPos, yPos] = 1;
                        }
                        if (xPos == Math.Round((yPos - c) / deltaErr))
                        {
                            graphic[xPos, yPos] = 1;
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Creates the tanks Bitmap ###PROVIDED###
        /// </summary>
        /// <param name="tankColour">
        /// Tanks Colour</param>
        /// <param name="angle">
        /// Angle of the barrel</param>
        /// <returns></returns>
        public Bitmap CreateTankBMP(Color tankColour, float angle)
        {
            int[,] tankGraphic = DisplayTankSprite(angle);
            int height = tankGraphic.GetLength(0);
            int width = tankGraphic.GetLength(1);

            Bitmap bmp = new Bitmap(width, height);
            Color transparent = Color.FromArgb(0, 0, 0, 0);
            Color tankOutline = Color.FromArgb(255, 0, 0, 0);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    if (tankGraphic[y, x] == 0)
                    {
                        bmp.SetPixel(x, y, transparent);
                    }
                    else
                    {
                        bmp.SetPixel(x, y, tankColour);
                    }
                }
            }

            // Outline each pixel
            for (int y = 1; y < height - 1; y++)
            {
                for (int x = 1; x < width - 1; x++)
                {
                    if (tankGraphic[y, x] != 0)
                    {
                        if (tankGraphic[y - 1, x] == 0)
                            bmp.SetPixel(x, y - 1, tankOutline);
                        if (tankGraphic[y + 1, x] == 0)
                            bmp.SetPixel(x, y + 1, tankOutline);
                        if (tankGraphic[y, x - 1] == 0)
                            bmp.SetPixel(x - 1, y, tankOutline);
                        if (tankGraphic[y, x + 1] == 0)
                            bmp.SetPixel(x + 1, y, tankOutline);
                    }
                }
            }

            return bmp;
        }

        public abstract int GetHealth();

        public abstract string[] GetWeapons();

        public abstract void FireWeapon(int weapon, GameplayTank playerTank, Battle currentGame);

        /// <summary>
        /// Creates new tank
        /// </summary>
        /// <param name="tankNumber">
        /// Player Number</param>
        /// <returns>New Tank</returns>
        public static Chassis CreateTank(int tankNumber)
        {
            return new MyTank();
        }
    }
}
