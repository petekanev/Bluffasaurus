namespace TexasHoldem.Tests.GameSimulations.GameSimulators
{
    using Logic.Players;
    using AI.Bluffasaurus;

    public class BluffasaurusVsBluffasaurusNormal : BaseGameSimulator
    {
        private readonly IPlayer firstPlayer = new Bluffasaurus();
        private readonly IPlayer secondPlayer = new BluffasaurusLame();

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
