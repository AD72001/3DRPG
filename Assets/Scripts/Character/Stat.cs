using UnityEngine;

public class Stat : MonoBehaviour
{
    public int lvl;

    // Exp statistics
    public int totalExp; // Player's total exp
    public int threshold; // Next level's exp
    private int expGap;

    // Base Stats
    public int baseStr;
    public int baseDef;
    public int baseVit;
    public int baseInt;

    // Bonus Stats
    public Attribute[] attributes;

    private void Awake() {
        if (threshold == 0)
        {
            threshold = 15;
        }
        expGap = threshold * 3 / 2;

        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i].SetParent(this);
        }

        if (!GetComponent<CharacterInventory>()) return;

        for (int i = 0; i < GetComponent<CharacterInventory>().equipment.GetSlots.Length; i++)
        {
            GetComponent<CharacterInventory>().equipment.GetSlots[i].OnBeforeUpdate += OnBeforeUpdateSlot;
            GetComponent<CharacterInventory>().equipment.GetSlots[i].OnAfterUpdate += OnAfterUpdateSlot;
        }
    }

    void LevelUp()
    {
        lvl += 1;
        threshold += expGap;
        expGap = expGap * 3 / 2;

        AddVit(1);
        AddStr(2);
        AddDef(5);
        AddInt(5);
    }

    public void AddExp(int _value)
    {
        totalExp += _value;

        if (totalExp >= threshold)
        {
            LevelUp();
        }
    }

    public void AddStr(int _value)
    {
        baseStr += _value;
    }

    public int GetStr()
    {
        return attributes[0].value.ModifiedValue + baseStr;
    }

    public void AddDef(int _value)
    {
        baseDef += _value;
    }

    public int GetDef()
    {
        return attributes[1].value.ModifiedValue + baseDef;
    }

    public void AddVit(int _value)
    {
        baseVit += _value;
        
        GetComponent<HP>().AddHP(_value*2);
        GetComponent<HP>().AddHPMax(_value*2);

        GetComponent<Energy>().AddEnergy(_value*10 + 5);
        GetComponent<Energy>().AddEnergyMax(_value*10 + 5);
    }

    public void UpdateStats()
    {
        GetComponent<HP>().startingHP = GetVit() * 2;
        GetComponent<HP>().currentHP = Mathf.Clamp(GetComponent<HP>().GetHP() + 0, 0, GetComponent<HP>().GetHPMax());

        GetComponent<Energy>().totalEnergy = GetVit() * 10 + 5;
        GetComponent<Energy>().currentEnergy = Mathf.Clamp(
            GetComponent<Energy>().GetEnergy() + 0, 0, GetComponent<Energy>().GetEnergyMax());

        GetComponent<Energy>().regenValue = GetInt() * 0.5f;
    }

    public int GetVit()
    {
        return attributes[2].value.ModifiedValue + baseVit;
    }

    public void AddInt(int _value)
    {
        baseInt += _value;
    }

    public int GetInt()
    {
        return attributes[3].value.ModifiedValue + baseInt;
    }

    public void OnBeforeUpdateSlot(InventorySlot _slot)
    {
        if (_slot == null) return;

        if (_slot.parent.inventory == null) return;

        switch (_slot.parent.inventory.type)
        {
            case InventoryType.Inventory:
                break;
            
            case InventoryType.Equipment:
                if (_slot.item.statBonus == null) break;

                for (int i = 0; i < _slot.item.statBonus.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.statBonus[i].attribute)
                        {
                            attributes[j].value.RemoveModifier(_slot.item.statBonus[i]);
                        }
                    }
                }
                UpdateStats();

                break;

            case InventoryType.Storage:
                break;

            default: break;
        }
    }

    public void OnAfterUpdateSlot(InventorySlot _slot)
    {
        if (_slot == null) return;

        switch (_slot.parent.inventory.type)
        {
            case InventoryType.Inventory:
                break;
            
            case InventoryType.Equipment:
                if (_slot.item.statBonus == null) break;

                for (int i = 0; i < _slot.item.statBonus.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].type == _slot.item.statBonus[i].attribute)
                        {
                            attributes[j].value.AddModifier(_slot.item.statBonus[i]);
                        }
                    }
                }

                UpdateStats();
                break;

            case InventoryType.Storage:
                break;

            default: break;
        }
    }
}
