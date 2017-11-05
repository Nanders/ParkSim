using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnappableObject : MonoBehaviour
{
    public SnapConnectorInfo[] snapConnectors;
    public bool Intersecting;
    private bool set;
	// Use this for initialization
	void Start ()
    {
        snapConnectors = GetComponentsInChildren<SnapConnectorInfo>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (PlayerController.obj.focus == gameObject && snapConnectors[0].snapCollider.enabled)
            foreach (var snapPoint in snapConnectors)
                snapPoint.snapCollider.enabled = false;
        else if (PlayerController.obj.focus != gameObject && !snapConnectors[0].snapCollider.enabled)
            foreach (var snapPoint in snapConnectors)
                snapPoint.snapCollider.enabled = true;
    }
}
