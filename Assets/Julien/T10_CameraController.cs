using System.Collections;
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
        offset = new Vector3(0, 0, -10);
    }
    void Update()
    {
        if (ui.isGameMenued || ui.isGamePaused) transform.position =
            Vector3.Lerp(transform.position, new Vector3(target.position.x + (Input.mousePosition.x - Screen.width / 2) / (amplitudeInMenu * 1000),
                target.position.y + (Input.mousePosition.y - Screen.height / 2) / (amplitudeInMenu * 1000), offset.z), smoothInMenu);
    }
    void FixedUpdate()
    {
        if (!ui.isGameMenued && !ui.isGamePaused) transform.position = Vector3.Lerp(transform.position, target.position + offset, smoothInGame);
    }
    public void ShakeCamera(float shakeDuration, float shakeAmount)
    {
        StartCoroutine(IEShakeCamera(shakeDuration, shakeAmount));
    }
    IEnumerator IEShakeCamera(float shakeDuration, float shakeAmount)
    {
        while (shakeDuration > 0)
        {
            transform.position = target.position + offset + Random.insideUnitSphere * shakeAmount;
            shakeDuration -= Time.deltaTime;
            yield return 0;
        }
    }
}