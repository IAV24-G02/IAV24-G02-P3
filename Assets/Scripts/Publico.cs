using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*
 * Se encarga de controlar si el público debería huir o quedarse en el patio de butacas
 */

public class Publico : MonoBehaviour
{
    //int lucesEncendidas = 2;
    bool miLuzEncendida;
    bool sentado = true;

    GameObject luzAsociada;
    private void Start()
    {
        //lucesEncendidas = 2;
        sentado = true;
    }

    public void LateUpdate()
    {
        //para que rote hacia donde se mueve
        if (GetComponent<NavMeshAgent>().velocity.sqrMagnitude > Mathf.Epsilon)
        {
            transform.rotation = Quaternion.LookRotation(GetComponent<NavMeshAgent>().velocity.normalized);
        }
        else if(miLuzEncendida)  //para que al llegar a su butaca miren hacia delante(el escenario)
            transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    public bool getLuces()
    {
        return sentado;
    }

    public void apagaLuz()
    {
        miLuzEncendida = false;
        sentado = false;
        //lucesEncendidas--;
        //sentado = lucesEncendidas == 2;
    }
    //se llama cuando el fantasma o el vizconde desactivan o activan las luces
    public void enciendeLuz()
    {
        miLuzEncendida = true;
        sentado = true;
        // lucesEncendidas++;
        //sentado = lucesEncendidas == 2;
    }

}
