using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] TMP_Text comboText;
    [SerializeField] TMP_Text countText;

    [SerializeField] Button pauseBtn;

    [SerializeField] Image item;
    [SerializeField] Image cart;
    [SerializeField] List<Sprite> carts;

    [SerializeField] Image kid;
    [SerializeField] Sprite kidStand;
    [SerializeField] List<Sprite> kidLefts;
    [SerializeField] List<Sprite> kidRights;

    [SerializeField] Slider kidSlider;
    [SerializeField] TMP_Text kidDistanceText;
    [SerializeField] Slider momSlider;
    [SerializeField] TMP_Text momDistanceText;

    public void SetComboText(string text)
    {
        comboText.text = text;
    }

    public void SetCountText(string text)
    {
        countText.text = text;
    }

    public void SetGoal(int min, int max)
    {
        momSlider.minValue = min;
        momSlider.maxValue = max;

        kidSlider.minValue = min;
        kidSlider.maxValue = max;
    }

    public void SetMomDistance(int distance)
    {
        momDistanceText.text = distance.ToString();
        momSlider.value = distance;
    }
    public void SetKidDistance(int distance)
    {
        kidDistanceText.text = distance.ToString();
        kidSlider.value = distance;
    }

    public void PlayPickupAnimation(Vector3 start, Sprite sprite)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform,
            start,
            null,
            out Vector2 uiPos
        );

        // 2) UI 아이콘 준비
        item.sprite = sprite;
        RectTransform iconRT = item.GetComponent<RectTransform>();
        iconRT.anchoredPosition = uiPos;
        iconRT.localScale = Vector3.one * 0.8f;
        item.gameObject.SetActive(true);

        // 3) 코루틴 실행
        StartCoroutine(CoItemAnimation(iconRT, cart.GetComponent<RectTransform>()));


        var left = start.x < Screen.width * 0.5f;

        StartCoroutine(CoKidAnimation(left));
    }

    private IEnumerator CoItemAnimation(RectTransform iconRT, RectTransform target)
    {
        float duration = 0.6f;
        float t = 0f;

        Vector2 startPos = iconRT.anchoredPosition;
        Vector2 endPos = target.anchoredPosition;

        Vector3 startScale = Vector3.one;
        Vector3 endScale = Vector3.one * 0.3f;

        // InQuad(Ease.InQuad) 비슷한 가속 이동을 직접 구현
        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            float easedT = t * t; // InQuad: t^2

            iconRT.anchoredPosition = Vector2.Lerp(startPos, endPos, easedT);
            iconRT.localScale = Vector3.Lerp(startScale, endScale, easedT);

            yield return null;
        }

        // 연출 종료 후 비활성화
        item.gameObject.SetActive(false);
    }

    private IEnumerator CoKidAnimation(bool left)
    {
        // kid
        var kids = left ? kidLefts : kidRights;
        cart.sprite = left ? carts[0] : carts[2];

        for (int i = 0; i < kids.Count; i++)
        {
            kid.sprite = kids[i];

            yield return new WaitForSeconds(0.15f);
        }

        cart.sprite = carts[1];

        kid.sprite = kidStand;
    }
}
