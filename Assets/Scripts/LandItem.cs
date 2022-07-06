using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LandItem : MonoBehaviour
{
    [SerializeField]
    public Button itemButton;

    [SerializeField]
    TextMeshProUGUI itemName;

    [SerializeField]
    Image itemImage;

    public void OnItemUpdate(string name, Color buttonColor)
    {
        itemName.text = name;
        itemImage.color = buttonColor;
    }
}
