using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour, IInteractable
{
    [SerializeField] Transform startPoint;
    [SerializeField] Transform endPoint;
    bool isPlayerOnLadder;
    public string Data()
    {
        if(isPlayerOnLadder)
            return "Drop from Ladder";
        return "Climb Ladder";
    }

    public void Interact()
    {
        if(isPlayerOnLadder){
            PlayerController.Instance.SetLadderSnap(false, this);
            isPlayerOnLadder = false;
        }
        else{
            PlayerController.Instance.SetLadderSnap(true, this);
            isPlayerOnLadder = true;
        }
    }


    public void IsPlayerOnLadder(bool value){
        isPlayerOnLadder = value;
    }
    public Transform GetStartPoint()
    {
        return startPoint;
    }
    public Transform GetEndPoint()
    {
        return endPoint;
    }
}

