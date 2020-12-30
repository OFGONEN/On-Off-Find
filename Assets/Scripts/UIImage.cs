using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using UnityEngine.UI;

public class UIImage : UIEntity
{
    [SerializeField]
    private Image imageRenderer;
    Vector2 imageSizeDelta;
    private void Awake()
    {
        imageSizeDelta = uiTransform.sizeDelta;
    }

    public void SetSprite(Sprite sprite)
    {
        if (imageSizeDelta.x > sprite.textureRect.width && imageSizeDelta.y > sprite.textureRect.height)
        {
            imageRenderer.preserveAspect = false;
            uiTransform.sizeDelta = new Vector2(sprite.textureRect.width, sprite.textureRect.height);
        }
        else
        {
            uiTransform.sizeDelta = imageSizeDelta;
            imageRenderer.preserveAspect = true;
        }

        imageRenderer.sprite = sprite;
    }
}
