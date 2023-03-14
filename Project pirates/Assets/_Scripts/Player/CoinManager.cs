using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CoinManager : MonoBehaviour
{
    
    [SerializeField]private  CoinObject _coinObject;
    private List<GameObject> coinsSpawned= new List<GameObject>();
    [field: SerializeField] public List<int> CoinList { get; private set; } = new List<int>();
    private float speed= 5.0f;
    private int bronzeCoins;
    private int silverCoins;
    private int goldCoins;
    private PlayerController playercontroller;
    private Camera _mainCamera;
    private PlayerSettings _playerSettings;
    public static CoinManager Instance; // SINGLETON
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start() {
        playercontroller = PlayerController.Instance;
        CoinList.Clear();
        _mainCamera = Camera.main;
        _playerSettings = SettingsManager.PlayerSettings;
        bronzeCoins =0;
        silverCoins= 0;
        goldCoins =0;
    }
    
    public void AddCoin(GameObject other) {
        if( other != null) // other.CompareTag("Coin") &&
        {   int type = getCoinType(other.GetComponent<TakeCoin>().coinType);
            CoinList.Add(type);

            if(type == 0){
                bronzeCoins ++;
                IngameCoins.Instance.OnTriggerChange(TakeCoin.TakeCoinType.Bronze,bronzeCoins);
            }else if(type == 1){
                silverCoins ++;
                IngameCoins.Instance.OnTriggerChange(TakeCoin.TakeCoinType.Silver,silverCoins);
            }else if(type == 2)
                goldCoins ++;
                IngameCoins.Instance.OnTriggerChange(TakeCoin.TakeCoinType.Gold,goldCoins);
            // Debug.Log("Add Coin type:"+ getCoinType(other.GetComponent<TakeCoin>().coinType));
            Destroy(other);
            // ParticleEffect Play
        }

    }
    public void DropCoin()
    {
        if(CoinList[0] >=0){
             if(CoinList[0] == 0){
                bronzeCoins --;
                IngameCoins.Instance.OnTriggerChange(TakeCoin.TakeCoinType.Bronze,bronzeCoins);
            }else if(CoinList[0] == 1){
                silverCoins --;
                IngameCoins.Instance.OnTriggerChange(TakeCoin.TakeCoinType.Silver,silverCoins);
            }else if(CoinList[0] == 2)
                goldCoins --;
                IngameCoins.Instance.OnTriggerChange(TakeCoin.TakeCoinType.Gold,goldCoins);
            GameObject instance = Instantiate(getCoinObject(CoinList[0]), transform.position, transform.rotation);
            CoinList.Remove(CoinList[0]);
            Vector2 deltaInput = playercontroller.GetDelta();
            instance.GetComponent<Rigidbody>().AddForce(_mainCamera.transform.forward * _playerSettings.InteractThrowMagnitude, ForceMode.Impulse);
        }

    }
    public void OpenCoinList()
    {
        // show all collected coins and Rotate around the player
        SpawnCoins();
        
    }

    private void SpawnCoins()
    {
        Transform _motherRotate = new GameObject(gameObject.name + "Surrounder").transform;
        _motherRotate.position = gameObject.transform.position;
        float angleStep = 360/ CoinList.Count;
        // foreach coin in list spawn Gameobject around player with calc distance 360/ count new Prefab
        for (int i = 0; i < CoinList.Count; i++)
        {
           
            GameObject _obj = Instantiate(getCoinObject(CoinList[i]));
            coinsSpawned.Add(_obj);
            _obj.transform.RotateAround(gameObject.transform.position, Vector3.up, angleStep*i);
            _obj.transform.SetParent(_motherRotate);
        }
        _motherRotate.gameObject.AddComponent<rotating>();
        _motherRotate.gameObject.GetComponent<rotating>().SetSpeed(80.0f);
        Destroy(_motherRotate, 5);
    }

    private int getCoinType(TakeCoin.TakeCoinType type)
    {
        return ((int)type);
    }
    private GameObject getCoinObject(int value)
    {
        switch (value)
        {
            case 0:
                return _coinObject.BronzeObject;
                
            case 1:
                return _coinObject.SilverObject;
                
            case 2:
                return _coinObject.GoldObject;
                
            default: 
            return _coinObject.BronzeObject;
            
        }
    }

}
