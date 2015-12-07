namespace TexasHoldem.AI.Bluffasaurus.Helpers
{
    using System.Collections.Generic;

    using Logic.Cards;

    public class FalseDeck
    {
        private IList<Card> allCards = new List<Card>();

        private static readonly IEnumerable<CardType> AllCardTypes = new List<CardType>
                                                                         {
                                                                             CardType.Two,
                                                                             CardType.Three,
                                                                             CardType.Four,
                                                                             CardType.Five,
                                                                             CardType.Six,
                                                                             CardType.Seven,
                                                                             CardType.Eight,
                                                                             CardType.Nine,
                                                                             CardType.Ten,
                                                                             CardType.Jack,
                                                                             CardType.Queen,
                                                                             CardType.King,
                                                                             CardType.Ace
                                                                         };

        private static readonly IEnumerable<CardSuit> AllCardSuits = new List<CardSuit>
                                                                         {
                                                                             CardSuit.Club,
                                                                             CardSuit.Diamond,
                                                                             CardSuit.Heart,
                                                                             CardSuit.Spade
                                                                         };

        public FalseDeck(IEnumerable<Card> cardsToBeExcluded)
        {
            foreach (var cardSuit in AllCardSuits)
            {
                foreach (var cardType in AllCardTypes)
                {
                    var hasToBeExcluded = false;

                    foreach (var card in cardsToBeExcluded)
                    {
                        if (cardSuit == card.Suit && cardType == card.Type)
                        {
                            hasToBeExcluded = true;
                            break;
                        }
                    }

                    if (!hasToBeExcluded)
                    {
                        this.AllCards.Add(new Card(cardSuit, cardType));
                    }
                }
            }
        }

        public IList<Card> AllCards
        {
            get
            {
                return this.allCards;
            }

            private set
            {
                this.allCards = value;
            }
        }
    }
}
