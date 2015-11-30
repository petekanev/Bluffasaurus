namespace TexasHoldem.AI.Bluffasaurus.Helpers
{
    using Logic.Cards;
    using System.Collections.Generic;

    public class VariationsGenerator
    {
        private static IList<Card[]> cardVariations;

        public static IList<Card[]> GetVariations(int numberOfCardsToVariate, IList<Card> setOfCards)
        {
            cardVariations = new List<Card[]>();
            Generate(numberOfCardsToVariate, setOfCards);
            return cardVariations;
        }

        private static void Generate(int k, IList<Card> set, Card[] variation = null, bool[] used = null)
        {
            if (variation == null)
            {
                variation = new Card[k];
            }

            if (used == null)
            {
                used = new bool[set.Count];
            }

            if (k == 0)
            {
                cardVariations.Add(variation.Clone() as Card[]);
                return;
            }

            for (int i = 0; i < set.Count; i++)
            {
                if (!used[i])
                {
                    variation[k - 1] = set[i];
                    used[i] = true;
                    Generate(k - 1, set, variation, used);
                    used[i] = false;
                }
            }
        }
    }
}
