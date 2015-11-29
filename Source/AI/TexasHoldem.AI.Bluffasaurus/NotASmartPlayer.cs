namespace TexasHoldem.AI.Bluffasaurus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Helpers;
    using Logic;
    using Logic.Cards;
    using Logic.Extensions;
    using Logic.Players;

    public class NotASmartPlayer : BasePlayer
    {
        public override string Name { get ; } = "SmartPlayer_" + Guid.NewGuid();

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            if (context.RoundType == GameRoundType.PreFlop)
            {
                var playHand = HandStrengthValuationSmarterBot.PreFlop(this.FirstCard, this.SecondCard);
                if (playHand == CardValuationTypeForSmarterBot.group1)
                {
                    if (context.CanCheck)
                    {
                        return PlayerAction.CheckOrCall();
                    }
                    else
                    {
                        return PlayerAction.Fold();
                    }
                }

                if (playHand == CardValuationTypeForSmarterBot.group2)
                {
                    return PlayerAction.CheckOrCall();
                }

                if (playHand == CardValuationTypeForSmarterBot.group3)
                {
                    return PlayerAction.CheckOrCall();
                }

                if (playHand == CardValuationTypeForSmarterBot.group4)
                {
                    return PlayerAction.CheckOrCall();
                }

                if (playHand == CardValuationTypeForSmarterBot.group5)
                {
                    return PlayerAction.CheckOrCall();
                }

                if (playHand == CardValuationTypeForSmarterBot.group6)
                {
                    var smallBlindsTimes = RandomProvider.Next(16, 32);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                if (playHand == CardValuationTypeForSmarterBot.group7)
                {
                    var smallBlindsTimes = RandomProvider.Next(64, 128);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                if (playHand == CardValuationTypeForSmarterBot.group8)
                {
                    var smallBlindsTimes = RandomProvider.Next(128, 256);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                if (playHand == CardValuationTypeForSmarterBot.group9)
                {
                    var smallBlindsTimes = RandomProvider.Next(500, 1000);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                return PlayerAction.CheckOrCall();
            }
            else if (context.RoundType == GameRoundType.Flop)
            {
                var flopCardStrength = CardsStrengthEvaluation.RateCards
                    (new List<Card> { FirstCard, SecondCard, CommunityCards.ElementAt(0), CommunityCards.ElementAt(1), CommunityCards.ElementAt(2) });

                if (flopCardStrength >= 6000)
                {
                    var smallBlindsTimes = RandomProvider.Next(128, 256);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                if (flopCardStrength >= 4000)
                {
                    var smallBlindsTimes = RandomProvider.Next(64, 128);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                if (flopCardStrength >= 1000)
                {
                    var smallBlindsTimes = RandomProvider.Next(16, 32);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                return PlayerAction.CheckOrCall();
            }
            else if (context.RoundType == GameRoundType.Turn)
            {
                var flopCardStrength = CardsStrengthEvaluation.RateCards
                    (new List<Card> { FirstCard, SecondCard, CommunityCards.ElementAt(0), CommunityCards.ElementAt(1), CommunityCards.ElementAt(2), CommunityCards.ElementAt(3) });

                if (flopCardStrength >= 6000)
                {
                    var smallBlindsTimes = RandomProvider.Next(128, 256);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                if (flopCardStrength >= 4000)
                {
                    var smallBlindsTimes = RandomProvider.Next(64, 128);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                return PlayerAction.CheckOrCall();
            }
            else if (context.RoundType == GameRoundType.Flop)
            {
                var flopCardStrength = CardsStrengthEvaluation.RateCards
                    (new List<Card> { FirstCard, SecondCard, CommunityCards.ElementAt(0), CommunityCards.ElementAt(1), CommunityCards.ElementAt(2), CommunityCards.ElementAt(3), CommunityCards.ElementAt(4) });

                if (flopCardStrength >= 6000)
                {
                    var smallBlindsTimes = RandomProvider.Next(128, 256);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                if (flopCardStrength >= 4000)
                {
                    var smallBlindsTimes = RandomProvider.Next(64, 128);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                if (flopCardStrength >= 1000)
                {
                    var smallBlindsTimes = RandomProvider.Next(16, 32);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                return PlayerAction.Fold();
            }

            return PlayerAction.CheckOrCall();
        }
    }
}
