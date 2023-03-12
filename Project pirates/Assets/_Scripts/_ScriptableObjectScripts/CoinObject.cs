using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Scriptable_Coins", menuName ="Scriptable_Coins")]
public class CoinObject: ScriptableObject
{
    [SerializeField] int COINS = 0;

    public int GetCoins {get => COINS;}
    public void UpdateCoins(int value){
        COINS += value;
    }
    // INSTANCING TO GAME
    // PUBLIC COIN CLASS 
    // GET PRIVATE SET
    // NEW COIN TO INVENTORY
    // PUBLIC VOID DROP COIN
    // SHOW HIDE ALL COINS TO ORBIT AROUND PLAYER
    // INTERACT WITH TAKE COIN SCRIPT AND INVENTORY
}
