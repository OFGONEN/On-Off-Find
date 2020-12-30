using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingEntity : Entity
{
    public DisappearingEntitySet disappearingEntitySet;
    public DisappearingEntitySet disappearedEntitySet;

    public ParticleObserver rightPlaceFX;
    public ParticleObserver rayFX;
    private void OnEnable()
    {
        disappearingEntitySet.AddDictionary(entityName, this);
    }
    private void OnDisable()
    {
        disappearingEntitySet.RemoveDictionary(entityName);
    }
    public void SetFX()
    {
        rightPlaceFX.particleRenderer.mesh = entityMesh.mesh;

        rightPlaceFX.particleStoped = () =>
        {
            rayFX.gameObject.SetActive(true);
            rayFX.particles.Play();
        };

        rightPlaceFX.transform.position = transform.position;
        rayFX.transform.position = transform.position;
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

        rightPlaceFX.gameObject.SetActive(true);
        rightPlaceFX.particles.Play();
    }
}
