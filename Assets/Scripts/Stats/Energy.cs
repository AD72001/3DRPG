using System;
using UnityEngine;

public class Energy : MonoBehaviour
{
    public float totalEnergy;
    [NonSerialized] public float currentEnergy;
    [SerializeField] private float regenRate;
    [NonSerialized] public float regenValue;
    private float regenCD = 0;

    private void Awake() {
        if (totalEnergy == 0)
        {
            totalEnergy = GetComponent<Stat>().GetVit() * 10 + 5;
        }

        regenValue = GetComponent<Stat>().GetInt() * 0.5f;

        currentEnergy = totalEnergy;
    }

    private void FixedUpdate() {

        if (regenCD >= regenRate)
        {
            regenCD = 0;
            AddEnergy(regenValue);
        }

        regenCD += Time.deltaTime;
    }

    public float GetEnergy()
    {
        return currentEnergy;
    }

    public float GetEnergyMax()
    {
        return totalEnergy;
    }

    public void AddEnergy(float value)
    {
        currentEnergy = Mathf.Clamp(currentEnergy + value, 0, totalEnergy);
    }

    public void AddEnergyMax(float value)
    {
        totalEnergy += value;
    }

}
