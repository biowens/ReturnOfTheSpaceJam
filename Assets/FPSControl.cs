using UnityEngine;
using System.Collections;

public class FPSControl : MonoBehaviour
{

    Transform camTrans, trans;
    Rigidbody rigid;

    public float mX, mY;
    public Vector3 rot, camRot;
    public float vertLim = 60;
    public float speed = 10f;

    // Use this for initialization
    void Start()
    {
        trans = transform;
        rigid = GetComponent<Rigidbody>();
        camTrans = trans.Find("Camera");
    }

    // Update is called once per frame
    void Update()
    {
        // Get mouse axes
        mX = Input.GetAxis("Mouse X");
        mY = Input.GetAxis("Mouse Y");

        // Get current rotations
        rot = trans.localRotation.eulerAngles;
        camRot = camTrans.localRotation.eulerAngles;
        // Affect the rotations
        rot.y += mX;
        if (camRot.x > 180) camRot.x -= 360;
        if (camRot.x < -180) camRot.x += 360;
        camRot.x -= mY;
        camRot.x = Mathf.Clamp(camRot.x, -vertLim, vertLim);

        // Rotate transforms
        trans.localRotation = Quaternion.Euler(rot);
        camTrans.localRotation = Quaternion.Euler(camRot);

        // Get movement Axes
        // Affect velocity
        Vector3 vel = Vector3.zero;
        vel += trans.forward * Input.GetAxis("Vertical");
        vel += trans.right * Input.GetAxis("Horizontal");
        if (vel.magnitude > 1) vel.Normalize();
        vel *= speed;
        // Set rigid.velocity
        vel.y = rigid.velocity.y;
        rigid.velocity = vel;
    }
}