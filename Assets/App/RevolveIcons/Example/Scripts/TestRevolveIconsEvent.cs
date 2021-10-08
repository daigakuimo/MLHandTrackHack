using UnityEngine;
using UnityEngine.UI;

namespace MLHandTrackHack.RevolveIcons
{
    public class TestRevolveIconsEvent : MonoBehaviour
    {
        private Text _resultText;
        private void Start()
        {
            _resultText = GameObject.Find("/[CONTENTS]/Canvas/Result").GetComponent<Text>();
        }
        public void OnIconFist(int number)
        {
            _resultText.text = number.ToString();
        }
    }
}