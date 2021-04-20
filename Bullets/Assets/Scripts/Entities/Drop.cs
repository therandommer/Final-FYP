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
    public int thisDropSpeed = 2;
    int dropSpeed = 0;
    bool isDropping = false;
    void Start()
    {
        Invoke("StartDrop", dropDelay);
    }
    void Update()
    {
        if(isDropping)
		{
            this.transform.position += new Vector3(Vector2.down.x, Vector2.down.y) * dropSpeed * Time.deltaTime;
		}
    }
    void OnTriggerEnter2D(Collider2D col)
	{
        if(col.gameObject.tag == "Player")
		{
            Actions.OnCollectableAcquired?.Invoke(this);
		}
	}
    void StartDrop()
	{
        dropSpeed = thisDropSpeed;
	}
    void Die()
	{
        Destroy(gameObject);
	}
}
