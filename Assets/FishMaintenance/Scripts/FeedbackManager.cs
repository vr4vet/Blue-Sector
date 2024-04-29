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
            "Bra jobba! Følg pilene rundt merden.",
            "",
            ""
        });
        feedback.Add("Håndforing", new List<string> {
            "Kast mat til fisken 5 ganger. Se video fra anlegget.",
            "",
            "Kjempebra, dette var en utfordrende oppgave!",
            "Hent BØTTE OG SPADE på båten for å starte denne oppgaven",
            "Bruk spaden til å hente mat fra bøtten. Kast så maten i merden."
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
        feedback.Add("Dødfisk håndtering", new List<string> {
            "",
            "",
            "",
            "",
            ""
        });
    }


    public void addFeedback(string subtaskName)
    {
        if (subtaskName == "Dødfisk håndtering")
        {
            return;
        }
        if (subtaskName == "Legg til splinter på kjetting" && manager.GetStep("Runde På Ring", "Legg til tau på merd").IsCompeleted() && manager.GetStep("Runde På Ring", "Reparer tau på merd").IsCompeleted())
        // if (subtaskName == "Legg til splinter på kjetting" || subtaskName == "Håndforing")
        {
            StartCoroutine(emergencyFeedback(subtaskName));
            return;
        }
        watch.addInstructions(feedback[subtaskName][0]);
        manager.effectiveBadgeEnabled(true);
        StartCoroutine(moreFeedback(subtaskName));
        if (subtaskName == "Håndforing")
        {
            StartCoroutine(emergencyFeedback(subtaskName));
        }
    }

    IEnumerator moreFeedback(string subtaskName)
    {
        yield return new WaitForSeconds(20f);
        if (subtaskName == "Hent Utstyr" && manager.stepCount < 2)
        {
            watch.addInstructions(feedback[subtaskName][1]);
        }
        // else if (subtaskName == "Håndforing" && manager.GetStep(subtaskName, "Kast mat til fisken").getRepNumber() < 2)
        // {
        //     watch.addInstructions(feedback[subtaskName][1]);
        // }
        else if (subtaskName == "Reparer tau på merd" && !manager.GetStep("Runde På Ring", subtaskName).IsCompeleted())
        {
            watch.addInstructions(feedback[subtaskName][1]);
            manager.effectiveBadgeEnabled(false);
        }
        else if (subtaskName == "Legg til tau på merd" && !manager.GetStep("Runde På Ring", "Reparer tau på merd").IsCompeleted())
        {
            watch.addInstructions(feedback[subtaskName][1]);
            manager.effectiveBadgeEnabled(false);
        }
        else if (subtaskName == "Legg til splinter på kjetting" && !manager.GetStep("Runde På Ring", "Reparer tau på merd").IsCompeleted() && !manager.GetStep("Runde På Ring", "Legg til tau på merd").IsCompeleted())
        {
            watch.addInstructions(feedback[subtaskName][1]);
            manager.effectiveBadgeEnabled(false);
        }
        // if (subtaskName != "Håndforing" && subtaskName != "Hent Utstyr")
        // {
        //     Task.Step badgeStep=manager.GetStep("Runde På Ring", subtaskName);
        //     if(badgeStep.IsCompeleted()) manager.BadgeChanged.Invoke(badgeStep);
        // }
    }

    IEnumerator emergencyFeedback(string subtaskName)
    {
        yield return new WaitForSeconds(40f);
        watch.addInstructions(feedback[subtaskName][4]);
    }

    public void feedbackOnTaskComplete(Task.Subtask subtask)
    {
        if (subtask.Compleated() && (subtask.SubtaskName == "Hent Utstyr" || subtask.SubtaskName == "Håndforing"))
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
