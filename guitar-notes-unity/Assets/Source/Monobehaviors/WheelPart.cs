using UnityEngine;
using UnityEngine.UI;
using Utils;

public class WheelPart : MonoBehaviour
{
    private readonly Color COLOR_HOVER = new Color(1f, 1f, 1f, 1f);
    private readonly Color COLOR_NO_HOVER = new Color(0f, 0f, 0f, 0f);

    public Text noteLabel;
    public string Key
    {
        get
        {
            return noteLabel.text;
        }
    }
    private GameObject onImage;
    private GameObject offImage;
    private GameObject keyIndicator;
    private Button selectButton;
    private Button SampleButton;

    public void Awake()
    {
        onImage = UnityUtils.FindGameObject(gameObject, "On");
        offImage = UnityUtils.FindGameObject(gameObject, "Off");
        keyIndicator = UnityUtils.FindGameObject(gameObject, "KeyIndicator");
        SampleButton = UnityUtils.FindGameObject(gameObject, "SampleButton").GetComponent<Button>();
        SampleButton.onClick.AddListener(OnSamplePressed);
        SampleButton.image.color = COLOR_NO_HOVER;
        SampleButton.GetComponent<UIHoverListener>().AddHoverListener(OnSampleHover);
        SetIsKey(false);
        selectButton = GetComponentInChildren<Button>();
        selectButton.onClick.AddListener(OnKeyButtonPressed);
    }

    private void OnSamplePressed()
    {
        Service.EventManager.SendEvent(EventId.OnSampleNotePressed, this);
    }

    private void OnSampleHover(bool isHovering)
    {
        Color hoverColor = isHovering ? COLOR_HOVER : COLOR_NO_HOVER;
        SampleButton.image.color = hoverColor;
    }

    private void OnKeyButtonPressed()
    {
        Service.EventManager.SendEvent(EventId.OnNoteSelected, this);
    }

    public void SetActive(bool isActive)
    {
        onImage.SetActive(isActive);
        offImage.SetActive(!isActive);
    }

    public void SetIsKey(bool isKey)
    {
        keyIndicator.SetActive(isKey);
    }

    public void SetNoteLabel(string content)
    {
        noteLabel.text = content;
    }
}
