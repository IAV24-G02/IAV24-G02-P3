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
 * Pone el piano en estado tocado cuando se colisiona otro objeto con él
 */

public class ControlPiano : MonoBehaviour
{
    public GameObject ghost;
    public bool tocado = false;
    public bool roto = false;
    public bool tocadoDaOtro = false;

    [SerializeField] int numToques = 0;
    [SerializeField] int maxToques = 3;

    private AudioSource myAudioSource;
    public AudioClip[] audioClips;

    private void Awake()
    {
        myAudioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Cantante>() || other.gameObject.GetComponent<Player>())
        {
            //tocado = false; // Solo lo hace el fantasma
            tocadoDaOtro = true;
            InteractVizconde();
        } 

    }



    public void InteractFantasma()
    {
        tocado = true;
        //GetComponent<AudioSource>().Play();
        myAudioSource.clip = audioClips[0];
        myAudioSource.Play();
    }

    public void InteractVizconde()
    {
        
        myAudioSource.clip = audioClips[1];
        myAudioSource.Play();

        numToques++;

        if(numToques == maxToques)
        {
            roto = true;
        }
    }

    public void ArreglaPiano()
    {
        roto = false;
        tocadoDaOtro = false;
        myAudioSource.Stop();
        numToques = 0;
    }
}
