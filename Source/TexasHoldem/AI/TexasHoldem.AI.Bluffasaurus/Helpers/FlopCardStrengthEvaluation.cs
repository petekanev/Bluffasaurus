namespace TexasHoldem.AI.Bluffasaurus.Helpers
{
    using System.Collections.Generic;

    using Logic.Cards;
    using Logic.Helpers;

    public class CardsStrengthEvaluation
    {
        public static int RateCards(IEnumerable<Card> cards)
        {
            var evaluator = new HandEvaluator();
            var strength = (int)evaluator.GetBestHand(cards).RankType;

            return strength;
        }
    }
}
