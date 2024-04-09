using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedbackManager : MonoBehaviour
{
    private AddInstructionsToWatch watch;
    private MaintenanceManager manager;
    private Dictionary<string, List<string>> feedback;

    void Start()
    {
        manager = this.gameObject.GetComponent<MaintenanceManager>();
        watch = this.gameObject.GetComponent<AddInstructionsToWatch>();
        feedback = new Dictionary<string, List<string>>();
        manager.SubtaskChanged.AddListener(feedbackOnTaskComplete);
        feedback.Add("Hent Utstyr", new List<string> {
            "Ta med deg utstyr.",
            "Berør utstyret for å ta det med deg.",
            "Bra jobba! Gå videre til neste sylinder.",
            "",
            ""
        });
        feedback.Add("Håndforing", new List<string> {
            "Kast mat til fisken 5 ganger.",
            "Bruk spaden til å hente mat fra bøtten. Kast så maten i merden.",
            "Kjempebra, dette var en utfordrende oppgave!",
            "Hent BØTTE OG SPADE på båten for å starte denne oppgaven",
            ""
        });
        feedback.Add("Legg til tau på merd", new List<string> {
            "Legg til det manglende tauet.",
            "Berør omrisset med tauet du har i venstre hånd.",
            "",
            "Hent TAU på båten for å starte denne oppgaven",
            ""
        });
        feedback.Add("Legg til splinter på kjetting", new List<string> {
            "Legg til splinten i kjettingen.",
            "Berør omrisset med splinten du har i venstre hånd.",
            "",
            "Hent SPLINT på båten for å starte denne oppgaven",
            "Berør omrisset på bakken med splinten du har i venstre hånd. Dette klarer du!"
        });
        feedback.Add("Reparer tau på merd", new List<string> {
            "Bytt ut det gamle tauet.",
            "Berør det utslitte tauet med tauet du har i venstre hånd.",
            "",
            "Hent TAU på båten for å starte denne oppgaven",
            "Berør det utslitte tauet med tauet du har i venstre hånd. Dette klarer du!"
        });
        feedback.Add("Runde På Ring", new List<string> {
            "",
            "",
            "Bra, nå er merden kontrollert.",
            "",
            ""
        });
        feedback.Add("Pause", new List<string> {
            "",
            "",
            "Bra jobba!",
            "",
            ""
        });
    }


    public void addFeedback(string subtaskName)
    {
        if (subtaskName == "Legg til splinter på kjetting" && manager.GetStep("Runde På Ring", "Legg til tau på merd").IsCompeleted() && manager.GetStep("Runde På Ring", "Reparer tau på merd").IsCompeleted())
        {
            StartCoroutine(emergencyFeedback(subtaskName));
            return;
        }
        watch.addInstructions(feedback[subtaskName][0]);
        StartCoroutine(moreFeedback(subtaskName));
    }

    IEnumerator moreFeedback(string subtaskName)
    {
        yield return new WaitForSeconds(20f);
        if (subtaskName == "Hent Utstyr" && manager.stepCount < 2)
        {
            watch.addInstructions(feedback[subtaskName][1]);
        }
        else if (subtaskName == "Håndforing" && manager.GetStep(subtaskName, "Kast mat til fisken").getRepNumber() < 2)
        {
            watch.addInstructions(feedback[subtaskName][1]);
        }
        else if (subtaskName == "Reparer tau på merd" && !manager.GetStep("Runde På Ring", subtaskName).IsCompeleted())
        {
            watch.addInstructions(feedback[subtaskName][1]);
        }
        else if (subtaskName == "Legg til tau på merd" && !manager.GetStep("Runde På Ring", "Reparer tau på merd").IsCompeleted())
        {
            watch.addInstructions(feedback[subtaskName][1]);
        }
        else if (subtaskName == "Legg til splinter på kjetting" && !manager.GetStep("Runde På Ring", "Reparer tau på merd").IsCompeleted() && !manager.GetStep("Runde På Ring", "Legg til tau på merd").IsCompeleted())
        {
            watch.addInstructions(feedback[subtaskName][1]);
        }
        if (subtaskName != "Håndforing")
        {
            manager.BadgeChanged.Invoke(manager.GetStep("Runde På Ring", subtaskName));
        }
    }

    IEnumerator emergencyFeedback(string subtaskName)
    {
        yield return new WaitForSeconds(40f);
        watch.addInstructions(feedback[subtaskName][4]);
    }

    public void feedbackOnTaskComplete(Task.Subtask subtask)
    {
        if (manager.stepCount < 6 || subtask.SubtaskName == "Håndforing")
        {
            watch.addInstructions(feedback[subtask.SubtaskName][2]);
        }
    }

    public void equipmentFeedback(string subtaskName)
    {
        watch.addInstructions(feedback[subtaskName][3]);
    }

    public void StopMoreFeedback()
    {
        StopAllCoroutines();
    }

    public void emptyInstructions()
    {
        watch.emptyInstructions();
        StopAllCoroutines();
    }

    public string getText()
    {
        return watch.getText();
    }
}
