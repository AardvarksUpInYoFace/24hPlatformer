using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using JoniPlatformer;
using UnityEngine.SceneManagement;

public class OpenerController : MonoBehaviour
{
    public Text Title, WText, AText, DText;
    public PlayerMovement PlayerMovement;
    public bool Resetting;

    private int KeyboardCheck = 0;

    private bool StartingGame, CanKeyboardCheck; 

    private void Start()
    {
        Cursor.visible = false;
        Screen.SetResolution(800, 800, false);
        PlayerMovement.CanMove = false;

        StartCoroutine(CoMoveYText(0, 2f, 220f, 240f, Title));
        StartCoroutine(CoFadeTextAlpha(0, 2f, 0f, 1f, Title));

        StartCoroutine(DelayedShowW(1.5f));
    }

    private void Update()
    {
        //remove keyboard icons check

        if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();


        if(PlayerMovement.CanMove && Input.GetKeyDown(KeyCode.R) && !Resetting)
        {
            Resetting = true;
            ResetGame();
        }

        if (!CanKeyboardCheck) return;

        if (Input.GetKeyDown(KeyCode.W) && WText != null)
        {
            Destroy(WText.gameObject);
            KeyboardCheck++;
        }
        if (Input.GetKeyDown(KeyCode.A) && AText != null)
        {
            Destroy(AText.gameObject);
            KeyboardCheck++;
        }
        if (Input.GetKeyDown(KeyCode.D) && DText != null)
        {
            Destroy(DText.gameObject);
            KeyboardCheck++;
        }

        if(!StartingGame && KeyboardCheck > 2)
        {
            StartingGame = true;
            StartCoroutine(CoFadeTextAlpha(0, 0.77f, 1f, 0f, Title));
            StartCoroutine(CoFadeImageAlpha(0, 0.77f, 1f, 0f, GetComponent<Image>()));
            StartCoroutine(DelayedStartGame(0.77f));
        }
    }

    public void EndGame()
    {
        Resetting = true;
        PlayerMovement.CanMove = false;
        StartCoroutine(CoFadeImageAlpha(0, 2.2f, 0, 1f, GetComponent<Image>()));

        Title.text = "End.";

        StartCoroutine(DelayedCoFadeTextAlpha(2.7f, 0, 2f, 0f, 1f, Title));

        StartCoroutine(DelayedCoFadeTextAlpha(5.5f, 0f, 2f, 1f, 0f, Title));
        StartCoroutine(DelayedReloadGame(8f));
    }

    private void ResetGame()
    {
        PlayerMovement.CanMove = false;
        StartCoroutine(CoFadeImageAlpha(0, 0.5f, 0, 1f, GetComponent<Image>()));
        StartCoroutine(DelayedReloadGame(0.5f));
    }

    #region Coroutines

    private IEnumerator CoMoveYText(float iterator, float time, float start, float end, Text text)
    {
        while (iterator < time)
        {
            iterator += Time.deltaTime;
            if (iterator > time) iterator = time;

            float val = JoniUtility.Easing.GetTerpedPosition(iterator, time, JoniUtility.Easing.Function.Sinusoidal, JoniUtility.Easing.Direction.Out);

            float newY = Mathf.Lerp(start, end, val);

            var pos = text.transform.localPosition;
            text.transform.localPosition = new Vector3(pos.x, newY, pos.z);

            yield return 0;
        }
    }
    private IEnumerator CoFadeTextAlpha(float iterator, float time, float start, float end, Text text)
    {
        while (iterator < time)
        {
            iterator += Time.deltaTime;
            if (iterator > time) iterator = time;

            float val = JoniUtility.Easing.GetTerpedPosition(iterator, time, JoniUtility.Easing.Function.Sinusoidal, JoniUtility.Easing.Direction.Out);

            float newA = Mathf.Lerp(start, end, val);

            var col = text.color;
            text.color = new Color(col.r,col.g,col.b, newA);

            yield return 0;
        }
    }
    private IEnumerator CoFadeImageAlpha(float iterator, float time, float start, float end, Image image)
    {
        while (iterator < time)
        {
            iterator += Time.deltaTime;
            if (iterator > time) iterator = time;

            float val = JoniUtility.Easing.GetTerpedPosition(iterator, time, JoniUtility.Easing.Function.Sinusoidal, JoniUtility.Easing.Direction.Out);

            float newA = Mathf.Lerp(start, end, val);

            var col = image.color;
            image.color = new Color(col.r, col.g, col.b, newA);

            yield return 0;
        }
    }
    private IEnumerator DelayedShowW(float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(CoMoveYText(0, 0.5f, -330f, -310f, WText));
        StartCoroutine(CoFadeTextAlpha(0, 0.5f, 0f, 1f, WText));
        StartCoroutine(DelayedShowAD(0.3f));
    }
    private IEnumerator DelayedShowAD(float time)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(CoMoveYText(0, 0.5f, -430f, -400f, AText));
        StartCoroutine(CoMoveYText(0, 0.5f, -430f, -400f, DText));
        StartCoroutine(CoFadeTextAlpha(0, 0.5f, 0f, 1f, AText));
        StartCoroutine(CoFadeTextAlpha(0, 0.5f, 0f, 1f, DText));

        StartCoroutine(DelayedCanCheck(0.5f));
    }
    private IEnumerator DelayedCanCheck(float time)
    {
        yield return new WaitForSeconds(time);
        CanKeyboardCheck = true;
    }
    private IEnumerator DelayedStartGame(float time)
    {
        yield return new WaitForSeconds(time);
        PlayerMovement.CanMove = true;
    }
    private IEnumerator DelayedReloadGame(float time)
    {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene(0);
    }
    private IEnumerator DelayedCoFadeTextAlpha(float time, float iterator, float time1, float start, float end, Text text)
    {
        yield return new WaitForSeconds(time);
        StartCoroutine(CoFadeTextAlpha(iterator, time1, start, end, text));
    }
    #endregion
}
