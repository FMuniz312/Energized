using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    [SerializeField] GameObject fingerRep;
 
    public void VisualTouchManager()
    {
#if UNITY_ANDROID || UNITY_IOS
        //Player touched the screen and is moving the finger in any direction
        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved)
        {
            if (!fingerRep.activeSelf) fingerRep.SetActive(true);
            //If the player touches the screen and moves the finger through the projectile, the character should move
            Vector3 fingerpos = Input.GetTouch(0).position;

            Vector3 worldFingerPos = Camera.main.ScreenToWorldPoint(new Vector3(fingerpos.x, fingerpos.y, 10));

            fingerRep.transform.position = worldFingerPos;

        }
        else
        {
            if (fingerRep.activeSelf) fingerRep.SetActive(false);

        }
#else
        if (Input.GetMouseButton(0))
        {
            if (!fingerRep.activeSelf) fingerRep.SetActive(true);
            //If the player touches the screen and moves the finger through the projectile, the character should move
            
            fingerRep.transform.position = MouseHelper.MouseWorldPos();

        }
        else
        {
            if (fingerRep.activeSelf) fingerRep.SetActive(false);

        }
#endif
    }
    public Vector2 GetMoveDirection()
    {
#if UNITY_ANDROID || UNITY_IOS
        return Input.touches[0].deltaPosition.normalized;

#else
        return MouseHelper.mouseDelta.normalized;

#endif
    }
 
}
