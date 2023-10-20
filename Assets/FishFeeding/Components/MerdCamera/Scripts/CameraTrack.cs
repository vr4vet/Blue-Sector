using UnityEditor;
using UnityEngine;

public class CameraTrack : MonoBehaviour
{
#if UNITY_EDITOR
    [CustomEditor(typeof(CameraTrack))]
    public class CameraTrackVisualization : Editor
    {
        public void OnSceneGUI()
        {
            // getting parameters for drawing visualization
            // they might (probably) not be correct, as MerdCameraController has some funky math going on
            var t = target as CameraTrack;
            var position = t.transform.position;
            var height = t.transform.localScale.y * 0.7f; 
            var width = t.transform.localScale.x;
            var length = t.transform.localScale.z;

            Handles.color = Color.green;
            Handles.DrawLine(position + new Vector3(-width, height, length), position + new Vector3(width, height, length));
            Handles.DrawLine(position + new Vector3(-width, height, -length), position + new Vector3(width, height, -length));
            Handles.DrawLine(position + new Vector3(-width, -height, length), position + new Vector3(width, -height, length));
            Handles.DrawLine(position + new Vector3(-width, -height, -length), position + new Vector3(width, -height, -length));

            Handles.DrawLine(position + new Vector3(-width, height, -length), position + new Vector3(-width, -height, -length));
            Handles.DrawLine(position + new Vector3(width, height, -length), position + new Vector3(width, -height, -length));
            Handles.DrawLine(position + new Vector3(-width, height, length), position + new Vector3(-width, -height, length));
            Handles.DrawLine(position + new Vector3(width, height, length), position + new Vector3(width, -height, length));

            Handles.DrawLine(position + new Vector3(-width, height, -length), position + new Vector3(-width, height, length));
            Handles.DrawLine(position + new Vector3(width, height, -length), position + new Vector3(width, height, length));
            Handles.DrawLine(position + new Vector3(-width, -height, -length), position + new Vector3(-width, -height, length));
            Handles.DrawLine(position + new Vector3(width, -height, -length), position + new Vector3(width, -height, length));
        }

    }
#endif
}
