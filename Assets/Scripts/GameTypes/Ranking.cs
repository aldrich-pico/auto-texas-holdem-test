using System.Collections.Generic;

namespace GameTypes
{
    public struct Ranking
    {
        public enum Rank
        {
            HIGH_CARD,
            ONE_PAIR,
            TWO_PAIRS,
            THREE_OF_A_KIND,
            STRAIGHT,
            FULL_HOUSE,
            FLUSH,
            FOUR_OF_A_KIND,
            STRAIGHT_FLUSH,
            ROYAL_FLUSH,
            MAX_RANKING
        }

        public Rank rank;
        public List<Card> cards;

        public Ranking(Rank rank, List<Card> cards)
        {
            this.rank = rank;
            this.cards = cards;
        }
    }
}


