using System;

public interface IState
{
    void Initialize(NotesCollection noteCollection, Action onSwitchState);
    void Dispose();
}
