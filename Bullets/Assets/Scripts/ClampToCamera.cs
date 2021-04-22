using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClampToCamera : MonoBehaviour
{
    public Camera mainCamera;
    Vector2 screenBounds;
    float objectWidth;
    float objectHeight;

    void Start()
    {
        //screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));
        //objectWidth = transform.GetComponent<SpriteRenderer>().bounds.extents.x / 2;
        //objectHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y / 2;
    }

    void Update()
    {
        Vector3 pos = mainCamera.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp01(pos.x);
        pos.y = Mathf.Clamp01(pos.y);
        Debug.Log($"Camera clamping poses= {pos}");
        if(pos.x < 0.02f)
		{
            pos.x = 0.02f;
		}
        if(pos.x > 0.98f)
		{
            pos.x = 0.98f;
		}
        if(pos.y < 0.05f)
		{
            pos.y = 0.05f;
		}
        if(pos.y > 0.95f)
		{
            pos.y = 0.95f;
		}
        transform.position = mainCamera.ViewportToWorldPoint(pos);
        /*Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, screenBounds.x + objectWidth, screenBounds.x * -1 - objectWidth);
        viewPos.y = Mathf.Clamp(viewPos.y, screenBounds.y + objectWidth, screenBounds.y * -1 - objectHeight);
        transform.position = viewPos;*/
    }
}
