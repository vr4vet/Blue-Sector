using UnityEngine;

public class NPCPathPoint : MonoBehaviour
{
    [SerializeField] private TargetType type = TargetType.Walk;
    [SerializeField] private DialogueTree nextConversationTree;
    public enum TargetType
    {
        Walk, Rotate
    }

    public TargetType GetTargetType()
    {
        return type;
    }

    public string GetNextConversationTree()
    {
        if (nextConversationTree != null)
            return nextConversationTree.name;
        else
            return string.Empty;
    }
}
