namespace TexasHoldem.AI.Bluffasaurus
{
    using System;
    using System.Collections.Generic;

    using Helpers;
    using Logic;
    using Logic.Cards;
    using Logic.Players;
    using System.Linq;

    public class Bluffasaurus : BasePlayer
    {
        private static bool inPosition = false;

        public override string Name { get; } = "Bluffasaurus" + Guid.NewGuid();

        private PlayerAction Fold()
        {
            inPosition = false;
            return PlayerAction.Fold();
        }

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            #region Preflop
            if (context.RoundType == GameRoundType.PreFlop)
            {
                var handValue = HandStrengthValuationBluffasaurus.PreFlop(this.FirstCard, this.SecondCard);
                var optimalValueCoeff = 2;

                var extreme = 64 - optimalValueCoeff;
                var powerful = 60 - optimalValueCoeff;
                var normal = 55 - optimalValueCoeff;
                var weak = 50 - (optimalValueCoeff * 2);
                var awful = 43 - (optimalValueCoeff * 2);
                var lowerLimit = 40 - optimalValueCoeff;

                // if we are first to act on a small blind
                if (context.MoneyToCall == context.SmallBlind && context.CurrentPot == context.SmallBlind * 3)
                {
                    inPosition = true;

                    if (handValue >= extreme)
                    {
                        return PlayerAction.Raise(context.SmallBlind * 20);
                    }
                    else if (handValue >= powerful)
                    {
                        return PlayerAction.Raise(context.SmallBlind * 16);
                    }
                    else if (handValue >= normal)
                    {
                        return PlayerAction.Raise(context.SmallBlind * 12);
                    }
                    else if (handValue >= awful) // that makes around 74% of all possible hands
                    {
                        // can be further optimized
                        if (context.SmallBlind > context.MoneyLeft / 50)
                        {
                            return PlayerAction.CheckOrCall();
                        }

                        return PlayerAction.Raise(context.SmallBlind * 10);
                    }
                    else if (handValue > lowerLimit && context.SmallBlind < context.MoneyLeft / 40)
                    {
                        return PlayerAction.CheckOrCall();
                    }
                    else
                    {
                        return this.Fold();
                    }
                }
                else  // we are on big blind or opp has raised
                {
                    // opponent has not raised
                    if (context.MoneyToCall == 0)
                    {
                        if (handValue >= extreme) // cards like AA, KK, AKs
                        {
                            return PlayerAction.Raise(context.SmallBlind * 20);
                        }
                        else if (handValue >= powerful)
                        {
                            return PlayerAction.Raise(context.SmallBlind * 16);
                        }
                        else if (handValue >= awful) // that makes around 74% of all possible hands
                        {
                            // can be further optimized
                            if (context.SmallBlind > context.MoneyLeft / 50)
                            {
                                return PlayerAction.CheckOrCall();
                            }

                            return PlayerAction.Raise(context.SmallBlind * 6);
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
                                return PlayerAction.Raise(context.SmallBlind * 16);
                            }
                            else if (handValue >= powerful)
                            {
                                // we have some more money and want to wait for a better shot
                                if (context.MoneyToCall > context.MoneyLeft / 4 && context.MoneyToCall > context.SmallBlind * 6)
                                {
                                    return this.Fold();
                                }
                                else
                                {
                                    return PlayerAction.CheckOrCall();
                                }
                            }
                            else
                            {
                                return this.Fold();
                            }
                        }
                        else // opponent has not raised a lot
                        {
                            if (handValue >= extreme) // cards like AA, KK, AKs
                            {
                                return PlayerAction.Raise(context.SmallBlind * 20);
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
                                    return PlayerAction.Raise(context.SmallBlind * 12);
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
                                return this.Fold();
                            }
                        }
                    }
                }
            }
            #endregion

            #region Flop
            else if (context.RoundType == GameRoundType.Flop)
            {
                if (context.MoneyLeft == 0)
                {
                    return PlayerAction.CheckOrCall();
                }

                var flopCardStrength = CardsStrengthEvaluation.RateCards
                    (new List<Card> { FirstCard, SecondCard, CommunityCards.ElementAt(0), CommunityCards.ElementAt(1), CommunityCards.ElementAt(2) });

                if (inPosition)
                {
                    if (flopCardStrength >= 2000)
                    {
                        return PlayerAction.Raise(context.SmallBlind * 16);
                    }
                    else if (flopCardStrength >= 1000)
                    {
                        var pairInfo = this.GetPairInfo();

                        if (pairInfo == 0)
                        {
                            return PlayerAction.CheckOrCall();
                        }
                        else
                        {
                            if (pairInfo >= 11)
                            {
                                return PlayerAction.Raise(context.SmallBlind * 12);
                            }
                            else
                            {
                                return PlayerAction.Raise(context.SmallBlind * 8);
                            }
                        }
                    }
                    else
                    {
                        return PlayerAction.CheckOrCall();
                    }
                }
                else
                {
                    // opponent has raised
                    if (context.MoneyToCall > 0)
                    {
                        // a lot
                        if (context.MoneyToCall > context.CurrentPot - context.MoneyToCall && context.MoneyToCall > 50)
                        {
                            if (flopCardStrength >= 2000)
                            {
                                return PlayerAction.Raise(context.MoneyToCall * 2);
                            }
                            else if (flopCardStrength >= 1000)
                            {
                                // is common pair logic
                                var pairInfo = this.GetPairInfo();

                                if (pairInfo == 0)
                                {
                                    return this.Fold();
                                }
                                else
                                {
                                    // money are a lot and we fold
                                    if (context.MoneyToCall > context.MoneyLeft / 3 && context.MoneyLeft > 300)
                                    {
                                        return this.Fold();
                                    }
                                    else
                                    {
                                        if (pairInfo >= 11)
                                        {
                                            return PlayerAction.CheckOrCall();
                                        }
                                        else
                                        {
                                            return this.Fold();
                                        }
                                    }
                                }
                            }
                            else
                            {
                                return this.Fold();
                            }
                        }
                        else //not a lot
                        {
                            if (flopCardStrength >= 2000)
                            {
                                return PlayerAction.Raise(context.MoneyToCall * 3);
                            }
                            else if (flopCardStrength >= 1000)
                            {
                                var pairInfo = this.GetPairInfo();

                                if (pairInfo == 0)
                                {
                                    return PlayerAction.CheckOrCall();
                                }
                                else
                                {
                                    if (pairInfo >= 11)
                                    {
                                        return PlayerAction.Raise(context.SmallBlind * 8);
                                    }
                                    else
                                    {
                                        return PlayerAction.CheckOrCall();
                                    }
                                }
                            }
                            else
                            {
                                if (context.MoneyToCall >= 20)
                                {
                                    return this.Fold();
                                }
                                else
                                {
                                    return PlayerAction.CheckOrCall();
                                }
                            }
                        }
                    }
                    else // opp has checked (has bad hand)
                    {
                        if (flopCardStrength >= 2000)
                        {
                            return PlayerAction.Raise(context.SmallBlind * 8);
                        }
                        else if (flopCardStrength >= 1000)
                        {
                            return PlayerAction.Raise(context.SmallBlind * 16);
                        }

                        return PlayerAction.CheckOrCall();
                    }
                }
            }
            #endregion

            #region Turn
            else if (context.RoundType == GameRoundType.Turn)
            {
                if (context.MoneyLeft == 0)
                {
                    return PlayerAction.CheckOrCall();
                }

                var hand = new List<Card>();
                hand.Add(this.FirstCard);
                hand.Add(this.SecondCard);

                var ehs = EffectiveHandStrenghtCalculator.CalculateEHS(hand, this.CommunityCards);

                if (ehs < 0.3)
                {
                    if (context.MoneyToCall <= context.MoneyLeft / 200)
                    {
                        return PlayerAction.CheckOrCall();
                    }
                    else
                    {
                        return this.Fold();
                    }
                }
                else if (ehs < 0.5)
                {
                    if (context.MoneyToCall <= context.MoneyLeft / 40)
                    {
                        return PlayerAction.CheckOrCall();
                    }
                    else
                    {
                        return this.Fold();
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
                    else if (context.MoneyToCall < context.MoneyLeft / 20 || context.MoneyToCall < 50)
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
                        return this.Fold();
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
                    else if (context.MoneyToCall < context.MoneyLeft / 3 || context.MoneyToCall < 150)
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
                        return this.Fold();
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
            #endregion

            #region River
            else if (context.RoundType == GameRoundType.River)
            {
                inPosition = false;

                if (context.MoneyLeft == 0)
                {
                    return PlayerAction.CheckOrCall();
                }

                var hand = new List<Card>();
                hand.Add(this.FirstCard);
                hand.Add(this.SecondCard);

                var ehs = EffectiveHandStrenghtCalculator.CalculateEHS(hand, this.CommunityCards);

                if (ehs < 0.3)
                {
                    if (context.MoneyToCall <= context.MoneyLeft / 200)
                    {
                        return PlayerAction.CheckOrCall();
                    }
                    else
                    {
                        return this.Fold();
                    }
                }
                else if (ehs < 0.5)
                {
                    if (context.MoneyToCall <= context.MoneyLeft / 40)
                    {
                        return PlayerAction.CheckOrCall();
                    }
                    else
                    {
                        return this.Fold();
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
                    else if (context.MoneyToCall < context.MoneyLeft / 20 || context.MoneyToCall < 50)
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
                        return this.Fold();
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
                    else if (context.MoneyToCall < context.MoneyLeft / 3 || context.MoneyToCall < 150)
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
                        return this.Fold();
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
            #endregion

            return PlayerAction.CheckOrCall(); // It should never reach this point
        }

        private int GetPairInfo()
        {
            if (this.FirstCard == this.SecondCard)
            {
                return (int)this.FirstCard.Type;
            }

            for (int i = 0; i < this.CommunityCards.Count; i++)
            {
                if (this.CommunityCards.ElementAt(i).Type == this.FirstCard.Type)
                {
                    return (int)this.FirstCard.Type;
                }

                if (this.CommunityCards.ElementAt(i).Type == this.SecondCard.Type)
                {
                    return (int)this.SecondCard.Type;
                }
            }

            return 0;
        }
    }
}
