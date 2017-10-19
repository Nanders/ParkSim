using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HubMotor : MonoBehaviour
{
    public KeyCode axisUp;
    public KeyCode axisDn;

    public float maxSpeed;
    public float pep;

    public float force;

    private HingeJoint hj;
    private JointMotor motor;

    private float speed;

    void Start ()
    {
        hj = GetComponent<HingeJoint>();
        motor = new JointMotor();
    }
	

	void FixedUpdate ()
    {
        if (Input.GetKey(axisUp) && speed < maxSpeed)
            speed += pep;
        if (Input.GetKey(axisDn) && speed > -maxSpeed)
            speed -= pep;

        motor.force = force;
        motor.targetVelocity = speed;
        hj.motor = motor;
    }
}
