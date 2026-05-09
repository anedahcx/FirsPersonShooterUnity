using UnityEngine;
using Photon.Pun;

public class Ordas : MonoBehaviour
{

    public int enemigosVivos;
    public int numRonda;

    public GameObject[] puntosDeSpawn;
    public GameObject prefabEnemigo;

    public PhotonView pv;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        numRonda = 0;
        
    }

    // Update is called once per frame
    void Update() {
        if(!PhotonNetwork.InRoom || (PhotonNetwork.IsMasterClient && pv.IsMine))
        {
            if (enemigosVivos == 0)
            {
                numRonda++;
                SiguienteOleada(numRonda);
            }
        }
        
    }

    //CAMBIA CODIGO
    private void SiguienteOleada(int ronda) {
        for (int i = 0; i < ronda; i++) {
            int randomPos = Random.Range(0, puntosDeSpawn.Length);
            GameObject puntosEmision = puntosDeSpawn[randomPos];
            GameObject instanciaEnemigo = null;

            if (PhotonNetwork.InRoom)
            {
                // --- MODO MULTIJUGADOR ---
                // Usamos .name porque el prefab debe estar en la carpeta "Resources"
                instanciaEnemigo = PhotonNetwork.Instantiate(prefabEnemigo.name, puntosEmision.transform.position, Quaternion.identity);
            }
            else
            {
                // --- MODO LOCAL ---
                // Usamos el Instantiate normal
                instanciaEnemigo = Instantiate(prefabEnemigo, puntosEmision.transform.position, Quaternion.identity);
            }

            //GameObject instanciaEnemigo = PhotonNetwork.Instantiate(prefabEnemigo.name, puntosEmision.transform.position, Quaternion.identity);
            //GameObject instanciaEnemigo = Instantiate(prefabEnemigo, puntosEmision.transform.position, Quaternion.identity);
            instanciaEnemigo.GetComponent<IAEnemigo>().ordas = GetComponent<Ordas>();
            enemigosVivos++;
        }
    }

}
