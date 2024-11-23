using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dano : MonoBehaviour
{

    public float cantidadDano;
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && other.GetComponent<Salud>())
        {
            other.GetComponent<Salud>().RecibirDanou(cantidadDano);

        }
    }
}
