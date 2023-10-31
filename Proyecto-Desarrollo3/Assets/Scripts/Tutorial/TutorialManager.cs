using TMPro;
using UnityEngine;

namespace Tutorial
{
    public class TutorialManager : MonoBehaviour
    {
        [SerializeField] private string[] dialogues;
        [SerializeField] private int[] dialoguesAmount;
        [SerializeField] private GameObject[] tutorialSteps;
        [SerializeField] private TMP_Text dialogue;
        [SerializeField] private GameObject tutorialObject;
        private int currentDialogue = 0;
        private int currentStep = 0;
        private int previousDialogues = 0;
        private int i = 0;

        private void Start()
        {
            if (dialogues.Length > 0)
            {
                dialogue.text = dialogues[0];
            }
            else
            {
                Debug.LogWarning("No dialogues are provided.");
            }
        }

        public void NextDialogue()
        {
            i++;
            if (i >= dialoguesAmount[currentDialogue])
            {
                currentDialogue++;
                
                if (currentDialogue >= tutorialSteps.Length)
                {
                    CloseTutorial();
                }
                else
                {
                    NextStep();
                    dialogue.text = dialogues[i];
                }
            }
            else
            {
                dialogue.text = dialogues[i];
            }
        }

        private void NextStep()
        {
            previousDialogues += dialoguesAmount[currentDialogue];
            tutorialSteps[currentStep].SetActive(false);
            currentStep++;
            tutorialSteps[currentStep].SetActive(true);
        }

        private void PreviousStep()
        {
            if(currentStep < 1) return;
            previousDialogues -= dialoguesAmount[currentDialogue];
            tutorialSteps[currentStep].SetActive(false);
            currentStep--;
            tutorialSteps[currentStep].SetActive(true);
        }

        public void PreviousDialogue()
        {
            if (i < 1) return;
            i--;
            dialogue.text = dialogues[i];
            if (i-previousDialogues < dialoguesAmount[currentDialogue])
            {
                PreviousStep();
            }
        }

        public void CloseTutorial()
        {
            tutorialObject.SetActive(false);
        }
    }
}