using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyValuePairs<TKey, TValue>
{
    public TKey key;
    public TValue value;
}

public class Register : MonoBehaviour
{
    public List<GameObject> enemies;
    public List<GameObject> hidingSpots;
    public List<GameObject> healthSpots;
    public List<KeyValuePairs<GameObject, List<GameObject>>> healthSpotsAvailableList = new List<KeyValuePairs<GameObject, List<GameObject>>>();
    public Dictionary<GameObject, List<GameObject>> healthSpotsAvailable;
    public List<GameObject> waypoints;

    void Awake()
    {
        foreach (var pair in healthSpotsAvailableList)
        {
            if (pair.value == null || pair.value.Count <= 0)
            {
                healthSpotsAvailable.Add(pair.key, null);
            }
            else
            {
                healthSpotsAvailable.Add(pair.key, pair.value);
            }
        }
    }
}
