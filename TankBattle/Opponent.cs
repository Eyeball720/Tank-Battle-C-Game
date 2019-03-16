using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    abstract public class Opponent
    {
        protected Battle currentGame;
        private string tankName;
        private Color tankColour;
        private Chassis tankType;
        private int totalWins;
        /// <summary>
        /// This is a concrete class that extends the Opponent class.
        /// Appends Parameters to Varibles.
        /// </summary>
        /// <param name="name">
        /// Name of the tank (players number)</param>
        /// <param name="tank">
        /// Players tank</param>
        /// <param name="colour">
        /// Tanks Colour</param>
        public Opponent(string name, Chassis tank, Color colour)
        {
            tankName = name;
            tankType = tank;
            tankColour = colour;
            totalWins = 0;
        }

        /// <summary>
        /// Creats the tank.
        /// </summary>
        /// <returns>The players type of tank</returns>
        public Chassis CreateTank()
        {
            return tankType;
        }

        /// <summary>
        /// Gets Player Number
        /// </summary>
        /// <returns>Players number</returns>
        public string Identifier()
        {
            return tankName;
        }

        /// <summary>
        /// Gets the tanks Colour
        /// </summary>
        /// <returns>The players tank colour</returns>
        public Color PlayerColour()
        {
            return tankColour;
        }

        /// <summary>
        /// Pluses 1 to the total wins
        /// </summary>
        public void AddScore()
        {
            totalWins += 1;
        }

        /// <summary>
        /// Gets players total wins
        /// </summary>
        /// <returns>Players total wins </returns>
        public int GetWins()
        {
            return totalWins;
        }

        public abstract void StartRound();

        public abstract void CommenceTurn(GameForm gameplayForm, Battle currentGame);

        public abstract void ProjectileHit(float x, float y);
    }
}
