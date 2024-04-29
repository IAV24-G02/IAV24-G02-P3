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

    private StateMachineManager sMM;

    // Start is called before the first frame update
    void Start()
    {
        sMM = GetComponent<StateMachineManager>();
        initialState = sMM.GetPatrol();
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

        // Se asume que ninguna transición ha sido activada
        Transition triggered = null;

        // Analiza cada transición y guarda la primera
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

            // Completa la transición y devuelve una lista de acciones
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