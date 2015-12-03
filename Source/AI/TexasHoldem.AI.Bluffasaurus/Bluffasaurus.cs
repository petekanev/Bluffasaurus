namespace TexasHoldem.AI.Bluffasaurus
{
    using System;
    using System.Collections.Generic;

    using Helpers;
    using Logic;
    using Logic.Cards;
    using Logic.Players;

    public class Bluffasaurus : BasePlayer
    {
        public override string Name { get; } = "Bluffasaurus" + Guid.NewGuid();

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            if (context.RoundType == GameRoundType.PreFlop)
            {
                var handValue = HandStrengthValuationBluffasaurus.PreFlop(this.FirstCard, this.SecondCard);

                // if we are first to act on a small blind
                if (context.MoneyToCall == context.SmallBlind && context.CurrentPot == context.SmallBlind * 3)
                {
                    if (handValue >= 64)
                    {
                        return PlayerAction.Raise(context.SmallBlind * 10);
                    }
                    else if (handValue >= 55)
                    {
                        return PlayerAction.Raise(context.SmallBlind * 7);
                    }
                    else if (handValue >= 43) // that makes around 74% of all possible hands
                    {
                        // can be further optimized
                        if (context.SmallBlind > context.MoneyLeft / 50)
                        {
                            return PlayerAction.CheckOrCall();
                        }

                        return PlayerAction.Raise(context.SmallBlind * 5);
                    }
                    else if (handValue > 40 && context.SmallBlind < context.MoneyLeft / 40)
                    {
                        return PlayerAction.CheckOrCall();
                    }
                    else
                    {
                        return PlayerAction.Fold();
                    }

                }
                else  // we are on big blind or opp has raised
                {
                    // opponent has not raised
                    if (context.MoneyToCall == 0)
                    {
                        if (handValue >= 64) // cards like AA, KK, AKs
                        {
                            return PlayerAction.Raise(context.SmallBlind * 12);
                        }
                        else if (handValue >= 60)
                        {
                            return PlayerAction.Raise(context.SmallBlind * 6);
                        }
                        else if (handValue >= 43) // that makes around 74% of all possible hands
                        {
                            // can be further optimized
                            if (context.SmallBlind > context.MoneyLeft / 50)
                            {
                                return PlayerAction.CheckOrCall();
                            }

                            return PlayerAction.Raise(context.SmallBlind * 2);
                        }
                        else
                        {
                            return PlayerAction.CheckOrCall();
                        }
                    }
                    else // opponent has raised
                    {
                        // if opp has raised a lot(has a very strong hand)
                        if (context.MoneyToCall > context.SmallBlind * 8 && context.MoneyToCall > 40)
                        {
                            if (handValue >= 64) // cards like AA, KK, AKs
                            {
                                return PlayerAction.Raise(context.SmallBlind * 8);
                            }
                            else if (handValue >= 60)
                            {
                                // we have some more money and want to wait for a better shot
                                if (context.MoneyToCall > context.MoneyLeft / 4 && context.MoneyToCall > context.SmallBlind * 6)
                                {
                                    return PlayerAction.Fold();
                                }
                                else
                                {
                                    return PlayerAction.CheckOrCall();
                                }
                            }
                            else
                            {
                                return PlayerAction.Fold();
                            }
                        }
                        else // opponent has not raised a lot
                        {
                            if (handValue >= 64) // cards like AA, KK, AKs
                            {
                                return PlayerAction.Raise(context.SmallBlind * 12);
                            }
                            else if (handValue >= 60)
                            {
                                // if we have already raised enough this round
                                if (context.MyMoneyInTheRound > context.SmallBlind * 10)
                                {
                                    return PlayerAction.CheckOrCall();
                                }
                                else
                                {
                                    return PlayerAction.Raise(context.SmallBlind * 6);
                                }
                            }
                            else if (handValue >= 55)
                            {
                                return PlayerAction.CheckOrCall();
                            }
                            else if (handValue >= 50 && context.MoneyToCall < 10)
                            {
                                return PlayerAction.CheckOrCall();
                            }
                            else
                            {
                                return PlayerAction.Fold();
                            }
                        }
                    }
                }

                //if (playHand == CardValuationTypeForBluffasaurus.group1)
                //{
                //    if (context.MoneyToCall < context.CurrentPot / 2 || context.MoneyToCall < context.MoneyLeft / 100)
                //    {
                //        return PlayerAction.CheckOrCall();
                //    }
                //    else
                //    {
                //        return PlayerAction.Fold();
                //    }
                //}

                //if (playHand == CardValuationTypeForBluffasaurus.group2)
                //{
                //    if (context.MoneyToCall < context.CurrentPot / 2 || context.MoneyToCall < context.MoneyLeft / 100)
                //    {
                //        return PlayerAction.CheckOrCall();
                //    }
                //    else
                //    {
                //        return PlayerAction.Fold();
                //    }
                //}

                //if (playHand == CardValuationTypeForBluffasaurus.group3)
                //{
                //    if (context.MoneyToCall < context.CurrentPot / 2 || context.MoneyToCall < context.MoneyLeft / 100)
                //    {
                //        return PlayerAction.CheckOrCall();
                //    }
                //    else
                //    {
                //        return PlayerAction.Fold();
                //    }
                //}

                //if (playHand == CardValuationTypeForBluffasaurus.group4)
                //{
                //    if (context.MoneyToCall < context.CurrentPot / 2)
                //    {
                //        return PlayerAction.CheckOrCall();
                //    }
                //    else
                //    {
                //        return PlayerAction.Fold();
                //    }
                //}

                //if (playHand == CardValuationTypeForBluffasaurus.group5)
                //{
                //    if (context.MoneyToCall <= context.MyMoneyInTheRound || context.MoneyToCall < context.MoneyLeft / 25)
                //    {
                //        return PlayerAction.CheckOrCall();
                //    }
                //    else
                //    {
                //        return PlayerAction.Fold();
                //    }
                //}

                //if (playHand == CardValuationTypeForBluffasaurus.group6)
                //{
                //    if (context.MoneyToCall < context.MoneyLeft / 7)
                //    {
                //        if (context.MoneyToCall + context.CurrentPot == context.SmallBlind * 4)
                //        {
                //            return PlayerAction.Raise(4 * context.SmallBlind);
                //        }

                //        return PlayerAction.CheckOrCall();
                //    }
                //    else
                //    {
                //        return PlayerAction.Fold();
                //    }
                //}

                //if (playHand == CardValuationTypeForBluffasaurus.group7)
                //{
                //    if (context.MoneyToCall < context.MoneyLeft / 5 || context.MoneyLeft < 150 || context.SmallBlind * 6 > context.MoneyToCall)
                //    {
                //        if (context.MoneyToCall + context.CurrentPot == context.SmallBlind * 4)
                //        {
                //            return PlayerAction.Raise(6 * context.SmallBlind);
                //        }

                //        return PlayerAction.CheckOrCall();
                //    }
                //    else
                //    {
                //        return PlayerAction.Fold();
                //    }
                //}

                //if (playHand == CardValuationTypeForBluffasaurus.group8)
                //{
                //    if (context.MoneyToCall < context.MoneyLeft / 4 || context.MoneyLeft < 250 || context.SmallBlind * 8 > context.MoneyToCall)
                //    {
                //        if (context.MoneyToCall + context.CurrentPot == context.SmallBlind * 4)
                //        {
                //            return PlayerAction.Raise(8 * context.SmallBlind);
                //        }

                //        return PlayerAction.CheckOrCall();
                //    }
                //    else
                //    {
                //        return PlayerAction.Fold();
                //    }
                //}

                //if (playHand == CardValuationTypeForBluffasaurus.group9)
                //{
                //    if (context.MoneyToCall < context.MoneyLeft / 3 || context.MoneyLeft < 250 || context.SmallBlind * 8 > context.MoneyToCall)
                //    {
                //        if (context.MoneyToCall + context.CurrentPot == context.SmallBlind * 4)
                //        {
                //            return PlayerAction.Raise(10 * context.SmallBlind);
                //        }

                //        return PlayerAction.CheckOrCall();
                //    }
                //    else
                //    {
                //        return PlayerAction.Fold();
                //    }
                //}

                //return PlayerAction.CheckOrCall();
            }
            else
            {
                var hand = new List<Card>();
                hand.Add(this.FirstCard);
                hand.Add(this.SecondCard);

                var ehs = EffectiveHandStrenghtCalculator.CalculateEHS(hand, this.CommunityCards);

                if (ehs < 0.3)
                {
                    if (context.MoneyToCall < context.MoneyLeft / 500)
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
                    if (context.MoneyToCall < context.MoneyLeft / 100)
                    {
                        return PlayerAction.CheckOrCall();
                    }
                    else
                    {
                        return PlayerAction.Fold();
                    }
                }
                else if (ehs < 0.75)
                {
                    if (context.MoneyToCall == 0)
                    {
                        var currentPot = context.CurrentPot;
                        int moneyToBet = (int)(currentPot * 0.85);
                        return PlayerAction.Raise(moneyToBet);
                    }
                    else if (context.MoneyToCall < context.MoneyLeft / 40 || context.MoneyToCall < 30)
                    {
                        if (context.MoneyToCall < context.CurrentPot * 0.85 && context.MyMoneyInTheRound == 0)
                        {
                            return PlayerAction.Raise((int)(context.CurrentPot * 0.85) - context.MoneyToCall + 1);
                        }
                        else
                        {
                            return PlayerAction.CheckOrCall();
                        }
                    }
                    else
                    {
                        return PlayerAction.Fold();
                    }
                }
                else if (ehs < 0.9)
                {
                    if (context.MoneyToCall == 0)
                    {
                        var currentPot = context.CurrentPot;
                        int moneyToBet = (int)(currentPot * 0.9);
                        if (moneyToBet < 20)
                        {
                            moneyToBet = 20;
                        }
                        return PlayerAction.Raise(moneyToBet);
                    }
                    else if (context.MoneyToCall < context.MoneyLeft / 7 || context.MoneyToCall < 150)
                    {
                        if (context.MoneyToCall < context.CurrentPot * 0.9 && context.MyMoneyInTheRound == 0)
                        {
                            return PlayerAction.Raise((int)(context.CurrentPot * 0.9) - context.MoneyToCall + 1);
                        }
                        else
                        {
                            return PlayerAction.CheckOrCall();
                        }
                    }
                    else
                    {
                        return PlayerAction.Fold();
                    }
                }
                else
                {
                    var currentPot = context.CurrentPot;
                    int moneyToBet = currentPot;
                    if (moneyToBet < 20)
                    {
                        moneyToBet = 20;
                    }
                    return PlayerAction.Raise(moneyToBet);
                }
            }
        }
    }
}
