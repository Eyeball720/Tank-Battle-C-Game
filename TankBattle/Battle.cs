using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace TankBattle
{
    public class Battle
    {
        // Declaring private variables
        private static Opponent[] players;
        private GameplayTank[] playerTanks;
        private List<Effect> effect;
        private static Random randomNo = new Random();
        private Map gameMap;
        private GameForm form;

        // Decalring Private ints
        private int[] positions;
        private int maxPlayers;
        private int maxRounds;
        private int currentRound;
        private int startingOpponent;
        private int currentPlayer;
        private int windSpeed;

        /// <summary>
        /// Constructer for “Battle”. Creates the game instance.
        /// </summary>
        /// <param name="numPlayers">
        /// Number of players playing the game. Between 2 and 8</param>
        /// <param name="numRounds">
        /// Number of players playing the game. Between 1 and 100</param>
        public Battle(int numPlayers, int numRounds)
        {
            // Check if values are in range.
            // if they are apply them to variables
            if ((numPlayers >= 2) && (numPlayers <= 8))
            {
                maxPlayers = numPlayers;
                players = new Opponent[numPlayers];
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }

            if ((numRounds >= 1) && (numRounds <= 100))
            {
                maxRounds = numRounds;
            }
            else
            {
                throw new Exception();
            }
            // creates new lift of effects (called by projectile)
            effect = new List<Effect>();
        }

        /// <summary>
        /// Returns the total number of Players.
        /// </summary>
        /// <returns></returns>
        public int PlayerCount()
        {
            // Gets the varible of "maxPlayers"
            return maxPlayers;
        }

        /// <summary>
        /// Returns the curremt rounds.
        /// </summary>
        /// <returns></returns>
        public int GetRound()
        {
            // Gets the varible of "currentRound"
            return currentRound;
        }

        /// <summary>
        /// Returns the total number of Rounds.
        /// </summary>
        /// <returns></returns>
        public int GetTotalRounds()
        {
            // Gets the varible of "maxRounds"
            return maxRounds;
        }

        /// <summary>
        /// Creates player from "Player" array, which defines the horizontal position
        /// </summary>
        /// <param name="playerNum">
        /// Current player (needs to be subtracted by 1)</param>
        /// <param name="player">
        /// Opponet Array (contians player information)</param>
        public void CreatePlayer(int playerNum, Opponent player)
        {
            players[playerNum - 1] = player;
        }

        /// <summary>
        /// Retruns the appropriate players' "Opponet" in the "players" "Opponet" array (Decalered at top)
        /// </summary>
        /// <param name="playerNum">
        /// Current player (needs to be subtracted by 1)</param>
        /// <returns></returns>
        public Opponent GetPlayerNumber(int playerNum)
        {
            // Returns player from array
            return players[playerNum - 1];
        }

        /// <summary>
        /// Retruns the appropriate players' "GameplayTank" in the "playerTanks" "GameplayTank" array (Decalered at top)
        /// </summary>
        /// <param name="playerNum">
        /// Current player (needs to be subtracted by 1)</param>
        /// <returns></returns>
        public GameplayTank PlayerTank(int playerNum)
        {
            return playerTanks[playerNum-1];
        }

        /// <summary>
        /// Appends colours to "colorSet" array. Used to set players' colour.
        /// </summary>
        /// <param name="playerNum">
        /// Current player (needs to be subtracted by 1)</param>
        /// <returns></returns>
        public static Color TankColour(int playerNum)
        {
            // Creates array of colours, and returns player's colour accourding to their number
            Color[] colorSet = { Color.Red, Color.Blue, Color.Green, Color.Cyan, Color.Purple, Color.Violet , Color.Yellow, Color.Lime};
            return colorSet[playerNum-1];
        }

        /// <summary>
        /// Creats and Appends information array that stores player horizontal locations.
        /// </summary>
        /// <param name="numPlayers">
        /// Number of players playing.</param>
        /// <returns>Array that's storeing all player horizontal locations.</returns>
        public static int[] GetPlayerLocations(int numPlayers)
        {
            // Checks if num of players is less then 2
            if (numPlayers < 2)
            {
                throw new ArgumentOutOfRangeException();
            }
            // Creates an array to hold the player horizontal locations
            int[] playerLocations = new int[numPlayers];
            // Finds average position of the players, then moves 
            //the 1st closer to the endge (left)
            float dBetweenPlayers = Map.WIDTH / numPlayers;
            double pos = Math.Round(dBetweenPlayers / 2);
            // Appened the 1st location
            playerLocations[0] = (int)pos - Chassis.WIDTH;
            // Caculate the rest starting from the 1st position
            // seperated by "dBetweenPlayers"
            for (int i = 1; i < numPlayers; i++)
            {
                pos += Math.Round(dBetweenPlayers);
                playerLocations[i] = (int)pos;
            }
            
            return playerLocations;
        }

        /// <summary>
        /// Shuffels given Array (like a deck of cards).
        /// </summary>
        /// <param name="array">
        /// Array that needs indexes' relocated</param>
        public static void Shuffle(int[] array)
        {
            int j = 0;
            // Creates temp array (used to randomize index positions)
            int[] MyRandomArray = array.OrderBy(x => randomNo.Next()).ToArray();
            // Appends temp Array to passed in Array "array"
            foreach (int item in MyRandomArray)
            {
                array[j] = item;
                j += 1;
            }
        }

        /// <summary>
        /// Begins a new game. Rests "currentRound" and "startingOpponent"
        /// </summary>
        public void BeginGame()
        {
            // Sets varibles 
            currentRound = 1;
            startingOpponent = 0;
            StartRound();
        }

        /// <summary>
        /// Starts a new round. Creates Game World
        /// </summary>
        public void StartRound()
        {
            currentPlayer = startingOpponent;
            // Appends a new map to Gamemap 
            gameMap =  new Map();
            // Appends player locations to Array (declared up top)
            positions = GetPlayerLocations(players.Length);
            // Start each "Opponent"s round
            for (int i = 0; i < players.Length; i++)
            {
                players[i].StartRound();
            }
            // Shuffel player positions
            Shuffle(positions);
            // creats new GameplayTank Array to store player information
            playerTanks = new GameplayTank[players.Length];

            for(int i = 0; i < playerTanks.Length; i++)
            {
                int xPos = positions[i];
                int yPos = gameMap.TankVerticalPosition(xPos);
                // Appends information to array
                playerTanks[i] = new GameplayTank(players[i], xPos, yPos, this);
            }
            
            windSpeed = randomNo.Next(-100, 101);
            // Craetes new GameForm (form that display game space)
            form = new GameForm(this);
            form.Show();

        }

        /// <summary>
        /// Get map instance
        /// </summary>
        /// <returns>Current Map</returns>
        public Map GetBattlefield()
        {
            return gameMap;
        }

        /// <summary>
        /// Draws each tank
        /// </summary>
        /// <param name="graphics">
        /// Encapsualted GDI + Drawing Surface</param>
        /// <param name="displaySize">
        /// Size of the Display</param>
        public void DrawTanks(Graphics graphics, Size displaySize)
        {
            for (int i = 0; i < playerTanks.Length; i++)
            {
                if (playerTanks[i].Alive())
                {
                    playerTanks[i].Paint(graphics, displaySize);
                }
            }
        }

        /// <summary>
        /// Returns Current GamplayTank from "playerTanks" Arrays.
        /// </summary>
        /// <returns>Current players GamplayTank data</returns>
        public GameplayTank GetCurrentGameplayTank()
        {
            return playerTanks[currentPlayer];
        }

        /// <summary>
        /// Appends weaponEffect to "effect" List (Declared at top).
        /// </summary>
        /// <param name="weaponEffect">
        /// Effect to be Appended to List of Effects</param>
        public void AddEffect(Effect weaponEffect)
        {
            effect.Add(weaponEffect);
            effect[(effect.Count() - 1)].ConnectGame(this);
        }

        /// <summary>
        /// Starts "Step" method for each effect in Effect List "effect".
        /// </summary>
        /// <returns>"Step()" for each effect in List</returns>
        public bool WeaponEffectStep()
        {
            if (effect.Count() == 0)
            {
                return false;
            }

            for (int i =0; i < effect.Count(); i++)
            {
                effect[i].Step();
            }
            return true;
        }

        /// <summary>
        /// Calls "Paint()" for each effect in "Effect" List "effect".
        /// </summary>
        /// <param name="graphics">
        /// Encapsualted GDI + Drawing Surface</param>
        /// <param name="displaySize">
        /// Size of the Display</param>
        public void DrawWeaponEffects(Graphics graphics, Size displaySize)
        {
            foreach (Effect effect in effect)
            {
                effect.Paint(graphics,displaySize);
            }
        }

        /// <summary>
        /// Removes passed in effect, from "Effect" List "effect"
        /// </summary>
        /// <param name="weaponEffect">
        /// Effect that needs to be removed from List</param>
        public void RemoveWeaponEffect(Effect weaponEffect)
        {
            int i = -1;
            // Checks where weaponEffect is in List
            for (int j = 0; j < effect.Count(); j++)
            {
                if (effect[j] == weaponEffect)
                {
                    i = j;
                }
            }
            // If effect is not in List
            if(i == -1)
            {
                throw new Exception("No weapon Effects in array");
            }
            effect.RemoveAt(i);
        }

        /// <summary>
        /// Checks if projectile has hit something
        /// </summary>
        /// <param name="projectileX">
        /// X position of projectile</param>
        /// <param name="projectileY">
        /// Y position of projectile</param>
        /// <returns> wether the Projectile hit something</returns>
        public bool DetectCollision(float projectileX, float projectileY)
        {
            int xPos;
            int yPos;
            // If projectlis is off the map. Stop operation and retrun false.
            if (projectileX > Map.WIDTH || projectileX < 0 || projectileY > Map.HEIGHT || projectileY < 0)
            {
                return false;
            }
            // If projectile hit terrain. Stop operation and return true.
            if (gameMap.TerrainAt((int)Math.Round(projectileX), (int)Math.Round(projectileY)))
            {
                return true;
            }
            // for very tank in GamePlayTank array "playerTanks"
            // get tank's location, check if projectile hit a tank (excluding itself)
            for (int i = 0; i < playerTanks.Length; i++)
            {
                xPos = playerTanks[i].GetX();
                yPos = playerTanks[i].Y();

                
                if (playerTanks[i].Alive())
                {
                    if (projectileX >= xPos && projectileX <= xPos + Chassis.WIDTH &&
                        projectileY >= yPos && projectileY <= yPos +Chassis.HEIGHT &&
                        playerTanks[i]!=GetCurrentGameplayTank())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Inflicts up to explosionDamage damage on any GameplayTanks 
        /// within the circle described by damageX, damageY and radius
        /// </summary>
        /// <param name="damageX">
        /// Impact X location</param>
        /// <param name="damageY">
        /// Impact Y location</param>
        /// <param name="explosionDamage">
        /// Max damage the explosion does</param>
        /// <param name="radius">
        /// Radius of the explosion</param>
        public void Damage(float damageX, float damageY, float explosionDamage, float radius)
        {
            // For every player in tank calculate distance between centre of tank
            // and straing point of the explosion if the tank is alive
            foreach (GameplayTank player in playerTanks)
            { 
                float a;
                float b;
                float c;
                int damage;
                // Calculate tank center
                float tankCentreX = (player.GetX() + (float)(0.5 * Chassis.WIDTH));
                float tankCentreY = (player.Y() + (float)(0.5 * Chassis.HEIGHT));
                
                if (player.Alive())
                {
                    // Pythagoras Theorem
                    a = Math.Abs(damageX - tankCentreX);
                    b = Math.Abs(damageY - tankCentreY);

                    c = (float)Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));

                    // if tank in explosion
                    if ((c < radius) && (c > radius / 2))
                    {
                        damage = (int)(explosionDamage * ((radius - c) / radius));
                        player.Damage(damage);
                    }
                    // If tank hit
                    else if (c < radius / 2)
                    {
                        damage = (int)explosionDamage;
                        player.Damage(damage);
                    }
                }
            }
        }

        /// <summary>
        /// Moves terrian down one if nothing is underneath it
        /// </summary>
        /// <returns>True if somthing moved. False if nothing moved</returns>
        public bool GravityStep()
        {
            bool stuffMoved = false;
            // Applies grvity to terrian
            if (gameMap.GravityStep())
            {
                stuffMoved = true;
            }
            // Applies gravity to each tank/player
            foreach (GameplayTank tank in playerTanks)
            {
                if (tank.GravityStep())
                {
                    stuffMoved = true;
                }
            }
            return stuffMoved;
        }

        /// <summary>
        /// Checks how many tanks/players are alive, if more then 1 
        /// player is alive changes whos turn it is.
        /// </summary>
        /// <returns>True if more then 1 tank is alive. 
        /// False if 1 less then playeris alive</returns>
        public bool TurnOver()
        {
            int tanksAlive = 0;
            foreach (GameplayTank tank in playerTanks)
            {
                if (tank.Alive())
                {
                    tanksAlive += 1;
                }
            }
            if (tanksAlive >= 2)
            {
                do
                {
                    currentPlayer++;
                    if (currentPlayer == playerTanks.Length)
                    {
                        currentPlayer = 0;
                    }
                } while (playerTanks[currentPlayer].Alive() == false);

                windSpeed += randomNo.Next(-10, 10);
                windSpeed = windSpeed > 100 ? 100 : windSpeed;
                windSpeed = windSpeed < -100 ? -100 : windSpeed;

                return true;
            }
            else
            {
                RewardWinner();
                return false;
            }
        }

        /// <summary>
        /// Adds to the winners overall score
        /// </summary>
        public void RewardWinner()
        {
            // Checks which player is alive. +1 to their score
            for (int i = 0; i < playerTanks.Length; i++)
            {
                if (playerTanks[i].Alive())
                {
                    players[i].AddScore();
                }
            }
        }

        /// <summary>
        /// Starts a new round after someone has won the previous round
        /// </summary>
        public void NextRound()
        {
            currentRound++;

            if (currentRound <= maxRounds)
            {
                startingOpponent++;
                if(startingOpponent > playerTanks.Length)
                {
                    startingOpponent = 0;
                }
                StartRound();
            }
            else
            {
                // Restart game
                IntroForm iForm = new IntroForm();
                iForm.Show();
            }
        }
        
        /// <summary>
        /// Get windSpeed
        /// </summary>
        /// <returns>returns WindSpeed. 
        /// Declared in Battle at the top</returns>
        public int WindSpeed()
        {
            return windSpeed;
        }
    }
}
