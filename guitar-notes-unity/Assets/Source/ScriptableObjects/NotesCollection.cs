using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/NotesCollection", order = 1)]
public class NotesCollection : ScriptableObject
{
    public List<NoteModel> NoteModels;
}
