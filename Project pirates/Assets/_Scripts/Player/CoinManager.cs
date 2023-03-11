using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    [SerializeField] CoinObject coinObject = null;
    public void AddCoin(GameObject other) {
        if(other.CompareTag("Coin") && other != null)
        {
            coinObject.UpdateCoins(coinObject.GetCoins+1);
            Destroy(other,1);
            // ParticleEffect Play
        }

    }
    public void DropCoin()
    {
        coinObject.UpdateCoins(coinObject.GetCoins-1);
        // Instantiate Coin and add force
    }
    public void OpenCoinList()
    {
        // show all collected coins
    }

}
