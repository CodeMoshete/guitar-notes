using System.Collections.Generic;
using UnityEngine.UI;

public class ChordsDisplayComponent
{
    private List<WheelPart> wheelParts;
    private Text display;

    public ChordsDisplayComponent(Text displayField, List<WheelPart> wheelParts)
    {
        display = displayField;
        this.wheelParts = wheelParts;
    }

    // F    : F, A, C      - 0, 4, 7
    // Fm   : F, Ab,C      - 0, 3, 7
    // F7   : F, A, C, Eb  - 0, 4, 7, 10
    // Fmaj7: F, A, C, E   - 0, 4, 7, 11
    // Fmin7: F, Ab,C, Eb  - 0, 3, 7, 10
    // Fm7b5: F, Ab,B, Eb  - 0, 3, 6, 10
    public void SetKeyIndex(WheelPart key)
    {
        List<string> chordsList = new List<string>();
        List<string> currentNotes = Constants.NoteStringAssociations[key.Key];
        int startIndex = GetNoteIndex(key.Key);
        List<int> offsetIndices = GetOffsetIndices(startIndex);
        for (int i = 0, count = Constants.ACTIVE_INDICES.Count; i < count; ++i)
        {
            int currentNoteIndex =
                ComputeIndex(Constants.ACTIVE_INDICES[i] + startIndex);
            string baseNote = currentNotes[currentNoteIndex];

            bool majorThree =
                offsetIndices.Contains(ComputeIndex(currentNoteIndex + 4));
            bool flatThree =
                offsetIndices.Contains(ComputeIndex(currentNoteIndex + 3));
            bool flatFive =
                offsetIndices.Contains(ComputeIndex(currentNoteIndex + 6));
            bool majorFive =
                offsetIndices.Contains(ComputeIndex(currentNoteIndex + 7));
            bool sharpFive =
                offsetIndices.Contains(ComputeIndex(currentNoteIndex + 8));
            bool majorSeven =
                offsetIndices.Contains(ComputeIndex(currentNoteIndex + 11));
            bool flatSeven =
                offsetIndices.Contains(ComputeIndex(currentNoteIndex + 10));

            if (majorThree && majorFive)
            {
                chordsList.Add(baseNote);
            }

            if (flatThree && !flatFive)
            {
                chordsList.Add(baseNote + "m");
            }

            if (!flatThree && !flatFive && flatSeven)
            {
                chordsList.Add(baseNote + "7");
            }

            if (majorThree&& majorFive && majorSeven)
            {
                chordsList.Add(baseNote + "maj7");
            }

            if (flatThree && !flatFive && flatSeven)
            {
                chordsList.Add(baseNote + "m7");
            }

            if (flatThree && flatFive && flatSeven)
            {
                chordsList.Add(baseNote + "m7b5");
            }
        }
        display.text = string.Join("\n", chordsList.ToArray());
    }

    private int GetNoteIndex(string noteName)
    {
        int index = Constants.NOTE_LABELS_SHARP.IndexOf(noteName);
        index = index < 0 ? Constants.NOTE_LABELS_FLAT.IndexOf(noteName) : index;
        return index;
    }

    private int ComputeIndex(int input)
    {
        if (input > 11)
        {
            input -= 12;
        }
        return input;
    }

    private List<int> GetOffsetIndices(int startIndex)
    {
        List<int> offsetIndices = new List<int>();
        for (int i = 0, count = Constants.ACTIVE_INDICES.Count; i < count; ++i)
        {
            offsetIndices.Add(ComputeIndex(Constants.ACTIVE_INDICES[i] + startIndex));
        }
        return offsetIndices;
    }
}
