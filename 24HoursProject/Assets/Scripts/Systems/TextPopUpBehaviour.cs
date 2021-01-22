using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPopUpBehaviour : MonoBehaviour
{
    Vector3 MoveSpeed;
    TextMeshPro TextMeshPro;
    bool SetupReady;
    float Duration;
    // Start is called before the first frame update
     
    public void Setup(Vector3 direction, float moveSpeed, Color color, float duration = 2)
    {
        MoveSpeed = direction * moveSpeed;
        TextMeshPro = GetComponent<TextMeshPro>();
        Duration = duration;
        TextMeshPro.color = color;
        SetupReady = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (SetupReady)
        {
            transform.position += MoveSpeed * Time.deltaTime;
            TextMeshPro.alpha -= Time.deltaTime/Duration;
            if (TextMeshPro.alpha <= 0)
            {
                Destroy(this.gameObject);
            }

        }
    }
}
