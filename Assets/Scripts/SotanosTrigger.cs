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

/*
 * Sirve para avisar al Blackboard de que el fantasma esta en el sotano en contacto con el trigger
 */

public class SotanosTrigger : MonoBehaviour
{
    GameBlackboard blackboard;
    public GameObject ghost;

    private void Awake()
    {
        blackboard = GameObject.FindGameObjectWithTag("Blackboard").GetComponent<GameBlackboard>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ghost)
        {
            blackboard.isGhostInSotano = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject == ghost)
        {
            blackboard.isGhostInSotano = false;
        }
    }
}
