namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using Logic.Players;

    public class BluffasaurusVsAlwaysRaisePlayerSimulator : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new AI.Bluffasaurus.Bluffasaurus();
        private readonly IPlayer secondPlayer = new AI.DummyPlayer.ConsolePlayer();

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
