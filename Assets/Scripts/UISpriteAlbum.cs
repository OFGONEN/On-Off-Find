using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SpriteAlbum", menuName = "FF/Data/SpriteAlbum")]
public class UISpriteAlbum : ScriptableObject
{
    public List<Sprite> spriteList;
    int index;

    public Sprite GiveSprite()
    {
        index = index % spriteList.Count;

        var _returnSprite = spriteList[index];
        index++;

        return _returnSprite;
    }
}
