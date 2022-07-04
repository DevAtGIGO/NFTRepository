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

    GameObject[,] landItems;

    // Start is called before the first frame update
    void Start()
    {
        buyLand.OnCheck();

        GetComponent<GridLayoutGroup>().constraintCount = landSizeCount;

        landItems = new GameObject[landSizeCount, landSizeCount];

        for (int i = 0; i < landSizeCount; i++)
        {
            for (int j = 0; j < landSizeCount; j++)
            {
                CreateLand(i, j);
            }
        }

        FirebaseFirestore.GetDocumentsInCollection("userLandDetails", this.name, "FetchAllLandData", "FailFetchLandData");
    }

    public void FetchAllLandData(string data)
    {

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
        GameObject landInstance = Instantiate(landPrefab, transform);
        landInstance.GetComponentInChildren<TextMeshProUGUI>().text = i + " : " + j;
        Button landButton = landInstance.GetComponent<Button>();
        landButton.onClick.RemoveAllListeners();
        landButton.onClick.AddListener(() => {
            buyLand.BuyLand(i, j);
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
