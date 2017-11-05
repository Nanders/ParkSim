using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapConnectorInfo : MonoBehaviour
{
    public SphereCollider snapCollider { get; private set; }

	// Use this for initialization
	void Start ()
    {
        snapCollider = GetComponent<SphereCollider>();
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        
    }
}
