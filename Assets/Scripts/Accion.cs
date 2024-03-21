/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com
   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).
   Autor: Federico Peinado 
   Contacto: email@federicopeinado.com
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Accion : MonoBehaviour
{

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Piano"))
        {
            other.gameObject.GetComponent<ControlPiano>().InteractFantasma();
            Debug.Log("Han golpeado al piano");
        }
        else if (other.gameObject.CompareTag("Palanca"))
        {
            other.gameObject.GetComponent<ControlPalanca>().Interact();
            Debug.Log("Ha tocado la palanca");
        }
        else if (other.gameObject.CompareTag("Puerta"))
        {
            Debug.Log("Ha tocado la puerta");
        }
    }
}