using UnityEngine;

public class RevolvingNosePieceTrigger : MonoBehaviour
{
    [SerializeField] private RevolvingNosePiece RevolvingNosePiece;
    [SerializeField] private Direction direction;
    private enum Direction
    {
        Right, Left
    }

    private void OnTriggerEnter(Collider other)
    {
        // check if player hand
        if (other.name == "Grabber" || other.name == "tip_collider_i")
            RevolvingNosePiece.RotateNosePiece(direction == Direction.Right);
    }
}
