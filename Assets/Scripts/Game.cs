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

    [SerializeField]
    private Animator communityCardsAnimator;
    [SerializeField]
    private Animator playerCardsAnimator;

    CardTextureHandler[] player1CardTexHdlr;
    CardTextureHandler[] player2CardTexHdlr;
    CardTextureHandler[] communityCardTexHdlr;

    [SerializeField]
    private Text player1HandRankText;
    [SerializeField]
    private Text player2HandRankText;
    [SerializeField]
    private Text resultPanelText;

    private Button playButton;
    private bool isFirstRun;

    private void Awake()
    {
        player1CardTexHdlr = player1Cards.GetComponentsInChildren<CardTextureHandler>();
        player2CardTexHdlr = player2Cards.GetComponentsInChildren<CardTextureHandler>();
        communityCardTexHdlr = communityCards.GetComponentsInChildren<CardTextureHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player1HandRankText.text = "";
        player2HandRankText.text = "";
        resultPanelText.text = "Ready";

        isFirstRun = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("escape"))
        {
            Application.Quit();
        }
    }

    public void onPlayButton(Button button)
    {
        Dealer.GetDealer().DealCards();
        Dealer.GetDealer().DealCommunityCards();
        Dealer.GetDealer().EvaluateHands();
        Dealer.GetDealer().EvaluateWinner();
        

        if (playButton == null)
            playButton = button;

        if(isFirstRun)
        {
            StartPlayerCardsAnimation();
            isFirstRun = false;
        }
        else
        {
            player1HandRankText.text = "";
            player2HandRankText.text = "";
            resultPanelText.text = "Playing...";
            communityCardsAnimator.SetBool("isReshuffle", true);
            playerCardsAnimator.SetBool("isReshuffle", true);
        }

        playButton.interactable = false;
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

    public void StartCommunityCardsAnimation()
    {
        communityCardsAnimator.SetBool("isRevealCommunityCards", true);
    }

    public void StartPlayerCardsAnimation()
    {
        UpdateCards();
        communityCardsAnimator.SetBool("isReshuffle", false);
        playerCardsAnimator.SetBool("isReshuffle", false);
        playerCardsAnimator.SetBool("isGameStart", true);
    }

    public void StartEvaluation()
    {
        UpdatePlayerPanels();
        UpdateResultPanel();

        playerCardsAnimator.SetBool("isGameStart", false);
        communityCardsAnimator.SetBool("isRevealCommunityCards", false);
        playButton.interactable = true;
    }
}
