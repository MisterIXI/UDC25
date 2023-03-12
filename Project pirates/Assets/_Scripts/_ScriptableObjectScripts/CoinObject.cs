using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "CoinObject", menuName ="CoinObject")]
public class CoinObject: ScriptableObject
{
    [field: Header("All Coins listed here:")]
    [field: SerializeField] public List<int> CoinList { get; set; } = new List<int>();

    [field: Header("CoinObjects:")]
    [field: SerializeField] public GameObject BronzeObject { get; private set; }
    [field: SerializeField] public GameObject SilverObject { get; private set; }
    [field: SerializeField] public GameObject GoldObject { get; private set; }

    
    
}
