/*
 * Developer: Jorge Garcia
 * Ask your questions on github: https://github.com/Jorest
 */

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tablet
{
    public class TaskListLoader1 : MonoBehaviour
    {
        private List<Task.Task> _tasks = new List<Task.Task>();
        private List<Task.Skill> _skills = new List<Task.Skill>();

        private StaticPanelManager panelManager;

        [SerializeField] private GameObject maintenanceManager;

        private MaintenanceManager manager => maintenanceManager.GetComponent<MaintenanceManager>();
        private AddInstructionsToWatch watch => maintenanceManager.GetComponent<AddInstructionsToWatch>();
        //main pages

        [Header("Main Page Canvas")]
        public GameObject tasksListCanvas;

        public GameObject subtaskPageCanvas;
        public GameObject TaskPageCanvas;
        public GameObject skillsListPageCanvas;

        //parents objects to load the buttons in
        [Header("Content Spaces")]
        public GameObject taskContent;

        public GameObject TaskSubtaskContent;
        public GameObject skillContent;
        [SerializeField] private GameObject _subtaskContent;
        [SerializeField] private GameObject _skillSubtaskContent;


        [Header("task other")]
        [SerializeField] private TMP_Text _taskNameTab;

        [SerializeField] private TMP_Text _taskFeedback;


        [Header("subtask other")]
        [SerializeField] private TMP_Text _subtaskNameTab;



        [Header("Experience Name")]
        [SerializeField] private string Exp_Name;

        [Header("UI Prefabs")]
        [SerializeField] private GameObject _stepListEntry;
        [SerializeField] private GameObject _subtaskListEntry;
        [SerializeField] private GameObject _taskListEntry;
        [SerializeField] private GameObject _skillBadgesList;
        [SerializeField] private GameObject _horizontalSkill;
        [SerializeField] private GameObject _simpleSkill;

        [Header("Additional Events")]
        [SerializeField] private UnityEvent _skillPageOpen;
        [SerializeField] private UnityEvent _tasksListOpen;
        [SerializeField] private UnityEvent _taskPageOpen;
        [SerializeField] private UnityEvent _subtaskPageOpen;

        private List<GameObject> _skillsClones = new List<GameObject>();

        public static TaskListLoader1 Ins;
        private void Start()
        {
            if (Ins == null)
            {
                Ins = this;
            }
            else
            {
                Destroy(this);
            }

        }
        private void Awake()
        {
            //setting loading the scriptable objects
            Task.TaskHolder th = GameObject.FindObjectsOfType<Task.TaskHolder>()[0];
            panelManager = gameObject.GetComponent<StaticPanelManager>();
            _tasks = th.taskList;
            _skills = th.skillList;
            StartCoroutine(LoadTabletInfo());
            manager.CurrentSubtask.AddListener(HandleCurrentSubtask);

        }

        //since task and skill won't change in the experience we can load them from the beginning
        private IEnumerator LoadTabletInfo()
        {
            yield return new WaitForSeconds(2);
            // TaskPageLoader(_tasks[0]);
            LoadSkillsPage();
        }
        private void HandleCurrentSubtask(Task.Subtask subtask)
        {
            //if the subtask is not null, load the subtask page
            if (subtask != null)
            {
                SubTaskPageLoader(subtask);
            }
        }


        public void LoadSkillBadge(Task.Skill skill, GameObject parent)
        {
            // Initiate a parent for list of badges and skill title
            GameObject skillBadgesContent = Instantiate(_simpleSkill, Vector3.zero, Quaternion.identity);
            // Add the horizontal list to vertical content list
            skillBadgesContent.transform.SetParent(parent.transform);
            skillBadgesContent.transform.localPosition = Vector3.zero;
            skillBadgesContent.transform.localScale = Vector3.one;
            skillBadgesContent.transform.localRotation = Quaternion.Euler(0, 0, 0);


            // Set title to be name of this skill
            TMP_Text skillName = skillBadgesContent.transform.Find("Txt_SkillName").GetComponent<TMP_Text>();
            skillName.text = skill.Name;

            // // Set instructions for unlocking this skill
            // TMP_Text unlockInstructions = skillBadgesContent.transform.Find("Txt_BadgeInfo").GetComponent<TMP_Text>();
            // unlockInstructions.text = skill.Instructions;

            // Find Badge Image and replace with the Icon for this skill
            GameObject badgeItem = skillBadgesContent.transform.Find("BadgeItem").gameObject;
            UnityEngine.UI.Image buttonIcon = badgeItem.transform.Find("icon_badge").GetComponent<UnityEngine.UI.Image>();
            buttonIcon.sprite = skill.Icon;

            // Set icon with shader and padlock if badge is locked, if unlocked set green badge active
            GameObject padlock = badgeItem.transform.Find("padlock").gameObject;
            GameObject completedBackground = badgeItem.transform.Find("CompletedBackground").gameObject;
            padlock.SetActive(skill.IsLocked());
            completedBackground.SetActive(!skill.IsLocked());

        }


        public void LoadSkillsPage()
        {
            if (_skillPageOpen != null) _skillPageOpen.Invoke();
            // Reset existing gameobjects in skills page
            // Transform parentTransform = skillContent.transform;
            // for (int i = 0; i < parentTransform.childCount; i++)
            // {
            //     Destroy(parentTransform.GetChild(i).gameObject);
            // }
            //loads each skill on the parent object
            Task.TaskHolder th = GameObject.FindObjectsOfType<Task.TaskHolder>()[0];

            GameObject horizontalSkill = Instantiate(_horizontalSkill, Vector3.zero, Quaternion.identity);


            for (int i = 0; i < th.skillList.Count; i++)
            {
                if (i % 2 == 0 && i != 0)
                {
                    horizontalSkill = Instantiate(_horizontalSkill, Vector3.zero, Quaternion.identity);
                }

                horizontalSkill.transform.SetParent(skillContent.transform);
                horizontalSkill.transform.localPosition = Vector3.zero;
                horizontalSkill.transform.localScale = Vector3.one;
                horizontalSkill.transform.localRotation = Quaternion.Euler(0, 0, 0);
                LoadSkillBadge(th.skillList[i], horizontalSkill);




            }
        }
        // refreshing after adding the new elements for the Page loader to set the pages correctly
        // skillContent.GetComponent<ContentPageChanger>().Refresh();
        // public void SkillPageLoader(Task.Skill skill)
        // {
        //     if (_skillPageOpen != null) _skillPageOpen.Invoke();



        //     _skillabout.text = skill.Description;

        //     _skillName.text = skill.Name;

        //     //cleaning list before loading the new subtasks
        //     foreach (Transform child in _skillSubtaskContent.transform)
        //     {
        //         GameObject.Destroy(child.gameObject);
        //     }


        //     foreach (Task.Badge badge in skill.ConnectedBadges)
        //     {
        //         GameObject item = Instantiate(_badgeEntry, Vector3.zero, Quaternion.identity);
        //         item.transform.SetParent(_skillSubtaskContent.transform);
        //         item.transform.localPosition = Vector3.zero;
        //         item.transform.localScale = Vector3.one;
        //         item.transform.localRotation = Quaternion.Euler(0, 0, 0);
        //     }



        //     // foreach (Task.Subtask sub in skill.Subtasks)
        //     // {
        //     //     //task for the list
        //     //     GameObject item = Instantiate(_subtaskListEntry, Vector3.zero, Quaternion.identity);
        //     //     item.transform.SetParent(_skillSubtaskContent.transform);
        //     //     item.transform.localPosition = Vector3.zero;
        //     //     item.transform.localScale = Vector3.one;
        //     //     item.transform.localRotation = Quaternion.Euler(0, 0, 0);

        //     //     TMP_Text caption = item.transform.Find("txt_SubTaskNr").GetComponent<TMP_Text>();
        //     //     // GameObject points = item.transform.Find("PointText").gameObject; points for later
        //     //     caption.text = sub.SubtaskName;

        //     //     Button button = item.transform.Find("btn_SubTask").GetComponent<Button>();
        //     //     GameObject checkmark = item.transform.Find("img_Checkmark").gameObject;
        //     //     if (sub.Compleated()) checkmark.SetActive(true);
        //     //     button.onClick.AddListener(() => SubTaskPageLoader(sub));
        //     // }
        //     // refreshing after adding the new elements for the Page loader to set the pages correctly
        // }

        //gets called on Start since the list of task is always the same
        public void LoadTaskPage()
        {
            if (_tasksListOpen != null) _tasksListOpen.Invoke();

            Task.TaskHolder th = GameObject.FindObjectsOfType<Task.TaskHolder>()[0];
            _tasks = th.taskList;


            //loads each task on the parent object
            // will add the task
            foreach (Task.Task task in _tasks)
            {

                // Remove line after testing and uncomment line in end of for loop 
                TaskPageLoader(task);



                //task for the list
                GameObject item = Instantiate(_taskListEntry, Vector3.zero, Quaternion.identity);
                item.transform.SetParent(taskContent.transform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(0, 0, 0);

                TMP_Text caption = item.transform.Find("txt_TaskNr").GetComponent<TMP_Text>();
                caption.text = task.TaskName;
                Button button = item.transform.Find("btn_Task").GetComponent<Button>();
                GameObject completedButton = item.transform.Find("btn_TaskComplete").gameObject;
                GameObject checkmark = item.transform.Find("img_Checkmark").gameObject;
                if (task.Compleated())
                {
                    checkmark.SetActive(true);
                    completedButton.SetActive(true);
                    button = item.transform.Find("btn_TaskComplete").GetComponent<Button>();
                };

                button.onClick.AddListener(() => panelManager.OnClickBackToAboutTask());
                // Commented out for testing to start on Task About page
                // button.onClick.AddListener(() => TaskPageLoader(task));
            }
            // refreshing after adding the new elements for the Page loader to set the pages correctly
            // taskContent.GetComponent<ContentPageChanger>().Refresh();
        }

        public void TaskPageLoader(Task.Task task)
        {
            //for extra events
            if (_taskPageOpen != null) _taskPageOpen.Invoke();

            // panelManager.OnClickBackToAboutTask();

            //cleaning list before loading the new subtasks
            foreach (Transform child in TaskSubtaskContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            foreach (Task.Subtask sub in task.Subtasks)
            {

                // if (sub.SubtaskName == "Pause")
                // {
                //     continue;
                // }
                //task for the list
                GameObject item = Instantiate(_subtaskListEntry, Vector3.zero, Quaternion.identity);
                item.transform.SetParent(TaskSubtaskContent.transform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(0, 0, 0);

                TMP_Text caption = item.transform.Find("txt_SubTaskNr").GetComponent<TMP_Text>();
                // GameObject points = item.transform.Find("PointText").gameObject; points for later
                caption.text = sub.SubtaskName;

                Button button = item.transform.Find("btn_SubTask").GetComponent<Button>();
                GameObject completedButton = item.transform.Find("btn_SubTaskComplete").gameObject;
                GameObject checkmark = item.transform.Find("img_Checkmark").gameObject;
                if (sub.Compleated())
                {
                    checkmark.SetActive(true);
                    completedButton.SetActive(true);
                    button = item.transform.Find("btn_SubTaskComplete").GetComponent<Button>();
                };
                button.onClick.AddListener(() => SubTaskPageLoader(sub));
            }
        }

        public void SubTaskPageLoader(Task.Subtask subtask)
        {

            if (_subtaskPageOpen != null) _subtaskPageOpen.Invoke();

            //hide previos pagee
            // panelManager.OnClickSkillSubtasks();

            TaskPageCanvas.SetActive(false);
            subtaskPageCanvas.SetActive(true);

            _subtaskNameTab.GetComponent<TMP_Text>().text = subtask.SubtaskName;


            //cleaning list before loading the new subtasks
            foreach (Transform child in _subtaskContent.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            // int stepNumber = 1;
            foreach (Task.Step step in subtask.StepList)
            {
                GameObject item = Instantiate(_stepListEntry, Vector3.zero, Quaternion.identity);
                item.transform.SetParent(_subtaskContent.transform);
                item.transform.localPosition = Vector3.zero;
                item.transform.localScale = Vector3.one;
                item.transform.localRotation = Quaternion.Euler(0, 0, 0);


                if (step.IsCompeleted())
                {

                    GameObject checkCircle = item.transform.Find("Checkcircle").gameObject;
                    checkCircle.SetActive(true);
                };
                TMP_Text caption = item.transform.Find("txt_Desc").GetComponent<TMP_Text>();
                caption.text = step.StepName;

                TMP_Text reps = item.transform.Find("txt_repetitionNr").GetComponent<TMP_Text>();
                if (step.RepetionNumber > 1) reps.text = step.RepetionsCompleated + "/" + step.RepetionNumber;

                TMP_Text number = item.transform.Find("txt_SubTaskNr").GetComponent<TMP_Text>();
                number.text = step.getStepNumber() + "";
                // number.text = stepNumber + "";
                // stepNumber++;
            }

        }
    }
}