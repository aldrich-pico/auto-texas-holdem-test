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

        public static string GetText(Rank rank)
        {
            switch(rank)
            {
                case Rank.HIGH_CARD:
                    return "High Card";
                case Rank.ONE_PAIR:
                    return "One Pair";
                case Rank.TWO_PAIRS:
                    return "Two Pairs";
                case Rank.THREE_OF_A_KIND:
                    return "Three of a Kind";
                case Rank.STRAIGHT:
                    return "Straight";
                case Rank.FULL_HOUSE:
                    return "Full House";
                case Rank.FLUSH:
                    return "Flush";
                case Rank.FOUR_OF_A_KIND:
                    return "Four of a Kind";
                case Rank.STRAIGHT_FLUSH:
                    return "Straight Flush";
                case Rank.ROYAL_FLUSH:
                    return "Royal Flush";
                default:
                    return "";
            }
        }
    }
}


