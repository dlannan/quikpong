using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using System;

// From Unity Network Tutorials - normally I would use Windows .NET networking tools for network comms.
public class NetworkManager : MonoBehaviour
{
    public  GameObject MainMenu;

    public bool isAtStartup = true;
    public string nwHost = "127.0.0.1";
    public int nwPort = 4444;

    public GameObject serverImg;
    public GameObject clientImg;
    public GameObject statusImg;

    public BatControl playerOne;
    public BatControl playerTwo;
    public BallController pongBall;

    NetworkClient myClient;
    private string nwState = "";
    private bool isServer = false;

    // Open the Pause menu to make the user select a Host/Client setup
    void Start()
    {

    }

    void Update()
    {
        if(statusImg.activeSelf)
        {
            int tm = (int)(Time.unscaledTime);
            string newText = nwState + new String('.', tm % 30 + 1); 
            statusImg.GetComponentsInChildren<Text>()[1].text = newText;
        }
    }

    public bool IsServer()
    {
        return isServer;
    }

    // Create a server and listen on a port
    public void SetupServer()
    {
        NetworkServer.Listen(nwPort);
        NetworkServer.RegisterHandler(MsgType.Connect, OnServerConnected);
        isAtStartup = false;
        clientImg.SetActive(false);
        serverImg.SetActive(false);
        statusImg.SetActive(true);
        nwState = "Server Waiting";
        isServer = true;
    }

    // Create a client and connect to the server port
    public void SetupClient()
    {
        myClient = new NetworkClient();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.Connect(nwHost, nwPort);
        isAtStartup = false;
        serverImg.SetActive(false);
        clientImg.SetActive(false);
        statusImg.SetActive(true);
        nwState = "Client Connecting";
        isServer = false;
    }

    // client function
    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server");
        nwState = "Client Connected.";
        statusImg.GetComponentsInChildren<Text>()[1].text = nwState;

        // Client disables player input for Server Side (Left Hand Side)
        playerOne.CommInput();
        // Player Two(client) must sync its key presses to move the bat with the server.
        playerTwo.CommOutput();
        // Client runs ball as per normal - with regular sync.
        pongBall.CommSyncStart(this);

        // All ready, jump into the game!!

    }

    // client function
    public void OnServerConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to client");
        nwState = "Server Connected.";
        statusImg.GetComponentsInChildren<Text>()[1].text = nwState;

        // Client enables player input for Server Side (Left Hand Side)
        playerOne.CommOutput();
        // Player Two(client) must sync its key presses to move the bat with the server.
        playerTwo.CommInput();
        // Client runs ball as per normal - with regular sync.
        pongBall.CommSyncStart(this);

        // All ready, jump into the game!!
    }
}