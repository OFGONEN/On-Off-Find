//
//  Outline.cs
//  QuickOutline
//
//  Created by Chris Nolet on 3/30/18.
//  Copyright © 2018 Chris Nolet. All rights reserved.
//
//  Modified by Burak Canik on 05/01/20.
//      Changes: Main modification is elimination of runtime addition of 2 materials to an already pre-existing material.
//               Updated version contains only 1 shader/material on the gameobject. Trade-off is, since there can
//               not be 2 SubShader tags in a SubShader, various outline modes do not work anymore. There is either
//               full outline around the object or none.
//               Also MaterialPropertyBlock is utilized instead of modifying the material directly, which should help
//               reduce overhead of multiple material instances.
//

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using FFStudio;
using DG.Tweening;

[DisallowMultipleComponent]

public class Outlined_Object : MonoBehaviour
{
    private static HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();

    //public enum Mode
    //{
    //    OutlineAll,
    //    OutlineVisible,
    //    OutlineHidden,
    //    OutlineAndSilhouette,
    //    SilhouetteOnly
    //}

    //public Mode OutlineMode
    //{
    //    get { return outlineMode; }
    //    set
    //    {
    //        outlineMode = value;
    //        needsUpdate = true;
    //    }
    //}

    //public Color OutlineColor
    //{
    //    get { return outlineColor; }
    //    set
    //    {
    //        outlineColor = value;
    //        needsUpdate = true;
    //    }
    //}

    //public float OutlineWidth
    //{
    //    get { return outlineWidth; }
    //    set
    //    {
    //        outlineWidth = value;
    //        needsUpdate = true;
    //    }
    //}

    [Serializable]
    private class ListVector3
    {
        public List<Vector3> data;
    }

    //[SerializeField]
    //private Mode outlineMode;

    public OutlinedObjectSet outlinedObjectSet;
    [SerializeField]
    private Color outlineColor = Color.white;

    public float outlineWidth = 2f;
    float defaultOutLineWidth;
    public CustomGameSettings gameSettings;

    public EventListenerDelegateResponse lightsTurnOffResponse;
    public EventListenerDelegateResponse lightsTurnedOnResponse;
    public EventListenerDelegateResponse startLevelResponse;
    public EventListenerDelegateResponse newLevelLoadingResponse;

    [Header("Optional")]

    [SerializeField, Tooltip("Precompute enabled: Per-vertex calculations are performed in the editor and serialized with the object. "
                            + "Precompute disabled: Per-vertex calculations are performed at runtime in Awake(). This may cause a pause for large meshes.")]
    private bool precomputeOutline = true;

    [SerializeField, HideInInspector]
    private List<Mesh> bakeKeys = new List<Mesh>();

    [SerializeField, HideInInspector]
    private List<ListVector3> bakeValues = new List<ListVector3>();

    private Renderer rend;

    private MaterialPropertyBlock materialPropertyBlock;

    private bool needsUpdate;

    private void OnEnable()
    {
        outlinedObjectSet.AddDictionary(GetInstanceID(), this);
        lightsTurnedOnResponse.OnEnable();
        lightsTurnOffResponse.OnEnable();
        startLevelResponse.OnEnable();
        newLevelLoadingResponse.OnEnable();
    }
    private void OnDisable()
    {
        outlinedObjectSet.RemoveDictionary(GetInstanceID());

        lightsTurnedOnResponse.OnDisable();
        lightsTurnOffResponse.OnDisable();
        startLevelResponse.OnDisable();
        newLevelLoadingResponse.OnDisable();
    }


    private void Awake()
    {
        defaultOutLineWidth = outlineWidth;
        outlineWidth = 0;
        // Cache renderer.
        rend = GetComponent<Renderer>();

        rend.material = Resources.Load<Material>("Materials/Outlined_Object");

        materialPropertyBlock = new MaterialPropertyBlock();

        // Retrieve or generate smooth normals.
        LoadSmoothNormals();

        // Apply material properties immediately.
        needsUpdate = true;
    }

    private void Start()
    {
        lightsTurnOffResponse.response = LightsTurnOffResponse;
        lightsTurnedOnResponse.response = LightTurnedOnResponse;
        startLevelResponse.response = LightTurnedOnResponse;
        newLevelLoadingResponse.response = () => { outlineWidth = 0; UpdateMaterialProperties(); };
    }
    public void LightsTurnOffResponse()
    {
        // DOTween.To(() => outlineWidth, x => outlineWidth = x, 0, gameSettings.outlineTurnOffDuration).SetEase(gameSettings.outlineTurnOffCurve).OnUpdate(UpdateMaterialProperties);
        DOTween.To(() => outlineWidth, x => outlineWidth = x, 0, gameSettings.outlineTurnOffDuration).OnUpdate(UpdateMaterialProperties);
    }
    public void LightTurnedOnResponse()
    {
        DOTween.To(() => outlineWidth, x => outlineWidth = x, defaultOutLineWidth, 0.2f).OnUpdate(UpdateMaterialProperties);
    }

    private void OnValidate()
    {
        // Update material properties.
        needsUpdate = true;

        // Clear cache when baking is disabled or corrupted.
        if (!precomputeOutline && bakeKeys.Count != 0 || bakeKeys.Count != bakeValues.Count)
        {
            bakeKeys.Clear();
            bakeValues.Clear();
        }

        // Generate smooth normals when baking is enabled.
        if (precomputeOutline && bakeKeys.Count == 0)
            Bake();
    }

    private void Update()
    {
        if (needsUpdate)
        {
            needsUpdate = false;

            UpdateMaterialProperties();
        }
    }

    private void Bake()
    {
        // Generate smooth normals for each mesh.
        var bakedMeshes = new HashSet<Mesh>();

        foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
        {
            // Skip duplicates.
            if (!bakedMeshes.Add(meshFilter.sharedMesh))
                continue;

            // Serialize smooth normals.
            var smoothNormals = SmoothNormals(meshFilter.sharedMesh);

            bakeKeys.Add(meshFilter.sharedMesh);
            bakeValues.Add(new ListVector3() { data = smoothNormals });
        }
    }

    private void LoadSmoothNormals()
    {
        // Retrieve or generate smooth normals.
        foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
        {
            // Skip if smooth normals have already been adopted.
            if (!registeredMeshes.Add(meshFilter.sharedMesh))
                continue;

            // Retrieve or generate smooth normals.
            var index = bakeKeys.IndexOf(meshFilter.sharedMesh);
            var smoothNormals = (index >= 0) ? bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);

            // Store smooth normals in UV3.
            meshFilter.sharedMesh.SetUVs(3, smoothNormals);
        }

        // Clear UV3 on skinned mesh renderers
        foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
        {
            if (registeredMeshes.Add(skinnedMeshRenderer.sharedMesh))
                skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];
        }
    }

    private List<Vector3> SmoothNormals(Mesh mesh)
    {
        // Group vertices by location.
        var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

        // Copy normals to a new list.
        var smoothNormals = new List<Vector3>(mesh.normals);

        // Average normals for grouped vertices.
        foreach (var group in groups)
        {
            // Skip single vertices.
            if (group.Count() == 1)
                continue;

            // Calculate the average normal.
            var smoothNormal = Vector3.zero;

            foreach (var pair in group)
                smoothNormal += mesh.normals[pair.Value];

            smoothNormal.Normalize();

            // Assign smooth normal to each vertex.
            foreach (var pair in group)
                smoothNormals[pair.Value] = smoothNormal;
        }

        return smoothNormals;
    }

    private void UpdateMaterialProperties()
    {
        // Apply properties according to mode.
        rend.GetPropertyBlock(materialPropertyBlock);
        materialPropertyBlock.SetColor("_OutlineColor", outlineColor);

        //switch( outlineMode )
        //{
        //    case Mode.OutlineAll:
        materialPropertyBlock.SetFloat("_MaskZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
        materialPropertyBlock.SetFloat("_FillZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
        materialPropertyBlock.SetFloat("_OutlineWidth", outlineWidth);
        //        break;

        //    case Mode.OutlineVisible:
        //        materialPropertyBlock.SetFloat( "_MaskZTest", ( float )UnityEngine.Rendering.CompareFunction.Always );
        //        materialPropertyBlock.SetFloat( "_FillZTest", ( float )UnityEngine.Rendering.CompareFunction.LessEqual );
        //        materialPropertyBlock.SetFloat( "_OutlineWidth", outlineWidth );
        //        break;

        //    case Mode.OutlineHidden:
        //        materialPropertyBlock.SetFloat( "_MaskZTest", ( float )UnityEngine.Rendering.CompareFunction.Always );
        //        materialPropertyBlock.SetFloat( "_FillZTest", ( float )UnityEngine.Rendering.CompareFunction.Greater );
        //        materialPropertyBlock.SetFloat( "_OutlineWidth", outlineWidth );
        //        break;

        //    case Mode.OutlineAndSilhouette:
        //        materialPropertyBlock.SetFloat( "_MaskZTest", ( float )UnityEngine.Rendering.CompareFunction.LessEqual );
        //        materialPropertyBlock.SetFloat( "_FillZTest", ( float )UnityEngine.Rendering.CompareFunction.Always );
        //        materialPropertyBlock.SetFloat( "_OutlineWidth", outlineWidth );
        //        break;

        //    case Mode.SilhouetteOnly:
        //        materialPropertyBlock.SetFloat( "_MaskZTest", ( float )UnityEngine.Rendering.CompareFunction.LessEqual );
        //        materialPropertyBlock.SetFloat( "_FillZTest", ( float )UnityEngine.Rendering.CompareFunction.Greater );
        //        materialPropertyBlock.SetFloat( "_OutlineWidth", 0 );
        //        break;
        //}

        rend.SetPropertyBlock(materialPropertyBlock);
    }
}
