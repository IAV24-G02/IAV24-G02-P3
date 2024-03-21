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
    Cada rio tiene un trigger en cada orilla que es consciente de si la barca esta ahi o no.

    Si el fantasma llega y esta la barca, se le hace hijo de la barca y se le dice a la barca
    que cambie al otro lado del rio. Si la barca no esta, se desactiva el NavMeshAgent del fantasma
    haciendole esperar hasta que este la barca, que entonces se hace lo explicado anteriormente

    Si el jugador es quien llega y esta la barca, hace lo mismo que con el fantasma. Si la barca no esta,
    se le dice que venga pero el jugador puede seguir moviendose, a diferencia del fantasma. Cuando la barca llega
    comprueba si el jugador esta esperando, si lo esta se le hace hijo de la barca y se mueve al otro lado del rio,
    sino esta esperando no se hace nada. 
 */


public class BarcaTriggers : MonoBehaviour
{
    public GameObject barca;
    public GameObject player;
    public GameObject ghost;
    public OffMeshLink salto;
    public bool barcaAqui;

    //tiempo desde que una barca llega al destino y se puede usarse de nuevo
    public float tiempoEsperaBarca = 10.0f;
    private float enfriamientoBarca = 0;

    private void Update()
    {

        if (enfriamientoBarca > 0)
        {
            enfriamientoBarca -= Time.deltaTime;
        }
       /* if (barca.GetComponent<Barca>().isMoving() && salto.enabled)
            salto.enabled = false;*/
        /*else if (enfriamientoBarca <= 0 && !salto.enabled && !barca.GetComponent<Barca>().isMoving())
        {
            salto.enabled = true;
        }*/
    }

    
    private void OnTriggerEnter(Collider other)
    {
        //si entra la barca chequear si tiene o no pasajeros
        if (other.gameObject == barca )
        {
            barcaAqui = true;
            //sin pasajeros entra en enfriamiento hasta poder usarla de nuevo
            if (barca.transform.childCount != 1)
            {
                enfriamientoBarca = tiempoEsperaBarca;
                //salto.enabled = false;
            }
        }   
        //si es algun posible pasajero y se puede usar la barca 
        else if(enfriamientoBarca <= 0 
                && (other.gameObject == ghost || other.gameObject == player)
                && !barca.GetComponent<Barca>().isMoving())
        {
            //si la barca no esta aqui, llamarla para que venga y si es el fantasma desactivar su NavMeshAgent
            if (!barcaAqui )
            {
                barca.GetComponent<Barca>().swapDestiny(transform.position);
                if(other.gameObject == ghost)
                    ghost.GetComponent<NavMeshAgent>().enabled = false;
            }//si te puedes subir a la barca
            else
            {
                barca.GetComponent<Barca>().swapdestiny(other.gameObject, transform.position);
            }
        }
    }

    //si el jugador ha llamado a la barca y la barca llega y sigue el jugador esperando, se sube
    private void OnTriggerStay(Collider other)
    {
        if( barcaAqui && barca.transform.childCount == 1 && enfriamientoBarca <= 0)
        {
            if(other.gameObject == player )
                barca.GetComponent<Barca>().swapdestiny(player, transform.position);
            else if(other.gameObject == ghost)
                barca.GetComponent<Barca>().swapdestiny(ghost, transform.position);
        }

    }

    //aumenta el coste del salto cuando no esta la barca en esa orilla
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == barca)
        {
            barcaAqui = false;
            //salto.costOverride = 20;
        }
    }

    //metodos para que el fantasma vea a las barcas

    public bool barcaUsable()
    {
        return enfriamientoBarca <= 0;
    }

    public void aumentaCoste()
    {
        salto.costOverride = 20;
    }

    public void disminuyeCoste()
    {
        salto.costOverride = -1;
    }

    public void quitaSalto()
    {
        if(salto.enabled)
            salto.enabled = false;
    }

    public void activaSalto()
    {
        if(!salto.enabled )
            salto.enabled = true;
    }

    public bool isMoving()
    {
        return barca.GetComponent<Barca>().isMoving();
    }
}
