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
        public Card highCard;
        public Card lowCard;

        public Ranking(Rank rank, Card highCard, Card lowCard)
        {
            this.rank = rank;
            this.highCard = highCard;
            this.lowCard = lowCard;
        }
    }
}


