using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoniPlatformer;

public class EndCollider : MonoBehaviour
{

    public OpenerController Opener;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        if (Opener.Resetting) return;

        Opener.EndGame();

        collision.GetComponent<PlayerMovement>().enabled = false;
        collision.GetComponent<Rigidbody2D>().gravityScale = 0;
        collision.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        collision.GetComponent<BetterJumping>().enabled = false;
    }



    private void Start()
    {
        StartCoroutine(CoScale(0, 4f, 10f, 12f));
    }

    private void Update()
    {
        transform.localRotation = Quaternion.Euler(transform.localEulerAngles + new Vector3(0, 0, 20 * Time.deltaTime));
    }

    private IEnumerator CoScale(float iterator, float time, float start, float end)
    {
        while (iterator < time)
        {
            iterator += Time.deltaTime;
            if (iterator > time) iterator = time;

            float val = JoniUtility.Easing.GetTerpedPosition(iterator, time, JoniUtility.Easing.Function.Sinusoidal, JoniUtility.Easing.Direction.Out);

            float newS = Mathf.Lerp(start, end, val);

            var pos = transform.localPosition;
            transform.localScale = new Vector3(newS, newS, newS);

            if (iterator == time)
            {
                float temp = end;
                end = start;
                start = temp;
                iterator = 0;
            }

            yield return 0;
        }
    }
}

