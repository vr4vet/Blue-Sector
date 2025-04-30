using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpdatedTabletPanelManager : MonoBehaviour
{
    [SerializeReference] UpdatedTabletTaskListLoader TaskList;
    //references
    [Header("Menu references")]
    [SerializeReference] GameObject TaskListMenu;
    [SerializeReference] GameObject TaskAboutMenu;
    [SerializeReference] GameObject SubtaskAboutMenu;
    [SerializeReference] GameObject SkillListMenu;
    [SerializeReference] GameObject NotificationAlertMenu;
    [SerializeReference] GameObject TaskSummaryPanel;

    [SerializeField] private TMP_Text summaryText;

    private AddInstructionsToWatch watch;
    private MaintenanceManager manager;
    private WatchManager watchManager;
    private List<GameObject> allMenus = new();

    private bool taskPageOpen = false;
    private bool subtaskPageOpen = false;

    void Start()
    {
        allMenus.AddRange(new List<GameObject>() { TaskListMenu, TaskAboutMenu, SubtaskAboutMenu, SkillListMenu, TaskSummaryPanel });
        watchManager = WatchManager.Instance;

        foreach (var item in allMenus)
        {
            item.SetActive(false);
        }
        SelectSubtask();


        if (FindObjectsOfType<MaintenanceManager>().Length > 0)
        {
            manager = GameObject.FindObjectsOfType<MaintenanceManager>()[0];
            manager.CurrentSubtask.AddListener(OnCurrentSubtaskChanged);
            manager.SkillCompleted.AddListener(OnSkillCompleted);
        }
        else
        {
            watchManager.CurrentSubtask.AddListener(OnCurrentSubtaskChanged);
            watchManager.SkillCompleted.AddListener(OnSkillCompleted);
        }
        if (FindObjectsOfType<AddInstructionsToWatch>().Length > 0)
        {
            watch = GameObject.FindObjectsOfType<AddInstructionsToWatch>()[0];
            watch.IncomingMessage.AddListener(SetAlertMenu);
        }
        AddHoverSupport();
    }

    private void OnCurrentSubtaskChanged(Task.Subtask subtask)
    {
        SwitchMenuTo(SubtaskAboutMenu);
        watchManager.UIChanged.Invoke();
    }

    private void OnSkillCompleted(Task.Skill skill)
    {
        SwitchMenuTo(SkillListMenu);
        watchManager.UIChanged.Invoke();
    }

    public void OnClickOpenTasks()
    {
        if (subtaskPageOpen)
        {
            OnClickOpenSubtask();
        }

        if (!taskPageOpen)
        {
            TaskListMenu.SetActive(true);
            taskPageOpen = true;
            TaskList.LoadTaskPage();
            watchManager.UIChanged.Invoke();
            return;
        }
        else if (taskPageOpen)
        {
            TaskListMenu.SetActive(false);
            taskPageOpen = false;
            watchManager.UIChanged.Invoke();
            return;
        }
    }

    public void OnClickOpenSubtask()
    {
        if (taskPageOpen)
        {
            OnClickOpenTasks();
            watchManager.UIChanged.Invoke();
        }
        if (!subtaskPageOpen)
        {
            TaskAboutMenu.SetActive(true);
            subtaskPageOpen = true;
            TaskList.SubtaskPageLoader(TaskList.activeTask);
            watchManager.UIChanged.Invoke();
            return;
        }
        else if (subtaskPageOpen)
        {
            TaskAboutMenu.SetActive(false);
            subtaskPageOpen = false;
            watchManager.UIChanged.Invoke();
            return;
        }
    }

    public void OnClickBackToAboutTask()
    {
        SwitchMenuTo(TaskAboutMenu);
        watchManager.UIChanged.Invoke();
    }
    public void OnClickBackToSkillMenu()
    {
        SwitchMenuTo(SkillListMenu);
        watchManager.UIChanged.Invoke();
    }

    void SwitchMenuTo(GameObject b)
    {
        foreach (var item in allMenus)
        {
            item.SetActive(false);
        }
        b.SetActive(true);
    }

    public void OnClickMenuTask()
    {
        SwitchMenuTo(TaskListMenu);
        watchManager.UIChanged.Invoke();
    }
    public void OnClickMenuSkills()
    {
        if (taskPageOpen)
        {
            OnClickOpenTasks();
        }
        if (subtaskPageOpen)
        {
            OnClickOpenSubtask();
        }
        SwitchMenuTo(SkillListMenu);
        watchManager.UIChanged.Invoke();
    }

    public void OnClickShowTaskSummary()
    {
        SwitchMenuTo(TaskSummaryPanel);
        watchManager.UIChanged.Invoke();

        ActionManager actionManager = ActionManager.Instance;
        if (actionManager != null)
        {
            actionManager.SendMessage("TaskSummary", SendMessageOptions.DontRequireReceiver);

            if (summaryText != null)
            {
                summaryText.text = actionManager.LatestSummary;


                StartCoroutine(ResizeScrollViewContent());
            }
        }
    }
    private IEnumerator ResizeScrollViewContent()
    {
        // Wait for the end of the frame to ensure text has been updated and measured
        yield return new WaitForEndOfFrame();

        // Get the Content RectTransform (parent of summaryText)
        RectTransform contentRectTransform = summaryText.transform.parent.GetComponent<RectTransform>();

        // Get the preferred height of the text after it's been updated
        float preferredHeight = summaryText.preferredHeight;

        // Add some padding
        preferredHeight += 50f;

        // Set the height of the content area - make sure it's larger than the viewport
        Vector2 sizeDelta = contentRectTransform.sizeDelta;
        sizeDelta.y = Mathf.Max(preferredHeight, 200f); // Ensure minimum height
        contentRectTransform.sizeDelta = sizeDelta;

        // Make sure the ScrollRect knows the content size has changed
        ScrollRect scrollRect = contentRectTransform.GetComponentInParent<ScrollRect>();
        if (scrollRect != null)
        {
            Canvas.ForceUpdateCanvases();
            scrollRect.normalizedPosition = new Vector2(0, 1); // Scroll to top

            // Ensure scroll sensitivity is high enough for finger interactions
            scrollRect.scrollSensitivity = 10f;

            // Make sure ScrollRect is set to vertical
            scrollRect.vertical = true;
        }
    }

    public void AddHoverSupport()
    {
        // Find the ScrollRect component in the TaskSummaryPanel
        ScrollRect scrollRect = TaskSummaryPanel.GetComponentInChildren<ScrollRect>();
        if (scrollRect != null)
        {
            // Make sure the scroll view is interactable
            var eventTrigger = scrollRect.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();

            // Add entry for pointer down events
            var pointerDownEntry = new UnityEngine.EventSystems.EventTrigger.Entry();
            pointerDownEntry.eventID = UnityEngine.EventSystems.EventTriggerType.PointerDown;
            pointerDownEntry.callback.AddListener((data) => { /* Debug here */ Debug.Log("Pointer Down on ScrollRect"); });
            eventTrigger.triggers.Add(pointerDownEntry);

            // Add entry for pointer drag events
            var dragEntry = new UnityEngine.EventSystems.EventTrigger.Entry();
            dragEntry.eventID = UnityEngine.EventSystems.EventTriggerType.Drag;
            dragEntry.callback.AddListener((data) => { /* Debug here */ Debug.Log("Dragging on ScrollRect"); });
            eventTrigger.triggers.Add(dragEntry);

            // Set the scroll sensitivity higher for VR
            scrollRect.scrollSensitivity = 25f;
            scrollRect.inertia = true;
            scrollRect.decelerationRate = 0.1f; // More responsive deceleration

            // Make sure the content is larger than the viewport
            RectTransform contentRect = scrollRect.content;
            RectTransform viewportRect = scrollRect.viewport;
            if (contentRect && viewportRect)
            {
                // Ensure content is taller than viewport for vertical scrolling
                if (contentRect.rect.height <= viewportRect.rect.height)
                {
                    contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, viewportRect.rect.height * 1.5f);
                }
            }

            // Ensure XR Interaction Toolkit components
            Canvas canvas = TaskSummaryPanel.GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                if (!canvas.gameObject.GetComponent<UnityEngine.XR.Interaction.Toolkit.UI.TrackedDeviceGraphicRaycaster>())
                {
                    canvas.gameObject.AddComponent<UnityEngine.XR.Interaction.Toolkit.UI.TrackedDeviceGraphicRaycaster>();
                }
            }
        }
    }

    public void OnClickBackFromTaskSummary()
    {
        OnClickMenuTask();
    }
    public void SetAlertMenu()
    {
        if (gameObject.activeInHierarchy)
        {
            StopAllCoroutines();
            NotificationAlertMenu.SetActive(true);
            StartCoroutine(sendAlertMenu());
        }
        else
        {
            NotificationAlertMenu.SetActive(false);
        }
    }

    IEnumerator sendAlertMenu()
    {
        yield return new WaitForSeconds(3f);
        NotificationAlertMenu.SetActive(false);
    }

    void OnDisable()
    {
        NotificationAlertMenu.SetActive(false);
    }

    public void SelectSubtask()
    {
        SwitchMenuTo(SubtaskAboutMenu);
        watchManager.UIChanged.Invoke();
    }
}
