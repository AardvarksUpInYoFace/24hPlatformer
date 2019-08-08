using UnityEngine;

namespace JoniPlatformer
{
    [RequireComponent(typeof(Collider2D))]
    public class FootCollideController : MonoBehaviour
    {
        private PlayerMovement PlayerMovement;

        private void Start()
        {
            PlayerMovement = GetComponentInParent<PlayerMovement>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.gameObject.CompareTag("Wall")) return;

            PlayerMovement.FloorWalls.Add(collision.gameObject);
            PlayerMovement.OnLanded();
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (PlayerMovement.FloorWalls.Contains(collision.gameObject))
                PlayerMovement.FloorWalls.Remove(collision.gameObject);

            if (PlayerMovement.FloorWalls.Count < 1) PlayerMovement.OffGround();
        }
    }

}
