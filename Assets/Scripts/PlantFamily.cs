using UnityEngine;

public class PlantFamily : MonoBehaviour
{
    public float temperaturePref;
    public float temperatureTolerance;
    public int temperatureBonusLvl = 1;
    public float humidityPref;
    public float humidityTolerance;
    public int humidityBonusLvl = 1;
    public float sulfurTolerance;
    public int sulfurBonusLvl = 1;
    public byte groundTypePref;
    public string plantName;
    public GameObject plantPrefab;
}