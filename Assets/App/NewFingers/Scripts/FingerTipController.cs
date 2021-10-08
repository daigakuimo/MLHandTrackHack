using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace MLHandTrackHack.NewFingers
{
    public class FingerTipController : MonoBehaviour
    {
        [SerializeField] private NewFingerApp newFingerApp;
        public NewFingerView.NewFingerType type = NewFingerView.NewFingerType.Index;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Thumb"))
            {
                newFingerApp.StartApp(type);
            }
        }
    }
}