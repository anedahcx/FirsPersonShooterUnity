using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
public class FileManager : MonoBehaviourPunCallbacks
{
    public string nombreArchivo = "JuegoGuardado";
    public string nombreDirectorio = "Partidas";
    public GameData datosjuego;
    public static int record;
    public static string nombreR;
    public static string nombreJugadorActual;
    void Start() {
        LoadFile();
    }

    public void SaveToFile() {

        if(!Directory.Exists(nombreDirectorio)) Directory.CreateDirectory(nombreDirectorio);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Create(nombreDirectorio + "/" + nombreArchivo + ".bin");
        GameData datosJuego = new GameData(ManagerDisparo.puntosPlayer, 5.1f, nombreJugadorActual);
        //GameData datosJuego = new GameData(ManagerDisparo.puntosPlayer, 5.1f, Navegacion.nombreJugador);
        formatter.Serialize(saveFile, datosJuego);
        saveFile.Close();
        Debug.Log("Guardado en " + Directory.GetCurrentDirectory().ToString() + "/Saves/" + nombreArchivo + ".bin");
    }

    private void LoadFile()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream saveFile = File.Open(nombreDirectorio + "/" + nombreArchivo + ".bin", FileMode.Open);
        GameData loadData = (GameData)formatter.Deserialize(saveFile);
        Debug.Log("Datos cargados *************");
        Debug.Log("Nombre " + loadData.nombre);
        Debug.Log("Puntos " + loadData.puntos);
        Debug.Log("Tiempo " + loadData.tiempo);
        record = loadData.puntos;
        nombreR = loadData.nombre;
    }

    /// <summary>
    /// Esta función es llamada automáticamente por Photon
    /// CUANDO HEMOS SALIDO DE LA SALA con éxito.
    /// </summary>
    public override void OnLeftRoom()
    {
        // Ahora que ya no estamos en la sala, es seguro cargar el Menú.
        SceneManager.LoadScene("Menu");
    }

}
