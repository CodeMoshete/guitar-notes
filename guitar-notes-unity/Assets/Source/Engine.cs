using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Engine : MonoBehaviour
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

    private void Start()
    {
        ReplayButton.onClick.AddListener(playNote);
        selectRandomNote();
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

    private void Update()
    {
        if (resultTimeout > 0)
        {
            resultTimeout -= Time.deltaTime;
            if (resultTimeout <= 0)
            {
                CorrectLabel.SetActive(false);
                IncorrectLabel.SetActive(false);
                ReplayButton.gameObject.SetActive(true);
                currentNotePart.SetIsKey(false);
                selectRandomNote();
            }
        }
    }
}
