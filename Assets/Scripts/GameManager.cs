using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum GameMode
{
    ANGRYMOM,
    TIMEATTACK,
}

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<GameManager>();

                if (instance == null)
                {
                    instance = new GameObject("GameManager").AddComponent<GameManager>();
                }
            }

            return instance;
        }
    }

    public Camera MainCam { get; private set; }

    // COMMON
    int score;
    int combo;
    bool isWarning;
    bool gameOver;
    bool touchable;
    MapController map;
    HUD hud;
    GameMode gameMode;

    // ANGRYMOM
    float momPosition;
    int myPosition;
    float momSpeed;
    float mySpeed;
    int phase;
    private static readonly IReadOnlyList<int> phaseGoals = new List<int> { 100, 250, 500, 1000 };
    AngryMomConfig angryMomConfig;

    // TIMEATTACK
    float timeLeft;
    int targetCode;

    // ETC
    Vector2 pointerBuffer;
    float bufferTimer;
    float AllowbufferTime = 0.3f;

    private void Awake()
    {
        map = Instantiate(Resources.Load<MapController>("Prefabs/MapController"));
        hud = Instantiate(Resources.Load<HUD>("Prefabs/UIs/HUD"));
        angryMomConfig = Resources.Load<AngryMomConfig>("Prefabs/SOs/AngryMomConfig");

        map.onMoved += () => touchable = true;

    }

    private void Start()
    {
        MainCam = Camera.main;
    }

    private void Update()
    {
        if (gameOver) return;

        if (Pointer.current != null && Pointer.current.press.wasPressedThisFrame)
        {
            pointerBuffer = Pointer.current.position.ReadValue();

            bufferTimer = AllowbufferTime;
        }

        if (!touchable)
        {
            bufferTimer -= Time.deltaTime;
        }
        else if (bufferTimer > 0f)
        {
            bufferTimer = 0;

            Ray ray = Camera.main.ScreenPointToRay(pointerBuffer);

            if (Physics.Raycast(ray, out RaycastHit hit, 15f, 1 << LayerMask.NameToLayer("Product")))
            {
                if (hit.collider.TryGetComponent(out Product target))
                {
                    target.OnSelected();
                }
            }
        }

        if (gameMode == GameMode.ANGRYMOM)
        {
            momPosition += momSpeed * Time.deltaTime;
            if (momPosition >= myPosition) GameOver();
            else if (momPosition - myPosition <= angryMomConfig.WarningGap) Warning();


            momSpeed += angryMomConfig.MomSpeedModifier * Time.deltaTime;
            momSpeed = Mathf.Min(momSpeed, angryMomConfig.MomMaxSpeed);

            hud.SetMomDistance(Mathf.RoundToInt(momPosition));
        }

        else if (gameMode == GameMode.TIMEATTACK)
        {
            timeLeft -= Time.deltaTime;
        }
    }

    public void Init(GameMode mode)
    {
        gameMode = mode;
        map.Init(mode);

        switch (mode)
        {
            case GameMode.ANGRYMOM:
                momPosition = 0f;
                myPosition = Mathf.RoundToInt(momPosition) + angryMomConfig.DefaultGap;
                momSpeed = angryMomConfig.MomBaseSpeed;
                mySpeed = angryMomConfig.MyBaseSpeed;

                hud.SetGoal(0, phaseGoals[phase]);
                hud.SetMomDistance(Mathf.RoundToInt(momPosition));
                hud.SetKidDistance(myPosition);
                break;
            case GameMode.TIMEATTACK:
                timeLeft = 60f;
                //targets 뽑기
                break;
        }

        score = 0;
        combo = 0;
        isWarning = false;
        gameOver = false;
        touchable = true;

        hud.SetCountText(score.ToString());
        hud.SetComboText(combo.ToString());
    }

    public bool IsValid(int id)
    {
        if (gameMode == GameMode.ANGRYMOM)
        {
            var item = DataManager.Instance.GetItem(id);

            if (item == null) return false;

            // 1페이즈가 되면 1티어(특대 사이즈) 아이템은 제외됨
            if (phase >= item.Tier) return false;

            return true;
        }

        else if (gameMode == GameMode.TIMEATTACK)
        {
            return (id & targetCode) != 0;
        }

        return false;
    }

    public void PickItem(int id)
    {
        touchable = false;

        if (gameMode == GameMode.ANGRYMOM)
        {
            if (IsValid(id))
            {
                score++;
                combo++;
                mySpeed += angryMomConfig.MySpeedModifier;
                mySpeed = Mathf.Min(mySpeed, angryMomConfig.MyMaxSpeed);

            }
            else
            {
                // 너무 큰 물건을 고름
                combo = 0;
                mySpeed = angryMomConfig.MyBaseSpeed;
            }

            if (myPosition >= phaseGoals[phase])
            {
                hud.SetGoal(phaseGoals[phase++], phaseGoals[phase]);
            }
            
            hud.SetKidDistance(myPosition++);
            hud.SetCountText(score.ToString());
            hud.SetComboText(combo.ToString());
        }

        else if (gameMode == GameMode.TIMEATTACK)
        {
            if (IsValid(id))
            {

            }
            else
            {
                // 메모지에 없는 아이템을 고름
            }
        }

        map.Move(mySpeed);
    }

    public void ShowPickupAnimation(Vector3 itemWorldPosition, int itemId)
    {
        var item = DataManager.Instance.GetItem(itemId);

        if (item == null) return;

        Vector3 screenPos = MainCam.WorldToScreenPoint(itemWorldPosition);

        hud.PlayPickupAnimation(screenPos, item.Sprite);
    }

    public void GameOver()
    {
        if (gameOver) return;

        gameOver = true;
    }

    public void Warning()
    {
        if (isWarning) return;

        isWarning = true;
    }
}