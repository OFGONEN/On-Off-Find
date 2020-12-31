using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using UnityEngine.UI;

public class UISpriteImage : UIEntity
{
    [SerializeField]
    private Image imageRenderer;
    public void SetSprite(Sprite sprite)
    {
        uiTransform.sizeDelta = new Vector2(sprite.textureRect.width, sprite.textureRect.height);
        imageRenderer.sprite = sprite;
    }
}
