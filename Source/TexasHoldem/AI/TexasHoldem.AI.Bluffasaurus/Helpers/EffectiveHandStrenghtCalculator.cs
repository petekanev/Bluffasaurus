namespace TexasHoldem.AI.Bluffasaurus.Helpers
{
    using System.Collections.Generic;
    using System.Linq;

    using Logic.Cards;
    using System;

    public class EffectiveHandStrenghtCalculator
    {
        private static int ahead;
        private static int tied;
        private static int behind;
        private static IList<Card> playerHand;
        private static IList<Card> cardsOnTable;
        private static IList<Card> lastHandAndCardsOnTable;
        private static double lastEHS;
        private static Random rand = new Random();

        public static double CalculateEHS(IList<Card> hand, IReadOnlyCollection<Card> board)
        {
            ahead = 0;
            tied = 0;
            behind = 0;
            playerHand = hand;
            cardsOnTable = board.ToList();

            var deck = new FalseDeck(playerHand.Concat(cardsOnTable)).AllCards;
            var handAndCardsOnTable = playerHand.Concat(cardsOnTable).ToList();

            // MEMOIZATION
            if (lastHandAndCardsOnTable == null)
            {
                lastHandAndCardsOnTable = new List<Card>();
            }

            // check if we memoised the cards to not calculate them again
            var isMemoised = false;
            if (lastHandAndCardsOnTable.Count() == handAndCardsOnTable.Count)
            {
                isMemoised = true;
                for (int i = 0; i < lastHandAndCardsOnTable.Count(); i++)
                {
                    if (handAndCardsOnTable[i].Suit != lastHandAndCardsOnTable[i].Suit || handAndCardsOnTable[i].Type != lastHandAndCardsOnTable[i].Type)
                    {
                        isMemoised = false;
                        break;
                    }
                }
            }

            if (!isMemoised)
            {
                GenerateCombinations(7 - cardsOnTable.Count, 0, deck);
                lastHandAndCardsOnTable = handAndCardsOnTable;
                lastEHS = (double)((ahead + (tied / 2)) / (double)(ahead + tied + behind));
            }

            return lastEHS;
        }

        private static void DetermineWhoWinsHand(IList<Card> hand, IList<Card> board, Card[] variation)
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

        private static void GenerateCombinations(int k, int startingPosition, IList<Card> set, Card[] combination = null)
        {
            if (combination == null)
            {
                combination = new Card[k];
            }

            if (k == 0)
            {
                GenerateOpponentHand(2, 0, combination);
                return;
            }

            for (int i = startingPosition; i < set.Count; i++)
            {
                combination[k - 1] = set[i];
                GenerateCombinations(k - 1, i + 1, set, combination);
            }
        }

        private static void GenerateOpponentHand(int k, int startingPosition, IList<Card> set, Card[] combination = null)
        {
            if (combination == null)
            {
                combination = new Card[k];
            }

            if (k == 0)
            {
                // MONTE CARLO -> we calculate only around 2500 cases
                switch (set.Count)
                {
                    case 3:
                        if (rand.Next(0, int.MaxValue) % 22 == 0)
                        {
                            var index1 = set.IndexOf(combination[0]);
                            Swap(0, index1, set.ToArray());
                            var index2 = set.IndexOf(combination[1]);
                            Swap(1, index2, set.ToArray());
                            DetermineWhoWinsHand(playerHand, cardsOnTable, set.ToArray());
                        }

                        break;
                    case 4:
                        if (rand.Next(0, int.MaxValue) % 420 == 0)
                        {
                            var index1 = set.IndexOf(combination[0]);
                            Swap(0, index1, set.ToArray());
                            var index2 = set.IndexOf(combination[1]);
                            Swap(1, index2, set.ToArray());
                            DetermineWhoWinsHand(playerHand, cardsOnTable, set.ToArray());
                        }

                        break;
                    default:
                        {
                            var index1 = set.IndexOf(combination[0]);
                            Swap(0, index1, set.ToArray());
                            var index2 = set.IndexOf(combination[1]);
                            Swap(1, index2, set.ToArray());
                            DetermineWhoWinsHand(playerHand, cardsOnTable, set.ToArray());
                            break;
                        }
                }

                return;
            }

            for (int i = startingPosition; i < set.Count; i++)
            {
                combination[k - 1] = set[i];
                GenerateOpponentHand(k - 1, i + 1, set, combination);
            }
        }

        private static void Swap(int start, int end, Card[] array)
        {
            var buffer = array[end];
            array[end] = array[start];
            array[start] = buffer;
        }
    }
}
