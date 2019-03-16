using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TankBattle
{
    public class ComputerOpponent : Opponent
    {
        private Chassis compOppTankType;
        private Color compOppTankColour;
        private Battle match;
        GameForm form;

        private int[] positions;
        private string compOppName;
        

        public ComputerOpponent(string name, Chassis tank, Color colour) : base(name, tank, colour)
        {
            compOppName = name;
            compOppTankType = tank;
            compOppTankColour = colour;
        }

        public override void StartRound()
        {
            positions = Battle.GetPlayerLocations(currentGame.PlayerCount());
        }

        public override void CommenceTurn(GameForm gameplayForm, Battle currentGame)
        {
            form = gameplayForm;
            match = currentGame;
        }

        public override void ProjectileHit(float x, float y)
        {
            throw new NotImplementedException();
        }
    }
}
