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
 * Accion de seguir a la cantante, cuando la alcanza devuelve Success
 */

public class GhostLlevarCantante : Action
{
    NavMeshAgent agent;
    NavMeshAgent singerNav;
    GameObject singer;

    GameObject sotanoNorte;

    public override void OnAwake()
    {
        agent = GetComponent<NavMeshAgent>();

        sotanoNorte = GameObject.FindGameObjectWithTag("Blackboard").GetComponent<GameBlackboard>().basement;
        singer = GameObject.FindGameObjectWithTag("Blackboard").GetComponent<GameBlackboard>().singer;
        singerNav = singer.GetComponent<NavMeshAgent>();        
    }

    public override TaskStatus OnUpdate()
    {
        agent.SetDestination(singer.transform.position);

        if (Vector3.SqrMagnitude(transform.position - singer.transform.position) < 1.2f)
        {
            agent.SetDestination(transform.position);
            singer.GetComponent<Cantante>().capturada = true;

            //return TaskStatus.Success;
            singerNav.enabled = false;
            singer.transform.position = transform.position + new Vector3(0, 1, 0);
            singer.transform.SetParent(transform, true);
            //agent.SetDestination(sotanoNorte.transform.position);

            return TaskStatus.Success;
        }
        else return TaskStatus.Running;

    }
}
