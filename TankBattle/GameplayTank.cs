using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    public class GameplayTank
    {
        private Opponent user;
        private Battle match;
        private Chassis tank;
        private Bitmap bitmap;
        private float angle;
        private int xPos;
        private int yPos;
        private int durability;
        private int power;
        private int weapon;

        /// <summary>
        /// Constructor for GameplayTank.
        /// Appends params to variables.
        /// Sets angle, power and weapon to their default values.
        /// </summary>
        /// <param name="player">
        /// Player number</param>
        /// <param name="tankX">
        /// Tank's X position</param>
        /// <param name="tankY">
        /// Tank's Y position</param>
        /// <param name="game">
        /// The current Game instance</param>
        public GameplayTank(Opponent player, int tankX, int tankY, Battle game)
        {
            xPos = tankX;
            yPos = tankY;
            user = player;
            match = game;
            tank = CreateTank();
            durability = tank.GetHealth();

            angle = 0;
            power = 25;
            weapon = 0;
            // Create Bitmap
            bitmap = tank.CreateTankBMP(user.PlayerColour(), angle);
        }

        /// <summary>
        /// Gets the Current player
        /// </summary>
        /// <returns>Current player</returns>
        public Opponent GetPlayerNumber()
        {
            return user;
        }

        /// <summary>
        /// Gets the Current player's tank
        /// </summary>
        /// <returns>Current player tank</returns>
        public Chassis CreateTank()
        {
            return user.CreateTank();
        }

        /// <summary>
        /// Gets the Current player's tank's angel
        /// </summary>
        /// <returns>Tanks angle</returns>
        public float GetPlayerAngle()
        {
            return angle;
        }

        /// <summary>
        /// Sets the Current player's set to the provided angle.
        /// And redraws tah Bitmap
        /// </summary>
        /// <param name="angle">
        /// Angle to set the Current player's tanks' angle to</param>
        public void SetAngle(float angle)
        {
            this.angle = angle;
            bitmap = tank.CreateTankBMP(user.PlayerColour(), this.angle);
        }

        /// <summary>
        /// Gets the Current player's Tanks Power
        /// </summary>
        /// <returns>Current player tank's power</returns>
        public int GetPowerLevel()
        {
            return power;
        }

        /// <summary>
        /// Sets the Current player's Tank's Power to the provided power 
        /// </summary>
        /// <param name="power">
        /// Power to set the tank's power to</param>
        public void SetPower(int power)
        {
            this.power = power;
        }

        /// <summary>
        /// Gets the Current player's Tanks Weapon
        /// </summary>
        /// <returns>
        /// Tanks Weapon</returns>
        public int GetWeapon()
        {
            return weapon;
        }

        /// <summary>
        /// Sets the Current player's Tank's Weapon to the newWeapon
        /// </summary>
        /// <param name="newWeapon">
        /// Weapon to set the tank's weapon to</param>
        public void SetWeaponIndex(int newWeapon)
        {
            weapon = newWeapon;
        }

        /// <summary>
        /// ###PROVIDED###
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="displaySize"></param>
        public void Paint(Graphics graphics, Size displaySize)
        {
            int drawX1 = displaySize.Width * xPos / Map.WIDTH;
            int drawY1 = displaySize.Height * yPos / Map.HEIGHT;
            int drawX2 = displaySize.Width * (xPos + Chassis.WIDTH) / Map.WIDTH;
            int drawY2 = displaySize.Height * (yPos + Chassis.HEIGHT) / Map.HEIGHT;
            graphics.DrawImage(tank.CreateTankBMP(user.PlayerColour(), angle), new Rectangle(drawX1, drawY1, drawX2 - drawX1, drawY2 - drawY1));

            int drawY3 = displaySize.Height * (yPos - Chassis.HEIGHT) / Map.HEIGHT;
            Font font = new Font("Arial", 8);
            Brush brush = new SolidBrush(Color.White);

            int pct = durability * 100 / tank.GetHealth();
            if (pct < 100)
            {
                graphics.DrawString(pct + "%", font, brush, new Point(drawX1, drawY3));
            }
        }

        /// <summary>
        /// Gets tank's X posiiton
        /// </summary>
        /// <returns>
        /// Tank's x Position</returns>
        public int GetX()
        {
            return xPos;
        }

        /// <summary>
        /// Gets tank's Y posiiton
        /// </summary>
        /// <returns>
        /// Tank's Y posiiton</returns>
        public int Y()
        {
            return yPos;
        }

        /// <summary>
        /// Fires the current weapon
        /// </summary>
        public void Shoot()
        {
            this.CreateTank().FireWeapon(GetWeapon(), this, match);
        }

        /// <summary>
        /// Deals damage to the tanks health by the passed amount
        /// </summary>
        /// <param name="damageAmount">
        /// Damage to be dealt</param>
        public void Damage(int damageAmount)
        {
            durability -= damageAmount;
        }

        /// <summary>
        /// Checks if tank is alive, by checking health/duribility
        /// </summary>
        /// <returns></returns>
        public bool Alive()
        {
            if (durability <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Moves tank down one if it's floating.
        /// </summary>
        /// <returns>
        /// True if tank moves</returns>
        public bool GravityStep()
        {

            if (!Alive())
            {
                return false;
            }

            if (match.GetBattlefield().TankCollisionAt(GetX(), Y() + 1))
            {
                return false;
            }
            else
            {
                yPos += 1;
                durability -= 1;

                if (yPos == (Map.HEIGHT - Chassis.HEIGHT))
                {
                    durability = 0;
                }
                return true;
            }            
        }
    }
}
