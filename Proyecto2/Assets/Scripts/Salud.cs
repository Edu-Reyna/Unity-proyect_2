using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Salud : MonoBehaviour
{

    public float salud = 50;
    public float saludMaxima = 50;

    [Header("Interfaz")]
    public Image barraSalud;
    public Text textoSalud;
    public CanvasGroup ojosRojos;


    [Header("Interfaz")]
    public GameObject muerte;

    // Update is called once per frame
    void Update()
    {
        if (ojosRojos.alpha > 0)
        {
            ojosRojos.alpha -= Time.deltaTime;
        }
        ActualizarInterfaz();
    }

    public void RecibirDanou(float dano) 
    {
        salud -= dano;
        ojosRojos.alpha = 1;

        if (salud <= 0) 
        {
            Instantiate(muerte);
            Destroy(gameObject);
        }
    }

    void ActualizarInterfaz()
    { 
        barraSalud.fillAmount = salud / saludMaxima;
        textoSalud.text = "+ " + salud.ToString("f0");
    }
}
