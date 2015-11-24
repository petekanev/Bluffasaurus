namespace TexasHoldem.AI.SmartPlayer
{
    using System;

    using TexasHoldem.AI.SmartPlayer.Helpers;
    using TexasHoldem.Logic;
    using TexasHoldem.Logic.Extensions;
    using TexasHoldem.Logic.Players;

    // TODO: This player is far far away from being smart!
    public class SmarterPlayer : BasePlayer
    {
        public override string Name { get; } = "SmarterPlayer" + Guid.NewGuid();

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

            return PlayerAction.CheckOrCall();
        }
    }
}
