using UnityEngine;
using UnityEngine.Networking;

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
        NetworkServer.Listen(nwPort);
        isAtStartup = false;
        clientImg.SetActive(false);
        serverImg.SetActive(false);
        statusImg.SetActive(true);
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
    }

    // client function
    public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server");
    }
}