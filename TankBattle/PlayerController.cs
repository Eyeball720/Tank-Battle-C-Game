using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TankBattle
{
    public class PlayerController : Opponent
    {
        /// <summary>
        /// Constructor for the PlayerController class. 
        /// All functionality is handled by Opponent
        /// </summary>
        /// <param name="name">
        /// Name of the tank (player Number)</param>
        /// <param name="tank">
        /// Players Tank</param>
        /// <param name="colour">
        /// Colour of the tank</param>
        public PlayerController(string name, Chassis tank, Color colour) : base(name, tank, colour)
        {
        }

        /// <summary>
        /// Starts a new Round. All functionality is handled by Opponent
        /// </summary>
        public override void StartRound()
        {
        }

        /// <summary>
        /// Starts the Opponets turn
        /// </summary>
        /// <param name="gameplayForm">
        /// Form the displays the game space (The one with the terian)</param>
        /// <param name="currentGame">
        /// Current game space</param>
        public override void CommenceTurn(GameForm gameplayForm, Battle currentGame)
        {
            gameplayForm.EnableTankButtons();
        }

        /// <summary>
        /// Checks if projectile hits something.
        /// All functionality is handled by Opponent
        /// </summary>
        /// <param name="x">
        /// Projectiles X positon</param>
        /// <param name="y">
        /// Projectiles Y position</param>
        public override void ProjectileHit(float x, float y)
        {
        }
    }
}
