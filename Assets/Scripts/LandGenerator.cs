using DG.Tweening;
using FirebaseWebGL.Scripts.FirebaseBridge;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LandGenerator : MonoBehaviour
{
    [SerializeField]
    int landSizeCount = 6;

    [SerializeField]
    GameObject landPrefab;

    [SerializeField]
    TextMeshProUGUI logger;

    [SerializeField]
    BuyLandHandler buyLand;

    [SerializeField]
    TMP_InputField newLandName;

    [SerializeField]
    CanvasGroup ownedLandCanvas;

    [SerializeField]
    CanvasGroup enemyLandCanvas;


    LandItem[,] landItems;
    string web3account;

    // Start is called before the first frame update
    void Start()
    {
        ownedLandCanvas.alpha = 0;
        enemyLandCanvas.alpha = 0;

        web3account = PlayerPrefs.GetString("Account").ToUpper();
        buyLand.onLandDataChange += OnFetchAllLandData;

        GetComponent<GridLayoutGroup>().constraintCount = landSizeCount;

        landItems = new LandItem[landSizeCount, landSizeCount];

        for (int i = 0; i < landSizeCount; i++)
        {
            for (int j = 0; j < landSizeCount; j++)
            {
                CreateLand(i, j);
            }
        }

        buyLand.OnLandUpdate();
    }

    private void OnDestroy()
    {
        buyLand.onLandDataChange -= OnFetchAllLandData;
    }

    public void OnFetchAllLandData(List<LandTokenData> landTokens)
    {
        // TODO: Sync up token data to firebase collection, if document doesn't exist, generate land properties
        FirebaseFirestore.GetDocumentsInCollection("userLandDetails", "", "LogSuccess", "LogError");

        foreach (LandTokenData token in landTokens)
        {
            bool isOwner = token.tokenOwner == web3account;

            landItems[token.x, token.y].OnItemUpdate(token.landName + "\n" + token.x + ":" + token.y, isOwner ? Color.green : Color.red);
            landItems[token.x, token.y].itemButton.onClick.RemoveAllListeners();

            if(isOwner)
            {
                landItems[token.x, token.y].itemButton.onClick.AddListener(() =>
                {
                    ShowPlayerLandDetails(token);
                });
            } else
            {
                landItems[token.x, token.y].itemButton.onClick.AddListener(() =>
                {
                    ShowEnemyLandDetails(token);
                });
            }
        }
    }

    void ShowPlayerLandDetails(LandTokenData data)
    {
        ownedLandCanvas.DOFade(1, 0.5f);
        enemyLandCanvas.DOFade(0, 0.5f);

        ownedLandCanvas.GetComponent<OwnerLandViewer>().SetItemDetails(data);
    }

    void ShowEnemyLandDetails(LandTokenData data)
    {
        enemyLandCanvas.DOFade(1, 0.5f);
        ownedLandCanvas.DOFade(0, 0.5f);

        enemyLandCanvas.GetComponent<EnemyLandViewer>().SetItemDetails(data);
    }

    public void FailFetchLandData(string data)
    {
        LogError(data);
    }

    private void LogError(string data)
    {
        logger.color = Color.red;
        logger.text = data;
    }

    private void LogSuccess(string data)
    {
        logger.color = Color.green;
        logger.text = data;
    }

    void CreateLand(int i, int j)
    {
        LandItem landInstance = Instantiate(landPrefab, transform).GetComponent<LandItem>();
        landInstance.OnItemUpdate(i + " : " + j, Color.white);
        landInstance.itemButton.onClick.RemoveAllListeners();
        landInstance.itemButton.onClick.AddListener(() => {
            string landName = string.IsNullOrEmpty(newLandName.text) ? "ExampleLand" : newLandName.text;
            // name should be taken from a dialog input box?
            buyLand.BuyLand(i, j, landName);
        });
        landInstance.name = "Land_" + i + "_" + j;
        landItems[i,j] = landInstance;
    }

    void CheckLandOwnership()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
