using UnityEngine;
using DG.Tweening;
using FFStudio;

public class DisappearingEntity : Entity
{
    public DisappearingEntitySet disappearingEntitySet;
    public DisappearingEntitySet disappearedEntitySet;

    public ParticleObserver rightPlaceFX;
    public ParticleObserver rayFX;
    public GameEvent rightPlaceSound;
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
        rightPlaceFX.transform.rotation = transform.rotation;

        rayFX.transform.position = entityRenderer.bounds.center;
    }
    public void Disappear()
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        gameObject.SetActive(false);
        disappearedEntitySet.AddDictionary(entityName, this);
    }
    public void Reappear()
    {
        gameObject.SetActive(true);
        disappearedEntitySet.RemoveDictionary(entityName);

        var _big = new Vector3(1.15f, 1.15f, 1.15f);
        var _mid = new Vector3(0.75f, 0.75f, 0.75f);

        var _squence = DOTween.Sequence();

        _squence.Append(transform.DOScale(_big, 0.5f));
        _squence.Append(transform.DOScale(_mid, 0.125f));
        _squence.Append(transform.DOScale(Vector3.one, 0.125f));

        _squence.AppendCallback(PlayRightPlaceFX);
    }

    void PlayRightPlaceFX()
    {
        rightPlaceSound.Raise();
        rightPlaceFX.gameObject.SetActive(true);
        rightPlaceFX.particles.Play();
    }
}
