using UnityEngine;

public class Paper2D : MonoBehaviour
{
    private void Update()
    {
        transform.LookAt(GameManager.Instance.MainCam.transform.position);
    }
}