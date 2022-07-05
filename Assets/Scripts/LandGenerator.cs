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

    LandItem[,] landItems;

    // Start is called before the first frame update
    void Start()
    {
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
        FirebaseFirestore.GetDocumentsInCollection("userLandDetails", "", "LogSuccess", "LogError");
        Debug.Log(landTokens.Count);

        foreach (LandTokenData token in landTokens)
        {
            landItems[token.x, token.y].OnItemUpdate(token.landName + "\n" + token.x + ":" + token.y, Color.red);
        }
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
