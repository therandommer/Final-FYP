using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameplay : MonoBehaviour
{
    [SerializeField]
    public Player playerStats;
    Vector2 mousePos = Vector2.zero;
    Vector2 mousePosWorld = Vector2.zero;
    Vector2 movement = Vector2.zero;
    float inputX = 0.0f;
    float inputY = 0.0f;
    bool isBoosting = false;
    float angle = 0.0f; // angle used to go between player and mouse

    FireType fireType = FireType.single;
    [SerializeField]
    Rigidbody2D rb = null;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError($"No Rigid Body for player: " + this.name);
        }
    }
    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        mousePosWorld = Camera.main.ScreenToWorldPoint(mousePos);
        inputX = Input.GetAxis("Horizontal");
        inputY = Input.GetAxis("Vertical");
        if (!isBoosting)
        {
            movement = new Vector2(inputX, inputY) * playerStats.moveSpeed;
        }
        else
        {
            movement = new Vector2(inputX, inputY) * playerStats.moveSpeed * playerStats.boostMult;
        }
        LookAtMouse(this.transform.position, mousePosWorld);
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log(mousePos.x);
            Debug.Log(mousePos.y);
            Debug.Log($"World {mousePosWorld.x}");
            Debug.Log($"World {mousePosWorld.y}");
        }
        if (Input.GetButton("Fire1") && fireType == FireType.auto)
        {

        }
        if(Input.GetButtonDown("Boost"))
		{
            isBoosting = true;
		}
        if(Input.GetButtonUp("Boost"))
		{
            isBoosting = false;
		}
    }

    void FixedUpdate()
    {
        rb.velocity = movement;
    }

    void LookAtMouse(Vector3 a, Vector3 b) // a = player, b = cursor
	{
        float AngleRad = Mathf.Atan2(b.y - a.y, b.x - a.x);
        float AngleDeg = (180 / Mathf.PI) * AngleRad;
        this.transform.rotation = Quaternion.Euler(0, 0, AngleDeg - 90);
	}
}
