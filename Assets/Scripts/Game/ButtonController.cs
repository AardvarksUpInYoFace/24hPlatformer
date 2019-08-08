using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    public GameManager GameManager;
    public bool Row, RightUp;
    public int index;



    private bool ButtonPressed;
    private AudioSource myAudio;
    private bool GoingDown, GoingUp;

    private List<Collider2D> Collisions = new List<Collider2D>();

    private void Start()
    {
        myAudio = GetComponent<AudioSource>();
    }

    private void OnPress()
    {
        ButtonPressed = true;
        GoDown();
        GameManager.RotateRooms(Row, index, RightUp);
        myAudio.Play();
    }

    private void Update()
    {
        if(!GameManager.RotatingRooms && !GoingDown)
        {
            if (Collisions.Count < 1 && transform.localPosition.y < 0f && !GoingUp) GoUp();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.RotatingRooms) return;

        if (!ButtonPressed && collision.gameObject.CompareTag("Player"))
        {
            OnPress();
            Collisions.Add(collision);
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") || GoingDown) return;

        if (Collisions.Contains(collision)) Collisions.Remove(collision);

        GoUp();
        ButtonPressed = false;
    }

    private void GoDown()
    {
        StopAllCoroutines();
        GoingDown = true;
        StartCoroutine(CoMoveY(0, 0.5f, transform.localPosition.y, -1.1f));
    }

    private void GoUp()
    {
        StopAllCoroutines();
        GoingUp = true;
        StartCoroutine(CoMoveY(0, 0.1f, transform.localPosition.y, 0f));
    }

    private IEnumerator CoMoveY(float iterator, float time, float start, float end)
    {
        while (iterator < time)
        {
            iterator += Time.deltaTime;
            if (iterator > time) iterator = time;

            float val = JoniUtility.Easing.GetTerpedPosition(iterator, time, JoniUtility.Easing.Function.Sinusoidal, JoniUtility.Easing.Direction.Out);

            float newY = Mathf.Lerp(start, end, val);

            var pos = transform.localPosition;
            transform.localPosition = new Vector3(pos.x, newY, pos.z);

            yield return 0;
        }

        if (start > end) GoingDown = false;
        else GoingUp = false;
    }

}
