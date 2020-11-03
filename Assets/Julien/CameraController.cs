using UnityEngine;
class CameraController : MonoBehaviour
{
    UI ui;
    // Camera
    Camera camera;
    Transform target;
    Vector3 offset;
    public float amplitudeInMenu = 2;
    public float smoothInMenu = .1F;
    public float smoothInGame = .1F;
    void Awake()
    {
        ui = GameObject.Find("UI").GetComponent<UI>();
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
}