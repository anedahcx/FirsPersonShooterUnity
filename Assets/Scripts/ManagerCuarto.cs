using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class ManagerCuarto : MonoBehaviour
{
    public static ManagerCuarto instanciaCompartida;
    private void Awake()
    {
        if (instanciaCompartida == null)
        {
            instanciaCompartida = this;
            DontDestroyOnLoad(instanciaCompartida);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene escena, LoadSceneMode modo)
    {
        // Si la escena que se cargó es "Menu", no hagas nada y sal de la función.
        if (escena.name == "Menu")
        {
            return;
        }

        Vector3 posicionAparicion = new Vector3(Random.Range(15f, 50f), 1, Random.Range(15f, 68f));
        if (PhotonNetwork.InRoom)
        {
            GameObject instanciaPlayer = PhotonNetwork.Instantiate("PlayerOnline", posicionAparicion, Quaternion.identity);
            instanciaPlayer.name = "PlayerOnline" + PhotonNetwork.CurrentRoom.PlayerCount.ToString();
            //PhotonNetwork.Instantiate("PlayerOnline", posicionAparicion, Quaternion.identity);
        }
        else
        {
            Instantiate(Resources.Load("PlayerOnline"), posicionAparicion, Quaternion.identity);

        }
    }
}
