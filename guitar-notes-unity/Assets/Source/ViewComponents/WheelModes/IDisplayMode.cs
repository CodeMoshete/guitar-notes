using System.Collections.Generic;
using UnityEngine.UI;

public interface IDisplayMode
{
    void Initialize(
        List<WheelPart> wheelParts,
        List<Text> noteNumbers,
        List<Text> noteLabels,
        Text keyOfText,
        ChordsDisplayComponent chordsComponent);

    void SetKeyIndex(WheelPart key);

    string DisplayName { get; }
}
