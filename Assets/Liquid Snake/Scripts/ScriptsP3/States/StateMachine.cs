using LiquidSnake.Enemies;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    [SerializeField]
    GameObject initialWaypoint;
    // Estamos en un estado cada vez

    State initialState;
    State currentState;

    List<IAction> currentActions;

    // Start is called before the first frame update
    void Awake()
    {
        //Creas estados
        //Creas acciones y transiciones y se las a�ades
        //Crear condiciones y se las a�ades a las transiciones.
        initialState = new Patrol(gameObject);
        currentState = initialState;
    }

    void Update()
    {
        currentActions = GetAllActions();
        foreach (IAction action in currentActions)
        {
            action.Update();
        }
    }

    // Update is called once per frame
    List<IAction> GetAllActions()
    {
        List<IAction> actions = new List<IAction>();

        // Se asume que ninguna transici�n ha sido activada
        Transition triggered = null;

        // Analiza cada transici�n y guarda la primera
        // que se active

        foreach (Transition transition in currentState.GetTransitions())
        {
            if (transition.IsTriggered())
            {
                triggered = transition;
            }
        }

        if (triggered != null)
        {
            State targetState = triggered.GetTargetState();
            // A�ade la acci�n saliente al antiguo estado, la
            // acci�n de transici�n y la entrante para el nuevo estado
            // actions = currentState.GetExitActions();
            actions.AddRange(triggered.GetActions());
            // actions.AddRange(targetState.GetEntryActions());

            // Completa la transici�n y devuelve una lista de acciones
            currentState = targetState;
            return actions;
        }
        
        // En otro caso solo devuelve las acciones del estado actual
        else
        {
            return currentState.GetWhileActions();
        }
    }
}