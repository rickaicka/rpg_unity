using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    
    public float zoomSpeed = 4f;
    public float minZoom = 5f;
    public float maxZoom = 15f;

    public float pitch = 2f;
    private float _currentZoom = 10f;
    

    // Update is called once per frame
    void Update()
    {
        _currentZoom -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;
        _currentZoom = Mathf.Clamp(_currentZoom, minZoom, maxZoom);    
    }

    private void LateUpdate()
    {
        transform.position = player.position - offset * _currentZoom;
        transform.LookAt(player.position + Vector3.up * pitch);
    }
}
