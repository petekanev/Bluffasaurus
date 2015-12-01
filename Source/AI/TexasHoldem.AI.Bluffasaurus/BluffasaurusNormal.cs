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

    public class BluffasaurusNormal : BasePlayer
    {
        public override string Name { get; } = "Bluffasaurus" + Guid.NewGuid();

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
                    var smallBlindsTimes = RandomProvider.Next(2, 4);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                if (playHand == CardValuationTypeForSmarterBot.group7)
                {
                    var smallBlindsTimes = RandomProvider.Next(4, 8);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                if (playHand == CardValuationTypeForSmarterBot.group8)
                {
                    var smallBlindsTimes = RandomProvider.Next(8, 16);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                if (playHand == CardValuationTypeForSmarterBot.group9)
                {
                    var smallBlindsTimes = RandomProvider.Next(16, 32);
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
                    var smallBlindsTimes = RandomProvider.Next(8, 16);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                if (flopCardStrength >= 4000)
                {
                    var smallBlindsTimes = RandomProvider.Next(4, 8);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                if (flopCardStrength >= 1000)
                {
                    var smallBlindsTimes = RandomProvider.Next(2, 4);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }

                return PlayerAction.CheckOrCall();
            }
            else
            {
                var hand = new List<Card>();
                hand.Add(this.FirstCard);
                hand.Add(this.SecondCard);

                var ehs = EffectiveHandStrenghtCalculator.CalculateEHS(hand, this.CommunityCards);

                if (ehs < 0.3)
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
                else if (ehs < 0.5)
                {
                    return PlayerAction.CheckOrCall();
                }
                else if (ehs < 0.7)
                {
                    var smallBlindsTimes = RandomProvider.Next(8, 16);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }
                else if (ehs < 0.9)
                {
                    var smallBlindsTimes = RandomProvider.Next(16, 32);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }
                else
                {
                    var smallBlindsTimes = RandomProvider.Next(32, 64);
                    return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
                }
            }
        }
    }
}
