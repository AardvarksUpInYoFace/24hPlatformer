using UnityEngine;

namespace JoniPlatformer
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BetterJumping : MonoBehaviour
    {
        private Rigidbody2D rb;
        public InputManager InputManager;
        public float fallMultiplier = 2.5f;
        public float lowJumpMultiplier = 2f;
        public float Gravity = -9.81f;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            if (rb.velocity.y < 0)
            {
                rb.velocity += Vector2.up * Gravity * (fallMultiplier - 1) * Time.deltaTime;
            }
            else if (rb.velocity.y > 0 && !InputManager.IsJumping())
            {
                rb.velocity += Vector2.up * Gravity * (lowJumpMultiplier - 1) * Time.deltaTime;
            }
        }
    }
}
