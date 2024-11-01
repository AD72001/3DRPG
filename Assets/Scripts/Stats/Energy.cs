using UnityEngine;

public class Energy : MonoBehaviour
{
    [SerializeField] private float totalEnergy;
    private float currentEnergy;
    [SerializeField] private float regenRate;
    [SerializeField] private float regenValue;
    private float regenCD = 0;

    private void Awake() {
        if (totalEnergy == 0)
        {
            totalEnergy = GetComponent<Stat>().vit * 10 + 5;
        }

        currentEnergy = totalEnergy;
    }

    private void Update() {

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
