using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IconBehaviour : MonoBehaviour
{
    [SerializeField] private UnityEvent OnFistEvents;
    //今一番近いかどうかのフラグ持たせる
    private Transform _player;

    private bool _isLaunch = false;
    
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
        OnFistEvents?.Invoke();
    }
}
