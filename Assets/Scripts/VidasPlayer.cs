using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class VidasPlayer : MonoBehaviour
{
    public Image vidaPlayer;
    private float anchoVidasPlayer;
    public static int vida;
    private bool haMuerto;
    public GameObject gameOver;
    private const int vidasINI = 5;
    public static int puedePerderVida = 1;

    public Text txtPuntos;
    public Text txtRecord;
    public Text nombreR;

    public GameObject fm;


    void Start(){
        anchoVidasPlayer = vidaPlayer.GetComponent<RectTransform>().sizeDelta.x;
        haMuerto = false;
        vida = vidasINI;
        gameOver.SetActive(false);

        txtRecord.text = "Record " + FileManager.record.ToString();
        nombreR.text = FileManager.nombreR;
    }

    private void Update()
    {
        txtPuntos.text = "Puntos: " + ManagerDisparo.puntosPlayer.ToString();
    }

    public void TomarDaño(int daño) {
        if (vida > 0 && puedePerderVida == 1) {
            puedePerderVida = 0;
            vida -= daño;
            DibujaVida(vida);
        }


        if(vida <= 0 && !haMuerto) {
            haMuerto =true;

            if (ManagerDisparo.puntosPlayer > FileManager.record)
            {
                // Primero, comprueba si 'fm' es nulo
                if (fm == null)
                {
                    // Si es nulo, búscalo en la escena por su nombre
                    fm = GameObject.Find("FileManager");
                }

                // Ahora, si 'fm' se encontró (o ya estaba asignado), úsalo
                if (fm != null)
                {
                    fm.GetComponent<FileManager>().SaveToFile();
                }
                else
                {
                    // Si sigue siendo nulo, es que no lo encontró
                    Debug.LogError("¡No se pudo encontrar el GameObject de FileManager para guardar!");
                }
            }

            StartCoroutine(EjecutaMuerte());
        }
    }

    private void DibujaVida(int vida) {
        RectTransform transformaImagen = vidaPlayer.GetComponent<RectTransform>();
        transformaImagen.sizeDelta = new Vector2(anchoVidasPlayer * (float)vida / (float)vidasINI , transformaImagen.sizeDelta.y);
    }

    IEnumerator EjecutaMuerte() {
        gameOver.SetActive(true);
        yield return new WaitForSeconds(2.1f);

        // Comprobamos si estamos en una sala de Photon
        if (PhotonNetwork.InRoom)
        {
            // Si es multijugador, nos salimos de la sala.
            // Photon se encargará de destruir al jugador y objetos de red.
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            // Si es modo local (un jugador), cargamos la escena manualmente.
            SceneManager.LoadScene("Menu");
        }

        //SceneManager.LoadScene("Menu");
    }

}
