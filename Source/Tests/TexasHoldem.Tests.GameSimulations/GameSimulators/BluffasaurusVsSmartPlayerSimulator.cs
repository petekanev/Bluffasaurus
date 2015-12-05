using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using AI.SmartPlayer;
    using Logic.Players;
    using AI.DummyPlayer;

    public class BluffasaurusVsSmartPlayerSimulator : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new AI.SmartPlayer.BluffasaurusNormal();
        private readonly IPlayer secondPlayer = new SmartPlayer();

        protected override IPlayer GetFirstPlayer()
        {
            return this.firstPlayer;
        }

        protected override IPlayer GetSecondPlayer()
        {
            return this.secondPlayer;
        }
    }
}
