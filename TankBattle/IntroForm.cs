using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TankBattle
{
    public partial class IntroForm : Form
    {
        /// <summary>
        /// Initializes Menu Form
        /// </summary>
        public IntroForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Begins game on click.
        /// </summary>
        /// <param name="sender">
        /// Form Object</param>
        /// <param name="e">
        /// Event Information</param>
        private void newGameButton_Click(object sender, EventArgs e)
        {
            // STARTS GAME!!!
            // And hides form
            Battle game = new Battle(2, 1);
            Opponent player1 = new PlayerController("Player 1", Chassis.CreateTank(1), Battle.TankColour(1));
            Opponent player2 = new PlayerController("Player 2", Chassis.CreateTank(1), Battle.TankColour(2));
            game.CreatePlayer(1, player1);
            game.CreatePlayer(2, player2);
            game.BeginGame();
            newGameButton.Text = "GAME ALREADY STARTED!";
            newGameButton.Enabled = false;
            this.Hide();
        }
    }
}
