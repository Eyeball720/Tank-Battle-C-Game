using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace TankBattle
{
    public abstract class Effect
    {
        protected Battle currentGame;

        /// <summary>
        /// Gets the current game instance
        /// </summary>
        /// <param name="game">
        /// Passed in game instance</param>
        public void ConnectGame(Battle game)
        {
            currentGame = game;
        }

        public abstract void Step();
        public abstract void Paint(Graphics graphics, Size displaySize);
    }
}
