using System.Collections.Generic;
using System.Linq;
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

    private void Update()
    {
        for (int i = 0; i < doors.Count; i++)
        {
            MeshRenderer meshRenderer = doors[i].GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                if (!meshRenderer.enabled)
                {
                    doors.RemoveAt(i);
                }
            }
            else
            {
                doors.RemoveAt(i);
            }
        }

        for (int i = 0; i < buttons.Count; i++)
        {
            if (!buttons[i].activeSelf)
            {
                buttons.RemoveAt(i);
            }
        }

        for (int i = 0; i < healthSpots.Count; i++)
        {
            if (!healthSpots[i].activeSelf)
            {
                healthSpots.RemoveAt(i);
            }
        }

        var keysToRemove = healthSpotsAvailable
        .Where(pair => !pair.Key.activeSelf)
        .Select(pair => pair.Key)
        .ToList();
        foreach (var key in keysToRemove)
        {
            healthSpotsAvailable.Remove(key);
        }

        for (int i = healthSpotsAvailableList.Count - 1; i >= 0; i--)
        {
            if (!healthSpotsAvailableList[i].key.activeSelf)
            {
                healthSpotsAvailableList.RemoveAt(i);
            }
        }

        for (int i = 0; i < rooms.Count; i++)
        {
            if (!rooms[i].activeSelf)
            {
                rooms.RemoveAt(i);
            }
        }
    }
}
