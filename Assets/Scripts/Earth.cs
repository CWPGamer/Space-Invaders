using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Earth : MonoBehaviour
{
    public static bool touchedEarth;
    // Start is called before the first frame update
    void Start()
    {
        touchedEarth = false;
        Debug.Log($"touchedEarth = {touchedEarth}");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Earth hit by enemy");
            touchedEarth = true;
            Debug.Log($"touchedEarth = {touchedEarth}");
            GameManager.Instance.GameOver();
        }
    }
}
