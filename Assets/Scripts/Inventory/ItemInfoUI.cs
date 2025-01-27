using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoUI : MonoBehaviour
{
    public Image icon;
    public Image defaultIcon;
    public TMP_Text nameText, strText, defText, vitText, intText;
    public TMP_Text desc;
    public TMP_Text price;

    public static ItemInfoUI instance {get; private set; }

    private void Awake() {
        instance = this;
    }

    public void SetInfo(ItemObject itemObject, Item item)
    {
        ResetInfo();

        if (itemObject == null)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            return;
        }

        nameText.text = itemObject.data.name;
        icon.sprite = itemObject.iconDisplay;
        desc.text = itemObject.description;
        
        if (itemObject.data.statBonus == null || 
            itemObject.data.statBonus.Length <= 0) return;

        foreach (var statBonus in item.statBonus)
        {
            if (statBonus.attribute == Attributes.Strength)
                strText.text = $"STR: {statBonus.value}";
            else if (statBonus.attribute == Attributes.Defense)
                defText.text = $"DEF: {statBonus.value}";
            else if (statBonus.attribute == Attributes.Vitality)
                vitText.text = $"VIT: {statBonus.value}";
            else if (statBonus.attribute == Attributes.Intelligent)
                intText.text = $"INT: {statBonus.value}";
        }

        price.text = $"Buy: {item.buyCost} / Sell: {item.sellCost}";

        if (!gameObject.transform.GetChild(0).gameObject.activeSelf)
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    public void SetInfoShop(ItemObject itemObject, Item item)
    {
        ResetInfo();

        if (itemObject == null)
        {
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            return;
        }

        nameText.text = itemObject.data.name;
        icon.sprite = itemObject.iconDisplay;
        desc.text = itemObject.description;
        
        if (itemObject.data.statBonus == null || 
            itemObject.data.statBonus.Length <= 0) return;

        foreach (var statBonus in item.statBonus)
        {
            if (statBonus.attribute == Attributes.Strength)
                strText.text = $"STR: {statBonus.min}-{statBonus.max}";
            else if (statBonus.attribute == Attributes.Defense)
                defText.text = $"DEF: {statBonus.min}-{statBonus.max}";
            else if (statBonus.attribute == Attributes.Vitality)
                vitText.text = $"VIT: {statBonus.min}-{statBonus.max}";
            else if (statBonus.attribute == Attributes.Intelligent)
                intText.text = $"INT: {statBonus.min}-{statBonus.max}";
        }

        price.text = $"Buy: {item.buyCost} / Sell: {item.sellCost}";

        if (!gameObject.transform.GetChild(0).gameObject.activeSelf)
            gameObject.transform.GetChild(0).gameObject.SetActive(true);
    }

    private void ResetInfo()
    {
        nameText.text = "";
        icon.sprite = defaultIcon.sprite;
        desc.text = "";

        strText.text = "";
        defText.text = "";
        vitText.text = "";
        intText.text = "";

        price.text = "";
    }
}
