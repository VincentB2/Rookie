using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementPlayer : MonoBehaviour
{
    public float speed;

    private RaycastHit2D hit;

    [SerializeField] private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 heading = new Vector2(mousePos.x - transform.position.x, mousePos.y - transform.position.y);
        float distance = heading.magnitude;
        Vector2 direction = heading / distance;

        float actualSpeed = speed;
        if (distance <= 1.3f && distance >= 0.1f)
        {
            actualSpeed = actualSpeed * (distance - 0.1f);

        }
        if (distance < 0.1f)
        {
            actualSpeed = 0;
        }

        rb.velocity = direction * actualSpeed;

    }
}
