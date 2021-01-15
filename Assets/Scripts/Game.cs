using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    private Text player1HandRankText;
    [SerializeField]
    private Text player2HandRankText;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        resultPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onPlayButton(Button playButton)
    {
        Debug.Log("Play button pressed!");

        playButton.interactable = false;

        Dealer.GetDealer().DealCards();
        Dealer.GetDealer().DealCommunityCards();
    }
}
