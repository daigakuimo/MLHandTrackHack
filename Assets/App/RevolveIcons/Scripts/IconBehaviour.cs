using UnityEngine;
using UnityEngine.Events;

namespace MLHandTrackHack.RevolveIcons
{
    public class IconBehaviour : MonoBehaviour
    {
        [SerializeField] private UnityEvent onFistEvents;

        //今一番近いかどうかのフラグ持たせる
        private Transform _player;

        private void Start()
        {
            _player = Camera.main.transform;
        }

        void Update()
        {
            transform.LookAt(_player);
        }

        public void OnFist()
        {
            onFistEvents?.Invoke();
        }
    }
}