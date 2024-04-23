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
    public Dictionary<GameObject, List<GameObject>> healthSpotsAvailable = new Dictionary<GameObject, List<GameObject>>();
    public List<GameObject> waypoints;

    public List<GameObject> buttons;
    public List<GameObject> doors;
    public List<GameObject> rooms;

    public GameObject exit;

    void Awake()
    {
        foreach (var pair in healthSpotsAvailableList)
        {
            healthSpotsAvailable.Add(pair.key, pair.value);
        }
    }
}
