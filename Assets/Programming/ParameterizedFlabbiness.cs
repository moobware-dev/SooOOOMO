//Using C#

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ParameterizedFlabbiness: MonoBehaviour
{
    public float Flabbiness;

    SkinnedMeshRenderer skinnedMeshRenderer;

    void Awake()
    {
        skinnedMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
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