namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using AI.SmartPlayer;
    using TexasHoldem.Logic.Players;

    public class SmartVsSmarterPlayerSimulator : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new SmarterPlayer();
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
