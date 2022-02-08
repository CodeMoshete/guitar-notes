using System.Collections.Generic;

public class Constants
{
    public static readonly List<string> NOTE_LABELS_SHARP = new List<string>(
        new string[] { "G#", "A", "A#", "B", "C", "C#", "D", "D#",
                        "E", "F", "F#", "G" });

    public static readonly List<string> NOTE_LABELS_FLAT = new List<string>(
        new string[] { "Ab", "A", "Bb", "B", "C", "Db", "D", "Eb",
                        "E", "F", "Gb", "G" });

    private static Dictionary<string, List<string>> noteStringAssociations;
    public static Dictionary<string, List<string>> NoteStringAssociations
    {
        get
        {
            if (noteStringAssociations == null)
            {
                noteStringAssociations = new Dictionary<string, List<string>>();
                noteStringAssociations.Add("Ab", NOTE_LABELS_FLAT);   // Ab
                noteStringAssociations.Add("G#", NOTE_LABELS_FLAT);
                noteStringAssociations.Add("A", NOTE_LABELS_SHARP);  // A
                noteStringAssociations.Add("Bb", NOTE_LABELS_FLAT);   // Bb
                noteStringAssociations.Add("A#", NOTE_LABELS_FLAT);
                noteStringAssociations.Add("B", NOTE_LABELS_SHARP);  // B
                noteStringAssociations.Add("C", NOTE_LABELS_SHARP);  // C
                noteStringAssociations.Add("Db", NOTE_LABELS_FLAT);   // Db
                noteStringAssociations.Add("C#", NOTE_LABELS_FLAT);
                noteStringAssociations.Add("D", NOTE_LABELS_SHARP);  // D
                noteStringAssociations.Add("Eb", NOTE_LABELS_FLAT);   // Eb
                noteStringAssociations.Add("D#", NOTE_LABELS_FLAT);
                noteStringAssociations.Add("E", NOTE_LABELS_SHARP);  // E
                noteStringAssociations.Add("F", NOTE_LABELS_FLAT);   // F
                noteStringAssociations.Add("F#", NOTE_LABELS_SHARP);  // F#
                noteStringAssociations.Add("Gb", NOTE_LABELS_SHARP);
                noteStringAssociations.Add("G", NOTE_LABELS_SHARP);  // G
            }
            return noteStringAssociations;
        }
    }

    private static List<List<string>> noteIntAssociations;
    public static List<List<string>> NoteIntAssociations
    {
        get
        {
            if (noteIntAssociations == null)
            {
                noteIntAssociations = new List<List<string>>();
                noteIntAssociations.Add(NOTE_LABELS_FLAT);   // Ab
                noteIntAssociations.Add(NOTE_LABELS_SHARP);  // A
                noteIntAssociations.Add(NOTE_LABELS_FLAT);   // Bb
                noteIntAssociations.Add(NOTE_LABELS_SHARP);  // B
                noteIntAssociations.Add(NOTE_LABELS_SHARP);  // C
                noteIntAssociations.Add(NOTE_LABELS_FLAT);   // Db
                noteIntAssociations.Add(NOTE_LABELS_SHARP);  // D
                noteIntAssociations.Add(NOTE_LABELS_FLAT);   // Eb
                noteIntAssociations.Add(NOTE_LABELS_SHARP);  // E
                noteIntAssociations.Add(NOTE_LABELS_FLAT);   // F
                noteIntAssociations.Add(NOTE_LABELS_SHARP);  // F#
                noteIntAssociations.Add(NOTE_LABELS_SHARP);  // G
            }
            return noteIntAssociations;
        }
    }

    public static readonly List<int> ACTIVE_INDICES =
        new List<int>(new int[] { 0, 2, 4, 5, 7, 9, 11 });

    public static readonly List<string> NOTE_NUMBERS = new List<string>(
        new string[] { "1", "2/9", "3", "4/11", "5", "6/13", "7" });
}
