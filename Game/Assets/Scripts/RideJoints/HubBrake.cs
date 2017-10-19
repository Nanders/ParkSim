using UnityEngine;

public class HubBrake : MonoBehaviour
{
    public KeyCode brakeKey;
    public float brakeForce;
    public float springDamper;

    private Quaternion rot;
    private Rigidbody rb;
    private HingeJoint hj;
    private JointMotor motor;
    private JointSpring spring;

    void Start ()
    {
        motor = new JointMotor();
        motor.targetVelocity = 0;
        motor.force = brakeForce;

        spring = new JointSpring();
        spring.spring = brakeForce;
        spring.damper = springDamper;

        hj = GetComponent<HingeJoint>();
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        //MethodA();
        //MethodB();
        MethodC();
    }

    void MethodA()
    {
        if (Input.GetKeyDown(brakeKey))
        {
            rot = transform.rotation * Quaternion.Inverse(hj.connectedBody.gameObject.transform.rotation);
        }

        if (Input.GetKey(brakeKey))
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX;
            rb.rotation = hj.connectedBody.gameObject.transform.rotation * rot;
        }
        else
        {
            rot = rb.rotation;
            rb.constraints = RigidbodyConstraints.None;
        }
    }

    void MethodB()
    {
        hj.useMotor = Input.GetKey(brakeKey);
        hj.motor = motor;
    }

    void MethodC()
    {
        hj.useSpring = Input.GetKey(brakeKey);
        if (Input.GetKeyDown(brakeKey))
            spring.targetPosition = hj.angle;
        hj.spring = spring;
    }
}
