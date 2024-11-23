using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaludEnemy : MonoBehaviour
{

    public float salud = 50;
    public float saludMaxima = 50;

    [Header("Interfaz")]
    public GameObject muerte;


    public void RecibirDanou(float dano)
    {
        salud -= dano;

        if (salud <= 0)
        {
            Instantiate(muerte);
            Destroy(gameObject);
        }
    }


}
