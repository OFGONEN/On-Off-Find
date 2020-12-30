﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingEntity : Entity
{
    public DisappearingEntitySet disappearingEntitySet;
    public DisappearingEntitySet disappearedEntitySet;
    private void OnEnable()
    {
        disappearingEntitySet.AddDictionary(entityName, this);
    }
    private void OnDisable()
    {
        disappearingEntitySet.RemoveDictionary(entityName);
    }
    public void Disappear()
    {
        gameObject.SetActive(false);
        disappearedEntitySet.AddDictionary(entityName, this);
    }
    public void Reappear()
    {
        gameObject.SetActive(true);
        disappearedEntitySet.RemoveDictionary(entityName);
    }
}
