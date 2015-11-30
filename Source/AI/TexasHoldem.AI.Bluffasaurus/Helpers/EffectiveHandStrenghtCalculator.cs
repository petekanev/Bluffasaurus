namespace TexasHoldem.AI.Bluffasaurus.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    using Logic.Cards;

    public class EffectiveHandStrenghtCalculator
    {
        public static double CalculateEHS(IList<Card> hand, IReadOnlyCollection<Card> board)
        {
            int ahead = 0;
            int tied = 0;
            int behind = 0;

            var deck = new FalseDeck(hand.Concat(board)).AllCards;

            if (board.Count >= 3)
            {
                var allCardVariations = VariationsGenerator.GetVariations(7 - board.Count, deck);
                foreach (var variation in allCardVariations)
                {
                    var communityCards = new List<Card>();
                    var otherPlayerHand = new List<Card>();

                    foreach (var card in board)
                    {
                        communityCards.Add(card);
                    }

                    for (int i = 0; i < variation.Length; i++)
                    {
                        if (i <= 1)
                        {
                            otherPlayerHand.Add(variation[i]);
                        }
                        else
                        {
                            communityCards.Add(variation[i]);
                        }
                    }

                    var betterHand = Logic.Helpers.Helpers.CompareCards(
                    hand.Concat(communityCards),
                    otherPlayerHand.Concat(communityCards));
                    if (betterHand > 0)
                    {
                        ahead++;
                    }
                    else if (betterHand < 0)
                    {
                        behind++;
                    }
                    else
                    {
                        tied++;
                    }
                }

                return (double) ahead / (ahead + tied + behind);
            }

            return 1;
        }
    }
}
