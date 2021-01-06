using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public MeshRenderer entityRenderer;
    public MeshFilter entityMesh;
    [HideInInspector]
    public string entityName;
    [HideInInspector]
    public Vector3 startPosition;
    protected virtual void Awake()
    {
        startPosition = transform.position;
        entityName = name;
    }
}
