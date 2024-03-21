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

/*
 * Devuelve Success cuando la cantante es sobre el palco
 */


public class VizcondeChocaCondition : Conditional
{
    GameObject Vizconde;
    NavMeshAgent agent;

    CapsuleCollider cc;
    bool golpeado = false;

    public override void OnAwake()
    {
        // IMPLEMENTAR 

    }

    public override TaskStatus OnUpdate()
    {
        // IMPLEMENTAR
        return TaskStatus.Success;

    }

}
