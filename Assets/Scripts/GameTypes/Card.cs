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

        public static string GetText(Value value)
        {
            switch(value)
            {
                case Value.TWO:
                    return "Two";
                case Value.THREE:
                    return "Three";
                case Value.FOUR:
                    return "Four";
                case Value.FIVE:
                    return "Five";
                case Value.SIX:
                    return "Six";
                case Value.SEVEN:
                    return "Seven";
                case Value.EIGHT:
                    return "Eight";
                case Value.NINE:
                    return "Nine";
                case Value.TEN:
                    return "Ten";
                case Value.JACK:
                    return "Jack";
                case Value.QUEEN:
                    return "Queen";
                case Value.KING:
                    return "King";
                case Value.ACE:
                    return "Ace";
                default:
                    return "";
            }
        }

        public static string GetText(Suit suit)
        {
            switch (suit)
            {
                case Suit.CLUB:
                    return "Club";
                case Suit.DIAMOND:
                    return "Diamond";
                case Suit.SPADE:
                    return "Spade";
                case Suit.HEART:
                    return "Heart";
                default:
                    return "";

            }
        }
    }
}

