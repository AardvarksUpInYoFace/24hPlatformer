using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JoniPlatformer
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        #region Public Variables

        public InputManager InputManager;
        public GameObject PFX;
        public float Speed = 10f, JumpForce = 12f, WallJumpLerp = 10f, SlideSpeed = 2;
        public bool OnGround, WallGrabLeft, WallGrabRight, WallJumped, CanMove = true, HasJumped;
        public List<GameObject> FloorWalls = new List<GameObject>();

        public bool OffingGround, OffingWallLeft, OffingWallRight;
        #endregion

        #region Private Variables

        private Rigidbody2D Rb;
        

        #endregion

        void Start()
        {
            Rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            ProcessInput();
        }

        #region Movement

        private void ProcessInput()
        {
            if (!CanMove) return;

            Vector2 MovVector = InputManager.GetInput();

            //handle horziontal movement
            Move(MovVector);

            //handle vertical movement
            //if we're on the ground, or on a wall, allow for a jump
            if (!InputManager.HasJumped()) return;
            //if on ground, and not near wall
            if (OnGround || OffingGround)
            {
                Jump(Vector2.up);
                return;
            }
            //else on either of the walls (can never be on both?)
            else if (WallGrabLeft || OffingWallLeft) WallJump(1f);
            else if (WallGrabRight || OffingWallRight) WallJump(-1f);
        }

        private void Move(Vector2 dir)
        {
            bool wallSlide = false;

            //prevent from holding onto walls, slip instead
            if (Rb.velocity.y < 0 && dir.y >= 0 && !OnGround && ((WallGrabLeft && dir.x <= 0) ||
                (WallGrabRight && dir.x >= 0)))
            {
                wallSlide = true;
                dir.x = 0;
                Rb.velocity = new Vector2(Rb.velocity.x, 0);
            }

            float ySpeed = wallSlide ? -SlideSpeed : Rb.velocity.y;

            if (!WallJumped) Rb.velocity = new Vector2(dir.x * Speed, ySpeed);
            else Rb.velocity = Vector2.Lerp(Rb.velocity, (new Vector2(dir.x * Speed, ySpeed)), WallJumpLerp * Time.deltaTime);
        }

        private IEnumerator DelayedEnableMovement(float time)
        {       
            yield return new WaitForSeconds(time);
            CanMove = true;
        }

        #region Jumping

        private void Jump(Vector2 dir)
        {
            OffingGround = false;
            HasJumped = true;
            var TotalJumpForce = JumpForce;

            Rb.velocity = new Vector2(Rb.velocity.x, 0);
            Rb.velocity += dir * TotalJumpForce;

            StopAllCoroutines();
            StartCoroutine(CoScaleJump(0, 0.1f, transform.localScale.y, 1.25f, transform.localScale.x, 1f));

            SpawnParticles();
        }

        private void WallJump(float JumpDir)
        {
            Jump(Vector2.up + new Vector2(JumpDir, 0) / 2f);
            WallJumped = true;
            CanMove = false;
            OffingWallLeft = false;
            OffingWallRight = false;

            StartCoroutine(DelayedEnableMovement(0.1f));

        }

        public void OffGround()
        {
            OnGround = false;
            OffingGround = true;
            StartCoroutine(DelayedOffGround(0.1f));
        }

        public void OffWall(bool Left)
        {
            if (HasJumped) return;

            if(Left) OffingWallLeft = true;
            else OffingWallRight = true;

            StartCoroutine(DelayedOffWall(0.15f));
        }

        private IEnumerator DelayedOffGround(float time)
        {
            yield return new WaitForSeconds(time);
            OffingGround = false;
        }

        private IEnumerator DelayedOffWall(float time)
        {
            yield return new WaitForSeconds(time);
            OffingWallLeft = false;
            OffingWallRight = false;
        }

        public void OnLanded()
        {
            OnGround = true;
            WallJumped = false;
            HasJumped = false;
            StopAllCoroutines();
            StartCoroutine(CoScaleJump(0, 0.1f, transform.localScale.y, 1f, transform.localScale.x, 1f));
        }

        private void SpawnParticles()
        {
            var pfx = Instantiate(PFX);
            pfx.transform.position = transform.GetChild(0).position;
        }


        #endregion

        #region Animation

        private IEnumerator CoScaleJump(float iterator, float time, float startY, float endY, float startX, float endX)
        {
            while (iterator < time)
            {
                iterator += Time.deltaTime;
                if (iterator > time) iterator = time;

                float val = JoniUtility.Easing.GetTerpedPosition(iterator, time, JoniUtility.Easing.Function.Sinusoidal, JoniUtility.Easing.Direction.Out);

                float newY = Mathf.Lerp(startY, endY, val);
                float newX = Mathf.Lerp(startX, endX, val);

                var scale = transform.localScale;
                transform.localScale = new Vector3(newX, newY, scale.z);

                yield return 0;
            }
        }

        #endregion
        #endregion
    }
}

