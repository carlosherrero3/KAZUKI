using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesactivarAlDetectar : MonoBehaviour
{
    // Asigna este objeto en el inspector
    public GameObject objetoObjetivo;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == objetoObjetivo)
        {
            gameObject.SetActive(false);
        }
    }

    // Alternativamente, si usas colisión física normal:
    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == objetoObjetivo)
        {
            gameObject.SetActive(false);
        }
    }
    */
}
