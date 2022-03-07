using System;
using UnityEngine;

public class Engine : MonoBehaviour
{
    public NotesCollection NoteCollection;
    private IState currentState;

    private void Start()
    {
        SwitchToNoteState();
    }

    private void SwitchToNoteState()
    {
        Debug.Log("Switching to note state");
        SwitchState(new NoteState(), SwitchToIntervalState);
    }

    private void SwitchToIntervalState()
    {
        Debug.Log("Switching to interval state");
        SwitchState(new IntervalState(), SwitchToNoteState);
    }

    private void SwitchState(IState nextState, Action nextTransition)
    {
        if (currentState != null)
        {
            currentState.Dispose();
        }
        currentState = nextState;
        currentState.Initialize(NoteCollection, nextTransition);
    }
}