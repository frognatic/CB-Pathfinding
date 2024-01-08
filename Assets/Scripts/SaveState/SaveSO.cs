using UnityEngine;

public class SaveSO : ScriptableObject
{
    public const string Path = "Assets/Res/Editor/ScriptableSave.asset";

    [SerializeField] public SaveState save;
}
