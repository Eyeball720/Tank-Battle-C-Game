using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    class MyTank : Chassis
    {
        /// <summary>
        /// Creats a new tank graphic.
        /// Caculate the end position of barrel, for DrawLine().
        /// </summary>
        /// <param name="angle">
        /// Angle of barrel</param>
        /// <returns>Tank graphic Array</returns>
        public override int[,] DisplayTankSprite(float angle)
        {
            int startX = 6;
            int startY = 7;
            int hyp = 4;
            double length;
            double height;
            double newAngle;
            int[,] graphic = { 
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
                { 0, 0, 1, 1, 0, 1, 0, 1, 0, 1, 0, 1, 1, 0, 0, 0 },
                { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
            };
            // Calculate end position through Pythagoras Therom
            angle = (angle < 0) ? Math.Abs(angle) : -angle;            

            newAngle = Math.PI /180.0 * angle;
            length = Math.Sin(newAngle) * hyp;
            height = Math.Cos(newAngle) * hyp;
            // Draw the Barrel
            DrawLine(graphic, startX, startY, startX- (int)height, startY- (int)length);
            
            return graphic;            
        }

        /// <summary>
        /// Fires weapon
        /// </summary>
        /// <param name="weapon">
        /// Weapon to fire</param>
        /// <param name="playerTank">
        /// Current tank type</param>
        /// <param name="currentGame">
        /// Curretn game instance</param>
        public override void FireWeapon(int weapon, GameplayTank playerTank, Battle currentGame)
        {
            float xPos = (float)(playerTank.GetX() + (0.5 * WIDTH));
            float yPos = (float)(playerTank.Y() + (0.5 * HEIGHT));

            Opponent player = playerTank.GetPlayerNumber();
            // Create new exlpotion
            Explosion newExplosion = new Explosion(100, 4, 4);
            // Create new projectile
            Projectile newBullet1 = new Projectile(xPos, yPos, playerTank.GetPlayerAngle(), 
                playerTank.GetPowerLevel(), 0.01f, newExplosion, player);
            // Add effect to List
            currentGame.AddEffect(newBullet1);
        }

        /// <summary>
        /// Sets base/starting health
        /// </summary>
        /// <returns>
        /// 100</returns>
        public override int GetHealth()
        {
            return 100;
        }

        /// <summary>
        /// Sets weapon Array
        /// </summary>
        /// <returns>
        /// Array of avalible weapons</returns>
        public override string[] GetWeapons()
        {
            return new string[] { "Standard shell" };
        }
    }
}
