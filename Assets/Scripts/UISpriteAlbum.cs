using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "SpriteAlbum", menuName = "FF/Data/SpriteAlbum")]
public class UISpriteAlbum : ScriptableObject
{
    public List<Sprite> spriteList;
    public int index;

    public Sprite GiveSprite()
    {
        var _returnSprite = spriteList[index];
        index++;

        index = index % spriteList.Count;

        return _returnSprite;
    }
}
