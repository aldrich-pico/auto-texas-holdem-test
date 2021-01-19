using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFunctionCaller : MonoBehaviour
{
    [SerializeField]
    private Game game;
    
    public void CallStartCommunityCardAnimation()
    {
        game.StartCommunityCardsAnimation();
    }

    public void CallStartPlayerCardAnimation()
    {
        game.StartPlayerCardsAnimation();
    }

    public void CallStartEvaluation()
    {
        game.StartEvaluation();
    }
}
