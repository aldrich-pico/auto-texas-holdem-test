namespace GameTypes
{
    public struct Card
    {
        public enum Suit
        {
            HEART,
            SPADE,
            CLUB,
            DIAMOND,
            MAX_SUIT
        }

        public enum Value
        {
            TWO,
            THREE,
            FOUR,
            FIVE,
            SIX,
            SEVEN,
            EIGHT,
            NINE,
            TEN,
            JACK,
            QUEEN,
            KING,
            ACE,
            MAX_VALUE
        }

        public Suit cardSuit;
        public Value cardValue;

        public Card(Suit cardSuit, Value cardValue)
        {
            this.cardSuit = cardSuit;
            this.cardValue = cardValue;
        }
    }
}

