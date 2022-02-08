using System.Collections.Generic;
using UnityEngine.UI;

public class NoteRotateDisplayMode : IDisplayMode
{
    private List<WheelPart> WheelParts;
    private List<Text> NoteNumbers;
    private List<Text> NoteLabels;
    private Text KeyOfText;
    private ChordsDisplayComponent chordsComponent;

    private List<string> previousNoteAssociations;

    public string DisplayName
    {
        get
        {
            return "Note Rotation";
        }
    }

    public void Initialize(
        List<WheelPart> wheelParts,
        List<Text> noteNumbers,
        List<Text> noteLabels,
        Text keyOfText,
        ChordsDisplayComponent chordsComponent)
    {
        WheelParts = wheelParts;
        NoteNumbers = noteNumbers;
        NoteLabels = noteLabels;
        KeyOfText = keyOfText;
        this.chordsComponent = chordsComponent;

        SetInitialState();
    }

    private void SetInitialState()
    {
        int index = 0;
        int currentIndex = index;
        int currentNoteIndex = 0;
        chordsComponent.SetKeyIndex(WheelParts[0]);
        for (int i = 0, count = WheelParts.Count; i < count; ++i)
        {
            bool isActive = Constants.ACTIVE_INDICES.Contains(i);
            WheelPart currentWheelpart = WheelParts[i];
            currentWheelpart.SetIsKey(i == 0);
            currentWheelpart.SetActive(isActive);

            Text currentNoteLabel = NoteLabels[i];
            previousNoteAssociations = Constants.NoteStringAssociations["Ab"];
            currentNoteLabel.text = Constants.NoteStringAssociations["Ab"][i];

            Text currentNoteNumber = NoteNumbers[currentIndex];
            currentNoteNumber.text = string.Empty;
            if (isActive)
            {
                if (Constants.NOTE_NUMBERS[currentNoteIndex] != null)
                {
                    currentNoteNumber.text = Constants.NOTE_NUMBERS[currentNoteIndex];
                }
                ++currentNoteIndex;
            }

            currentIndex = currentIndex + 1 >= count ? 0 : currentIndex + 1;
        }
        KeyOfText.text = WheelParts[0].Key;
    }

    public void SetKeyIndex(WheelPart key)
    {
        List<string> noteNames = Constants.NoteStringAssociations[key.Key];
        int startIndex = previousNoteAssociations.IndexOf(key.Key);
        int currentIndex = startIndex;
        for (int i = 0, count = WheelParts.Count; i < count; ++i)
        {
            WheelPart part = WheelParts[i];
            part.SetNoteLabel(noteNames[currentIndex]);
            currentIndex = currentIndex >= noteNames.Count - 1 ? 0 : currentIndex + 1;
        }
        previousNoteAssociations = noteNames;
        KeyOfText.text = WheelParts[0].Key;
        chordsComponent.SetKeyIndex(WheelParts[0]);
    }
}
