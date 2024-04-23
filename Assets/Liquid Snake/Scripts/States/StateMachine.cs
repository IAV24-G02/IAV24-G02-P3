using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    // Estamos en un estado cada vez

    State initialState;
    State currentState;

    // Start is called before the first frame update
    void Start()
    {
        currentState = initialState;
    }

    void Update()
    {
        GetAllActions();
    }

    // Update is called once per frame
    List<Action> GetAllActions()
    {
        List<Action> actions;

        // Assume no transition is triggered.
        Transition triggered = null;

        // Check through each transition and store the first
        // one that triggers.

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
            // Add the exit action of the old state, the
            // transition action and the entry for the new state.
            actions = currentState.GetExitActions();
            actions.AddRange(triggered.GetActions());
            actions.AddRange(targetState.GetEntryActions());

            // Complete the transition and return the action list.
            currentState = targetState;
            return actions;
        }
        
        // Otherwise just return the current state’s actions.
        else
        {
            return currentState.GetActions();
        }
    }
}