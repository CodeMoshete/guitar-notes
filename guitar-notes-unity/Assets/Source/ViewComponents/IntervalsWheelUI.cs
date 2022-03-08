using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntervalsWheelUI : MonoBehaviour
{
    public List<WheelPart> WheelParts;
    public Button ReplayBaseButton;
    public Button ReplayIntervalButton;
    public Button SwitchStateButton;
    public AudioSource AudioSourceBase;
    public AudioSource AudioSourceInterval;
    public GameObject CorrectLabel;
    public GameObject IncorrectLabel;
}
