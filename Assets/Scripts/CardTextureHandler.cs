using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameTypes;

public class CardTextureHandler : MonoBehaviour
{
    public Image image;
    public Image backingImage;
    public Sprite[] backingSprites;
    public Sprite[] diamondSprites;
    public Sprite[] heartSprites;
    public Sprite[] clubSprites;
    public Sprite[] spadeSprites;

    private void Awake()
    {
        image = GetComponent<Image>();
        backingImage = GetComponentInChildren<Image>();
    }

    public void SetCardTexture(Card card)
    {
        SetCardTexture(card.cardSuit, card.cardValue);
    }

    public void SetCardTexture(Card.Suit suit, Card.Value value)
    {
        switch (suit)
        {
            case Card.Suit.CLUB:
                image.sprite = clubSprites[(int)value];
                break;
            case Card.Suit.DIAMOND:
                image.sprite = diamondSprites[(int)value];
                break;
            case Card.Suit.HEART:
                image.sprite = heartSprites[(int)value];
                break;
            case Card.Suit.SPADE:
                image.sprite = spadeSprites[(int)value];
                break;
        }
    }

    public void SetBackingImage(int idx)
    {
        backingImage.sprite = backingSprites[idx];
    }
}
