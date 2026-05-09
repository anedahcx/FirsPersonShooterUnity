using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Photon.Pun;

[RequireComponent(typeof(LineRenderer))]

public class ManagerDisparo : MonoBehaviour
{
    public Camera playerCamera;
    //public Transform origenRayo;

    public GameObject objPistola, objRifle, objEscopeta;

    public Arma pistola;
    public Arma rifle;
    public Arma escopeta;

    [SerializeField] private float frecuenciaDisparo;
    [SerializeField] private float rango;
    [SerializeField] private int dañoCausado;
    [SerializeField] private int capacidadArma;

    //public float duracion = 0.1f; //tiempo que dura el rayo en pantalla
    private float TiempoDisparo; //Tiempo para poder dispara

    [SerializeField]private LayerMask lMask, otro;

    public ParticleSystem particulasDisparo, particulasEscopeta, particulasRifle;
    public GameObject impacto, impactoEnemigo;

    public static int puntosPlayer; //Puntos de la partida, posteriormente seran almacenados en el archivo de records
    //public PhotonView pv;

    private void Awake(){

        //Instancias de las armas a utilizar en el juego
        pistola = new Arma(30, 1.0f, 1, 5);
        rifle = new Arma(50, 0.25f, 2, 10);
        escopeta = new Arma(10, 2.0f, 4, 3);

        // Carga la pistola por default
        OcultaArmas();
        objPistola.SetActive(true);
        rango = pistola.alcance;
        frecuenciaDisparo = pistola.frecuenciaDisparo;
        dañoCausado = pistola.dañoCausado;
        capacidadArma = pistola.capacidad;

        puntosPlayer = 0;
    }


    // Update is called once per frame
    void Update(){
        CambiaArma();
        TiempoDisparo += Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && TiempoDisparo > frecuenciaDisparo){
            Dispara();
        }
    }

    private void CambiaArma()
    {
        if (Input.GetKeyUp(KeyCode.Alpha1))
        { //Pistola
            OcultaArmas();
            objPistola.SetActive(true);
            rango = pistola.alcance;
            frecuenciaDisparo = pistola.frecuenciaDisparo;
            dañoCausado = pistola.dañoCausado;
            capacidadArma = pistola.capacidad;
        }
        
        if (Input.GetKeyUp(KeyCode.Alpha2))
        { //Rifle
            OcultaArmas();
            objRifle.SetActive(true);
            rango = rifle.alcance;
            frecuenciaDisparo = rifle.frecuenciaDisparo;
            dañoCausado = rifle.dañoCausado;
            capacidadArma = rifle.capacidad;
        }
        
        if (Input.GetKeyUp(KeyCode.Alpha3))
        { //Escopeta
            OcultaArmas();
            objEscopeta.SetActive(true);
            rango = escopeta.alcance;
            frecuenciaDisparo = escopeta.frecuenciaDisparo;
            dañoCausado = escopeta.dañoCausado;
            capacidadArma = escopeta.capacidad;
        }
    }

    private void Dispara(){

        if (objPistola.activeInHierarchy)
        {
            particulasDisparo.Play();
        }
        else if (objEscopeta.activeInHierarchy)
        {
            particulasEscopeta.Play();
        }
        else if (objRifle.activeInHierarchy) { 
            particulasRifle.Play();
        }


            TiempoDisparo = 0;
        
        Vector3 origen = playerCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        if (Physics.Raycast(origen, playerCamera.transform.forward, out hit, rango, lMask)) {
            GameObject objImpacto = Instantiate(impactoEnemigo, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(objImpacto, 1.2f);

            IAEnemigo enemigo = hit.transform.GetComponent<IAEnemigo>();

            if (enemigo != null)
            { // se esta disparando a un enemigo
                puntosPlayer++; //cada que impactemos a un enemigo, se suman puntos
                Debug.Log("COLISION AL ENEMIGO");
                enemigo.TomarDaño(dañoCausado);
            }

            //Destroy(hit.transform.gameObject);
            //rayoLaser.SetPosition(1, hit.point);

            

        } else if (Physics.Raycast(origen, playerCamera.transform.forward, out hit, rango, otro)) {

            if (hit.rigidbody != null) {
                hit.rigidbody.AddForce(hit.normal * 70.0f);
            }

        GameObject objImpacto = Instantiate(impacto, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(objImpacto, 1.2f);
        }

    }

    private void OcultaArmas()
    {
        objPistola.SetActive(false);
        objRifle.SetActive(false);
        objEscopeta.SetActive(false);
    }

}



