using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TankBattle
{
    public partial class GameForm : Form
    {
        private Color landscapeColour;
        private Random rng = new Random();
        private Image backgroundImage = null;
        private int levelWidth = 160;
        private int levelHeight = 120;
        private Battle currentGame;

        private BufferedGraphics backgroundGraphics;
        private BufferedGraphics gameplayGraphics;

        string[] imageFilenames = { "Images\\background1.jpg",
                            "Images\\background2.jpg",
                            "Images\\background3.jpg",
                            "Images\\background4.jpg"};
        Color[] landscapeColours = { Color.FromArgb(255, 0, 0, 0),
                             Color.FromArgb(255, 73, 58, 47),
                             Color.FromArgb(255, 148, 116, 93),
                             Color.FromArgb(255, 133, 119, 109) };

        /// <summary>
        /// Game Form cunstructor
        /// </summary>
        /// <param name="game">
        /// Current Game instance</param>
        public GameForm(Battle game)
        {
            currentGame = game;
            int chosenSet = rng.Next(0,4);
            landscapeColour = landscapeColours[chosenSet];
            backgroundImage = Image.FromFile(imageFilenames[chosenSet], true);

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.UserPaint, true);

            InitializeComponent();

            backgroundGraphics = InitRenderBuffer();
            gameplayGraphics = InitRenderBuffer();
            DrawBackground();
            DrawGameplay();
            NewTurn();


        }

        // From https://stackoverflow.com/questions/13999781/tearing-in-my-animation-on-winforms-c-sharp
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000; // WS_EX_COMPOSITED
                return cp;
            }
        }

        /// <summary>
        /// Allows user to user controls
        /// </summary>
        public void EnableTankButtons()
        {
            controlPanel.Enabled = true;
        }

        /// <summary>
        /// Sets angle control to provided angle
        /// </summary>
        /// <param name="angle">
        /// Provided angle</param>
        public void SetAngle(float angle)
        {
            angleNumeric.Value = (decimal)angle;
        }

        /// <summary>
        /// Sets power control to provided power
        /// </summary>
        /// <param name="power">
        /// Power to be set</param>
        public void SetPower(int power)
        {
            powerBar.Value = power;
        }

        /// <summary>
        /// Sets wepaon control to provided weapon
        /// </summary>
        /// <param name="weapon">
        /// weapon to set</param>
        public void SetWeaponIndex(int weapon)
        {
            weaponComboBox.SelectedIndex = weapon;
        }

        /// <summary>
        /// Fires weapon.
        /// Calls tanks Shoot(), Disables controles, enables timer.
        /// </summary>
        public void Shoot()
        {
            currentGame.GetCurrentGameplayTank().Shoot();
            controlPanel.Enabled = false;
            timer.Enabled = true;
        }

        /// <summary>
        /// Applies background Image to Game Form. 
        /// Applies Terrian Bitmap to Gameform
        /// </summary>
        private void DrawBackground()
        {
            Graphics graphics = backgroundGraphics.Graphics;
            Image background = backgroundImage;
            graphics.DrawImage(backgroundImage, new Rectangle(0, 0, displayPanel.Width, displayPanel.Height));

            Map battlefield = currentGame.GetBattlefield();
            Brush brush = new SolidBrush(landscapeColour);
            // Draws terrian
            for (int y = 0; y < Map.HEIGHT; y++)
            {
                for (int x = 0; x < Map.WIDTH; x++)
                {
                    if (battlefield.TerrainAt(x, y))
                    {
                        int drawX1 = displayPanel.Width * x / levelWidth;
                        int drawY1 = displayPanel.Height * y / levelHeight;
                        int drawX2 = displayPanel.Width * (x + 1) / levelWidth;
                        int drawY2 = displayPanel.Height * (y + 1) / levelHeight;
                        graphics.FillRectangle(brush, drawX1, drawY1, drawX2 - drawX1, drawY2 - drawY1);
                    }
                }
            }
        }

        /// <summary>
        /// Draws tanks and effects
        /// </summary>
        private void DrawGameplay()
        {
            backgroundGraphics.Render(gameplayGraphics.Graphics);
            currentGame.DrawTanks(gameplayGraphics.Graphics, displayPanel.Size);
            currentGame.DrawWeaponEffects(gameplayGraphics.Graphics, displayPanel.Size);
        }

        /// <summary>
        /// Updates form elemets
        /// </summary>
        private void NewTurn()
        {
            Opponent player = currentGame.GetCurrentGameplayTank().GetPlayerNumber();
            GameplayTank currentTank = currentGame.GetCurrentGameplayTank();

            //Updates Form elements
            this.Text = String.Format("Tank Battle - Round {0} of {1}", currentGame.GetRound(), currentGame.GetTotalRounds());
            controlPanel.BackColor = player.PlayerColour();
            playerLable.Text = player.Identifier();
            currentTank.SetAngle((float)angleNumeric.Value);
            currentTank.SetPower(powerBar.Value);
            int windS = currentGame.WindSpeed();
            windSpeedLable.Text = String.Format("{0} {1}",windS, windS <0 ? "W" : "E");
            //Refreshes avalible weapons
            weaponComboBox.Items.Clear();
            string[] avalibleWeapons = currentTank.CreateTank().GetWeapons();
            foreach (string weapon in avalibleWeapons)
            {
                weaponComboBox.Items.Add(weapon);
            }
            currentTank.SetWeaponIndex(currentTank.GetWeapon());
            // Starts new players turn
            player.CommenceTurn(this,currentGame);
        }

        public BufferedGraphics InitRenderBuffer()
        {
            BufferedGraphicsContext context = BufferedGraphicsManager.Current;
            Graphics graphics = displayPanel.CreateGraphics();
            Rectangle dimensions = new Rectangle(0, 0, displayPanel.Width, displayPanel.Height);
            BufferedGraphics bufferedGraphics = context.Allocate(graphics, dimensions);
            return bufferedGraphics;
        }

        /// <summary>
        /// draws everything
        /// </summary>
        /// <param name="sender">
        /// Item form Form</param>
        /// <param name="e">
        /// Event Data</param>
        private void displayPanel_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = displayPanel.CreateGraphics();
            gameplayGraphics.Render(graphics);
        }

        private void playerLable_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Sets Current Tank's weapon to selected weapon
        /// </summary>
        /// <param name="sender">
        /// Item form Form</param>
        /// <param name="e">
        /// Event Data</param>
        private void weaponComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GameplayTank currentTank = currentGame.GetCurrentGameplayTank();
            currentTank.SetWeaponIndex(weaponComboBox.SelectedIndex);
        }

        private void windSpeedLable_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Sets Current Tank's angle to selected angle.
        /// Redraws Gamplay elements
        /// </summary>
        /// <param name="sender">
        /// Item form Form</param>
        /// <param name="e">
        /// Event Data</param>
        private void angleNumeric_ValueChanged(object sender, EventArgs e)
        {
            GameplayTank currentTank = currentGame.GetCurrentGameplayTank();
            currentTank.SetAngle((float)angleNumeric.Value);
            DrawGameplay();
            displayPanel.Invalidate();
        }

        /// <summary>
        /// Fires Current Weapon
        /// </summary>
        /// <param name="sender">
        /// Item form Form</param>
        /// <param name="e">
        /// Event Data</param>
        private void fireButton_Click(object sender, EventArgs e)
        {
            Shoot();
        }

        /// <summary>
        /// Sets Current tanks Power to selected power
        /// </summary>
        /// <param name="sender">
        /// Item form Form</param>
        /// <param name="e">
        /// Event Data</param>
        private void powerBar_Scroll(object sender, EventArgs e)
        {
            GameplayTank currentTank = currentGame.GetCurrentGameplayTank();
            currentTank.SetPower(powerBar.Value);
            powerNumLable.Text = Convert.ToString(powerBar.Value);
        }

        /// <summary>
        /// Updates form elements
        /// </summary>
        /// <param name="sender">
        /// Item form Form</param>
        /// <param name="e">
        /// Event Data</param>
        private void timer_Tick(object sender, EventArgs e)
        {
            
            if (!currentGame.WeaponEffectStep())
            {
                // Apply Garvity to elemnts, until no more gavity needs to be applied
                currentGame.GravityStep();
                DrawBackground();
                DrawGameplay();
                displayPanel.Invalidate();
                if (currentGame.GravityStep() == true)
                {
                    return;
                }
                // If all Gravity has been applied stop timer
                if (currentGame.GravityStep() == false)
                {
                    timer.Enabled = false;

                    if(currentGame.TurnOver() == true)
                    {
                        NewTurn();
                    }
                    else
                    {
                        Dispose();
                        currentGame.NextRound();
                        return;
                    }
                }
            }
            else
            {
                DrawGameplay();
                displayPanel.Invalidate();
                return;
            }
        }
    }
}
