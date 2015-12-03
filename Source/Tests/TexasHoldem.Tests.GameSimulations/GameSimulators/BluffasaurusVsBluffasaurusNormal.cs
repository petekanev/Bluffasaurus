namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using Logic.Players;

    public class BluffasaurusVsBluffasaurusNormal : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new AI.DummyPlayer.AlwaysRaiseDummyPlayer();
        private readonly IPlayer secondPlayer = new AI.SmartPlayer.BluffasaurusNormal();

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
