using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CenterOfMass : MonoBehaviour
{
    Rigidbody rb;
    public Vector3 centerOfMass;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
	}

}
