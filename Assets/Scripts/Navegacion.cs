using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Navegacion : MonoBehaviour
{

    public string nombreArchivo = "JuegoGuardado";
    public string nombreDirectorio = "Partidas";
    public GameData datosJuego;

    public static string nombreJugador = "";

    public void Awake()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void Start()
    {
        if (!Directory.Exists(nombreDirectorio))
        {
            Directory.CreateDirectory(nombreDirectorio);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream saveFile = File.Create(nombreDirectorio + "/" + nombreArchivo + ".bin");
            formatter.Serialize(saveFile, datosJuego);
            saveFile.Close();
            Debug.Log("Guardado en " + Directory.GetCurrentDirectory().ToString() + "/" + nombreDirectorio + "/" + nombreArchivo + ".bin");
        }
    }

    public void IrJuego()
    {
        nombreJugador = GameObject.Find("txtNombre").GetComponent<InputField>().text;
        FileManager.nombreJugadorActual = nombreJugador;
        SceneManager.LoadScene("escenaAvanzada");
    }

}
