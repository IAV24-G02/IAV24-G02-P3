using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Cantante : MonoBehaviour
{
    // Segundos que estara cantando
    public double tiempoDeCanto;
    // Segundo en el que comezo a cantar
    private double tiempoComienzoCanto;
    // Segundos que esta descanasando
    public double tiempoDeDescanso;
    // Segundo en el que comezo a descansar
    private double tiempoComienzoDescanso;
    // Si esta capturada
    public bool capturada = false;

    [Range(0, 180)]
    // Angulo de vision en horizontal
    public double anguloVistaHorizontal;
    // Distancia maxima de vision
    public double distanciaVista;
    // Objetivo al que ver"
    public Transform objetivo;

    // Segundos que puede estar merodeando
    public double tiempoDeMerodeo;
    // Segundo en el que comezo a merodear
    public double tiempoComienzoMerodeo = 0;
    // Distancia de merodeo
    public int distanciaDeMerodeo = 16;
    // Si canta o no
    public bool cantando = false;

    // Componente cacheado NavMeshAgent
    private NavMeshAgent agente;

    // Objetivos de su itinerario
    public Transform Escenario;
    public Transform Bambalinas;

    // La blackboard
    public GameBlackboard bb;

    //para seguir al fantasma o al vizconde
    public GameObject fantasma;

    public void Awake()
    {
        agente = GetComponent<NavMeshAgent>();
    }

    public void Start()
    {
        agente.updateRotation = false;
    }

    public void LateUpdate()
    {
        if (agente.velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(agente.velocity.normalized);
        }
    }

    // Comienza a cantar, reseteando el temporizador
    public void Cantar()
    {
        tiempoComienzoCanto = 0;
        cantando = true;
    }

    // Comprueba si tiene que dejar de cantar
    public bool TerminaCantar()
    {
        // IMPLEMENTAR
        return true;
    }

    // Comienza a descansar, reseteando el temporizador
    public void Descansar()
    {
        // IMPLEMENTAR
    }

    // Comprueba si tiene que dejar de descansar
    public bool TerminaDescansar()
    {
        // IMPLEMENTAR
        return true;
    }

    // Comprueba si se encuentra en la celda
    public bool EstaEnCelda()
    {
        // IMPLEMENTAR
        return true;
    }

    // Comprueba si esta en un sitio desde el cual sabe llegar al escenario
    public bool ConozcoEsteSitio()
    {
        // IMPLEMENTAR
        return true;
    }

    //Mira si ve al vizconde con un angulo de vision y una distancia maxima
    public bool Scan()
    {
        // IMPLEMENTAR
        return true;
    }

    // Genera una posicion aleatoria a cierta distancia dentro de las areas permitidas
    private Vector3 RandomNavSphere(float distance) 
    {
        // IMPLEMENTAR
        return new Vector3();
    }

    // Genera un nuevo punto de merodeo cada vez que agota su tiempo de merodeo actual
    public void IntentaMerodear()
    {
        // IMPLEMENTAR
    }
    public bool GetCapturada()
    {
        // IMPLEMENTAR
        return true;
    }

    public void setCapturada(bool cap)
    {
        // IMPLEMENTAR
    }

    public GameObject sigueFantasma()
    {
        // IMPLEMENTAR
        return null;
    }

    public void sigueVizconde()
    {
        // IMPLEMENTAR
    }

    private void nuevoObjetivo(GameObject obj)
    {
        // IMPLEMENTAR
    }
}
