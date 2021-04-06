using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Entity", menuName = "Entity/Player", order = 1)]
public class Player : Entity
{
    FireType fireType = FireType.single;
    public Vector2 moveSpeed = new Vector2(10, 10);
    public float boostMult = 1.5f;
    [SerializeField]
    List<Bullet> bullets = null;
    // Start is called before the first frame update
    void Start()
    {
       
    }



    protected override void Die(Player playerRef)
	{
        Actions.OnPlayerKilled?.Invoke(this); //triggered if not null
    }
}
