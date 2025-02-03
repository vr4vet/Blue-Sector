using UnityEngine;
using UnityEngine.Events;

public class RevolvingNosePieceTrigger : MonoBehaviour
{
    [SerializeField] private RevolvingNosePiece RevolvingNosePiece;
    [SerializeField] private Direction direction;
    public UnityEvent m_OnMagnificationAdjusted;
    private enum Direction
    {
        Right, Left
    }

    private void Start()
    {
        if (m_OnMagnificationAdjusted == null)
            m_OnMagnificationAdjusted = new UnityEvent();
    }

    private void OnTriggerEnter(Collider other)
    {
        // check if player hand
        if (other.name == "Grabber" || other.name.Equals("hands_coll:b_r_index3") || other.name.Equals("hands_coll:b_r_index3 (1)"))
        {
            RevolvingNosePiece.RotateNosePiece(direction == Direction.Right);
            m_OnMagnificationAdjusted.Invoke();
        }
    }
}
