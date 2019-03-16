using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TankBattle
{
    public class Projectile : Effect
    {
        private Explosion explosion;
        private Opponent player;
        private float xPos;
        private float yPos;
        private float angle;
        private float power;
        private float gravity;
        private float xVelocity;
        private float yVelocity;

        /// <summary>
        /// Projectile constructer. 
        /// Creates an new projectile. Appends passed in position, angle, power, gravity, explosion and player.
        /// Calculates Velocity;
        /// </summary>
        /// <param name="x">
        /// Projectile X position</param>
        /// <param name="y">
        /// Projectile Y position</param>
        /// <param name="angle">
        /// Projectile Angle</param>
        /// <param name="power">
        /// Projectile Power (like speed)</param>
        /// <param name="gravity">
        /// Projectile Gravity (Downward force)</param>
        /// <param name="explosion">
        /// Projectiles' explosion</param>
        /// <param name="player">
        /// Player thet shot the Projectile</param>
        public Projectile(float x, float y, float angle, float power, float gravity, Explosion explosion, Opponent player)
        {
            // Append params to variables
            xPos = x;
            yPos = y;
            this.angle = angle;
            this.power = power;
            this.gravity = gravity;
            this.player = player;
            this.explosion = explosion;
            // Soften power
            float magnitude = power / 50;
            // Calculate trajectory useing Pythagoras
            float angleRadians = (90 - angle) * (float)Math.PI / 180;
            xVelocity = (float)Math.Cos(angleRadians) * magnitude;
            yVelocity = (float)Math.Sin(angleRadians) * -magnitude;

            // Shoot up straight up
            if (angle == 0)
            {
                xVelocity = 0;
                yVelocity = -magnitude;
            }

        }

        /// <summary>
        /// Moves the given projectile according to its angle, power, gravity and the wind.
        /// </summary>
        public override void Step()
        {
            // Do 10 times.
            for (int i = 0; i <10; i++)
            {
                xPos += xVelocity;
                yPos += yVelocity;
                xPos += (currentGame.WindSpeed()/ 1000.0f);
                // If Projectile goes of screen, remove it
                if (xPos <=0 || xPos >= Map.WIDTH-1 || yPos >= Map.HEIGHT)
                {
                    currentGame.RemoveWeaponEffect(this);
                    return;
                }
                // Detact if Projectile hits anything
                if (currentGame.DetectCollision(xPos,yPos))
                {
                    player.ProjectileHit(xPos,yPos);
                    explosion.Activate(xPos,yPos);
                    currentGame.AddEffect(explosion);
                    currentGame.RemoveWeaponEffect(this);
                    return;
                }
                // Calculate gravity, by movng Projectile down
                yVelocity += gravity;
            }
        }

        /// <summary>
        /// Displays Projectile, ##Provided##
        /// </summary>
        /// <param name="graphics">
        /// Encapsulated GDI + Drawingsurface</param>
        /// <param name="size">
        /// Size of the Display</param>
        public override void Paint(Graphics graphics, Size size)
        {
            float x = xPos * size.Width / Map.WIDTH;
            float y = yPos * size.Height / Map.HEIGHT;
            float s = size.Width / Map.WIDTH;

            RectangleF r = new RectangleF(x - s / 2.0f, y - s / 2.0f, s, s);
            Brush b = new SolidBrush(Color.WhiteSmoke);

            graphics.FillEllipse(b, r);
        }
    }
}
