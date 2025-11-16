using UnityEngine;
using UnityEngine.UI;

public class ModeUI : MonoBehaviour
{
    [SerializeField] Button _anrgyMom;
    [SerializeField] Button _timeAttack;
    [SerializeField] Button _start;

    GameMode _selectMode;

    private void Awake()
    {
        _anrgyMom.onClick.AddListener(() => SetMode(GameMode.ANGRYMOM));
        _timeAttack.onClick.AddListener(() => SetMode(GameMode.TIMEATTACK));

        _start.onClick.AddListener(StartGame);
    }

    private void SetMode(GameMode mode)
    {
        _selectMode = mode;
        _start.interactable = true;
    }

    private void StartGame()
    {
        GameManager.Instance.Init(_selectMode);
        gameObject.SetActive(false);
    }
}
