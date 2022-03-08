using System;
using UnityEngine;

public class IntervalState : IState
{
    private const string INTERVAL_RESOURCE = "IntervalsUI";
    private const float RESULT_TIME = 3f;
    private const float INTERVAL_PLAY_TIME = 1f;

    private GameObject majorScaleWheel;
    private NotesCollection noteCollection;
    private MajorScalesWheelUI wheelUi;
    private NoteModel currentNote;
    private WheelPart currentNotePart;
    private int currentOctaveIndex;
    private Action onSwitchState;

    private float resultTimeout = 0f;
    private float intervalPlayTimeout = 0f;

    public void Initialize(NotesCollection noteCollection, Action onSwitchState)
    {
        this.onSwitchState = onSwitchState;
        this.noteCollection = noteCollection;
        majorScaleWheel = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(INTERVAL_RESOURCE));
        wheelUi = majorScaleWheel.GetComponent<MajorScalesWheelUI>();
        wheelUi.ReplayButton.onClick.AddListener(PlayNote);
        wheelUi.SwitchStateButton.onClick.AddListener(SwitchState);
        Service.EventManager.AddListener(EventId.OnNoteSelected, OnNoteSelected);
        Service.EventManager.AddListener(EventId.OnSampleNotePressed, OnSampleNotePressed);
        SelectRandomInterval();
        Service.UpdateManager.AddObserver(OnUpdate);
    }

    private void SelectRandomInterval()
    {
        currentNote = noteCollection.NoteModels[UnityEngine.Random.Range(0, noteCollection.NoteModels.Count - 1)];
        currentOctaveIndex = UnityEngine.Random.Range(0, currentNote.OctaveNotes.Count - 1);

        for (int i = 0, count = wheelUi.WheelParts.Count; i < count; ++i)
        {
            if (wheelUi.WheelParts[i].noteLabel.text == currentNote.NoteName)
            {
                currentNotePart = wheelUi.WheelParts[i];
                break;
            }
        }

        PlayNote();
    }

    private bool OnNoteSelected(object cookie)
    {
        WheelPart wheelPart = (WheelPart)cookie;
        SelectNote(wheelPart);
        return true;
    }

    private bool OnSampleNotePressed(object cookie)
    {
        WheelPart wheelPart = (WheelPart)cookie;
        SampleNote(wheelPart);
        return true;
    }

    private void PlayNote()
    {
        wheelUi.AudioSource.clip = currentNote.OctaveNotes[currentOctaveIndex];
        wheelUi.AudioSource.Play();
    }

    private void SwitchState()
    {
        onSwitchState();
    }

    public void SelectNote(WheelPart selectedNote)
    {
        wheelUi.ReplayButton.gameObject.SetActive(false);
        bool isCorrect = selectedNote == currentNotePart;
        wheelUi.CorrectLabel.SetActive(isCorrect);
        wheelUi.IncorrectLabel.SetActive(!isCorrect);
        currentNotePart.SetIsKey(true);
        resultTimeout = RESULT_TIME;
    }

    public void SampleNote(WheelPart selectedNote)
    {
        NoteModel tmpModel = null;

        for (int i = 0, count = noteCollection.NoteModels.Count; i < count; ++i)
        {
            if (noteCollection.NoteModels[i].NoteName == selectedNote.noteLabel.text)
            {
                tmpModel = noteCollection.NoteModels[i];
                break;
            }
        }

        wheelUi.AudioSource.clip = tmpModel.OctaveNotes[currentOctaveIndex];
        wheelUi.AudioSource.Play();
    }

    private void OnUpdate(float dt)
    {
        if (resultTimeout > 0)
        {
            resultTimeout -= Time.deltaTime;
            if (resultTimeout <= 0)
            {
                wheelUi.CorrectLabel.SetActive(false);
                wheelUi.IncorrectLabel.SetActive(false);
                wheelUi.ReplayButton.gameObject.SetActive(true);
                currentNotePart.SetIsKey(false);
                SelectRandomInterval();
            }
        }
    }

    public void Dispose()
    {
    }
}
