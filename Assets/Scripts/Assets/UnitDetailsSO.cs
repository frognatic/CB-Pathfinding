using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(UnitDetailsSO), menuName = "Scriptables/Assets/" + nameof(UnitDetailsSO))]
public class UnitDetailsSO : AssetType<UnitDetails>
{
    private readonly Dictionary<string, UnitDetails> unitDetailsDictionary = new();
    public override void Init()
    {
        unitDetailsDictionary.Clear();
        foreach (var assetContainer in assetContainers)
        {
            if (unitDetailsDictionary.TryAdd(assetContainer.id, assetContainer.element))
                continue;
            
            Debug.LogError($"Duplicated unit details ID: {assetContainer.id}");
        }
    }

    public UnitDetails GetUnitDetails(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;
        return unitDetailsDictionary.TryGetValue(id, out UnitDetails value) ? value : null;
    }
}

[Serializable]
public class UnitDetails
{
    public Color color;
    public Vector3 spawnPosition;
}
