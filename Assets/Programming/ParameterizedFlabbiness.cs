//Using C#

using UnityEngine;
using System.Collections;

public class ParameterizedFlabbiness: MonoBehaviour
{
    public float Flabbiness;

    SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh skinnedMesh;

    void Awake()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        skinnedMesh = skinnedMeshRenderer.sharedMesh;
    }

    void Start()
    {

    }

    void Update()
    {
        Mathf.Clamp(Flabbiness, 0, 100);
        // 100 - because I modelled him at full flabbiness originally
        skinnedMeshRenderer.SetBlendShapeWeight(0, 100 - Flabbiness);
    }
}