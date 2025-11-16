using UnityEngine;

public class DataManager : MonoBehaviour
{
    private static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<DataManager>();

                if (instance == null)
                {
                    instance = new GameObject("DataManager").AddComponent<DataManager>();
                }
            }

            return instance;
        }
    }

    ItemSO itemSO;

    private void Awake()
    {
        itemSO = Resources.Load<ItemSO>("Prefabs/SOs/ItemSO");
    }

    public Item GetItem(int id) => itemSO ? itemSO.GetItem(id) : null;
    public int GetItemsCount() => itemSO.Count;
}