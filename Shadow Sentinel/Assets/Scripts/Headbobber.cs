using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Headbobber : MonoBehaviour
{
    [Header("Head Bob Settings")]
    public float bobSpeed = 0.18f;
    public float bobAmount = 0.05f;

    private float defaultYPos = 0;
    private float timer = 0;

    private void Start()
    {
        defaultYPos = transform.localPosition.y;
    }

    public void UpdateHeadbob(Vector3 moveDir, bool isSprinting, bool isCrouching)
    {
        if (moveDir.magnitude > 0.1f)
        {
            timer += Time.deltaTime * (isSprinting ? bobSpeed * 1.5f : bobSpeed);
            float newY = defaultYPos + Mathf.Sin(timer) * bobAmount * (isCrouching ? 0.5f : 1f);
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
        else
        {
            timer = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, defaultYPos, Time.deltaTime * 5f), transform.localPosition.z);
        }
    }
}
