using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class IAEnemigo : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agente;
    private GameObject player;
    private float velEnemigo;
    private float dist;
    private float frecAtaque = 2.5f, tiempoSigAtaque = 0, iniciarConteo;
    public static int vidaEnemigo;

    public PhotonView pvEnemigo;

    public Ordas ordas;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player");
        player = GameObject.Find("Capsule");
        ordas = GameObject.Find("ORDAS").GetComponent<Ordas>();
        dist = Vector3.Distance(player.transform.position, transform.position);
        agente.speed = Random.Range(1.0f, 5.0f);
        vidaEnemigo = 1;
        //pvEnemigo = GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        dist = Vector3.Distance(player.transform.position, transform.position);

        if (tiempoSigAtaque > 0){
            tiempoSigAtaque = frecAtaque + iniciarConteo - Time.time;
        } else {
            tiempoSigAtaque = 0;
            agente.SetDestination(player.transform.position);
            VidasPlayer.puedePerderVida = 1;
        }

//        if (dist <= 15) { //Enemigo sigue al jugador si la distancia es menor a
//            agente.SetDestination(player.transform.position);
//       }
    }

    private void OnTriggerEnter(Collider obj){
        if(obj.tag == "Player"){ //Daño que el enemigo le genera al jugador
            tiempoSigAtaque = frecAtaque;
            iniciarConteo = Time.time;
            obj.transform.GetComponentInChildren<VidasPlayer>().TomarDaño(1);
        }
    }

    //CAMBIA CODIGO
    public void TomarDaño(int daño)
    {
        //pvEnemigo.RPC("AplicarDano", RpcTarget.All, daño, pvEnemigo.ViewID);

        if (PhotonNetwork.InRoom)
        {
            // MODO MULTIJUGADOR: Llama al RPC para que todos lo vean
            pvEnemigo.RPC("AplicarDano", RpcTarget.All, daño, pvEnemigo.ViewID);
        }
        else
        {
            // MODO LOCAL: Llama a la función de daño directamente
            // Pasamos 0 como ViewID porque en local no importa
            AplicarDano(daño, 0);
        }
    }

    //CAMBIA CODIGO
    [PunRPC]
    public void AplicarDano(int daño, int viewID){
        if (!PhotonNetwork.InRoom || pvEnemigo.ViewID == viewID)
        {
            vidaEnemigo -= daño;
            if (vidaEnemigo <= 0) { 
                if(!PhotonNetwork.InRoom || (PhotonNetwork.IsMasterClient && pvEnemigo.IsMine))
                {
                    Debug.Log("Decrementa en horda");
                    ordas.enemigosVivos--;
                }
                Destroy(gameObject);
            }
        }
        
    }

}
