using UnityEngine;
using System.Collections;

public class BatControl : MonoBehaviour {

    public GameObject PlayerBat;

    public float BatSpeed = 15.0f;
    public float TopWall = 7.0f;
    public float BottomWall = -7.0f;

    public bool secondPlayer = false;

    private float PlayerMove = 0.0f;
    private Vector3 StoredPosition = new Vector3();

    // Use this for initialization
    void Start () {
        Reset();
    }
	
    void Reset()
    {
        StoredPosition = transform.position;
        PlayerMove = 0.0f;
    }

	// Update is called once per frame
	void Update () {

        CheckKeys();
        // Always maintain Rigid body velocity (so it doesnt slow down)
        float newx = StoredPosition.x + BatSpeed * PlayerMove * Time.deltaTime;
        if (newx < TopWall && newx > BottomWall)
            StoredPosition.x = newx;
    }

    void LateUpdate()
    {
        PlayerBat.transform.position = StoredPosition;
    }

    void CheckKeys()
    {
        PlayerMove = 0.0f;
        if (!secondPlayer)
        {
            if (Input.GetKey(KeyCode.A))
                PlayerMove = 1.0f;

            if (Input.GetKey(KeyCode.Z))
                PlayerMove = -1.0f;
        } else
        {
            if (Input.GetKey(KeyCode.K))
                PlayerMove = 1.0f;

            if (Input.GetKey(KeyCode.M))
                PlayerMove = -1.0f;
        }
    }
}
