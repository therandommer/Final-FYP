using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DropType
{
    eScore,
    eWeapon,
    eShield,
    eHealth
}
public class Drop : MonoBehaviour
{
    public float dropDelay = 5.0f;
    public DropType type;
    public int dropStrength = 1;
    public int scoreValue = 100;
    public float thisDropSpeed = 2;
    float dropSpeed = 0;
    bool isDropping = false;
    bool isPaused = false;
    Rigidbody2D rb = null;
    void OnEnable()
	{
        Actions.OnPause += UpdatePause;
	}
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Invoke("StartDrop", dropDelay);
    }
    void Update()
    {
        if(isDropping && !isPaused)
		{
            rb.velocity = new Vector2(0, -dropSpeed);
		}
        if(isPaused && isDropping)
		{
            rb.velocity = Vector2.zero;
		}
    }
    void OnTriggerEnter2D(Collider2D col)
	{
        if(col.gameObject.tag == "Player")
		{
            Actions.OnCollectableAcquired?.Invoke(this);
            Die();
		}
	}
    void StartDrop()
	{
        Debug.Log("Powerup leaving");
        isDropping = true;
        dropSpeed = thisDropSpeed;
	}
    void UpdatePause()
	{
        isPaused = !isPaused;
	}
    void Die()
	{
        Destroy(gameObject);
	}
}
