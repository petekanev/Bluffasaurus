namespace TexasHoldem.AI.Bluffasaurus.Helpers
{
    using TexasHoldem.Logic.Cards;

    public class HandStrengthValuationBluffasaurus
    {
        private const int MaxCardTypeValue = 14;

        private static readonly int[,] StartingHandRecommendations =
            {
                { 85, 67, 66, 65, 65, 63, 62, 61, 60, 60, 59, 58, 57 }, // AA AKs AQs AJs ATs A9s A8s A7s A6s A5s A4s A3s A2s
                { 65, 82, 63, 62, 62, 60, 59, 58, 57, 56, 55, 54, 53 }, // AKo KK KQs KJs KTs K9s K8s K7s K6s K5s K4s K3s K2s
                { 64, 61, 80, 60, 59, 58, 56, 54, 54, 53, 52, 51, 50 }, // AQo KQo QQ QJs QTs Q9s Q8s Q7s Q6s Q5s Q4s Q3s Q2s
                { 64, 61, 58, 78, 58, 56, 54, 52, 51, 50, 49, 48, 47 }, // AJo KJo QJo JJ JTs J9s J8s J7s J6s J5s J4s J3s J2s
                { 63, 60, 57, 55, 75, 54, 53, 51, 49, 47, 46, 46, 45 }, // ATo KTo QTo JTo TT T9s T8s T7s T6s T5s T4s T3s T2s
                { 61, 58, 56, 53, 52, 72, 51, 50, 48, 46, 44, 43, 42 }, // A9o K9o Q9o J9o T9o 99 98s 97s 96s 95s 94s 93s 92s
                { 60, 56, 54, 52, 50, 48, 69, 48, 47, 45, 43, 41, 40 }, // A8o K8o Q8o J8o T8o 98o 88 87s 86s 85s 84s 83s 82s
                { 59, 55, 52, 50, 48, 47, 46, 66, 48, 44, 42, 40, 38 }, // A7o K7o Q7o J7o T7o 97o 87o 77 76s 75s 74s 73s 72s
                { 58, 54, 51, 48, 46, 45, 44, 43, 63, 43, 41, 40, 38 }, // A6o K6o Q6o J6o T6o 96o 86o 76o 66 65s 64s 63s 62s
                { 58, 53, 50, 47, 44, 43, 42, 41, 40, 60, 41, 39, 38 }, // A5o K5o Q5o J5o T5o 95o 85o 75o 65o 55 54s 53s 52s
                { 56, 52, 49, 46, 43, 41, 40, 39, 38, 38, 57, 38, 36 }, // A4o K4o Q4o J4o T4o 94o 84o 74o 64o 54o 44 43s 42s
                { 56, 51, 48, 45, 42, 30, 38, 37, 36, 36, 34, 54, 35 }, // A3o K3o Q3o J3o T3o 93o 83o 73o 63o 53o 43o 33 32s
                { 55, 50, 47, 44, 41, 39, 37, 35, 34, 34, 33, 31, 50 } // A2o K2o Q2o J2o T2o 92o 82o 72o 62o 52o 42o 32o 22
            };

        // http://www.rakebackpros.net/texas-holdem-starting-hands/
        public static int PreFlop(Card firstCard, Card secondCard)
        {
            var value = firstCard.Suit == secondCard.Suit
                          ? (firstCard.Type > secondCard.Type
                                 ? StartingHandRecommendations[MaxCardTypeValue - (int)firstCard.Type, MaxCardTypeValue - (int)secondCard.Type]
                                 : StartingHandRecommendations[MaxCardTypeValue - (int)secondCard.Type, MaxCardTypeValue - (int)firstCard.Type])
                          : (firstCard.Type > secondCard.Type
                                 ? StartingHandRecommendations[MaxCardTypeValue - (int)secondCard.Type, MaxCardTypeValue - (int)firstCard.Type]
                                 : StartingHandRecommendations[MaxCardTypeValue - (int)firstCard.Type, MaxCardTypeValue - (int)secondCard.Type]);

            return value;
        }
    }
}
