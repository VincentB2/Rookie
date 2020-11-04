﻿using System.Collections;
using UnityEngine;
class T10_CameraController : MonoBehaviour
{
    T10_UI ui;
    // Camera
    Camera camera;
    Transform target;
    Vector3 offset;
    public float amplitudeInMenu = 2,
        smoothInMenu = .1F,
        smoothInGame = .1F;
    void Awake()
    {
        ui = GameObject.Find("UI").GetComponent<T10_UI>();
        // Camera
        camera = GetComponent<Camera>();
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
        offset = transform.position;
    }
    void Update()
    {
        if (ui.isGameMenued || ui.isGamePaused) transform.position =
            Vector3.Lerp(transform.position, new Vector3((Input.mousePosition.x - Screen.width / 2) / (amplitudeInMenu * 1000),
                (Input.mousePosition.y - Screen.height / 2) / (amplitudeInMenu * 1000), offset.z), smoothInMenu);
        else transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothInGame);
    }
    public void ShakeCamera(float shakeDuration, float shakeAmount)
    {
        StartCoroutine(IEShakeCamera(shakeDuration, shakeAmount));
    }
    IEnumerator IEShakeCamera(float shakeDuration, float shakeAmount)
    {
        while (shakeDuration > 0)
        {
            transform.position = offset + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime;
            yield return 0;
        }
    }
}