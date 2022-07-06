using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyLandViewer : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI itemName;

    [SerializeField]
    TextMeshProUGUI itemOwnedBy;

    public void SetItemDetails(LandTokenData tokenData)
    {
        itemName.text = tokenData.landName;
        itemOwnedBy.text = tokenData.tokenOwner;
    }
}
