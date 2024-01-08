using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class AssetSO : ScriptableObject
{
    public abstract void Init();
}

public abstract class AssetType<T> : AssetSO
{
    [Serializable]
    public class AssetContainer
    {
        public string id;
        public T element;
    }

    public List<AssetContainer> assetContainers;
}
