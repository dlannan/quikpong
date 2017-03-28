using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class BallController : NetworkBehaviour
{

    public GameObject playerOne;
    public GameObject playerTwo;

    public GameObject wall1;
    public GameObject wall2;
    public GameObject wall3;

    public Text ScoreUI1;
    public Text ScoreUI2;

    public float Speed = 5.0f;
    private Vector3 Direction = new Vector3();
    private Vector3 StoredPosition = new Vector3();

    // Player scores should go in a manager - here will do for time being
    private int p1Score = 0;
    private int p2Score = 0;

    // Network Manager so we can call comms methods - set on connection (client or server)
    private NetworkManager NetMgr;
    private bool syncBall = false;

    // The data the ball shares - client <-> server
    public SyncListFloat syncData;

    // Target position is used on client to get to latest update
    private Vector3 TargetPosition = new Vector3();

    enum Score
    {
        PlayerOne,
        PlayerTwo
    };

    // Use this for initialization
    void Start()
    {
        ResetBall();
        ResetScore();
    }

    void ResetScore()
    {
        p1Score = 0;
        p2Score = 0;
        TargetPosition = StoredPosition;
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
        if (syncBall == true)
        {
            // Sync ball if its enabled
            if (NetMgr.IsServer())
            {
                syncData[0] = StoredPosition.x;
                syncData[1] = StoredPosition.z;
                syncData.Dirty(0);
                syncData.Dirty(1);
            }
            else
            {
                TargetPosition.y = StoredPosition.y;
                StoredPosition = Vector3.Lerp(StoredPosition, TargetPosition, Time.deltaTime + 0.01f);
            }
        }
        else
        {
            // Always maintain Rigid body velocity (so it doesnt slow down)
            StoredPosition = StoredPosition + Direction * Speed * Time.deltaTime;
        }

        // Reset if out of bounds!!
        if (StoredPosition.z < -15.0f)
        {
            ResetBall();
            ScorePlayer(Score.PlayerOne);
        }
        if (StoredPosition.z > 15.0f)
        {
            ResetBall();
            ScorePlayer(Score.PlayerTwo);
        }
    }

    void LateUpdate()
    {
        transform.position = StoredPosition;
    }

    void OnDataChanged(SyncListFloat.Operation op, int index)
    {
        if (index == 0) TargetPosition.x = syncData[index];
        if (index == 1) TargetPosition.z = syncData[index];

    }

    void OnTriggerEnter(Collider other)
    {
        if (!NetMgr.IsServer())
            return;

        var dir = Direction;
        if (other.gameObject == wall1)
        {
            dir.x = -dir.x;
        }
        else if (other.gameObject == wall2)
        {
            dir.x = -dir.x;
        }
        else if (other.gameObject == wall3)
        {
            dir.z = -dir.z;
        }
        else if (other.gameObject == playerOne || other.gameObject == playerTwo)
        {
            dir.z = -dir.z;
        }

        Direction = dir;
    }

    void ScorePlayer(Score player)
    {
        if(player == Score.PlayerTwo)
        {
            p2Score++;
            ScoreUI2.text = p2Score.ToString();
        }

        if (player == Score.PlayerOne)
        {
            p1Score++;
            ScoreUI1.text = p1Score.ToString();
        }
    }

    public void CommSyncStart(NetworkManager mgr)
    {
        NetMgr = mgr;
        syncBall = true;

        // Only bother with x and z (can expand later)
        syncData = new SyncListFloat();

        if (mgr.IsServer())
        {
            syncData.Add(StoredPosition.x);
            syncData.Add(StoredPosition.z);
            syncData.Add(Direction.x);
            syncData.Add(Direction.z);
        }
        else
        {
            syncData.Callback = this.OnDataChanged;
        }
    }
}

