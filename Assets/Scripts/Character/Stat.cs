using UnityEngine;

public class Stat : MonoBehaviour
{
    public int str;
    public int def;
    public int vit;
    public int lvl;

    // Exp statistics
    public int totalExp; // Player's total exp
    public int threshold;
    private int expGap;

    private void Awake() {
        if (threshold == 0)
        {
            threshold = 15;
        }
        expGap = threshold * 3 / 2;
    }

    private void Update() {
        LevelUp();
    }

    void LevelUp()
    {
        if (totalExp >= threshold)
        {
            lvl += 1;
            threshold += expGap;
            expGap = expGap * 3 / 2;

            AddVit(1);
            AddStr(2);
            AddDef(5);
        }
    }

    public void AddExp(int value)
    {
        totalExp += value;
    }

    public void AddStr(int value)
    {
        str += value;
    }

    public void AddDef(int value)
    {
        def += value;
    }

    public void AddVit(int value)
    {
        vit += value;
        
        GetComponent<HP>().AddHP(value*2);
        GetComponent<HP>().AddHPMax(value*2);
    }
}
