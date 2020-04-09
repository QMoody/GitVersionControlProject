using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steer : MonoBehaviour
{
    public GameObject board;
    public GameObject player;
    Transform cam;
    public float maxSpeed;
    public float rotationSpeed;
    public float maxRotation;
    float angle;
    private Rigidbody m_rb;
    private Collider m_co;
    private Vector3 targetDirection;
    private float m_drag;
    public float fallForce = 20;
    public float fowardForce = 20;
    public float brakeAmount = 2;
    public float slopeIncline = 20;

    private void Start()
    {
        cam = Camera.main.transform;
        Physics.autoSimulation = false;
        if (GetComponent<Rigidbody>() == null)
        {
            if (transform.parent.GetComponent<Rigidbody>() != null)
                m_rb = transform.parent.GetComponent<Rigidbody>();
        }
        else
        m_rb = GetComponent<Rigidbody>();

        if (GetComponent<Collider>() == null)
        {
            if (transform.parent.GetComponent<Collider>() != null)
                m_co = transform.parent.GetComponent<Collider>();
        }
        else
            m_co = GetComponent<Collider>();

        targetDirection = new Vector3();
        m_drag = m_rb.drag;
    }

    private Matrix4x4 slopeRot;
    // Update is called once per frame
    void FixedUpdate()
    {
        Align();

        Turn();
        Fall();
        Go();

        //player.transform.localEulerAngles = targetDirection; //Fixed rotation in this prototype due to derpy rotation (change "player" to "board" to see difference)
        Physics.Simulate(Time.fixedDeltaTime);
    }
    
    void Align()
    {
        Vector3 upRef = Vector3.up; // raycast down, get the normal of the ground
        // use the normal as the reference of up vector

        float rollAng = Vector3.Angle(Vector3.up, Vector3.ProjectOnPlane(upRef, Vector3.forward));
        Quaternion roll = Quaternion.AngleAxis(rollAng, Vector3.forward);

        float pitchAng = Vector3.Angle(Vector3.up, Vector3.ProjectOnPlane(upRef, Vector3.right));
        Quaternion pitch = Quaternion.AngleAxis(pitchAng, Vector3.right);

        // rotate the player somehow with these rotations applied in global space

        slopeRot = Matrix4x4.Rotate(Quaternion.AngleAxis(slopeIncline, Vector3.right));
        Vector3 slopeUp = slopeRot.MultiplyVector(Vector3.up);
    }

    private void Update()
    {
        if (Input.GetAxis("Fire1")>=.01)
        {
            //Break();
        }
    }

    void Turn()
    {
        if (m_rb.rotation.eulerAngles.x > 0 ) //stops player from riding up the hill
        {
            //turns based on mouse position on screen
            angle = (Mathf.Atan2(Input.mousePosition.x - Screen.width / 2, Screen.height) * 180 / Mathf.PI);
           targetDirection = new Vector3(board.transform.localEulerAngles.x, (Mathf.Atan2(Input.mousePosition.x - Screen.width / 2, Screen.height) * 180 / Mathf.PI), board.transform.localEulerAngles.z);
           //m_rb.drag = m_drag + angle;
        }

        float rotation = m_rb.rotation.eulerAngles.y;
        if (rotation > 180)
            rotation = -360 + rotation;

        if (Mathf.Abs(rotation) < maxRotation)
        {
            //Debug.Log(rotation);
            m_rb.MoveRotation(m_rb.rotation * Quaternion.AngleAxis(Mathf.Lerp(0, angle, rotationSpeed * Time.fixedDeltaTime), transform.up));
        }
        else
        {
            
            if (angle < 0 && rotation > maxRotation)
            {
                m_rb.MoveRotation(m_rb.rotation * Quaternion.AngleAxis(Mathf.Lerp(0, angle, rotationSpeed * Time.fixedDeltaTime), transform.up));
            }
            else if (angle > 0 && rotation < -maxRotation)
            {
                m_rb.MoveRotation(m_rb.rotation * Quaternion.AngleAxis(Mathf.Lerp(0, angle, rotationSpeed * Time.fixedDeltaTime), transform.up));
            }
            
        }
    }

    private void Break()
    {
        m_rb.drag = 20;
        //Vector3 breakForce = -m_rb.velocity * Time.deltaTime;
        //m_rb.AddRelativeForce(breakForce*10, ForceMode.Impulse);
        //print(breakForce);
    }

    void Fall()
    {
        //Applies force to keep the player on the ground
               
        RaycastHit hit;

        if (Physics.Raycast(transform.position + transform.forward *m_co.bounds.size.z/2, transform.TransformDirection(Vector3.down), out hit, 2000)) //Cast a Raycast to see if any colliders are under that point.
        {
            //Debug.DrawRay(transform.position + transform.forward * m_co.bounds.size.z / 2, transform.TransformDirection(Vector3.down));
            m_rb.AddForce(hit.normal * -1 * fallForce, ForceMode.Force);
        }


        //print(player.transform.localEulerAngles.x / 90); //testing gravity scaling (simulates picking up speed)
    }

    void Go()
    {
        //Applies force foward
        Vector3 forwardVect = Vector3.Project(m_rb.transform.forward, slopeRot.MultiplyVector(Vector3.forward));
        m_rb.AddForce(forwardVect * fowardForce + Vector3.forward);
    }
}
