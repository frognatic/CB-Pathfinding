using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(SpritesSO), menuName = "Scriptables/Assets/" + nameof(SpritesSO))]
public class SpritesSO : AssetType<Sprite>
{
    private readonly Dictionary<string, Sprite> spritesDictionary = new();
    public override void Init()
    {
        spritesDictionary.Clear();
        foreach (var assetContainer in assetContainers)
        {
            if (spritesDictionary.TryAdd(assetContainer.id, assetContainer.element))
                continue;
            
            Debug.LogError($"Duplicated sprite ID: {assetContainer.id}");
        }
    }

    public Sprite GetSprite(string id)
    {
        if (string.IsNullOrEmpty(id)) return null;
        return spritesDictionary.TryGetValue(id, out Sprite value) ? value : null;
    }
}
