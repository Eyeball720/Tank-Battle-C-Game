using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    public class Map
    {
        public const int WIDTH = 160;
        public const int HEIGHT = 120;
        private bool[,] map;
        private static Random randNum = new Random();
        private int verPos = 0;

        /// <summary>
        /// Map constructor.
        /// Randomly creates the Terrian
        /// </summary>
        public Map()
        {
            map = new bool[HEIGHT, WIDTH];
            int startPoint = randNum.Next(70,90);
            int preXpos = startPoint;

            // Create the 1st piller of terrain. Relitive to the randome middle postion (startPoint)
            for (int Xpos = startPoint; Xpos < HEIGHT; Xpos++)
            {
                map[Xpos, 0] = true;
            }
            // Create the terrain, left to right
            for (int Ypos  = 1; Ypos< WIDTH; Ypos++)
            {
                //Generater deviation of hight. -2,3 for normal terrian. -4,5 for hills and valleys. -1,1 for a mostly flat world
                preXpos = preXpos + randNum.Next(-2,3);
                // If hieght is to low or high set to the extremity
                if (preXpos >= HEIGHT-4)
                {
                    preXpos = HEIGHT-4;
                }
                else
                {
                    if (preXpos <= 0 + Chassis.HEIGHT)
                    {
                        preXpos = Chassis.HEIGHT + 1;
                    }
                }
                // Append terrain to map Array (Declared at top)
                for (int Xpos = preXpos; Xpos < HEIGHT; Xpos++)
                {
                    map[Xpos, Ypos] = true;
                }
            }       
        }

        /// <summary>
        /// Checks if there is Terrain at Provided positions
        /// </summary>
        /// <param name="x">
        /// X Position to check</param>
        /// <param name="y">
        /// Y Position to check</param>
        /// <returns> True if there is terrian.False if there isn't any</returns>
        public bool TerrainAt(int x, int y)
        {
            return map[y, x];
        }

        /// <summary>
        /// Checks if a tank can fit at the Provided positions 
        /// </summary>
        /// <param name="x">
        /// X Position to check</param>
        /// <param name="y">
        /// Y Position to check</param>
        /// <returns></returns>
        public bool TankCollisionAt(int x, int y)
        {
            // for the each point the tank takes up, 
            // Check if there is Terrian there.
            for (int xPos = x; xPos < x + Chassis.WIDTH; xPos++)
            {
                for (int yPos = y; yPos < y + Chassis.HEIGHT; yPos++)
                {
                    if (TerrainAt(xPos, yPos))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Gets the Tanks Vertical Position.
        /// </summary>
        /// <param name="x">
        /// Horizontal position of th tank</param>
        /// <returns>The vertical position of the tank in relation to its Horizontal position</returns>
        public int TankVerticalPosition(int x)
        {
            // From the top of the map, down checking if a tank can fit there
            int yPos = 0;
            while (!TankCollisionAt(x,yPos))
            {
                yPos += 1;
            }
            // -1 so the tank is not in the ground
            yPos -= 1;
            verPos = yPos;
            return verPos;
        }

        /// <summary>
        /// Removes terrian from the given position, with in the radius
        /// </summary>
        /// <param name="destroyX">
        /// X Position of explosion</param>
        /// <param name="destroyY">
        /// Y Position of explosion</param>
        /// <param name="radius">
        /// Size of the explosion</param>
        public void DestroyTiles(float destroyX, float destroyY, float radius)
        {
            float a;
            float b;
            float c;

            for (int xPos = 0; xPos < HEIGHT; xPos++)
            {
                for (int yPos = 0; yPos < WIDTH; yPos++)
                {
                    a = Math.Abs(destroyX - yPos);
                    b = Math.Abs(destroyY - xPos);

                    c = (float)Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));

                    if (c < radius)
                    {
                        map[xPos, yPos] = false;
                    }
                }
            }               
        }

        /// <summary>
        /// Moves floating terrian down, one position.
        /// </summary>
        /// <returns>
        /// True if some terrian moved. false if nothing moved</returns>
        public bool GravityStep()
        {
            bool blocksMoved = false;
            // Gravity
            // Working from the bottom up, through all 120 rows.
            for (int Ypos = 119; Ypos > 0; Ypos--)
            {
                // Working Right to Left, through all 160 columns.
                for (int Xpos = 159; Xpos >= 0; Xpos--)
                {
                    // If there is terrain at position, check below it.
                    if (!TerrainAt(Xpos,Ypos))
                    {
                        if (TerrainAt(Xpos,Ypos-1))
                        {
                            // Swap values
                            map[Ypos, Xpos] = true;
                            map[Ypos-1, Xpos] = false;

                            blocksMoved = true;
                        }
                    }
                }
            }
            return blocksMoved;
        }
    }
}
