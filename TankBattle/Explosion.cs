using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    public class Explosion : Effect
    {
        private int damage;
        private int expRadius;
        private int earthDestRadius;
        private float xPos;
        private float yPos;
        private float lifeSpan;

        /// <summary>
        /// Constructer for Explosion.
        /// Appends Params to Variables
        /// </summary>
        /// <param name="explosionDamage">
        /// Max damage the explosion can deal</param>
        /// <param name="explosionRadius">
        /// Radius of the explosion</param>
        /// <param name="earthDestructionRadius">
        /// Radius that the terrain gets destroyed</param>
        public Explosion(int explosionDamage, int explosionRadius, int earthDestructionRadius)
        {
            damage = explosionDamage;
            expRadius = explosionRadius;
            earthDestRadius = earthDestructionRadius;
        }
        
        /// <summary>
        /// Sterts the explosion
        /// </summary>
        /// <param name="x">
        /// X Posision of explosion</param>
        /// <param name="y">
        /// Y posision of explosion</param>
        public void Activate(float x, float y)
        {
            xPos = x;
            yPos = y;
            lifeSpan = 1.0f;
        }

        /// <summary>
        /// Reduces eplosions life span, can checks if it "dies".
        /// Gets called every Tick.
        /// </summary>
        public override void Step()
        {
            // If explosion is dies, apply damage, remove terrain and  remove it from List.
            if (lifeSpan <= 0)
            {
                currentGame.Damage(xPos, yPos, damage, expRadius);
                currentGame.GetBattlefield().DestroyTiles(xPos, yPos, expRadius);
                currentGame.RemoveWeaponEffect(this);
            }
            lifeSpan -= .05f;
        }

        /// <summary>
        /// Displays explosion ###PROVIDED###
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="displaySize"></param>
        public override void Paint(Graphics graphics, Size displaySize)
        {
            float x = xPos * displaySize.Width / Map.WIDTH;
            float y = yPos * displaySize.Height / Map.HEIGHT;
            float radius = displaySize.Width * (float)((1.0 - lifeSpan) * expRadius * 3.0 / 2.0) / Map.WIDTH;

            int alpha = 0, red = 0, green = 0, blue = 0;

            if (lifeSpan < 1.0 / 3.0)
            {
                red = 255;
                alpha = (int)(lifeSpan * 3.0 * 255);
            }
            else if (lifeSpan < 2.0 / 3.0)
            {
                red = 255;
                alpha = 255;
                green = (int)((lifeSpan * 3.0 - 1.0) * 255);
            }
            else
            {
                red = 255;
                alpha = 255;
                green = 255;
                blue = (int)((lifeSpan * 3.0 - 2.0) * 255);
            }

            RectangleF rect = new RectangleF(x - radius, y - radius, radius * 2, radius * 2);
            Brush b = new SolidBrush(Color.FromArgb(alpha, red, green, blue));

            graphics.FillEllipse(b, rect);
        }
    }
}
