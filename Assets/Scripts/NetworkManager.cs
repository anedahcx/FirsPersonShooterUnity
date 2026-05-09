using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;


public class NetworkManager : MonoBehaviourPunCallbacks
{
    public Button btnMultiPlayer;
    void Start()
    {
        Debug.Log("conexión a servidor");
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Unirse a Lobby");
        PhotonNetwork.JoinLobby();
    }
        
    public override void OnJoinedLobby()
    {
        string nombreJugador = GameObject.Find("txtNombre").GetComponent<InputField>().text;
        FileManager.nombreJugadorActual = nombreJugador;

        Debug.Log("Preparados para juego multijugador");
        btnMultiPlayer.interactable = true;
    }
    public void EncuentraPartida()
    {
        Debug.Log("Buscando sala");
        PhotonNetwork.JoinRandomRoom();
    }
    
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreaCuarto();
    }

    private void CreaCuarto()
    {
        int numCuarto = UnityEngine.Random.Range(0, 23);
        RoomOptions opcionesSala = new RoomOptions()
        {
            IsVisible = true,
            IsOpen = true,
            MaxPlayers = 5,
            PublishUserId = true
        };
        PhotonNetwork.CreateRoom($"Cuarto_{numCuarto}", opcionesSala);
        Debug.Log($"Sala creada: {numCuarto}");
    }
    public override void OnJoinedRoom() {
        Debug.Log("Cargando escena");
        PhotonNetwork.LoadLevel("JuegoEnLinea");
    }
}
