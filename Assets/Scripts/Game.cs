using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameTypes;

public class Game : MonoBehaviour
{
    [SerializeField]
    private GameObject communityCards;
    [SerializeField]
    private GameObject player1Cards;
    [SerializeField]
    private GameObject player2Cards;
    [SerializeField]
    private GameObject resultPanel;

    CardTextureHandler[] player1CardTexHdlr;
    CardTextureHandler[] player2CardTexHdlr;
    CardTextureHandler[] communityCardTexHdlr;

    [SerializeField]
    private Text player1HandRankText;
    [SerializeField]
    private Text player2HandRankText;
    [SerializeField]
    private Text resultPanelText;

    private void Awake()
    {
        player1CardTexHdlr = player1Cards.GetComponentsInChildren<CardTextureHandler>();
        player2CardTexHdlr = player2Cards.GetComponentsInChildren<CardTextureHandler>();
        communityCardTexHdlr = communityCards.GetComponentsInChildren<CardTextureHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //resultPanel.SetActive(false);
        player1HandRankText.text = "";
        player2HandRankText.text = "";
        resultPanelText.text = "Ready";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    public void onPlayButton(Button playButton)
    {
        //playButton.interactable = false;

        Dealer.GetDealer().DealCards();
        Dealer.GetDealer().DealCommunityCards();
        Dealer.GetDealer().EvaluateHands();
        Dealer.GetDealer().EvaluateWinner();
        UpdateCards();
        UpdatePlayerPanels();
        UpdateResultPanel();
    }

    public void UpdateCards()
    {
        for(int i = 0; i < player1CardTexHdlr.Length; i++)
        {
            player1CardTexHdlr[i].SetCardTexture(Dealer.GetDealer().playerHands[0][i]);
        }
        for (int i = 0; i < player2CardTexHdlr.Length; i++)
        {
            player2CardTexHdlr[i].SetCardTexture(Dealer.GetDealer().playerHands[1][i]);
        }
        for (int i = 0; i < communityCardTexHdlr.Length; i++)
        {
            communityCardTexHdlr[i].SetCardTexture(Dealer.GetDealer().communityCards[i]);
        }

    }

    public void UpdatePlayerPanels()
    {
        player1HandRankText.text = "Hand: " + Ranking.GetText(Dealer.GetDealer().rankings[0].rank);
        player2HandRankText.text = "Hand: " + Ranking.GetText(Dealer.GetDealer().rankings[1].rank);
    }

    public void UpdateResultPanel()
    {
        switch(Dealer.GetDealer().result)
        {
            case RankManager.Result.WIN:
                resultPanelText.text = "YOU WIN!";
                break;
            case RankManager.Result.LOSE:
                resultPanelText.text = "YOU LOSE...";
                break;
            case RankManager.Result.DRAW:
                resultPanelText.text = "IT'S A DRAW.";
                break;
        }
        
    }
}
