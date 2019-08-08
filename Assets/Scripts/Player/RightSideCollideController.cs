using UnityEngine;

namespace JoniPlatformer
{
    [RequireComponent(typeof(Collider2D))]
    public class RightSideCollideController : MonoBehaviour
    {
        private PlayerMovement PlayerMovement;

        private void Start()
        {
            PlayerMovement = GetComponentInParent<PlayerMovement>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                PlayerMovement.WallGrabRight = true;
                PlayerMovement.HasJumped = false;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                PlayerMovement.WallGrabRight = false;
                PlayerMovement.OffWall(false);
            }
        }
    }
}
