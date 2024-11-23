using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanoEnemy : MonoBehaviour
{

    public float cantidadDano;
    // Start is called before the first frame update

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && other.GetComponent<SaludEnemy>())
        {
            other.GetComponent<SaludEnemy>().RecibirDanou(cantidadDano);

        }
    }
    
}
