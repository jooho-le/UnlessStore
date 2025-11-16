using UnityEngine;


public class Product : MonoBehaviour
{
    [SerializeField] SpriteRenderer sr;
    [SerializeField] float defaultScale = 0.5f;

    int id;

    public void Init(int itemId)
    {
        var item = DataManager.Instance.GetItem(itemId);
        if (item == null) return;

        id = item.Id;
        sr.sprite = item.Sprite;
        transform.localScale = Vector3.one * item.Scale * defaultScale;

        gameObject.SetActive(true);
    }

    public void OnSelected()
    {
        GameManager.Instance.PickItem(id);

        if (GameManager.Instance.IsValid(id))
        {
            GameManager.Instance.ShowPickupAnimation(transform.position, id);
        }

        gameObject.SetActive(false);
    }
}
