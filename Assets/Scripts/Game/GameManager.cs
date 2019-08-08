using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoniPlatformer;

[System.Serializable]
public class RoomArray
{
    public GameObject[] Rooms;
}

public class GameManager : MonoBehaviour
{
    #region Public Variables

    public Camera Camera;
    public PlayerMovement PlayerMovement;
    public float ShakeAmount = 0.2f;
    public RoomArray[] RoomsArray;
    public bool RotatingRooms { get; private set; }

    #endregion

    #region Private Variables

    private AudioSource myAudio;
    private bool Shrinking;
    private Vector2 CurrentRoom;
    private bool CameraShaking;
    private float RotateRoomTime = 1.7f;
    private int MaxViewSize = 45;

    #endregion

    private void Start()
    {
        CurrentRoom = new Vector2(-1, -1);
        myAudio = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (RotatingRooms) return;

        ZoomCheck();
        CheckPlayerPos();
    }


    #region Rotating Rooms

    private void RotateRow(int row, bool ToRight)
    {
        List<GameObject> TempRooms = new List<GameObject>();

        foreach(GameObject room in RoomsArray[row].Rooms)
        {
            TempRooms.Add(room);
        }

        var ii = 0;

        for(int i = 0; i < RoomsArray[row].Rooms.Length; i++)
        {
            bool duplicateRoom = false;

            if (ToRight || i != 0) ii += 1;
            else ii -= 1;

            //if we overflow on either side of the array then we know we're at the room that needs to switch side of the screen
            if (ii >= RoomsArray[row].Rooms.Length)
            {
                ii = 0;
                if(ToRight) duplicateRoom = true;
            }

            if (ii < 0)
            {
                ii = RoomsArray[row].Rooms.Length - 1;
                duplicateRoom = true;
            }


            if (duplicateRoom)
            {
                var newRoom = Instantiate(TempRooms[i], TempRooms[i].transform.parent);

                if (ToRight) newRoom.transform.localPosition -= new Vector3((TempRooms.Count) *30f, 0, 0);
                else newRoom.transform.localPosition += new Vector3((TempRooms.Count) * 30f, 0, 0);

                var oldRoom = TempRooms[i];
                TempRooms[i] = newRoom;

                var posOld = oldRoom.transform.localPosition;
                StartCoroutine(CoRoomMove(0, RotateRoomTime, posOld, posOld + new Vector3(ToRight ? 30f : -30f, 0, 0), oldRoom));
                StartCoroutine(DelayedDestroy(RotateRoomTime, oldRoom));
            }

            RoomsArray[row].Rooms[ii] = TempRooms[i];
            var pos = TempRooms[i].transform.localPosition;
            StartCoroutine(CoRoomMove(0, RotateRoomTime, pos, pos + new Vector3(ToRight ? 30f : -30f, 0, 0), TempRooms[i]));
        }
    }

    private void RotateColumn(int column, bool ToUp)
    {
        List<GameObject> TempRooms = new List<GameObject>();

        foreach (RoomArray RoomArray in RoomsArray)
        {
            TempRooms.Add(RoomArray.Rooms[column]);
        }

        var ii = 0;

        for (int i = 0; i < RoomsArray.Length; i++)
        {
            bool duplicateRoom = false;

            if (!ToUp || i != 0) ii += 1;
            else ii -= 1;

            //if we overflow on either side of the array then we know we're at the room that needs to switch side of the screen
            if (ii >= RoomsArray.Length)
            {
                ii = 0;
                if (!ToUp) duplicateRoom = true;
            }
            if (ii < 0)
            {
                ii = RoomsArray.Length - 1;
                duplicateRoom = true;
            }

            if (duplicateRoom)
            {
                var newRoom = Instantiate(TempRooms[i], TempRooms[i].transform.parent);

                if (!ToUp) newRoom.transform.localPosition += new Vector3(0, (TempRooms.Count) * 30f, 0);
                else newRoom.transform.localPosition -= new Vector3(0, (TempRooms.Count) * 30f, 0);

                var oldRoom = TempRooms[i];
                TempRooms[i] = newRoom;

                var posOld = oldRoom.transform.localPosition;
                StartCoroutine(CoRoomMove(0, RotateRoomTime, posOld, posOld + new Vector3(0, ToUp ? 30f : -30f, 0), oldRoom));
                StartCoroutine(DelayedDestroy(RotateRoomTime, oldRoom));
            }

            RoomsArray[ii].Rooms[column] = TempRooms[i];
            var pos = TempRooms[i].transform.localPosition;
            StartCoroutine(CoRoomMove(0, RotateRoomTime, pos, pos + new Vector3(0, ToUp ? 30f : -30f, 0), TempRooms[i]));
        }
    }

    public void RotateRooms(bool Row, int index, bool RightUp)
    {
        if (RotatingRooms) return;
        //screen shake//
        //prevent movement.//
        //trigger a forced zoom out//
        //trigger the rooms rotating.

        PlayerMovement.CanMove = false;
        PlayerMovement.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        RotatingRooms = true;

        StopAllCoroutines();
        StartCoroutine(CoEnlarge(0, 0.5f, Camera.orthographicSize, MaxViewSize, Camera.transform.localPosition, true));
        StartCoroutine(DelayedCameraShake(0.7f));
        StartCoroutine(DelayedRotateRooms(0.8f,Row, index, RightUp));
        StartCoroutine(DelayedEndCameraShake(RotateRoomTime + 1f));
        StartCoroutine(DelayedEndRotateRooms(RotateRoomTime + 1.7f));
    }

    private IEnumerator CoRoomMove(float iterator, float time, Vector3 startPos, Vector3 endPos, GameObject room)
    {
        while (iterator < time)
        {
            iterator += Time.deltaTime;
            if (iterator > time) iterator = time;
            float val = JoniUtility.Easing.GetTerpedPosition(iterator, time, JoniUtility.Easing.Function.Sinusoidal, JoniUtility.Easing.Direction.InOut);
            float newX = JoniUtility.ExtendedLerp.LerpWithoutClamp(startPos.x, endPos.x, val);
            float newY = JoniUtility.ExtendedLerp.LerpWithoutClamp(startPos.y, endPos.y, val);

            var pos = room.transform.localPosition;
            room.transform.localPosition = new Vector3(newX, newY, pos.z);

            yield return 0;
        }
    }

    private IEnumerator DelayedDestroy(float time, GameObject gameObject)
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }

    private IEnumerator DelayedRotateRooms(float time, bool Row, int index, bool RightUp)
    {
        yield return new WaitForSeconds(time);
        if (Row) RotateRow(index, RightUp);
        else RotateColumn(index, RightUp);

        myAudio.Play();
    }

    private IEnumerator DelayedEndRotateRooms(float time)
    {
        yield return new WaitForSeconds(time);

        StartCoroutine(CoShrink(0, 0.5f, Camera.orthographicSize, 15f, Camera.transform.localPosition));
        StartCoroutine(DelayedCanMovePlayer(0.5f));
    }

    private IEnumerator DelayedCanMovePlayer(float time)
    {
        yield return new WaitForSeconds(time);
        PlayerMovement.CanMove = true;
        RotatingRooms = false;
    }

    #endregion

    #region Camera Movement

    private void ZoomCheck()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            PlayerMovement.CanMove = false;
            PlayerMovement.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            Shrinking = false;
            StopAllCoroutines();
            StartCoroutine(CoEnlarge(0, 0.7f, Camera.orthographicSize, MaxViewSize, Camera.transform.localPosition));
        }

        else if (!Input.GetKey(KeyCode.Q) && !Shrinking && Camera.orthographicSize > 15)
        {
            PlayerMovement.CanMove = true;
            Shrinking = true;
            StartCoroutine(CoShrink(0, 0.7f, Camera.orthographicSize, 15, Camera.transform.localPosition));
        }
    }
    private void CheckPlayerPos()
    {
        bool hasChanged = false;

        if (PlayerMovement.transform.localPosition.x > 15 + CurrentRoom.x * 30)
        {
            CurrentRoom.x++;
            hasChanged = true;
        }
        if (PlayerMovement.transform.localPosition.x < -15 + CurrentRoom.x * 30)
        {
            CurrentRoom.x--;
            hasChanged = true;
        }
        if(PlayerMovement.transform.localPosition.y > 15 + CurrentRoom.y * 30)
        {
            CurrentRoom.y++;
            hasChanged = true;
        }
        if (PlayerMovement.transform.localPosition.y < -15 + CurrentRoom.y * 30)
        {
            CurrentRoom.y--;
            hasChanged = true;
        }


        if (!hasChanged) return;

        StopAllCoroutines();
        ResetCameraView();
    }
    private void ResetCameraView()
    {
        StartCoroutine(CoCameraMove(0, 0.5f, Camera.transform.localPosition, Camera.orthographicSize));
    }

    #region Coroutines
    private IEnumerator CoShrink(float iterator, float time, float start, float end, Vector3 startPos)
    {
        while (iterator < time)
        {
            iterator += Time.deltaTime;
            if (iterator > time) iterator = time;

            float val = JoniUtility.Easing.GetTerpedPosition(iterator, time, JoniUtility.Easing.Function.Sinusoidal, JoniUtility.Easing.Direction.Out);

            float newS = Mathf.Lerp(start, end, val);
            Camera.orthographicSize = newS;

            float newX = Mathf.Lerp(startPos.x, CurrentRoom.x * 30f, val);
            float newY = Mathf.Lerp(startPos.y, CurrentRoom.y * 30f, val);
            Camera.transform.localPosition = new Vector3(newX, newY, Camera.transform.localPosition.z);

            yield return 0;
        }
    }
    private IEnumerator CoEnlarge(float iterator, float time, float start, float end, Vector3 startPos, bool forced = false)
    {
        bool canContinue = true;

        while (iterator < time && canContinue)
        {
            if (!Input.GetKey(KeyCode.Q) && !forced) canContinue = false;

            iterator += Time.deltaTime;
            if (iterator > time) iterator = time;

            float val = JoniUtility.Easing.GetTerpedPosition(iterator, time, JoniUtility.Easing.Function.Sinusoidal, JoniUtility.Easing.Direction.Out);

            float newS = Mathf.Lerp(start, end, val);
            Camera.orthographicSize = newS;

            float newX = Mathf.Lerp(startPos.x, 0, val);
            float newY = Mathf.Lerp(startPos.y, 0, val);
            Camera.transform.localPosition = new Vector3(newX, newY, Camera.transform.localPosition.z);

            yield return 0;
        }
    }
    private IEnumerator CoCameraMove(float iterator, float time, Vector3 pos, float currentSize)
    {
        while (iterator < time)
        {
            iterator += Time.deltaTime;
            if (iterator > time) iterator = time;
            float val = JoniUtility.Easing.GetTerpedPosition(iterator, time, JoniUtility.Easing.Function.Sinusoidal, JoniUtility.Easing.Direction.Out);
            float newX = Mathf.Lerp(pos.x, CurrentRoom.x * 30f, val);
            float newY = Mathf.Lerp(pos.y, CurrentRoom.y * 30f, val);
            Camera.transform.localPosition = new Vector3(newX, newY, Camera.transform.localPosition.z);

            float newS = Mathf.Lerp(currentSize, 15, val);
            Camera.orthographicSize = newS;

            yield return 0;
        }
    }
    private IEnumerator CoCameraShake()
    {
        while(CameraShaking)
        {
            var rangeX = Random.Range(-ShakeAmount, ShakeAmount);
            var rangeY = Random.Range(-ShakeAmount, ShakeAmount);

            Camera.transform.localPosition = new Vector3(rangeX, rangeY, Camera.transform.localPosition.z);

            yield return 0;
        }
    }
    private IEnumerator DelayedCameraShake(float time)
    {
        yield return new WaitForSeconds(time);

        CameraShaking = true;
        StartCoroutine(CoCameraShake());
    }
    private IEnumerator DelayedEndCameraShake(float time)
    {
        yield return new WaitForSeconds(time);  
        CameraShaking = false;
    }
    #endregion

    #endregion
}
