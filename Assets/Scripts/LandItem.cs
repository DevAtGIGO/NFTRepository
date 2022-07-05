using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LandItem : MonoBehaviour
{
    public Button itemButton;
    TextMeshProUGUI itemName;
    Image itemImage;

    // Start is called before the first frame update
    void Start()
    {
        itemButton = this.GetComponent<Button>();
        itemImage = this.GetComponent<Image>();
        itemName = this.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnItemUpdate(string name, Color buttonColor)
    {
        if(itemName == null)
        {
            itemButton = this.GetComponent<Button>();
            itemImage = this.GetComponent<Image>();
            itemName = this.GetComponentInChildren<TextMeshProUGUI>();
        }
        itemName.text = name;
        itemImage.color = buttonColor;
    }
}
