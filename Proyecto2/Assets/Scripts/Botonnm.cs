using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Botonnm : MonoBehaviour
{

    public int numeroScena;

    public void Iniciar() 
    {
        SceneManager.LoadScene(numeroScena); 
    }

    public void Salir()
    {
        Application.Quit();
        Debug.Log("Aqui se cierra el juego");
    }


}
