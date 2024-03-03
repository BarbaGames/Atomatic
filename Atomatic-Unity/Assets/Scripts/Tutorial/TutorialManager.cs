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
        private int _currentDialogue = 0;
        private int _currentStep = 0;
        private int _previousDialogues = 0;
        private int _i = 0;
        private int _dialogueOffset = 0;


        private void Start()
        {
            if (dialogues.Length > 0)
            {
                dialogue.text = dialogues[0];
                tutorialSteps[_currentStep].SetActive(true);
            }
            else
            {
                Debug.LogWarning("No dialogues are provided.");
            }
        }

        public void NextDialogue()
        {
            _currentDialogue++;

            if (_currentDialogue >= dialoguesAmount[_currentStep])
            {
                _dialogueOffset += _currentDialogue;
                _currentDialogue = 0;

                tutorialSteps[_currentStep].SetActive(false);

                _currentStep++;

                if (_currentStep < tutorialSteps.Length) tutorialSteps[_currentStep].SetActive(true);
            }

            if (_currentStep >= tutorialSteps.Length)
            {
                CloseTutorial();
            }
            else
            {
                dialogue.text = dialogues[_dialogueOffset + _currentDialogue];
            }
        }

        public void NextDialogue2()
        {
            if (_i >= dialoguesAmount[_currentDialogue])
            {
                _currentDialogue++;

                if (_currentDialogue >= tutorialSteps.Length)
                {
                    CloseTutorial();
                }
                else
                {
                    NextStep();
                    dialogue.text = dialogues[_i];
                }
            }
            else
            {
                dialogue.text = dialogues[_i];
            }

            _i++;
        }

        private void NextStep()
        {
            _previousDialogues += dialoguesAmount[_currentDialogue];

            tutorialSteps[_currentStep].SetActive(false);

            _currentStep++;

            tutorialSteps[_currentStep].SetActive(true);
        }

        private void PreviousStep()
        {
            if (_currentStep < 1) return;
            _dialogueOffset -= dialoguesAmount[_currentStep];
            tutorialSteps[_currentStep].SetActive(false);
            _currentStep--;
            tutorialSteps[_currentStep].SetActive(true);
            _currentDialogue = dialoguesAmount[_currentStep];
        }

        public void PreviousDialogue()
        {
            if (_currentStep < 0 || _currentDialogue < 0) return;

            _currentDialogue--;
            if (_currentDialogue + _dialogueOffset >= 0)
            {
                dialogue.text = dialogues[_dialogueOffset + _currentDialogue];
            }

            if (_currentDialogue < dialoguesAmount[_currentStep])
            {
                PreviousStep();
            }
        }

        public void CloseTutorial()
        {
            tutorialObject.SetActive(false);
            // final music start tener en cuenta la segunda vez que se abre le tuto. 
        }
    }
}