using System;
using UnityEngine;

public class IntervalState : IState
{
    private const string INTERVAL_RESOURCE = "IntervalsUI";
    private const float RESULT_TIME = 3f;
    private const float INTERVAL_PLAY_TIME = 1f;
    private readonly int[] MAJOR_INTERVAL_INDECIES = new int[] {0, 2, 4, 5, 7, 9, 11};

    private GameObject intervalWheel;
    private NotesCollection noteCollection;
    private IntervalsWheelUI wheelUi;
    private NoteModel currentNote;
    private WheelPart currentIntervalPart;
    private int currentOctaveIndex;
    private NoteModel currentIntervalNote;
    private int currentIntervalIndex;
    private Action onSwitchState;

    private float resultTimeout = 0f;
    private bool includeAllIntervals;

    public void Initialize(NotesCollection noteCollection, Action onSwitchState)
    {
        this.onSwitchState = onSwitchState;
        this.noteCollection = noteCollection;
        intervalWheel = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>(INTERVAL_RESOURCE));
        wheelUi = intervalWheel.GetComponent<IntervalsWheelUI>();
        wheelUi.ReplayIntervalButton.onClick.AddListener(PlayInterval);
        wheelUi.ReplayBaseButton.onClick.AddListener(PlayBase);
        wheelUi.SwitchStateButton.onClick.AddListener(SwitchState);
        wheelUi.IncludeAllIntervalsToggle.onValueChanged.AddListener(OnIncludeIntervalsToggled);
        includeAllIntervals = wheelUi.IncludeAllIntervalsToggle.isOn;
        Service.EventManager.AddListener(EventId.OnNoteSelected, OnNoteSelected);
        Service.EventManager.AddListener(EventId.OnSampleNotePressed, OnSampleNotePressed);
        SelectRandomInterval();
        Service.UpdateManager.AddObserver(OnUpdate);
    }

    private void SelectRandomInterval()
    {
        currentNote = noteCollection.NoteModels[UnityEngine.Random.Range(0, noteCollection.NoteModels.Count - 1)];
        currentOctaveIndex = UnityEngine.Random.Range(0, currentNote.OctaveNotes.Count - 1);

        if (includeAllIntervals)
        {
            currentIntervalIndex = UnityEngine.Random.Range(0, 12);
        }
        else
        {
            int randomSelectionIndex = UnityEngine.Random.Range(0, MAJOR_INTERVAL_INDECIES.Length);
            currentIntervalIndex = MAJOR_INTERVAL_INDECIES[randomSelectionIndex];
        }

        currentIntervalNote = GetNodeModelForInterval(currentNote, currentIntervalIndex);
        currentIntervalPart = wheelUi.WheelParts[currentIntervalIndex];
            
        PlayBaseAndInterval();
    }

    private void OnIncludeIntervalsToggled(bool newValue)
    {
        includeAllIntervals = newValue;
        SelectRandomInterval();
    }

    private bool OnNoteSelected(object cookie)
    {
        WheelPart wheelPart = (WheelPart)cookie;
        SelectInterval(wheelPart);
        return true;
    }

    private bool OnSampleNotePressed(object cookie)
    {
        WheelPart wheelPart = (WheelPart)cookie;
        SampleNote(wheelPart);
        return true;
    }

    private void PlayBase()
    {
        wheelUi.AudioSourceBase.clip = currentNote.OctaveNotes[currentOctaveIndex];
        wheelUi.AudioSourceBase.Play();
    }

    private void PlayInterval()
    {
        wheelUi.AudioSourceInterval.clip = currentIntervalNote.OctaveNotes[currentOctaveIndex];
        wheelUi.AudioSourceInterval.Play();
    }

    private void PlayBaseAndInterval()
    {
        PlayBase();
        Service.TimerManager.CreateTimer(INTERVAL_PLAY_TIME, (object o) =>
        {
            PlayInterval();
        }, null, null);
    }

    private void SwitchState()
    {
        onSwitchState();
    }

    public void SelectInterval(WheelPart selectedInterval)
    {
        wheelUi.ReplayBaseButton.gameObject.SetActive(false);
        wheelUi.ReplayIntervalButton.gameObject.SetActive(false);
        int selectedIntervalIndex = wheelUi.WheelParts.IndexOf(selectedInterval);
        bool isCorrect = selectedIntervalIndex == currentIntervalIndex;
        wheelUi.CorrectLabel.SetActive(isCorrect);
        wheelUi.IncorrectLabel.SetActive(!isCorrect);
        currentIntervalPart.SetIsKey(true);
        resultTimeout = RESULT_TIME;
    }

    public void SampleNote(WheelPart selectedInterval)
    {
        int selectedIntervalIndex = wheelUi.WheelParts.IndexOf(selectedInterval);
        NoteModel tmpModel = GetNodeModelForInterval(currentNote, selectedIntervalIndex);
        wheelUi.AudioSourceBase.clip = tmpModel.OctaveNotes[currentOctaveIndex];
        wheelUi.AudioSourceBase.Play();
    }

    private NoteModel GetNodeModelForInterval(NoteModel baseNote, int interval)
    {
        NoteModel tmpModel = null;

        int noteIndex = noteCollection.NoteModels.IndexOf(currentNote) + interval;
        int numNotes = noteCollection.NoteModels.Count;
        if (noteIndex > numNotes - 1)
        {
            noteIndex -= numNotes;
        }
        tmpModel = noteCollection.NoteModels[noteIndex];

        return tmpModel;
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
                wheelUi.ReplayBaseButton.gameObject.SetActive(true);
                wheelUi.ReplayIntervalButton.gameObject.SetActive(true);
                currentIntervalPart.SetIsKey(false);
                SelectRandomInterval();
            }
        }
    }

    public void Dispose()
    {
        GameObject.Destroy(intervalWheel);
        Service.UpdateManager.RemoveObserver(OnUpdate);
        Service.EventManager.RemoveListener(EventId.OnNoteSelected, OnNoteSelected);
        Service.EventManager.RemoveListener(EventId.OnSampleNotePressed, OnSampleNotePressed);
    }
}
