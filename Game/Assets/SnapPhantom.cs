using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SnapPhantom : MonoBehaviour
{
    public GameObject[] children;

    public void SetChildTransforms(SnappableObject snapObject)
    {
        foreach (var child in children)
            Destroy(child);

        var snapConnectors = snapObject.snapConnectors;
        children = new GameObject[snapConnectors.Length];

        for (int i = 0; i < snapConnectors.Length; i++)
        {
            children[i] = new GameObject("Connector_" + i);
            children[i].transform.SetParent(transform);
            children[i].transform.localPosition = snapConnectors[i].transform.localPosition;
        }
    }
}
