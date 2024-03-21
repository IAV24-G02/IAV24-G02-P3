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
 * Mueve el agente en este objeto utilizando la NavMesh hacia la posición del ratón
 */

[RequireComponent(typeof(NavMeshAgent))]
public class MoveToClick : MonoBehaviour
{
    GameObject obj;

    public Transform pointer;

    // Start is called before the first frame update
    void Start()
    {
        obj = new GameObject();
        obj.transform.position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetButton("Fire1") || Input.GetButton("Fire2")) && Camera.main.enabled)
        {
            Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane));
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("PointerLayer")))
            {
                obj.transform.position = hit.point;
                if(GetComponent<NavMeshAgent>().enabled){
                    GetComponent<NavMeshAgent>().SetDestination(obj.transform.position);
                    pointer.gameObject.SetActive(true);
                    pointer.position = hit.point + new Vector3(0, (float)0.5, 0);
                }
            }
        }
    }
}