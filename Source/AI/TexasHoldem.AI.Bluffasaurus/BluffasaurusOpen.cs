namespace TexasHoldem.AI.Bluffasaurus
{
    using System;
    using System.Collections.Generic;

    using Helpers;
    using Logic;
    using Logic.Cards;
    using Logic.Players;

    public class BluffasaurusOpen : BasePlayer
    {
        public override string Name { get; } = "Bluffasaurus" + Guid.NewGuid();

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            if (context.RoundType == GameRoundType.PreFlop)
            {
                var handValue = HandStrengthValuationBluffasaurus.PreFlop(this.FirstCard, this.SecondCard);
                var opponentAgression = 2;

                var extreme = 64 - opponentAgression;
                var powerful = 60 - opponentAgression;
                var normal = 55 - opponentAgression;
                var weak = 50 - (opponentAgression * 2);
                var awful = 43 - (opponentAgression * 2);
                var lowerLimit = 40 - opponentAgression;

                // if we are first to act on a small blind
                if (context.MoneyToCall == context.SmallBlind && context.CurrentPot == context.SmallBlind * 3)
                {
                    if (handValue >= extreme)
                    {
                        return PlayerAction.Raise(context.SmallBlind * 10);
                    }
                    else if (handValue >= normal)
                    {
                        return PlayerAction.Raise(context.SmallBlind * 8);
                    }
                    else if (handValue >= awful) // that makes around 74% of all possible hands
                    {
                        // can be further optimized
                        if (context.SmallBlind > context.MoneyLeft / 50)
                        {
                            return PlayerAction.CheckOrCall();
                        }

                        return PlayerAction.Raise(context.SmallBlind * 5);
                    }
                    else if (handValue > lowerLimit && context.SmallBlind < context.MoneyLeft / 40)
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
                        if (handValue >= extreme) // cards like AA, KK, AKs
                        {
                            return PlayerAction.Raise(context.SmallBlind * 10);
                        }
                        else if (handValue >= powerful)
                        {
                            return PlayerAction.Raise(context.SmallBlind * 8);
                        }
                        else if (handValue >= awful) // that makes around 74% of all possible hands
                        {
                            // can be further optimized
                            if (context.SmallBlind > context.MoneyLeft / 50)
                            {
                                return PlayerAction.CheckOrCall();
                            }

                            return PlayerAction.Raise(context.SmallBlind * 4);
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
                            if (handValue >= extreme) // cards like AA, KK, AKs
                            {
                                return PlayerAction.Raise(context.SmallBlind * 8);
                            }
                            else if (handValue >= powerful)
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
                            if (handValue >= extreme) // cards like AA, KK, AKs
                            {
                                return PlayerAction.Raise(context.SmallBlind * 8);
                            }
                            else if (handValue >= powerful)
                            {
                                // if we have already raised enough this round
                                if (context.MyMoneyInTheRound > context.SmallBlind * 10)
                                {
                                    return PlayerAction.CheckOrCall();
                                }
                                else
                                {
                                    return PlayerAction.Raise(context.SmallBlind * 4);
                                }
                            }
                            else if (handValue >= normal)
                            {
                                return PlayerAction.CheckOrCall();
                            }
                            else if (handValue >= weak && (context.MoneyToCall <= 20 || context.MoneyToCall <= context.SmallBlind * 3))
                            {
                                return PlayerAction.CheckOrCall();
                            }
                            else if (handValue >= awful && (context.MoneyToCall <= 20 || context.MoneyToCall <= context.SmallBlind * 2))
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
            }
            else
            {
                //var hand = new List<Card>();
                //hand.Add(this.FirstCard);
                //hand.Add(this.SecondCard);

                //var ehs = EffectiveHandStrenghtCalculator.CalculateEHS(hand, this.CommunityCards);

                //if (ehs < 0.3)
                //{
                //    if (context.MoneyToCall < context.MoneyLeft / 500)
                //    {
                //        return PlayerAction.CheckOrCall();
                //    }
                //    else
                //    {
                //        return PlayerAction.Fold();
                //    }
                //}
                //else if (ehs < 0.5)
                //{
                //    if (context.MoneyToCall < context.MoneyLeft / 100)
                //    {
                //        return PlayerAction.CheckOrCall();
                //    }
                //    else
                //    {
                //        return PlayerAction.Fold();
                //    }
                //}
                //else if (ehs < 0.75)
                //{
                //    if (context.MoneyToCall == 0)
                //    {
                //        var currentPot = context.CurrentPot;
                //        int moneyToBet = (int)(currentPot * 0.85);
                //        return PlayerAction.Raise(moneyToBet);
                //    }
                //    else if (context.MoneyToCall < context.MoneyLeft / 40 || context.MoneyToCall < 30)
                //    {
                //        if (context.MoneyToCall < context.CurrentPot * 0.85 && context.MyMoneyInTheRound == 0)
                //        {
                //            return PlayerAction.Raise((int)(context.CurrentPot * 0.85) - context.MoneyToCall + 1);
                //        }
                //        else
                //        {
                //            return PlayerAction.CheckOrCall();
                //        }
                //    }
                //    else
                //    {
                //        return PlayerAction.Fold();
                //    }
                //}
                //else if (ehs < 0.9)
                //{
                //    if (context.MoneyToCall == 0)
                //    {
                //        var currentPot = context.CurrentPot;
                //        int moneyToBet = (int)(currentPot * 0.9);
                //        if (moneyToBet < 20)
                //        {
                //            moneyToBet = 20;
                //        }
                //        return PlayerAction.Raise(moneyToBet);
                //    }
                //    else if (context.MoneyToCall < context.MoneyLeft / 7 || context.MoneyToCall < 150)
                //    {
                //        if (context.MoneyToCall < context.CurrentPot * 0.9 && context.MyMoneyInTheRound == 0)
                //        {
                //            return PlayerAction.Raise((int)(context.CurrentPot * 0.9) - context.MoneyToCall + 1);
                //        }
                //        else
                //        {
                //            return PlayerAction.CheckOrCall();
                //        }
                //    }
                //    else
                //    {
                //        return PlayerAction.Fold();
                //    }
                //}
                //else
                //{
                //    var currentPot = context.CurrentPot;
                //    int moneyToBet = currentPot;
                //    if (moneyToBet < 20)
                //    {
                //        moneyToBet = 20;
                //    }
                //    return PlayerAction.Raise(moneyToBet);
                //}

                return PlayerAction.CheckOrCall();
            }
        }
    }
}
