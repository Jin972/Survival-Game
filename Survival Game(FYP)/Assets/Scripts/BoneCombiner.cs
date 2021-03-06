using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneCombiner
{
    private readonly Dictionary<int, Transform> _rootBoneDictionary = new Dictionary<int, Transform>();
    private readonly Transform[] _boneTransforms = new Transform[67];

    private readonly Transform _transform;

    public BoneCombiner(GameObject rootObj)
    {
        _transform = rootObj.transform;
        TraverseHierarchy(_transform);
    }



    public Transform AddLimb(SkinnedMeshRenderer bonedObj)
    {
        var limb = ProcessBonedObject(bonedObj);
        limb.SetParent(_transform);
        return limb;
    }

    private Transform ProcessBonedObject(SkinnedMeshRenderer renderer)
    {
        var bonedObject = new GameObject().transform;

        var meshRenderer = bonedObject.gameObject.AddComponent<SkinnedMeshRenderer>();

        var bones = renderer.bones;
        for (var i = 0; i < bones.Length; i++)
        {
            _boneTransforms[i] = _rootBoneDictionary[bones[i].name.GetHashCode()];
        }

        meshRenderer.bones = _boneTransforms;
        meshRenderer.sharedMesh = renderer.sharedMesh;
        meshRenderer.materials = renderer.sharedMaterials;

        return bonedObject;

    }

    public Transform[] GetBone(SkinnedMeshRenderer renderer)
    {
        Transform[] bones = renderer.bones;
        for (var i = 0; i < bones.Length; i++)
        {
            _boneTransforms[i] = _rootBoneDictionary[bones[i].name.GetHashCode()];
        }

        return _boneTransforms;
    }



    private void TraverseHierarchy(IEnumerable transform)
    {
        foreach (Transform child in transform)
        {
            _rootBoneDictionary.Add(child.name.GetHashCode(), child);
            TraverseHierarchy(child);
        }
    }
}
