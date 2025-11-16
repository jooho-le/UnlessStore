using UnityEngine;

[CreateAssetMenu(fileName = "AngryMomConfig", menuName = "Scriptable Objects/AngryMomConfig")]
public class AngryMomConfig : ScriptableObject
{
    [field: SerializeField] public float MomBaseSpeed { get; private set; }
    [field: SerializeField] public float MomMaxSpeed { get; private set; }
    [field: SerializeField] public float MomSpeedModifier { get; private set; }


    [field: SerializeField] public float MyBaseSpeed { get; private set; }
    [field: SerializeField] public float MyMaxSpeed { get; private set; }
    [field: SerializeField] public float MySpeedModifier { get; private set; }

    [field: SerializeField] public int DefaultGap { get; private set; }
    [field: SerializeField] public int WarningGap { get; private set; }
}
