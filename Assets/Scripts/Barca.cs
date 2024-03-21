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
    Cada barca conoce los dos triggers del rio al que pertenece. Estos triggers son la que le dicen
    que se mueva con pasajero o sin pasajero. Si no tiene pasajero simplemente se mueve a traves del rio
    cuando se le indica y se para al llegar a la otra orilla. Si tiene pasajeros al llegar a la otro orilla 
    se encarga de "bajarlos", que significa hacer que no sean sus hijos y colocarlos en la orilla 
 */
public class Barca : MonoBehaviour
{
    public float speed;
    public GameObject b_ini, b_dest;
    GameObject sailor;
    
    Vector3 ini, dest;
    bool moving;

    void Start()
    {
        ini = b_ini.transform.position;
        dest = b_dest.transform.position;
        moving = false;
    }

    //devuelve si la barca esta en movimiento
    public bool isMoving() { return moving; }

    void Update()
    {
        if (moving)
        {
            float mov = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, dest, mov);
            if ((transform.position - dest).magnitude <= 0.1f)
            {
                moving = false;
                if (sailor!=null)
                {
                    sailor.transform.SetParent(null);
                    sailor.transform.position = dest;
                    if (sailor.GetComponent<NavMeshAgent>())
                    {
                        sailor.GetComponent<NavMeshAgent>().enabled = true;
                        sailor.GetComponent<NavMeshAgent>().SetDestination(dest);
                    }
                    else if (sailor.GetComponent<JugadorAgente>())
                        sailor.GetComponent<JugadorAgente>().enabled = true;
                    sailor.GetComponent<Collider>().enabled = true;
                } 
                sailor = null;               
            }
        }
    }

    //se mueve a traves del rio con un pasajero
    public void swapdestiny(GameObject _sailor, Vector3 posOrigen)
    {
        moving = true;
        sailor = _sailor;
        NavMeshAgent nv = sailor.GetComponent<NavMeshAgent>();
        if (nv)
        {
            nv.enabled = false;
        }            
        sailor.transform.SetParent(transform);
        sailor.transform.localPosition = new Vector3(0.0f, 0.15f, 0.0f);

        if (posOrigen != ini)
        {
            dest = ini;
            ini = posOrigen;
        }
    }

    //se mueve a traves del rio sin pasajero
    public void swapDestiny(Vector3 posDest)
    {
        moving = true;
        if (posDest != dest)
        {
            ini = dest;
            dest = posDest;
        }       
    }
}
