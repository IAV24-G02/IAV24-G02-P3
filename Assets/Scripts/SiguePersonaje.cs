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
//se usa para que la camara siga al jugador
public class SiguePersonaje : MonoBehaviour {

    public Transform target;
    //public float distance = 15;
    Vector3 offset;
    List<GameObject> obstacles;
    Vector3 focusPoint;
    [SerializeField, Range(1f, 360f)]
    float rotationSpeed = 90f;
    Vector2 orbitAngles = new Vector2(45f, 0f);

    void Awake()
    {
        focusPoint = target.position;
    }
    //Asignamos un offset al principio que será la distancia entre la cámara y el jugador
    void Start ()
    {
        offset = transform.position - target.position;
        obstacles = new List<GameObject>();
	}

    void Update()
    {
        avoidObstacles();
    }

    //LateUpdate para asegurar que la cámara se mueva siempre después del jugador
    void LateUpdate ()
    {
        /* if (target != null)                                         //Si el jugador muere (se destruye), la cámara pierde su referencia, por eso se comprueba si existe
             transform.position = target.position + offset;*/

        UpdateFocusPoint();
        ManualRotation();
        Quaternion lookRotation = Quaternion.Euler(orbitAngles);
        Vector3 lookDirection = lookRotation * Vector3.forward;
        Vector3 lookPosition = focusPoint - lookDirection * offset.magnitude;
        transform.SetPositionAndRotation(lookPosition, lookRotation);
       
    }

    void UpdateFocusPoint()
    {
        Vector3 targetPoint = target.position;
        focusPoint = targetPoint;
    }

    void ManualRotation()
    {
        Vector2 input = new Vector2(
            Input.GetAxis("Vertical Camera"),
            Input.GetAxis("Horizontal Camera")
        );
        const float e = 0.001f;
        if (input.x < -e || input.x > e || input.y < -e || input.y > e)
        {
            orbitAngles += rotationSpeed * Time.unscaledDeltaTime * input;
        }
        else
        {
            orbitAngles = new Vector2(30,0);
        }
    }

    // Hace invisible cualquier obstaculo que se interponga entre la camara y el jugador
    void avoidObstacles()
    {
        List<GameObject> newObs = new List<GameObject>();
        RaycastHit[] hits = Physics.RaycastAll(transform.position, -offset.normalized, offset.magnitude);
        for(int i = 0; i < hits.Length; i++)
        {
          
            RaycastHit hit = hits[i];
            GameObject other = hit.collider.gameObject;
            if (other.GetComponent<Player>() == null)
            {
                MeshRenderer rend = other.GetComponent<MeshRenderer>();
                if (rend)
                {
                    newObs.Add(other);
                    rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;     // Off si queda mal
                }
            }
        }
        for(int i = 0; i < obstacles.Count; i++)
        {
            if (!newObs.Contains(obstacles[i]))
            {
                obstacles[i].GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
            }
        }
        obstacles = newObs;
    }
}
