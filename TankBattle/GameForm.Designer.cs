namespace TankBattle
{
    partial class GameForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameForm));
            this.displayPanel = new System.Windows.Forms.Panel();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.powerBar = new System.Windows.Forms.TrackBar();
            this.fireButton = new System.Windows.Forms.Button();
            this.powerNumLable = new System.Windows.Forms.Label();
            this.powerLable = new System.Windows.Forms.Label();
            this.angleNumeric = new System.Windows.Forms.NumericUpDown();
            this.angleLable = new System.Windows.Forms.Label();
            this.weaponComboBox = new System.Windows.Forms.ComboBox();
            this.weaponLable = new System.Windows.Forms.Label();
            this.windSpeedLable = new System.Windows.Forms.Label();
            this.windLable = new System.Windows.Forms.Label();
            this.playerLable = new System.Windows.Forms.Label();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.controlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.powerBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.angleNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // displayPanel
            // 
            this.displayPanel.Location = new System.Drawing.Point(0, 32);
            this.displayPanel.Name = "displayPanel";
            this.displayPanel.Size = new System.Drawing.Size(800, 600);
            this.displayPanel.TabIndex = 0;
            this.displayPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.displayPanel_Paint);
            // 
            // controlPanel
            // 
            this.controlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.controlPanel.BackColor = System.Drawing.Color.OrangeRed;
            this.controlPanel.Controls.Add(this.powerBar);
            this.controlPanel.Controls.Add(this.fireButton);
            this.controlPanel.Controls.Add(this.powerNumLable);
            this.controlPanel.Controls.Add(this.powerLable);
            this.controlPanel.Controls.Add(this.angleNumeric);
            this.controlPanel.Controls.Add(this.angleLable);
            this.controlPanel.Controls.Add(this.weaponComboBox);
            this.controlPanel.Controls.Add(this.weaponLable);
            this.controlPanel.Controls.Add(this.windSpeedLable);
            this.controlPanel.Controls.Add(this.windLable);
            this.controlPanel.Controls.Add(this.playerLable);
            this.controlPanel.Enabled = false;
            this.controlPanel.Location = new System.Drawing.Point(0, 0);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(800, 32);
            this.controlPanel.TabIndex = 1;
            // 
            // powerBar
            // 
            this.powerBar.LargeChange = 10;
            this.powerBar.Location = new System.Drawing.Point(536, 0);
            this.powerBar.Maximum = 100;
            this.powerBar.Minimum = 5;
            this.powerBar.Name = "powerBar";
            this.powerBar.Size = new System.Drawing.Size(162, 45);
            this.powerBar.TabIndex = 11;
            this.powerBar.TickFrequency = 5;
            this.powerBar.Value = 5;
            this.powerBar.Scroll += new System.EventHandler(this.powerBar_Scroll);
            // 
            // fireButton
            // 
            this.fireButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fireButton.Location = new System.Drawing.Point(730, 4);
            this.fireButton.Name = "fireButton";
            this.fireButton.Size = new System.Drawing.Size(58, 23);
            this.fireButton.TabIndex = 10;
            this.fireButton.Text = "Fire!";
            this.fireButton.UseVisualStyleBackColor = true;
            this.fireButton.Click += new System.EventHandler(this.fireButton_Click);
            // 
            // powerNumLable
            // 
            this.powerNumLable.AutoSize = true;
            this.powerNumLable.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.powerNumLable.Location = new System.Drawing.Point(695, 5);
            this.powerNumLable.Name = "powerNumLable";
            this.powerNumLable.Size = new System.Drawing.Size(30, 17);
            this.powerNumLable.TabIndex = 9;
            this.powerNumLable.Text = "No.";
            // 
            // powerLable
            // 
            this.powerLable.AutoSize = true;
            this.powerLable.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.powerLable.Location = new System.Drawing.Point(470, 5);
            this.powerLable.Name = "powerLable";
            this.powerLable.Size = new System.Drawing.Size(73, 25);
            this.powerLable.TabIndex = 7;
            this.powerLable.Text = "Power:";
            // 
            // angleNumeric
            // 
            this.angleNumeric.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.angleNumeric.Location = new System.Drawing.Point(419, 6);
            this.angleNumeric.Maximum = new decimal(new int[] {
            90,
            0,
            0,
            0});
            this.angleNumeric.Minimum = new decimal(new int[] {
            90,
            0,
            0,
            -2147483648});
            this.angleNumeric.Name = "angleNumeric";
            this.angleNumeric.Size = new System.Drawing.Size(48, 20);
            this.angleNumeric.TabIndex = 6;
            this.angleNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.angleNumeric.ValueChanged += new System.EventHandler(this.angleNumeric_ValueChanged);
            // 
            // angleLable
            // 
            this.angleLable.AutoSize = true;
            this.angleLable.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.angleLable.Location = new System.Drawing.Point(355, 4);
            this.angleLable.Name = "angleLable";
            this.angleLable.Size = new System.Drawing.Size(69, 25);
            this.angleLable.TabIndex = 5;
            this.angleLable.Text = "Angle:";
            this.angleLable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // weaponComboBox
            // 
            this.weaponComboBox.FormattingEnabled = true;
            this.weaponComboBox.Location = new System.Drawing.Point(228, 5);
            this.weaponComboBox.Name = "weaponComboBox";
            this.weaponComboBox.Size = new System.Drawing.Size(121, 21);
            this.weaponComboBox.TabIndex = 4;
            this.weaponComboBox.SelectedIndexChanged += new System.EventHandler(this.weaponComboBox_SelectedIndexChanged);
            // 
            // weaponLable
            // 
            this.weaponLable.AutoSize = true;
            this.weaponLable.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.weaponLable.Location = new System.Drawing.Point(140, 3);
            this.weaponLable.Name = "weaponLable";
            this.weaponLable.Size = new System.Drawing.Size(93, 25);
            this.weaponLable.TabIndex = 3;
            this.weaponLable.Text = "Weapon:";
            // 
            // windSpeedLable
            // 
            this.windSpeedLable.AutoSize = true;
            this.windSpeedLable.Location = new System.Drawing.Point(105, 17);
            this.windSpeedLable.Name = "windSpeedLable";
            this.windSpeedLable.Size = new System.Drawing.Size(27, 13);
            this.windSpeedLable.TabIndex = 2;
            this.windSpeedLable.Text = "0 W";
            this.windSpeedLable.Click += new System.EventHandler(this.windSpeedLable_Click);
            // 
            // windLable
            // 
            this.windLable.AutoSize = true;
            this.windLable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windLable.Location = new System.Drawing.Point(100, 3);
            this.windLable.Name = "windLable";
            this.windLable.Size = new System.Drawing.Size(36, 13);
            this.windLable.TabIndex = 1;
            this.windLable.Text = "Wind";
            // 
            // playerLable
            // 
            this.playerLable.AutoSize = true;
            this.playerLable.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playerLable.Location = new System.Drawing.Point(3, 3);
            this.playerLable.Name = "playerLable";
            this.playerLable.Size = new System.Drawing.Size(91, 25);
            this.playerLable.TabIndex = 0;
            this.playerLable.Text = "Player 1";
            this.playerLable.Click += new System.EventHandler(this.playerLable_Click);
            // 
            // timer
            // 
            this.timer.Interval = 20;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 629);
            this.Controls.Add(this.controlPanel);
            this.Controls.Add(this.displayPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "GameForm";
            this.Text = "Form1";
            this.controlPanel.ResumeLayout(false);
            this.controlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.powerBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.angleNumeric)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel displayPanel;
        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.Label playerLable;
        private System.Windows.Forms.Label windLable;
        private System.Windows.Forms.Label windSpeedLable;
        private System.Windows.Forms.ComboBox weaponComboBox;
        private System.Windows.Forms.Label weaponLable;
        private System.Windows.Forms.Label angleLable;
        private System.Windows.Forms.NumericUpDown angleNumeric;
        private System.Windows.Forms.Label powerNumLable;
        private System.Windows.Forms.Label powerLable;
        private System.Windows.Forms.Button fireButton;
        private System.Windows.Forms.TrackBar angleTrackBar;
        private System.Windows.Forms.TrackBar powerBar;
        private System.Windows.Forms.Timer timer;
    }
}

