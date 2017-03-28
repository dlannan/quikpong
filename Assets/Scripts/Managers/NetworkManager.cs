using UnityEngine;
using UnityEngine.Networking;

// From Unity Network Tutorials - normally I would use Windows .NET networking tools for network comms.
public class NetworkManager : MonoBehaviour
{
    public  GameObject MainMenu;

    public bool isAtStartup = true;
    public string nwHost = "127.0.0.1";

    public GameObject serverImg;
    public GameObject clientImg;

    NetworkClient myClient;

    // Open the Pause menu to make the user select a Host/Client setup
    void Start()
    {

    }

    void Update()
    {
    }

    // Create a server and listen on a port
    public void SetupServer()
    {
        NetworkServer.Listen(4444);
        isAtStartup = false;
    }

    // Create a client and connect to the server port
    public void SetupClient()
    {
        myClient = new NetworkClient();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.Connect(nwHost, 4444);
        isAtStartup = false;
    }

    // Create a local client and connect to the local server
    public void SetupLocalClient()
    {
        myClient = ClientScene.ConnectLocalServer();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        isAtStartup = false;
    }

    // client function
    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server");
    }
}