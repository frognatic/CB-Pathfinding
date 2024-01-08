using UnityEngine;

public class SaveSO : ScriptableObject
{
    public const string Path = "Assets/Resources/Editor/ScriptableSave.asset";

    [SerializeField] public SaveState save;
}
