using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using TankBattle;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace TankBattleTestSuite
{
    class RequirementException : Exception
    {
        public RequirementException()
        {
        }

        public RequirementException(string message) : base(message)
        {
        }

        public RequirementException(string message, Exception inner) : base(message, inner)
        {
        }
    }

    class Test
    {
        #region Testing Code

        private delegate bool TestCase();

        private static string ErrorDescription = null;

        private static void SetErrorDescription(string desc)
        {
            ErrorDescription = desc;
        }

        private static bool FloatEquals(float a, float b)
        {
            if (Math.Abs(a - b) < 0.01) return true;
            return false;
        }

        private static Dictionary<string, string> unitTestResults = new Dictionary<string, string>();

        private static void Passed(string name, string comment)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("[passed] ");
            Console.ResetColor();
            Console.Write("{0}", name);
            if (comment != "")
            {
                Console.Write(": {0}", comment);
            }
            if (ErrorDescription != null)
            {
                throw new Exception("ErrorDescription found for passing test case");
            }
            Console.WriteLine();
        }
        private static void Failed(string name, string comment)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("[failed] ");
            Console.ResetColor();
            Console.Write("{0}", name);
            if (comment != "")
            {
                Console.Write(": {0}", comment);
            }
            if (ErrorDescription != null)
            {
                Console.Write("\n{0}", ErrorDescription);
                ErrorDescription = null;
            }
            Console.WriteLine();
        }
        private static void FailedToMeetRequirement(string name, string comment)
        {
            Console.Write("[      ] ");
            Console.Write("{0}", name);
            if (comment != "")
            {
                Console.Write(": ");
                Console.ForegroundColor = ConsoleColor.DarkGray;
                Console.Write("{0}", comment);
                Console.ResetColor();
            }
            Console.WriteLine();
        }

        private static void DoTest(TestCase test)
        {
            // Have we already completed this test?
            if (unitTestResults.ContainsKey(test.Method.ToString()))
            {
                return;
            }

            bool passed = false;
            bool metRequirement = true;
            string exception = "";
            try
            {
                passed = test();
            }
            catch (RequirementException e)
            {
                metRequirement = false;
                exception = e.Message;
            }
            catch (Exception e)
            {
                exception = e.GetType().ToString();
            }

            string className = test.Method.ToString().Replace("Boolean Test", "").Split('0')[0];
            string fnName = test.Method.ToString().Split('0')[1];

            if (metRequirement)
            {
                if (passed)
                {
                    unitTestResults[test.Method.ToString()] = "Passed";
                    Passed(string.Format("{0}.{1}", className, fnName), exception);
                }
                else
                {
                    unitTestResults[test.Method.ToString()] = "Failed";
                    Failed(string.Format("{0}.{1}", className, fnName), exception);
                }
            }
            else
            {
                unitTestResults[test.Method.ToString()] = "Failed";
                FailedToMeetRequirement(string.Format("{0}.{1}", className, fnName), exception);
            }
            Cleanup();
        }

        private static Stack<string> errorDescriptionStack = new Stack<string>();


        private static void Requires(TestCase test)
        {
            string result;
            bool wasTested = unitTestResults.TryGetValue(test.Method.ToString(), out result);

            if (!wasTested)
            {
                // Push the error description onto the stack (only thing that can change, not that it should)
                errorDescriptionStack.Push(ErrorDescription);

                // Do the test
                DoTest(test);

                // Pop the description off
                ErrorDescription = errorDescriptionStack.Pop();

                // Get the proper result for out
                wasTested = unitTestResults.TryGetValue(test.Method.ToString(), out result);

                if (!wasTested)
                {
                    throw new Exception("This should never happen");
                }
            }

            if (result == "Failed")
            {
                string className = test.Method.ToString().Replace("Boolean Test", "").Split('0')[0];
                string fnName = test.Method.ToString().Split('0')[1];

                throw new RequirementException(string.Format("-> {0}.{1}", className, fnName));
            }
            else if (result == "Passed")
            {
                return;
            }
            else
            {
                throw new Exception("This should never happen");
            }

        }

        #endregion

        #region Test Cases
        private static Battle InitialiseGame()
        {
            Requires(TestBattle0Battle);
            Requires(TestChassis0CreateTank);
            Requires(TestOpponent0PlayerController);
            Requires(TestBattle0CreatePlayer);

            Battle game = new Battle(2, 1);
            Chassis tank = Chassis.CreateTank(1);
            Opponent player1 = new PlayerController("player1", tank, Color.Orange);
            Opponent player2 = new PlayerController("player2", tank, Color.Purple);
            game.CreatePlayer(1, player1);
            game.CreatePlayer(2, player2);
            return game;
        }
        private static void Cleanup()
        {
            while (Application.OpenForms.Count > 0)
            {
                Application.OpenForms[0].Dispose();
            }
        }
        private static bool TestBattle0Battle()
        {
            Battle game = new Battle(2, 1);
            return true;
        }
        private static bool TestBattle0PlayerCount()
        {
            Requires(TestBattle0Battle);

            Battle game = new Battle(2, 1);
            return game.PlayerCount() == 2;
        }
        private static bool TestBattle0GetTotalRounds()
        {
            Requires(TestBattle0Battle);

            Battle game = new Battle(3, 5);
            return game.GetTotalRounds() == 5;
        }
        private static bool TestBattle0CreatePlayer()
        {
            Requires(TestBattle0Battle);
            Requires(TestChassis0CreateTank);

            Battle game = new Battle(2, 1);
            Chassis tank = Chassis.CreateTank(1);
            Opponent player = new PlayerController("playerName", tank, Color.Orange);
            game.CreatePlayer(1, player);
            return true;
        }
        private static bool TestBattle0GetPlayerNumber()
        {
            Requires(TestBattle0Battle);
            Requires(TestChassis0CreateTank);
            Requires(TestOpponent0PlayerController);

            Battle game = new Battle(2, 1);
            Chassis tank = Chassis.CreateTank(1);
            Opponent player = new PlayerController("playerName", tank, Color.Orange);
            game.CreatePlayer(1, player);
            return game.GetPlayerNumber(1) == player;
        }
        private static bool TestBattle0TankColour()
        {
            Color[] arrayOfColours = new Color[8];
            for (int i = 0; i < 8; i++)
            {
                arrayOfColours[i] = Battle.TankColour(i + 1);
                for (int j = 0; j < i; j++)
                {
                    if (arrayOfColours[j] == arrayOfColours[i]) return false;
                }
            }
            return true;
        }
        private static bool TestBattle0GetPlayerLocations()
        {
            int[] positions = Battle.GetPlayerLocations(8);
            for (int i = 0; i < 8; i++)
            {
                if (positions[i] < 0) return false;
                if (positions[i] > 160) return false;
                for (int j = 0; j < i; j++)
                {
                    if (positions[j] == positions[i]) return false;
                }
            }
            return true;
        }
        private static bool TestBattle0Shuffle()
        {
            int[] ar = new int[100];
            for (int i = 0; i < 100; i++)
            {
                ar[i] = i;
            }
            Battle.Shuffle(ar);
            for (int i = 0; i < 100; i++)
            {
                if (ar[i] != i)
                {
                    return true;
                }
            }
            return false;
        }
        private static bool TestBattle0BeginGame()
        {
            Battle game = InitialiseGame();
            game.BeginGame();

            foreach (Form f in Application.OpenForms)
            {
                if (f is GameForm)
                {
                    return true;
                }
            }
            return false;
        }
        private static bool TestBattle0GetBattlefield()
        {
            Requires(TestMap0Map);
            Battle game = InitialiseGame();
            game.BeginGame();
            Map battlefield = game.GetBattlefield();
            if (battlefield != null) return true;

            return false;
        }
        private static bool TestBattle0GetCurrentGameplayTank()
        {
            Requires(TestBattle0Battle);
            Requires(TestChassis0CreateTank);
            Requires(TestOpponent0PlayerController);
            Requires(TestBattle0CreatePlayer);
            Requires(TestGameplayTank0GetPlayerNumber);

            Battle game = new Battle(2, 1);
            Chassis tank = Chassis.CreateTank(1);
            Opponent player1 = new PlayerController("player1", tank, Color.Orange);
            Opponent player2 = new PlayerController("player2", tank, Color.Purple);
            game.CreatePlayer(1, player1);
            game.CreatePlayer(2, player2);

            game.BeginGame();
            GameplayTank ptank = game.GetCurrentGameplayTank();
            if (ptank.GetPlayerNumber() != player1 && ptank.GetPlayerNumber() != player2)
            {
                return false;
            }
            if (ptank.CreateTank() != tank)
            {
                return false;
            }

            return true;
        }

        private static bool TestChassis0CreateTank()
        {
            Chassis tank = Chassis.CreateTank(1);
            if (tank != null) return true;
            else return false;
        }
        private static bool TestChassis0DisplayTankSprite()
        {
            Requires(TestChassis0CreateTank);
            Chassis tank = Chassis.CreateTank(1);

            int[,] tankGraphic = tank.DisplayTankSprite(45);
            if (tankGraphic.GetLength(0) != 12) return false;
            if (tankGraphic.GetLength(1) != 16) return false;
            // We don't really care what the tank looks like, but the 45 degree tank
            // should at least look different to the -45 degree tank
            int[,] tankGraphic2 = tank.DisplayTankSprite(-45);
            for (int y = 0; y < 12; y++)
            {
                for (int x = 0; x < 16; x++)
                {
                    if (tankGraphic2[y, x] != tankGraphic[y, x])
                    {
                        return true;
                    }
                }
            }

            SetErrorDescription("Tank with turret at -45 degrees looks the same as tank with turret at 45 degrees");

            return false;
        }
        private static void DisplayLine(int[,] array)
        {
            string report = "";
            report += "A line drawn from 3,0 to 0,3 on a 4x4 array should look like this:\n";
            report += "0001\n";
            report += "0010\n";
            report += "0100\n";
            report += "1000\n";
            report += "The one produced by Chassis.DrawLine() looks like this:\n";
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    report += array[y, x] == 1 ? "1" : "0";
                }
                report += "\n";
            }
            SetErrorDescription(report);
        }
        private static bool TestChassis0DrawLine()
        {
            int[,] ar = new int[,] { { 0, 0, 0, 0 },
                                     { 0, 0, 0, 0 },
                                     { 0, 0, 0, 0 },
                                     { 0, 0, 0, 0 } };
            Chassis.DrawLine(ar, 3, 0, 0, 3);

            // Ideally, the line we want to see here is:
            // 0001
            // 0010
            // 0100
            // 1000

            // However, as we aren't that picky, as long as they have a 1 in every row and column
            // and nothing in the top-left and bottom-right corners

            int[] rows = new int[4];
            int[] cols = new int[4];
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 4; x++)
                {
                    if (ar[y, x] == 1)
                    {
                        rows[y] = 1;
                        cols[x] = 1;
                    }
                    else if (ar[y, x] > 1 || ar[y, x] < 0)
                    {
                        // Only values 0 and 1 are permitted
                        SetErrorDescription(string.Format("Somehow the number {0} got into the array.", ar[y, x]));
                        return false;
                    }
                }
            }

            for (int i = 0; i < 4; i++)
            {
                if (rows[i] == 0)
                {
                    DisplayLine(ar);
                    return false;
                }
                if (cols[i] == 0)
                {
                    DisplayLine(ar);
                    return false;
                }
            }
            if (ar[0, 0] == 1)
            {
                DisplayLine(ar);
                return false;
            }
            if (ar[3, 3] == 1)
            {
                DisplayLine(ar);
                return false;
            }

            return true;
        }
        private static bool TestChassis0GetHealth()
        {
            Requires(TestChassis0CreateTank);
            // As long as it's > 0 we're happy
            Chassis tank = Chassis.CreateTank(1);
            if (tank.GetHealth() > 0) return true;
            return false;
        }
        private static bool TestChassis0GetWeapons()
        {
            Requires(TestChassis0CreateTank);
            // As long as there's at least one result and it's not null / a blank string, we're happy
            Chassis tank = Chassis.CreateTank(1);
            if (tank.GetWeapons().Length == 0) return false;
            if (tank.GetWeapons()[0] == null) return false;
            if (tank.GetWeapons()[0] == "") return false;
            return true;
        }

        private static Opponent CreateTestingPlayer()
        {
            Requires(TestChassis0CreateTank);
            Requires(TestOpponent0PlayerController);

            Chassis tank = Chassis.CreateTank(1);
            Opponent player = new PlayerController("player1", tank, Color.Aquamarine);
            return player;
        }

        private static bool TestOpponent0PlayerController()
        {
            Requires(TestChassis0CreateTank);

            Chassis tank = Chassis.CreateTank(1);
            Opponent player = new PlayerController("player1", tank, Color.Aquamarine);
            if (player != null) return true;
            return false;
        }
        private static bool TestOpponent0CreateTank()
        {
            Requires(TestChassis0CreateTank);
            Requires(TestOpponent0PlayerController);

            Chassis tank = Chassis.CreateTank(1);
            Opponent p = new PlayerController("player1", tank, Color.Aquamarine);
            if (p.CreateTank() == tank) return true;
            return false;
        }
        private static bool TestOpponent0Identifier()
        {
            Requires(TestChassis0CreateTank);
            Requires(TestOpponent0PlayerController);

            const string PLAYER_NAME = "kfdsahskfdajh";
            Chassis tank = Chassis.CreateTank(1);
            Opponent p = new PlayerController(PLAYER_NAME, tank, Color.Aquamarine);
            if (p.Identifier() == PLAYER_NAME) return true;
            return false;
        }
        private static bool TestOpponent0PlayerColour()
        {
            Requires(TestChassis0CreateTank);
            Requires(TestOpponent0PlayerController);

            Color playerColour = Color.Chartreuse;
            Chassis tank = Chassis.CreateTank(1);
            Opponent p = new PlayerController("player1", tank, playerColour);
            if (p.PlayerColour() == playerColour) return true;
            return false;
        }
        private static bool TestOpponent0AddScore()
        {
            Opponent p = CreateTestingPlayer();
            p.AddScore();
            return true;
        }
        private static bool TestOpponent0GetWins()
        {
            Requires(TestOpponent0AddScore);

            Opponent p = CreateTestingPlayer();
            int wins = p.GetWins();
            p.AddScore();
            if (p.GetWins() == wins + 1) return true;
            return false;
        }
        private static bool TestPlayerController0StartRound()
        {
            Opponent p = CreateTestingPlayer();
            p.StartRound();
            return true;
        }
        private static bool TestPlayerController0CommenceTurn()
        {
            Requires(TestBattle0BeginGame);
            Requires(TestBattle0GetPlayerNumber);
            Battle game = InitialiseGame();

            game.BeginGame();

            // Find the gameplay form
            GameForm gameplayForm = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f is GameForm)
                {
                    gameplayForm = f as GameForm;
                }
            }
            if (gameplayForm == null)
            {
                SetErrorDescription("Gameplay form was not created by Battle.BeginGame()");
                return false;
            }

            // Find the control panel
            Panel controlPanel = null;
            foreach (Control c in gameplayForm.Controls)
            {
                if (c is Panel)
                {
                    foreach (Control cc in c.Controls)
                    {
                        if (cc is NumericUpDown || cc is Label || cc is TrackBar)
                        {
                            controlPanel = c as Panel;
                        }
                    }
                }
            }

            if (controlPanel == null)
            {
                SetErrorDescription("Control panel was not found in GameForm");
                return false;
            }

            // Disable the control panel to check that NewTurn enables it
            controlPanel.Enabled = false;

            game.GetPlayerNumber(1).CommenceTurn(gameplayForm, game);

            if (!controlPanel.Enabled)
            {
                SetErrorDescription("Control panel is still disabled after HumanPlayer.NewTurn()");
                return false;
            }
            return true;

        }
        private static bool TestPlayerController0ProjectileHit()
        {
            Opponent p = CreateTestingPlayer();
            p.ProjectileHit(0, 0);
            return true;
        }

        private static bool TestGameplayTank0GameplayTank()
        {
            Opponent p = CreateTestingPlayer();
            Battle game = InitialiseGame();
            GameplayTank playerTank = new GameplayTank(p, 32, 32, game);
            return true;
        }
        private static bool TestGameplayTank0GetPlayerNumber()
        {
            Requires(TestGameplayTank0GameplayTank);
            Opponent p = CreateTestingPlayer();
            Battle game = InitialiseGame();
            GameplayTank playerTank = new GameplayTank(p, 32, 32, game);
            if (playerTank.GetPlayerNumber() == p) return true;
            return false;
        }
        private static bool TestGameplayTank0CreateTank()
        {
            Requires(TestGameplayTank0GameplayTank);
            Requires(TestOpponent0CreateTank);
            Opponent p = CreateTestingPlayer();
            Battle game = InitialiseGame();
            GameplayTank playerTank = new GameplayTank(p, 32, 32, game);
            if (playerTank.CreateTank() == playerTank.GetPlayerNumber().CreateTank()) return true;
            return false;
        }
        private static bool TestGameplayTank0GetPlayerAngle()
        {
            Requires(TestGameplayTank0GameplayTank);
            Opponent p = CreateTestingPlayer();
            Battle game = InitialiseGame();
            GameplayTank playerTank = new GameplayTank(p, 32, 32, game);
            float angle = playerTank.GetPlayerAngle();
            if (angle >= -90 && angle <= 90) return true;
            return false;
        }
        private static bool TestGameplayTank0SetAngle()
        {
            Requires(TestGameplayTank0GameplayTank);
            Requires(TestGameplayTank0GetPlayerAngle);
            float angle = 75;
            Opponent p = CreateTestingPlayer();
            Battle game = InitialiseGame();
            GameplayTank playerTank = new GameplayTank(p, 32, 32, game);
            playerTank.SetAngle(angle);
            if (FloatEquals(playerTank.GetPlayerAngle(), angle)) return true;
            return false;
        }
        private static bool TestGameplayTank0GetPowerLevel()
        {
            Requires(TestGameplayTank0GameplayTank);
            Opponent p = CreateTestingPlayer();
            Battle game = InitialiseGame();
            GameplayTank playerTank = new GameplayTank(p, 32, 32, game);

            playerTank.GetPowerLevel();
            return true;
        }
        private static bool TestGameplayTank0SetPower()
        {
            Requires(TestGameplayTank0GameplayTank);
            Requires(TestGameplayTank0GetPowerLevel);
            int power = 65;
            Opponent p = CreateTestingPlayer();
            Battle game = InitialiseGame();
            GameplayTank playerTank = new GameplayTank(p, 32, 32, game);
            playerTank.SetPower(power);
            if (playerTank.GetPowerLevel() == power) return true;
            return false;
        }
        private static bool TestGameplayTank0GetWeapon()
        {
            Requires(TestGameplayTank0GameplayTank);

            Opponent p = CreateTestingPlayer();
            Battle game = InitialiseGame();
            GameplayTank playerTank = new GameplayTank(p, 32, 32, game);

            playerTank.GetWeapon();
            return true;
        }
        private static bool TestGameplayTank0SetWeaponIndex()
        {
            Requires(TestGameplayTank0GameplayTank);
            Requires(TestGameplayTank0GetWeapon);
            int weapon = 3;
            Opponent p = CreateTestingPlayer();
            Battle game = InitialiseGame();
            GameplayTank playerTank = new GameplayTank(p, 32, 32, game);
            playerTank.SetWeaponIndex(weapon);
            if (playerTank.GetWeapon() == weapon) return true;
            return false;
        }
        private static bool TestGameplayTank0Paint()
        {
            Requires(TestGameplayTank0GameplayTank);
            Size bitmapSize = new Size(640, 480);
            Bitmap image = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            Graphics graphics = Graphics.FromImage(image);
            Opponent p = CreateTestingPlayer();
            Battle game = InitialiseGame();
            GameplayTank playerTank = new GameplayTank(p, 32, 32, game);
            playerTank.Paint(graphics, bitmapSize);
            graphics.Dispose();

            for (int y = 0; y < bitmapSize.Height; y++)
            {
                for (int x = 0; x < bitmapSize.Width; x++)
                {
                    if (image.GetPixel(x, y) != image.GetPixel(0, 0))
                    {
                        // Something changed in the image, and that's good enough for me
                        return true;
                    }
                }
            }
            SetErrorDescription("Nothing was drawn.");
            return false;
        }
        private static bool TestGameplayTank0GetX()
        {
            Requires(TestGameplayTank0GameplayTank);

            Opponent p = CreateTestingPlayer();
            int x = 73;
            int y = 28;
            Battle game = InitialiseGame();
            GameplayTank playerTank = new GameplayTank(p, x, y, game);
            if (playerTank.GetX() == x) return true;
            return false;
        }
        private static bool TestGameplayTank0Y()
        {
            Requires(TestGameplayTank0GameplayTank);

            Opponent p = CreateTestingPlayer();
            int x = 73;
            int y = 28;
            Battle game = InitialiseGame();
            GameplayTank playerTank = new GameplayTank(p, x, y, game);
            if (playerTank.Y() == y) return true;
            return false;
        }
        private static bool TestGameplayTank0Shoot()
        {
            Requires(TestGameplayTank0GameplayTank);

            Opponent p = CreateTestingPlayer();
            Battle game = InitialiseGame();
            GameplayTank playerTank = new GameplayTank(p, 32, 32, game);
            playerTank.Shoot();
            return true;
        }
        private static bool TestGameplayTank0Damage()
        {
            Requires(TestGameplayTank0GameplayTank);
            Opponent p = CreateTestingPlayer();

            Battle game = InitialiseGame();
            GameplayTank playerTank = new GameplayTank(p, 32, 32, game);
            playerTank.Damage(10);
            return true;
        }
        private static bool TestGameplayTank0Alive()
        {
            Requires(TestGameplayTank0GameplayTank);
            Requires(TestGameplayTank0Damage);

            Opponent p = CreateTestingPlayer();
            Battle game = InitialiseGame();
            GameplayTank playerTank = new GameplayTank(p, 32, 32, game);
            if (!playerTank.Alive()) return false;
            playerTank.Damage(playerTank.CreateTank().GetHealth());
            if (playerTank.Alive()) return false;
            return true;
        }
        private static bool TestGameplayTank0GravityStep()
        {
            Requires(TestBattle0GetBattlefield);
            Requires(TestMap0DestroyTiles);
            Requires(TestGameplayTank0GameplayTank);
            Requires(TestGameplayTank0Damage);
            Requires(TestGameplayTank0Alive);
            Requires(TestGameplayTank0CreateTank);
            Requires(TestChassis0GetHealth);

            Opponent p = CreateTestingPlayer();
            Battle game = InitialiseGame();
            game.BeginGame();
            // Unfortunately we need to rely on DestroyTerrain() to get rid of any terrain that may be in the way
            game.GetBattlefield().DestroyTiles(Map.WIDTH / 2.0f, Map.HEIGHT / 2.0f, 20);
            GameplayTank playerTank = new GameplayTank(p, Map.WIDTH / 2, Map.HEIGHT / 2, game);
            int oldX = playerTank.GetX();
            int oldY = playerTank.Y();

            playerTank.GravityStep();

            if (playerTank.GetX() != oldX)
            {
                SetErrorDescription("Caused X coordinate to change.");
                return false;
            }
            if (playerTank.Y() != oldY + 1)
            {
                SetErrorDescription("Did not cause Y coordinate to increase by 1.");
                return false;
            }

            int initialArmour = playerTank.CreateTank().GetHealth();
            // The tank should have lost 1 armour from falling 1 tile already, so do
            // (initialArmour - 2) damage to the tank then drop it again. That should kill it.

            if (!playerTank.Alive())
            {
                SetErrorDescription("Tank died before we could check that fall damage worked properly");
                return false;
            }
            playerTank.Damage(initialArmour - 2);
            if (!playerTank.Alive())
            {
                SetErrorDescription("Tank died before we could check that fall damage worked properly");
                return false;
            }
            playerTank.GravityStep();
            if (playerTank.Alive())
            {
                SetErrorDescription("Tank survived despite taking enough falling damage to destroy it");
                return false;
            }

            return true;
        }
        private static bool TestMap0Map()
        {
            Map battlefield = new Map();
            return true;
        }
        private static bool TestMap0TerrainAt()
        {
            Requires(TestMap0Map);

            bool foundTrue = false;
            bool foundFalse = false;
            Map battlefield = new Map();
            for (int y = 0; y < Map.HEIGHT; y++)
            {
                for (int x = 0; x < Map.WIDTH; x++)
                {
                    if (battlefield.TerrainAt(x, y))
                    {
                        foundTrue = true;
                    }
                    else
                    {
                        foundFalse = true;
                    }
                }
            }

            if (!foundTrue)
            {
                SetErrorDescription("IsTileAt() did not return true for any tile.");
                return false;
            }

            if (!foundFalse)
            {
                SetErrorDescription("IsTileAt() did not return false for any tile.");
                return false;
            }

            return true;
        }
        private static bool TestMap0TankCollisionAt()
        {
            Requires(TestMap0Map);
            Requires(TestMap0TerrainAt);

            Map battlefield = new Map();
            for (int y = 0; y <= Map.HEIGHT - Chassis.HEIGHT; y++)
            {
                for (int x = 0; x <= Map.WIDTH - Chassis.WIDTH; x++)
                {
                    int colTiles = 0;
                    for (int iy = 0; iy < Chassis.HEIGHT; iy++)
                    {
                        for (int ix = 0; ix < Chassis.WIDTH; ix++)
                        {

                            if (battlefield.TerrainAt(x + ix, y + iy))
                            {
                                colTiles++;
                            }
                        }
                    }
                    if (colTiles == 0)
                    {
                        if (battlefield.TankCollisionAt(x, y))
                        {
                            SetErrorDescription("Found collision where there shouldn't be one");
                            return false;
                        }
                    }
                    else
                    {
                        if (!battlefield.TankCollisionAt(x, y))
                        {
                            SetErrorDescription("Didn't find collision where there should be one");
                            return false;
                        }
                    }
                }
            }

            return true;
        }
        private static bool TestMap0TankVerticalPosition()
        {
            Requires(TestMap0Map);
            Requires(TestMap0TerrainAt);

            Map battlefield = new Map();
            for (int x = 0; x <= Map.WIDTH - Chassis.WIDTH; x++)
            {
                int lowestValid = 0;
                for (int y = 0; y <= Map.HEIGHT - Chassis.HEIGHT; y++)
                {
                    int colTiles = 0;
                    for (int iy = 0; iy < Chassis.HEIGHT; iy++)
                    {
                        for (int ix = 0; ix < Chassis.WIDTH; ix++)
                        {

                            if (battlefield.TerrainAt(x + ix, y + iy))
                            {
                                colTiles++;
                            }
                        }
                    }
                    if (colTiles == 0)
                    {
                        lowestValid = y;
                    }
                }

                int placedY = battlefield.TankVerticalPosition(x);
                if (placedY != lowestValid)
                {
                    SetErrorDescription(string.Format("Tank was placed at {0},{1} when it should have been placed at {0},{2}", x, placedY, lowestValid));
                    return false;
                }
            }
            return true;
        }
        private static bool TestMap0DestroyTiles()
        {
            Requires(TestMap0Map);
            Requires(TestMap0TerrainAt);

            Map battlefield = new Map();
            for (int y = 0; y < Map.HEIGHT; y++)
            {
                for (int x = 0; x < Map.WIDTH; x++)
                {
                    if (battlefield.TerrainAt(x, y))
                    {
                        battlefield.DestroyTiles(x, y, 0.5f);
                        if (battlefield.TerrainAt(x, y))
                        {
                            SetErrorDescription("Attempted to destroy terrain but it still exists");
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
            }
            SetErrorDescription("Did not find any terrain to destroy");
            return false;
        }
        private static bool TestMap0GravityStep()
        {
            Requires(TestMap0Map);
            Requires(TestMap0TerrainAt);
            Requires(TestMap0DestroyTiles);

            Map battlefield = new Map();
            for (int x = 0; x < Map.WIDTH; x++)
            {
                if (battlefield.TerrainAt(x, Map.HEIGHT - 1))
                {
                    if (battlefield.TerrainAt(x, Map.HEIGHT - 2))
                    {
                        // Seek up and find the first non-set tile
                        for (int y = Map.HEIGHT - 2; y >= 0; y--)
                        {
                            if (!battlefield.TerrainAt(x, y))
                            {
                                // Do a gravity step and make sure it doesn't slip down
                                battlefield.GravityStep();
                                if (!battlefield.TerrainAt(x, y + 1))
                                {
                                    SetErrorDescription("Moved down terrain even though there was no room");
                                    return false;
                                }

                                // Destroy the bottom-most tile
                                battlefield.DestroyTiles(x, Map.HEIGHT - 1, 0.5f);

                                // Do a gravity step and make sure it does slip down
                                battlefield.GravityStep();

                                if (battlefield.TerrainAt(x, y + 1))
                                {
                                    SetErrorDescription("Terrain didn't fall");
                                    return false;
                                }

                                // Otherwise this seems to have worked
                                return true;
                            }
                        }


                    }
                }
            }
            SetErrorDescription("Did not find any appropriate terrain to test");
            return false;
        }
        private static bool TestEffect0ConnectGame()
        {
            Requires(TestExplosion0Explosion);
            Requires(TestBattle0Battle);

            Effect weaponEffect = new Explosion(1, 1, 1);
            Battle game = new Battle(2, 1);
            weaponEffect.ConnectGame(game);
            return true;
        }
        private static bool TestProjectile0Projectile()
        {
            Requires(TestExplosion0Explosion);
            Opponent player = CreateTestingPlayer();
            Explosion explosion = new Explosion(1, 1, 1);
            Projectile projectile = new Projectile(25, 25, 45, 30, 0.02f, explosion, player);
            return true;
        }
        private static bool TestProjectile0Step()
        {
            Requires(TestBattle0BeginGame);
            Requires(TestExplosion0Explosion);
            Requires(TestProjectile0Projectile);
            Requires(TestEffect0ConnectGame);
            Battle game = InitialiseGame();
            game.BeginGame();
            Opponent player = game.GetPlayerNumber(1);
            Explosion explosion = new Explosion(1, 1, 1);

            Projectile projectile = new Projectile(25, 25, 45, 100, 0.01f, explosion, player);
            projectile.ConnectGame(game);
            projectile.Step();

            // We can't really test this one without a substantial framework,
            // so we just call it and hope that everything works out

            return true;
        }
        private static bool TestProjectile0Paint()
        {
            Requires(TestBattle0BeginGame);
            Requires(TestBattle0GetPlayerNumber);
            Requires(TestExplosion0Explosion);
            Requires(TestProjectile0Projectile);
            Requires(TestEffect0ConnectGame);

            Size bitmapSize = new Size(640, 480);
            Bitmap image = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.Clear(Color.Black); // Blacken out the image so we can see the projectile
            Opponent p = CreateTestingPlayer();
            Battle game = InitialiseGame();
            game.BeginGame();
            Opponent player = game.GetPlayerNumber(1);
            Explosion explosion = new Explosion(1, 1, 1);

            Projectile projectile = new Projectile(25, 25, 45, 100, 0.01f, explosion, player);
            projectile.ConnectGame(game);
            projectile.Paint(graphics, bitmapSize);
            graphics.Dispose();

            for (int y = 0; y < bitmapSize.Height; y++)
            {
                for (int x = 0; x < bitmapSize.Width; x++)
                {
                    if (image.GetPixel(x, y) != image.GetPixel(0, 0))
                    {
                        // Something changed in the image, and that's good enough for me
                        return true;
                    }
                }
            }
            SetErrorDescription("Nothing was drawn.");
            return false;
        }
        private static bool TestExplosion0Explosion()
        {
            Opponent player = CreateTestingPlayer();
            Explosion explosion = new Explosion(1, 1, 1);

            return true;
        }
        private static bool TestExplosion0Activate()
        {
            Requires(TestExplosion0Explosion);
            Requires(TestEffect0ConnectGame);
            Requires(TestBattle0GetPlayerNumber);
            Requires(TestBattle0BeginGame);

            Battle game = InitialiseGame();
            game.BeginGame();
            Opponent player = game.GetPlayerNumber(1);
            Explosion explosion = new Explosion(1, 1, 1);
            explosion.ConnectGame(game);
            explosion.Activate(25, 25);

            return true;
        }
        private static bool TestExplosion0Step()
        {
            Requires(TestExplosion0Explosion);
            Requires(TestEffect0ConnectGame);
            Requires(TestBattle0GetPlayerNumber);
            Requires(TestBattle0BeginGame);
            Requires(TestExplosion0Activate);

            Battle game = InitialiseGame();
            game.BeginGame();
            Opponent player = game.GetPlayerNumber(1);
            Explosion explosion = new Explosion(1, 1, 1);
            explosion.ConnectGame(game);
            explosion.Activate(25, 25);
            explosion.Step();

            // Again, we can't really test this one without a full framework

            return true;
        }
        private static bool TestExplosion0Paint()
        {
            Requires(TestExplosion0Explosion);
            Requires(TestEffect0ConnectGame);
            Requires(TestBattle0GetPlayerNumber);
            Requires(TestBattle0BeginGame);
            Requires(TestExplosion0Activate);
            Requires(TestExplosion0Step);

            Size bitmapSize = new Size(640, 480);
            Bitmap image = new Bitmap(bitmapSize.Width, bitmapSize.Height);
            Graphics graphics = Graphics.FromImage(image);
            graphics.Clear(Color.Black); // Blacken out the image so we can see the explosion
            Opponent p = CreateTestingPlayer();
            Battle game = InitialiseGame();
            game.BeginGame();
            Opponent player = game.GetPlayerNumber(1);
            Explosion explosion = new Explosion(10, 10, 10);
            explosion.ConnectGame(game);
            explosion.Activate(25, 25);
            // Step it for a bit so we can be sure the explosion is visible
            for (int i = 0; i < 10; i++)
            {
                explosion.Step();
            }
            explosion.Paint(graphics, bitmapSize);

            for (int y = 0; y < bitmapSize.Height; y++)
            {
                for (int x = 0; x < bitmapSize.Width; x++)
                {
                    if (image.GetPixel(x, y) != image.GetPixel(0, 0))
                    {
                        // Something changed in the image, and that's good enough for me
                        return true;
                    }
                }
            }
            SetErrorDescription("Nothing was drawn.");
            return false;
        }

        private static GameForm InitialiseGameForm(out NumericUpDown angleCtrl, out TrackBar powerCtrl, out Button fireCtrl, out Panel controlPanel, out ListBox weaponSelect)
        {
            Requires(TestBattle0BeginGame);

            Battle game = InitialiseGame();

            angleCtrl = null;
            powerCtrl = null;
            fireCtrl = null;
            controlPanel = null;
            weaponSelect = null;

            game.BeginGame();
            GameForm gameplayForm = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f is GameForm)
                {
                    gameplayForm = f as GameForm;
                }
            }
            if (gameplayForm == null)
            {
                SetErrorDescription("Battle.BeginGame() did not create a GameForm and that is the only way GameForm can be tested");
                return null;
            }

            bool foundDisplayPanel = false;
            bool foundControlPanel = false;

            foreach (Control c in gameplayForm.Controls)
            {
                // The only controls should be 2 panels
                if (c is Panel)
                {
                    // Is this the control panel or the display panel?
                    Panel p = c as Panel;

                    // The display panel will have 0 controls.
                    // The control panel will have separate, of which only a few are mandatory
                    int controlsFound = 0;
                    bool foundFire = false;
                    bool foundAngle = false;
                    bool foundAngleLabel = false;
                    bool foundPower = false;
                    bool foundPowerLabel = false;


                    foreach (Control pc in p.Controls)
                    {
                        controlsFound++;

                        // Mandatory controls for the control panel are:
                        // A 'Fire!' button
                        // A NumericUpDown for controlling the angle
                        // A TrackBar for controlling the power
                        // "Power:" and "Angle:" labels

                        if (pc is Label)
                        {
                            Label lbl = pc as Label;
                            if (lbl.Text.ToLower().Contains("angle"))
                            {
                                foundAngleLabel = true;
                            }
                            else
                            if (lbl.Text.ToLower().Contains("power"))
                            {
                                foundPowerLabel = true;
                            }
                        }
                        else
                        if (pc is Button)
                        {
                            Button btn = pc as Button;
                            if (btn.Text.ToLower().Contains("fire"))
                            {
                                foundFire = true;
                                fireCtrl = btn;
                            }
                        }
                        else
                        if (pc is TrackBar)
                        {
                            foundPower = true;
                            powerCtrl = pc as TrackBar;
                        }
                        else
                        if (pc is NumericUpDown)
                        {
                            foundAngle = true;
                            angleCtrl = pc as NumericUpDown;
                        }
                        else
                        if (pc is ListBox)
                        {
                            weaponSelect = pc as ListBox;
                        }
                    }

                    if (controlsFound == 0)
                    {
                        foundDisplayPanel = true;
                    }
                    else
                    {
                        if (!foundFire)
                        {
                            SetErrorDescription("Control panel lacks a \"Fire!\" button OR the display panel incorrectly contains controls");
                            return null;
                        }
                        else
                        if (!foundAngle)
                        {
                            SetErrorDescription("Control panel lacks an angle NumericUpDown OR the display panel incorrectly contains controls");
                            return null;
                        }
                        else
                        if (!foundPower)
                        {
                            SetErrorDescription("Control panel lacks a power TrackBar OR the display panel incorrectly contains controls");
                            return null;
                        }
                        else
                        if (!foundAngleLabel)
                        {
                            SetErrorDescription("Control panel lacks an \"Angle:\" label OR the display panel incorrectly contains controls");
                            return null;
                        }
                        else
                        if (!foundPowerLabel)
                        {
                            SetErrorDescription("Control panel lacks a \"Power:\" label OR the display panel incorrectly contains controls");
                            return null;
                        }

                        foundControlPanel = true;
                        controlPanel = p;
                    }

                }
                else
                {
                    SetErrorDescription(string.Format("Unexpected control ({0}) named \"{1}\" found in GameForm", c.GetType().FullName, c.Name));
                    return null;
                }
            }

            if (!foundDisplayPanel)
            {
                SetErrorDescription("No display panel found");
                return null;
            }
            if (!foundControlPanel)
            {
                SetErrorDescription("No control panel found");
                return null;
            }
            return gameplayForm;
        }

        private static bool TestGameForm0GameForm()
        {
            NumericUpDown angle;
            TrackBar power;
            Button fire;
            Panel controlPanel;
            ListBox weaponSelect;
            GameForm gameplayForm = InitialiseGameForm(out angle, out power, out fire, out controlPanel, out weaponSelect);

            if (gameplayForm == null) return false;

            return true;
        }
        private static bool TestGameForm0EnableTankButtons()
        {
            Requires(TestGameForm0GameForm);
            Battle game = InitialiseGame();
            game.BeginGame();

            // Find the gameplay form
            GameForm gameplayForm = null;
            foreach (Form f in Application.OpenForms)
            {
                if (f is GameForm)
                {
                    gameplayForm = f as GameForm;
                }
            }
            if (gameplayForm == null)
            {
                SetErrorDescription("Gameplay form was not created by Battle.BeginGame()");
                return false;
            }

            // Find the control panel
            Panel controlPanel = null;
            foreach (Control c in gameplayForm.Controls)
            {
                if (c is Panel)
                {
                    foreach (Control cc in c.Controls)
                    {
                        if (cc is NumericUpDown || cc is Label || cc is TrackBar)
                        {
                            controlPanel = c as Panel;
                        }
                    }
                }
            }

            if (controlPanel == null)
            {
                SetErrorDescription("Control panel was not found in GameForm");
                return false;
            }

            // Disable the control panel to check that EnableControlPanel enables it
            controlPanel.Enabled = false;

            gameplayForm.EnableTankButtons();

            if (!controlPanel.Enabled)
            {
                SetErrorDescription("Control panel is still disabled after GameForm.EnableTankButtons()");
                return false;
            }
            return true;

        }
        private static bool TestGameForm0SetAngle()
        {
            Requires(TestGameForm0GameForm);
            NumericUpDown angle;
            TrackBar power;
            Button fire;
            Panel controlPanel;
            ListBox weaponSelect;
            GameForm gameplayForm = InitialiseGameForm(out angle, out power, out fire, out controlPanel, out weaponSelect);

            if (gameplayForm == null) return false;

            float testAngle = 27;

            gameplayForm.SetAngle(testAngle);
            if (FloatEquals((float)angle.Value, testAngle)) return true;

            else
            {
                SetErrorDescription(string.Format("Attempted to set angle to {0} but angle is {1}", testAngle, (float)angle.Value));
                return false;
            }
        }
        private static bool TestGameForm0SetPower()
        {
            Requires(TestGameForm0GameForm);
            NumericUpDown angle;
            TrackBar power;
            Button fire;
            Panel controlPanel;
            ListBox weaponSelect;
            GameForm gameplayForm = InitialiseGameForm(out angle, out power, out fire, out controlPanel, out weaponSelect);

            if (gameplayForm == null) return false;

            int testPower = 71;

            gameplayForm.SetPower(testPower);
            if (power.Value == testPower) return true;

            else
            {
                SetErrorDescription(string.Format("Attempted to set power to {0} but power is {1}", testPower, power.Value));
                return false;
            }
        }
        private static bool TestGameForm0SetWeaponIndex()
        {
            Requires(TestGameForm0GameForm);
            NumericUpDown angle;
            TrackBar power;
            Button fire;
            Panel controlPanel;
            ListBox weaponSelect;
            GameForm gameplayForm = InitialiseGameForm(out angle, out power, out fire, out controlPanel, out weaponSelect);

            if (gameplayForm == null) return false;

            gameplayForm.SetWeaponIndex(0);

            // WeaponSelect is optional behaviour, so it's okay if it's not implemented here, as long as the method works.
            return true;
        }
        private static bool TestGameForm0Shoot()
        {
            Requires(TestGameForm0GameForm);
            // This is something we can't really test properly without a proper framework, so for now we'll just click
            // the button and make sure it disables the control panel
            NumericUpDown angle;
            TrackBar power;
            Button fire;
            Panel controlPanel;
            ListBox weaponSelect;
            GameForm gameplayForm = InitialiseGameForm(out angle, out power, out fire, out controlPanel, out weaponSelect);

            controlPanel.Enabled = true;
            fire.PerformClick();
            if (controlPanel.Enabled)
            {
                SetErrorDescription("Control panel still enabled immediately after clicking fire button");
                return false;
            }

            return true;
        }
        private static void UnitTests()
        {
            DoTest(TestBattle0Battle);
            DoTest(TestBattle0PlayerCount);
            DoTest(TestBattle0GetTotalRounds);
            DoTest(TestBattle0CreatePlayer);
            DoTest(TestBattle0GetPlayerNumber);
            DoTest(TestBattle0TankColour);
            DoTest(TestBattle0GetPlayerLocations);
            DoTest(TestBattle0Shuffle);
            DoTest(TestBattle0BeginGame);
            DoTest(TestBattle0GetBattlefield);
            DoTest(TestBattle0GetCurrentGameplayTank);
            DoTest(TestChassis0CreateTank);
            DoTest(TestChassis0DisplayTankSprite);
            DoTest(TestChassis0DrawLine);
            DoTest(TestChassis0GetHealth);
            DoTest(TestChassis0GetWeapons);
            DoTest(TestOpponent0PlayerController);
            DoTest(TestOpponent0CreateTank);
            DoTest(TestOpponent0Identifier);
            DoTest(TestOpponent0PlayerColour);
            DoTest(TestOpponent0AddScore);
            DoTest(TestOpponent0GetWins);
            DoTest(TestPlayerController0StartRound);
            DoTest(TestPlayerController0CommenceTurn);
            DoTest(TestPlayerController0ProjectileHit);
            DoTest(TestGameplayTank0GameplayTank);
            DoTest(TestGameplayTank0GetPlayerNumber);
            DoTest(TestGameplayTank0CreateTank);
            DoTest(TestGameplayTank0GetPlayerAngle);
            DoTest(TestGameplayTank0SetAngle);
            DoTest(TestGameplayTank0GetPowerLevel);
            DoTest(TestGameplayTank0SetPower);
            DoTest(TestGameplayTank0GetWeapon);
            DoTest(TestGameplayTank0SetWeaponIndex);
            DoTest(TestGameplayTank0Paint);
            DoTest(TestGameplayTank0GetX);
            DoTest(TestGameplayTank0Y);
            DoTest(TestGameplayTank0Shoot);
            DoTest(TestGameplayTank0Damage);
            DoTest(TestGameplayTank0Alive);
            DoTest(TestGameplayTank0GravityStep);
            DoTest(TestMap0Map);
            DoTest(TestMap0TerrainAt);
            DoTest(TestMap0TankCollisionAt);
            DoTest(TestMap0TankVerticalPosition);
            DoTest(TestMap0DestroyTiles);
            DoTest(TestMap0GravityStep);
            DoTest(TestEffect0ConnectGame);
            DoTest(TestProjectile0Projectile);
            DoTest(TestProjectile0Step);
            DoTest(TestProjectile0Paint);
            DoTest(TestExplosion0Explosion);
            DoTest(TestExplosion0Activate);
            DoTest(TestExplosion0Step);
            DoTest(TestExplosion0Paint);
            DoTest(TestGameForm0GameForm);
            DoTest(TestGameForm0EnableTankButtons);
            DoTest(TestGameForm0SetAngle);
            DoTest(TestGameForm0SetPower);
            DoTest(TestGameForm0SetWeaponIndex);
            DoTest(TestGameForm0Shoot);
        }
        
        #endregion
        
        #region CheckClasses

        private static bool CheckClasses()
        {
            string[] classNames = new string[] { "Program", "ComputerOpponent", "Map", "Explosion", "GameForm", "Battle", "PlayerController", "Projectile", "Opponent", "GameplayTank", "Chassis", "Effect" };
            string[][] classFields = new string[][] {
                new string[] { "Main" }, // Program
                new string[] { }, // ComputerOpponent
                new string[] { "TerrainAt","TankCollisionAt","TankVerticalPosition","DestroyTiles","GravityStep","WIDTH","HEIGHT"}, // Map
                new string[] { "Activate" }, // Explosion
                new string[] { "EnableTankButtons","SetAngle","SetPower","SetWeaponIndex","Shoot","InitRenderBuffer"}, // GameForm
                new string[] { "PlayerCount","GetRound","GetTotalRounds","CreatePlayer","GetPlayerNumber","PlayerTank","TankColour","GetPlayerLocations","Shuffle","BeginGame","StartRound","GetBattlefield","DrawTanks","GetCurrentGameplayTank","AddEffect","WeaponEffectStep","DrawWeaponEffects","RemoveWeaponEffect","DetectCollision","Damage","GravityStep","TurnOver","RewardWinner","NextRound","WindSpeed"}, // Battle
                new string[] { }, // PlayerController
                new string[] { }, // Projectile
                new string[] { "CreateTank","Identifier","PlayerColour","AddScore","GetWins","StartRound","CommenceTurn","ProjectileHit"}, // Opponent
                new string[] { "GetPlayerNumber","CreateTank","GetPlayerAngle","SetAngle","GetPowerLevel","SetPower","GetWeapon","SetWeaponIndex","Paint","GetX","Y","Shoot","Damage","Alive","GravityStep"}, // GameplayTank
                new string[] { "DisplayTankSprite","DrawLine","CreateTankBMP","GetHealth","GetWeapons","FireWeapon","CreateTank","WIDTH","HEIGHT","NUM_TANKS"}, // Chassis
                new string[] { "ConnectGame","Step","Paint"} // Effect
            };

            Assembly assembly = Assembly.GetExecutingAssembly();

            Console.WriteLine("Checking classes for public methods...");
            foreach (Type type in assembly.GetTypes())
            {
                if (type.IsPublic)
                {
                    if (type.Namespace != "TankBattle")
                    {
                        Console.WriteLine("Public type {0} is not in the TankBattle namespace.", type.FullName);
                        return false;
                    }
                    else
                    {
                        int typeIdx = -1;
                        for (int i = 0; i < classNames.Length; i++)
                        {
                            if (type.Name == classNames[i])
                            {
                                typeIdx = i;
                                classNames[typeIdx] = null;
                                break;
                            }
                        }
                        foreach (MemberInfo memberInfo in type.GetMembers())
                        {
                            string memberName = memberInfo.Name;
                            bool isInherited = false;
                            foreach (MemberInfo parentMemberInfo in type.BaseType.GetMembers())
                            {
                                if (memberInfo.Name == parentMemberInfo.Name)
                                {
                                    isInherited = true;
                                    break;
                                }
                            }
                            if (!isInherited)
                            {
                                if (typeIdx != -1)
                                {
                                    bool fieldFound = false;
                                    if (memberName[0] != '.')
                                    {
                                        foreach (string allowedFields in classFields[typeIdx])
                                        {
                                            if (memberName == allowedFields)
                                            {
                                                fieldFound = true;
                                                break;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        fieldFound = true;
                                    }
                                    if (!fieldFound)
                                    {
                                        Console.WriteLine("The public field \"{0}\" is not one of the authorised fields for the {1} class.\n", memberName, type.Name);
                                        Console.WriteLine("Remove it or change its access level.");
                                        return false;
                                    }
                                }
                            }
                        }
                    }

                    //Console.WriteLine("{0} passed.", type.FullName);
                }
            }
            for (int i = 0; i < classNames.Length; i++)
            {
                if (classNames[i] != null)
                {
                    Console.WriteLine("The class \"{0}\" is missing.", classNames[i]);
                    return false;
                }
            }
            Console.WriteLine("All public methods okay.");
            return true;
        }
        
        #endregion

        public static void Main()
        {
            if (CheckClasses())
            {
                UnitTests();

                int passed = 0;
                int failed = 0;
                foreach (string key in unitTestResults.Keys)
                {
                    if (unitTestResults[key] == "Passed")
                    {
                        passed++;
                    }
                    else
                    {
                        failed++;
                    }
                }

                Console.WriteLine("\n{0}/{1} unit tests passed", passed, passed + failed);
                if (failed == 0)
                {
                    Console.WriteLine("Starting up TankBattle...");
                    Program.Main();
                    return;
                }
            }
            
            Console.WriteLine("\nPress enter to exit.");
            Console.ReadLine();
        }
    }
}
