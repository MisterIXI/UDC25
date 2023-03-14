using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class IngameCoins : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI bronzetext;
    [SerializeField] private TextMeshProUGUI silvertext;
    [SerializeField] private TextMeshProUGUI goldtext;
    public TakeCoin.TakeCoinType type;
    public static IngameCoins Instance; // SINGLETON
    private void Awake()
        {
            if(Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
    // Start is called before the first frame update
    public void OnTriggerChange(TakeCoin.TakeCoinType type, int value)
    {
        if (type == TakeCoin.TakeCoinType.Bronze)
            bronzetext.text = value.ToString();
        if (type == TakeCoin.TakeCoinType.Silver)
            silvertext.text = value.ToString();
        if (type == TakeCoin.TakeCoinType.Gold)
            goldtext.text = value.ToString();
    }

}
