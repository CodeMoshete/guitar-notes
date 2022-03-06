using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntervalState
{
    public List<WheelPart> WheelParts;
    public List<NoteModel> NoteModels;
    public Button ReplayButton;
    public AudioSource AudioSource;
    public GameObject CorrectLabel;
    public GameObject IncorrectLabel;

    private NoteModel currentNote;
    private WheelPart currentNotePart;
    private int currentOctaveIndex;

    private float resultTimeout = 0f;

    public IntervalState()
    {
        
    }

    public void Initialize()
    {
        ReplayButton.onClick.AddListener(playNote);
        selectRandomNote();
        Service.UpdateManager.AddObserver(OnUpdate);
    }

    private void selectRandomNote()
    {
        currentNote = NoteModels[Random.Range(0, NoteModels.Count - 1)];
        currentOctaveIndex = Random.Range(0, currentNote.OctaveNotes.Count - 1);

        for (int i = 0, count = WheelParts.Count; i < count; ++i)
        {
            if (WheelParts[i].noteLabel.text == currentNote.NoteName)
            {
                currentNotePart = WheelParts[i];
                break;
            }
        }

        playNote();
    }

    private void playNote()
    {
        AudioSource.clip = currentNote.OctaveNotes[currentOctaveIndex];
        AudioSource.Play();
    }

    public void SelectNote(WheelPart selectedNote)
    {
        ReplayButton.gameObject.SetActive(false);
        bool isCorrect = selectedNote == currentNotePart;
        CorrectLabel.SetActive(isCorrect);
        IncorrectLabel.SetActive(!isCorrect);
        currentNotePart.SetIsKey(true);
        resultTimeout = 3f;
    }

    public void SampleNote(WheelPart selectedNote)
    {
        NoteModel tmpModel = null;

        for (int i = 0, count = NoteModels.Count; i < count; ++i)
        {
            if (NoteModels[i].NoteName == selectedNote.noteLabel.text)
            {
                tmpModel = NoteModels[i];
                break;
            }
        }

        AudioSource.clip = tmpModel.OctaveNotes[currentOctaveIndex];
        AudioSource.Play();
    }

    private void OnUpdate(float dt)
    {

    }
}
