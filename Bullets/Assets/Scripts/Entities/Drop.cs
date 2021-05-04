using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DropType
{
    eScore,
    eWeapon,
    eShield,
    eHealth,
    eMultiplier //might implement this later, currently works with score drops mostly
}
public class Drop : MonoBehaviour
{
    public float dropDelay = 5.0f;
    public DropType type;
    public int dropStrength = 1;
    public int scoreValue = 100;
    public int multiplierIncrease = 1; //higher for more multiBoost
    public float thisDropSpeed = 2;
    float dropSpeed = 0;
    bool isDropping = false;
    bool isPaused = false;
    Rigidbody2D rb = null;
    void OnEnable()
	{
        Actions.OnPause += UpdatePause;
        Actions.OnPlayerKilled += DestroyThis;
        Actions.OnLevelRestart += DestroyThis2;
        Actions.OnLevelComplete += DestroyThis3;
	}
    void OnDisable()
    {
        Actions.OnPause -= UpdatePause;
        Actions.OnPlayerKilled -= DestroyThis;
        Actions.OnLevelRestart -= DestroyThis2;
        Actions.OnLevelComplete -= DestroyThis3;
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
        //Debug.Log("Powerup leaving");
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
    //yup, all do the same thing from each trigger, ensures they aren't left over between respawns.
    void DestroyThis(GameObject _reference)
	{
        Die();
	}
    void DestroyThis2()
    {
        Die();
    }
    void DestroyThis3()
    {
        Die();
    }
}
