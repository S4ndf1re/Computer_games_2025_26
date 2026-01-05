using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class FillBoneRenderer : MonoBehaviour
{
    public Transform rootTransform;
    public BoneRenderer boneRenderer;



    [ContextMenu("Fill")]
    public void Fill()
    {
        if (rootTransform != null && boneRenderer != null)
        {
            List<Transform> transforms = new();
            List<Transform> bonesToVisist = new();
            bonesToVisist.Add(rootTransform);
            while (bonesToVisist.Count > 0)
            {
                var current = bonesToVisist[bonesToVisist.Count - 1];
                bonesToVisist.RemoveAt(bonesToVisist.Count - 1);

                for (var i = 0; i < current.childCount; i++)
                {
                    bonesToVisist.Add(current.GetChild(i));
                }

                transforms.Add(current);
            }
            // boneRenderer.transforms = transforms.ToArray();
        }
    }

}
