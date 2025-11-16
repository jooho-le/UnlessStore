using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] List<Product> products;

    Vector3 destination;
    static bool guarantee;

    public void SetDestination(Vector3 destination)
    {
        this.destination = destination;
    }

    public bool MoveTowards(float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, destination, 0.1f * speed);

        return (Vector3.Distance(transform.position, destination) > 0.05f);
    }

    public void Stop()
    {
        transform.position = destination;
    }

    public static void Init(bool grnt)
    {
        guarantee = grnt;
    }

    public void Init()
    {
        SetProductsRandomly();

        if (guarantee)
        {
            SetProductValidly();
        }
    }

    private void SetProductsRandomly()
    {
        var maxId = DataManager.Instance.GetItemsCount();

        foreach (var product in products)
        {
            var id = Random.Range(0, maxId);
            product.gameObject.SetActive(true);
            product.Init(id);
        }
    }

    private void SetProductValidly()
    {
        var maxId = DataManager.Instance.GetItemsCount();

        int randomItemId;
        do {
            randomItemId = Random.Range(0, maxId);
            if (DataManager.Instance.GetItem(randomItemId) == null) break;
        } while (!GameManager.Instance.IsValid(randomItemId));

        var product = products[Random.Range(0, products.Count)];
        product.gameObject.SetActive(true);
        product.Init(randomItemId);
    }
}