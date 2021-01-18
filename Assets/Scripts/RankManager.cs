﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameTypes;

public class RankManager
{
    private RankManager()
    {

    }

    private static RankManager _instance;
    public static RankManager GetHandRankManager()
    {
        if (_instance == null)
            _instance = new RankManager();

        return _instance;
    }

    public enum Result
    {
        WIN,
        LOSE,
        DRAW,
        MAX_RESULT
    }

    public Result CompareRanks(Ranking rank, Ranking comparedToRank)
    {
        if ((int)rank.rank > (int)comparedToRank.rank)
        {
            return Result.WIN;
        }
        else if ((int)rank.rank < (int)comparedToRank.rank)
        {
            return Result.LOSE;
        }
        else
        {
            //return ResolveDraw(rank, comparedToRank);
            return Result.DRAW;
        }
    }

    public Ranking EvaluateRank(List<Card> cards)
    {
        Ranking ranking = new Ranking();
        ranking.cards = new List<Card>();
        ranking.rank = Ranking.Rank.HIGH_CARD;

        SortHand(ref cards);

        //Count pairs, threes and fours
        Dictionary<Card.Value, List<Card>> pairSets = GatherPairs(cards);
        int fourKindCtr = 0;
        int threeKindCtr = 0;
        int onePairCtr = 0;

        foreach (KeyValuePair<Card.Value, List<Card>> cardSet in pairSets)
        {
            if (cardSet.Value.Count == 4)
                fourKindCtr++;
            else if (cardSet.Value.Count == 3)
                threeKindCtr++;
            else if (cardSet.Value.Count == 2)
                onePairCtr++;
        }

        List<Card> tmpCards = new List<Card>();
        //Check for Straight, Royal Flush and Straight Flush from the highest straight
        if (CheckForStraight(cards, ref ranking.cards))
        {
            ranking.rank = Ranking.Rank.STRAIGHT; //Hand is automatically a straight, override if found royal flush or straight flush
            Debug.Log("Hand is a straight");

            tmpCards.Clear();
            tmpCards.AddRange(ranking.cards);
            if (CheckForFlush(tmpCards, ref ranking.cards))
            {
                if (ranking.cards[0].cardValue == Card.Value.ACE)
                {
                    ranking.rank = Ranking.Rank.ROYAL_FLUSH;
                    Debug.Log("Hand is a royal flush");
                }
                else
                {
                    ranking.rank = Ranking.Rank.STRAIGHT_FLUSH;
                    Debug.Log("Hand is a royal flush");
                }
                return ranking;
            }
            else
            {
                //Get card subsets for edge case testing
                //Check for edge case straight flush from a possible lower straight
                for (int i = 0; i < cards.Count - 5; i++)
                {
                    List<Card> cardSubset = new List<Card>();
                    int subsetIdx = i + 1;
                    cardSubset.AddRange(cards.GetRange(subsetIdx, cards.Count - subsetIdx));
                    if (CheckForStraight(cardSubset, ref ranking.cards))
                    {
                        tmpCards.Clear();
                        tmpCards.AddRange(ranking.cards);
                        if (CheckForFlush(tmpCards, ref ranking.cards))
                        {
                            ranking.rank = Ranking.Rank.STRAIGHT_FLUSH;
                            Debug.Log("Hand is a straight flush");
                            return ranking;
                        }
                    }
                }
            }
        }

        


        ranking.cards.Clear();
        int kickerCardCtr = 0;
        if (fourKindCtr >= 1)
        {
            kickerCardCtr = 0;
            foreach (KeyValuePair<Card.Value, List<Card>> cardSet in pairSets)
            {
                if (cardSet.Value.Count == 4)
                {
                    ranking.cards.AddRange(cardSet.Value);
                }
                else if (kickerCardCtr < 1) //Fill the hand with 1 high card to pair with Four of a Kind hand
                {
                    kickerCardCtr++;
                    ranking.cards.Add(cardSet.Value[0]);
                }
                else if (ranking.cards.Count >= 5)
                    break;
            }
            ranking.rank = Ranking.Rank.FOUR_OF_A_KIND;
            Debug.Log("Hand is four of a kind!");
            return ranking;
        }
        //Check for Flush
        else if (CheckForFlush(cards, ref ranking.cards)) //If previous check found a straight then this found a flush (but not share the same cards) override to flush
        {
            ranking.rank = Ranking.Rank.FLUSH;

            Debug.Log("Hand is a flush");
            return ranking;
        }
        //Check for Full House
        else if (threeKindCtr >= 2 || (threeKindCtr == 1 && onePairCtr >= 1))
        {
            int fullHouseCtr = 0;
            foreach (KeyValuePair<Card.Value, List<Card>> cardSet in pairSets)
            {
                if (cardSet.Value.Count == 3 && fullHouseCtr < 1)
                {
                    fullHouseCtr++;
                    ranking.cards.AddRange(cardSet.Value);
                }
                else if (cardSet.Value.Count >= 2)
                {
                    ranking.cards.AddRange(cardSet.Value.GetRange(0, 2));
                }
            }
            ranking.rank = Ranking.Rank.FULL_HOUSE;
            Debug.Log("Hand is full house");
            return ranking;
        }
        //Check if Straight already
        else if(ranking.rank == Ranking.Rank.STRAIGHT)
        {
            return ranking;
        }
        //Check for Three of a Kind
        else if (threeKindCtr == 1)
        {
            kickerCardCtr = 0;
            foreach (KeyValuePair<Card.Value, List<Card>> cardSet in pairSets)
            {
                if (cardSet.Value.Count == 3)
                {
                    ranking.cards.AddRange(cardSet.Value);
                }
                else if (kickerCardCtr < 2) //Fill the hand with 2 high cards to pair with Three of a Kind hand
                {
                    kickerCardCtr++;
                    ranking.cards.Add(cardSet.Value[0]);
                }
                else if (ranking.cards.Count >= 5)
                    break;
            }
            ranking.rank = Ranking.Rank.THREE_OF_A_KIND;
            Debug.Log("Hand is three of a kind");
            return ranking;
        }
        //Check for Two Pairs
        else if (onePairCtr > 1)
        {
            kickerCardCtr = 0;
            foreach (KeyValuePair<Card.Value, List<Card>> cardSet in pairSets)
            {
                if (cardSet.Value.Count == 2 && ranking.cards.Count < 4)
                {
                    ranking.cards.AddRange(cardSet.Value);
                }
                else if (kickerCardCtr < 1) //Fill the hand with 1 high card to pair with Two Pairs hand
                {
                    kickerCardCtr++;
                    ranking.cards.Add(cardSet.Value[0]);
                }
                else if (ranking.cards.Count >= 5)
                    break;
            }
            ranking.rank = Ranking.Rank.TWO_PAIRS;
            Debug.Log("Hand is two pairs");
            return ranking;
        }
        //Check for One Pair
        else if (onePairCtr == 1)
        {
            kickerCardCtr = 0;
            foreach (KeyValuePair<Card.Value, List<Card>> cardSet in pairSets)
            {
                if (cardSet.Value.Count == 2)
                {
                    ranking.cards.AddRange(cardSet.Value);
                }
                else if (kickerCardCtr < 3) //Fill the hand with 3 high cards to pair with One Pair hand
                {
                    kickerCardCtr++;
                    ranking.cards.Add(cardSet.Value[0]);
                }
                else if (ranking.cards.Count >= 5)
                    break;
            }
            ranking.rank = Ranking.Rank.ONE_PAIR;
            Debug.Log("Hand is one pair");
            return ranking;
        }
        else
        {
            ranking.cards.AddRange(cards.GetRange(0, 5));
            ranking.rank = Ranking.Rank.HIGH_CARD;
            Debug.Log("Hand is high card");
        }

        return ranking;
    }

    private void SortHand(ref List<Card> cardsToSort)
    {
        List<Card> tmpCards = new List<Card>();

        tmpCards.Add(cardsToSort[0]);
        for (int i = 1; i < cardsToSort.Count; i++)
        {
            for (int j = 0; j < tmpCards.Count; j++)
            {
                if ((int)cardsToSort[i].cardValue <= (int)tmpCards[j].cardValue)
                {
                    tmpCards.Insert(j, cardsToSort[i]);

                    break;
                }
                else if (j == tmpCards.Count - 1)
                {
                    tmpCards.Add(cardsToSort[i]);

                    break;
                }

            }
        }

        cardsToSort.Clear();
        cardsToSort.AddRange(tmpCards);
        cardsToSort.Reverse();
    }

    private int[] ComputeValuesDifferences(List<Card> cardsToCompute)
    {
        int[] val = new int[cardsToCompute.Count];
        int[] diff = new int[cardsToCompute.Count - 1];

        for (int i = 0; i < cardsToCompute.Count; i++)
        {
            val[i] = (int)cardsToCompute[i].cardValue;
        }

        for (int i = 0; i < diff.Length; i++)
        {
            diff[i] = val[i] - val[i + 1];
        }

        return diff;
    }

    private bool CheckForStraight(List<Card> cardsToCheck, ref List<Card> participatingCards)
    {
        participatingCards.Clear();
        int straightCtr = 0;
        int[] diff = ComputeValuesDifferences(cardsToCheck);

        //Check from the highest card going down to automatically get the highest possible straight
        for (int i = 0; i < diff.Length; i++)
        {
            if (diff[i] > 1)
            {
                straightCtr = 0;
                participatingCards.Clear();
            }
            else if (diff[i] == 1)
            {
                straightCtr++;
                participatingCards.Add(cardsToCheck[i]);

                if (straightCtr >= 4)
                {
                    participatingCards.Add(cardsToCheck[i + 1]);
                    return true;
                }

            }
        }

        return false;
    }

    private bool CheckForFlush(List<Card> cardsToCheck, ref List<Card> participatingCards)
    {
        participatingCards.Clear();
        int[] suitsCtr = new int[(int)Card.Suit.MAX_SUIT];

        for (int i = 0; i < cardsToCheck.Count; i++)
        {
            suitsCtr[(int)cardsToCheck[i].cardSuit]++;
        }

        for (int i = 0; i < suitsCtr.Length; i++)
        {
            if (suitsCtr[i] >= 5)
            {
                for (int j = 0; j < cardsToCheck.Count; j++)
                {
                    if (cardsToCheck[j].cardSuit == (Card.Suit)i)
                    {
                        participatingCards.Add(cardsToCheck[j]);

                        if (participatingCards.Count >= 5)
                            break;
                    }
                }
                return true;
            }
        }

        return false;
    }

    private Dictionary<Card.Value, List<Card>> GatherPairs(List<Card> cardsToCheck)
    {
        Dictionary<Card.Value, List<Card>> pairs = new Dictionary<Card.Value, List<Card>>();

        for (int i = 0; i < cardsToCheck.Count; i++)
        {
            if (!pairs.ContainsKey(cardsToCheck[i].cardValue))
            {
                List<Card> cardList = new List<Card>();
                cardList.Add(cardsToCheck[i]);
                pairs.Add(cardsToCheck[i].cardValue, cardList);
            }
            else
            {
                pairs[cardsToCheck[i].cardValue].Add(cardsToCheck[i]);
            }
        }

        return pairs;
    }

    /*private Result ResolveDraw(Ranking rank, Ranking comparedToRank)
    {
        if (rank.rank == Ranking.Rank.STRAIGHT ||
            rank.rank == Ranking.Rank.STRAIGHT_FLUSH ||
            rank.rank == Ranking.Rank.ROYAL_FLUSH ||
            rank.rank == Ranking.Rank.FLUSH ||
            rank.rank == Ranking.Rank.HIGH_CARD)
        {
            for(int i = 0; i < rank.cards.Count; i++)
            {
                if (rank.cards[i].cardValue > comparedToRank.cards[i].cardValue)
                {
                    return Result.WIN;
                }
                else if (rank.cards[i].cardValue < comparedToRank.cards[i].cardValue)
                {
                    return Result.LOSE;
                }
                else
                    continue;
            }

            return Result.DRAW;
        }
        else if(rank.rank == Ranking.Rank.FULL_HOUSE || rank.rank == Ranking.Rank.TWO_PAIRS)
        {
            Dictionary<Card.Value, List<Card>> pairs1 = GatherPairs(rank.cards);
            Dictionary<Card.Value, List<Card>> pairs2 = GatherPairs(comparedToRank.cards);

            List<Card.Value> cardValues1 = new List<Card.Value>();
            List<Card.Value> cardValues2 = new List<Card.Value>();

            foreach (KeyValuePair<Card.Value, List<Card>> pair in pairs1)
            {
                cardValues1.Add(pair.Key);
            }
            foreach (KeyValuePair<Card.Value, List<Card>> pair in pairs2)
            {
                cardValues2.Add(pair.Key);
            }

            for(int i = 0; i < 2; i++)
            {
                if((int)cardValues1[i] > (int)cardValues2[i])
                {
                    return Result.WIN;
                }
                else if ((int)cardValues1[i] > (int)cardValues2[i])
                {
                    return Result.LOSE;
                }
                else
                {
                    
                    //if (rank.rank == Ranking.Rank.TWO_PAIRS)
                    //{
                    //    Card.Value highCardValue1;
                    //    Card.Value highCardValue2;

                    //    for(int j = 0; j < rank.cards.Count; j++)
                    //    {
                    //        cardValues1.
                    //    }

                    //}
                    //else
                        return Result.DRAW;
                }
            }
        }
        //else if(rank.rank == Ranking.Rank.ONE_PAIR ||
        //    rank.rank == Ranking.Rank.THREE_OF_A_KIND ||
        //    rank.rank == Ranking.Rank.FOUR_OF_A_KIND)
        //{

        //}
        else 
            return Result.DRAW;
    }*/
}