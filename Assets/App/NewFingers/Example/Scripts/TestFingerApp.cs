using UnityEngine;
using UnityEngine.UI;

namespace MLHandTrackHack.NewFingers
{
    public class TestFingerApp : NewFingerApp
    {
        [SerializeField] private Text resultText;
        public override void StartApp(NewFingerView.NewFingerType type)
        {
            resultText.text = type.ToString();
        }
    }
}
