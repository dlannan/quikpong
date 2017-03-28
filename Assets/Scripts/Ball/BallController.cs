using UnityEngine;
using System.Collections;

public class BallController : MonoBehaviour
{

    public GameObject playerOne;
    public GameObject playerTwo;

    public GameObject wall1;
    public GameObject wall2;
    public GameObject wall3;

    public float Speed = 5.0f;
    private Vector3 Direction = new Vector3();
    private Vector3 StoredPosition = new Vector3();

    // Use this for initialization
    void Start()
    {
        ResetBall();
    }

    // Everytime we need a new ball use this function
    void ResetBall()
    {
        // This will change for two player.
        StoredPosition = new Vector3(0.0f, 0.51f, 2.0f);

        // Random direction start for ball (towards Player one initially)
        float deg = Mathf.Deg2Rad * Random.Range(-60.0f, 60.0f);
        // This is our random direction vector 
        Vector3 dir = new Vector3(Mathf.Sin(deg), 0.0f, Mathf.Cos(deg));
        // Set the balls physics vector to the direction vector
        Direction = dir;
    }

    // Update is called once per frame
    void Update()
    {
        // Always maintain Rigid body velocity (so it doesnt slow down)
        StoredPosition = StoredPosition + Direction * Speed * Time.deltaTime;

        // Reset if out of bounds!!
        if (StoredPosition.z < -15.0f || StoredPosition.z > 15.0f)
            ResetBall();
    }

    void FixedUpdate()
    {
        transform.position = StoredPosition;
    }

    void OnTriggerEnter(Collider other)
    {

        var dir = Direction;
        if (other.gameObject == wall1)
        {
            dir.x = -dir.x;
            Debug.Log("Wall1....");
        }
        else if (other.gameObject == wall2)
        {
            dir.x = -dir.x;
            Debug.Log("Wall2....");
        }
        else if (other.gameObject == wall3)
        {
            dir.z = -dir.z;
            Debug.Log("Wall3....");
        }
        else if (other.gameObject == playerOne)
        {
            dir.z = -dir.z;
            Debug.Log("Bat One....");
        }

        Direction = dir;
    }
}