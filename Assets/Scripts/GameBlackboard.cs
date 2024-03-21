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
using UnityEngine.AI; 

/*
 * Tiene una referencia a todas las variables del mundo necesarias para que otros objetos tengan acceso a estas
 * Referencia a las habitaciones, a la posición (parcial) del fantasma o la cantante
 * Si el piano o el fantasma ha sido golpeado
 * Si la puerta de la celda está cerrada
 */

public class GameBlackboard : MonoBehaviour
{
    public GameObject musicRoom;
    public GameObject celda;
    public GameObject stage;
    public GameObject basement;
    public GameObject backStage;

    public GameObject singer;
    public GameObject player;

    public GameObject westLever;
    public GameObject eastLever;
    public GameObject piano;
    public GameObject puerta;
    public GameObject[] randomSitios;
    public bool imprisoned = false;

    public bool isGhostInSotano { get; set; }

    public bool pianoed { get { return piano.GetComponent<ControlPiano>().tocado; } }
    public bool hited;

    //public bool gate { get; set; }
    public bool gate = false; //gate close -> false

    void Awake()
    {
        imprisoned = false;
        gate = false;
        hited = false;
    }

    private void Update()
    {
        imprisoned = !singer.GetComponent<AudioSource>().isPlaying;
    }

    // Permite al fantasma saber a qué palanca debería ir
    public GameObject nearestLever(GameObject go) 
    {
        //return ((westLever.transform.position - go.transform.position).magnitude > (eastLever.transform.position - go.transform.position).magnitude) ? eastLever : westLever; 

        if (!westLever.GetComponentInChildren<ControlPalanca>().caido && !eastLever.GetComponentInChildren<ControlPalanca>().caido)
        {
            //se nessuno dei due e caduto vai al piu vicino
            return ((westLever.transform.position - go.transform.position).magnitude > (eastLever.transform.position - go.transform.position).magnitude) ? eastLever : westLever;
        }
        else if(!westLever.GetComponentInChildren<ControlPalanca>().caido && eastLever.GetComponentInChildren<ControlPalanca>().caido)
        {
            //e caduto quello di destra, vai a SX
            return westLever;
        }
        else if (westLever.GetComponentInChildren<ControlPalanca>().caido && !eastLever.GetComponentInChildren<ControlPalanca>().caido)
        {
            //vai a DX
            return eastLever;
        }
        else return null;
    }


    // Permite al fantasma escoger un lugar aleatorio para buscar a la cantante
    public GameObject getRandomSitio()
    {
        GameObject go = randomSitios[Random.Range(0, randomSitios.Length)];
        Debug.Log(go.name);
        return go;
    }
}
