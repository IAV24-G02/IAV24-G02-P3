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
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine.AI;


public class GhostArreglaPianoAction : Action
{
    ControlPiano pianoControl;
    GameObject piano;

    public override void OnAwake()
    {
        pianoControl = GameObject.FindGameObjectWithTag("Piano").GetComponent<ControlPiano>();
        piano = GameObject.FindGameObjectWithTag("Piano");
    }

    public override TaskStatus OnUpdate()
    {
        if (pianoControl.roto)
        {
            pianoControl.ArreglaPiano();
            return TaskStatus.Success;
        }
        else return TaskStatus.Running;
    }
}
