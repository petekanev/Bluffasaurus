namespace TexasHoldem.AI.Bluffasaurus.Helpers
{
    using TexasHoldem.Logic.Cards;

    class HandStrengthValuationSmarterBot
    {
        private const int MaxCardTypeValue = 14;

        private static readonly int[,] StartingHandRecommendations =
            {
                { 1, 1, 2, 2, 3, 5, 5, 5, 5, 5, 5, 5, 5 }, // AA AKs AQs AJs ATs A9s A8s A7s A6s A5s A4s A3s A2s
                { 2, 1, 2, 3, 4, 7, 7, 7, 7, 7, 7, 7, 7 }, // AKo KK KQs KJs KTs K9s K8s K7s K6s K5s K4s K3s K2s
                { 3, 4, 1, 3, 4, 5, 9, 9, 9, 9, 9, 9, 9 }, // AQo KQo QQ QJs QTs Q9s Q8s Q7s Q6s Q5s Q4s Q3s Q2s
                { 4, 5, 5, 1, 3, 4, 6, 8, 9, 9, 9, 9, 9 }, // AJo KJo QJo JJ JTs J9s J8s J7s J6s J5s J4s J3s J2s
                { 6, 6, 6, 5, 2, 4, 5, 9, 9, 9, 9, 9, 9 }, // ATo KTo QTo JTo TT T9s T8s T7s T6s T5s T4s T3s T2s
                { 8, 8, 8, 7, 7, 3, 4, 5, 8, 9, 9, 9, 9 }, // A9o K9o Q9o J9o T9o 99 98s 97s 96s 95s 94s 93s 92s
                { 9, 9, 9, 8, 8, 7, 4, 5, 6, 8, 9, 9, 9 }, // A8o K8o Q8o J8o T8o 98o 88 87s 86s 85s 84s 83s 82s
                { 9, 9, 9, 9, 9, 9, 8, 5, 5, 6, 8, 9, 9 }, // A7o K7o Q7o J7o T7o 97o 87o 77 76s 75s 74s 73s 72s
                { 9, 9, 9, 9, 9, 9, 9, 8, 5, 6, 7, 9, 9 }, // A6o K6o Q6o J6o T6o 96o 86o 76o 66 65s 64s 63s 62s
                { 9, 9, 9, 9, 9, 9, 9, 9, 8, 6, 6, 7, 9 }, // A5o K5o Q5o J5o T5o 95o 85o 75o 65o 55 54s 53s 52s
                { 9, 9, 9, 9, 9, 9, 9, 9, 9, 8, 7, 7, 8 }, // A4o K4o Q4o J4o T4o 94o 84o 74o 64o 54o 44 43s 42s
                { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 7, 8 }, // A3o K3o Q3o J3o T3o 93o 83o 73o 63o 53o 43o 33 32s
                { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 7 } // A2o K2o Q2o J2o T2o 92o 82o 72o 62o 52o 42o 32o 22
            };

        // http://www.rakebackpros.net/texas-holdem-starting-hands/
        public static CardValuationTypeForSmarterBot PreFlop(Card firstCard, Card secondCard)
        {
            var value = firstCard.Suit == secondCard.Suit
                          ? (firstCard.Type > secondCard.Type
                                 ? StartingHandRecommendations[MaxCardTypeValue - (int)firstCard.Type, MaxCardTypeValue - (int)secondCard.Type]
                                 : StartingHandRecommendations[MaxCardTypeValue - (int)secondCard.Type, MaxCardTypeValue - (int)firstCard.Type])
                          : (firstCard.Type > secondCard.Type
                                 ? StartingHandRecommendations[MaxCardTypeValue - (int)secondCard.Type, MaxCardTypeValue - (int)firstCard.Type]
                                 : StartingHandRecommendations[MaxCardTypeValue - (int)firstCard.Type, MaxCardTypeValue - (int)secondCard.Type]);

            switch (value)
            {
                case 1:
                    return CardValuationTypeForSmarterBot.group9;
                case 2:
                    return CardValuationTypeForSmarterBot.group8;
                case 3:
                    return CardValuationTypeForSmarterBot.group7;
                case 4:
                    return CardValuationTypeForSmarterBot.group6;
                case 5:
                    return CardValuationTypeForSmarterBot.group5;
                case 6:
                    return CardValuationTypeForSmarterBot.group4;
                case 7:
                    return CardValuationTypeForSmarterBot.group3;
                case 8:
                    return CardValuationTypeForSmarterBot.group2;
                case 9:
                    return CardValuationTypeForSmarterBot.group1;
                default:
                    return CardValuationTypeForSmarterBot.group1;
            }
        }
    }
}
