using UnityEngine;

namespace UntitledBallGame
{
    public class LevelBounds : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D box;
        [Header("Borders")] [SerializeField] private BoxCollider2D left;
        [SerializeField] private BoxCollider2D right;
        [SerializeField] private BoxCollider2D top;
        [SerializeField] private BoxCollider2D bottom;

        public Bounds Box => box.bounds;
        public Bounds Left => left.bounds;
        public Bounds Right => right.bounds;
        public Bounds Top => top.bounds;
        public Bounds Bottom => bottom.bounds;

        public bool ContainsObject(Vector2 position)
        {
            return box.bounds.Contains(position);
        }

        public Vector3 GetObjectInBounds(Bounds objBounds)
        {
            float rightBound = box.bounds.center.x + box.bounds.extents.x;
            float leftBound = box.bounds.center.x - box.bounds.extents.x;
            float topBound = box.bounds.center.y + box.bounds.extents.y;
            float bottomBound = box.bounds.center.y - box.bounds.extents.y;

            float distanceToRightBound = rightBound - (objBounds.center.x + objBounds.extents.x);
            float distanceToLeftBound = -leftBound + (objBounds.center.x - objBounds.extents.x);
            float distanceToTopBound = topBound - (objBounds.center.y + objBounds.extents.y);
            float distanceToBottomBound = -bottomBound + (objBounds.center.y - objBounds.extents.y);

            Vector3 output = objBounds.center;

            if (distanceToRightBound < 0)
                output.x = rightBound - objBounds.extents.x;
            if (distanceToLeftBound < 0)
                output.x = leftBound + objBounds.extents.x;
            if (distanceToTopBound < 0)
                output.y = topBound - objBounds.extents.y;
            if (distanceToBottomBound < 0)
                output.y = bottomBound + objBounds.extents.y;

            return output;
        }
    }
}