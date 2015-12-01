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

    public class Bluffasaurus : BasePlayer
    {
        public override string Name { get; } = "Bluffasaurus" + Guid.NewGuid();

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            if (context.RoundType == GameRoundType.PreFlop)
            {
                var playHand = HandStrengthValuationSmarterBot.PreFlop(this.FirstCard, this.SecondCard);
                if (playHand == CardValuationTypeForSmarterBot.group1)
                {
                    if (context.MoneyToCall < context.CurrentPot / 2 || context.MoneyToCall < context.MoneyLeft / 100)
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
                    if (context.MoneyToCall < context.CurrentPot / 2 || context.MoneyToCall < context.MoneyLeft / 100)
                    {
                        return PlayerAction.CheckOrCall();
                    }
                    else
                    {
                        return PlayerAction.Fold();
                    }
                }

                if (playHand == CardValuationTypeForSmarterBot.group3)
                {
                    if (context.MoneyToCall < context.CurrentPot / 2 || context.MoneyToCall < context.MoneyLeft / 100)
                    {
                        return PlayerAction.CheckOrCall();
                    }
                    else
                    {
                        return PlayerAction.Fold();
                    }
                }

                if (playHand == CardValuationTypeForSmarterBot.group4)
                {
                    if (context.MoneyToCall < context.CurrentPot / 2)
                    {
                        return PlayerAction.CheckOrCall();
                    }
                    else
                    {
                        return PlayerAction.Fold();
                    }
                }

                if (playHand == CardValuationTypeForSmarterBot.group5)
                {
                    if (context.MoneyToCall <= context.MyMoneyInTheRound || context.MoneyToCall < context.MoneyLeft / 25)
                    {
                        return PlayerAction.CheckOrCall();
                    }
                    else
                    {
                        return PlayerAction.Fold();
                    }
                }

                if (playHand == CardValuationTypeForSmarterBot.group6)
                {
                    if (context.MoneyToCall < context.MoneyLeft / 7)
                    {
                        if (context.MoneyToCall + context.CurrentPot == context.SmallBlind * 4)
                        {
                            return PlayerAction.Raise(4 * context.SmallBlind);
                        }

                        return PlayerAction.CheckOrCall();
                    }
                    else
                    {
                        return PlayerAction.Fold();
                    }
                }

                if (playHand == CardValuationTypeForSmarterBot.group7)
                {
                    if (context.MoneyToCall < context.MoneyLeft / 5 || context.MoneyLeft < 150 || context.SmallBlind * 6 > context.MoneyToCall)
                    {
                        if (context.MoneyToCall + context.CurrentPot == context.SmallBlind * 4)
                        {
                            return PlayerAction.Raise(6 * context.SmallBlind);
                        }

                        return PlayerAction.CheckOrCall();
                    }
                    else
                    {
                        return PlayerAction.Fold();
                    }
                }

                if (playHand == CardValuationTypeForSmarterBot.group8)
                {
                    if (context.MoneyToCall < context.MoneyLeft / 4 || context.MoneyLeft < 250 || context.SmallBlind * 8 > context.MoneyToCall)
                    {
                        if (context.MoneyToCall + context.CurrentPot == context.SmallBlind * 4)
                        {
                            return PlayerAction.Raise(8 * context.SmallBlind);
                        }

                        return PlayerAction.CheckOrCall();
                    }
                    else
                    {
                        return PlayerAction.Fold();
                    }
                }

                if (playHand == CardValuationTypeForSmarterBot.group9)
                {
                    if (context.MoneyToCall < context.MoneyLeft / 3 || context.MoneyLeft < 250 || context.SmallBlind * 8 > context.MoneyToCall)
                    {
                        if (context.MoneyToCall + context.CurrentPot == context.SmallBlind * 4)
                        {
                            return PlayerAction.Raise(10 * context.SmallBlind);
                        }

                        return PlayerAction.CheckOrCall();
                    }
                    else
                    {
                        return PlayerAction.Fold();
                    }
                }

                return PlayerAction.CheckOrCall();
            }
            //else if (context.RoundType == GameRoundType.Flop)
            //{
            //    var flopCardStrength = CardsStrengthEvaluation.RateCards
            //        (new List<Card> { FirstCard, SecondCard, CommunityCards.ElementAt(0), CommunityCards.ElementAt(1), CommunityCards.ElementAt(2) });

            //    if (flopCardStrength >= 6000)
            //    {
            //        var smallBlindsTimes = RandomProvider.Next(8, 16);
            //        return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            //    }

            //    if (flopCardStrength >= 4000)
            //    {
            //        var smallBlindsTimes = RandomProvider.Next(4, 8);
            //        return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            //    }

            //    if (flopCardStrength >= 1000)
            //    {
            //        var smallBlindsTimes = RandomProvider.Next(2, 4);
            //        return PlayerAction.Raise(context.SmallBlind * smallBlindsTimes);
            //    }

            //    return PlayerAction.CheckOrCall();
            //}
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
                    // var smallBlindsTimes = RandomProvider.Next(8, 16);
                    var currentPot = context.CurrentPot;
                    int moneyToBet = (int)(currentPot * 0.85);
                    return PlayerAction.Raise(moneyToBet);
                }
                else if (ehs < 0.9)
                {
                    // var smallBlindsTimes = RandomProvider.Next(16, 32);
                    var currentPot = context.CurrentPot;
                    int moneyToBet = (int)(currentPot * 0.9);
                    return PlayerAction.Raise(moneyToBet);
                }
                else
                {
                    // var smallBlindsTimes = RandomProvider.Next(32, 64);
                    var currentPot = context.CurrentPot;
                    int moneyToBet = currentPot;
                    return PlayerAction.Raise(moneyToBet);
                }
            }
        }
    }
}
