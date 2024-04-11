using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DialogueTriggeer : MonoBehaviour
{
    [SerializeField] private List<dialogueStrings> dialogueStrings = new List<dialogueStrings>();
    [SerializeField] private Transform NPCTransform;

    private bool hasSpoken = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<DialogueManager>().DialogueStart(dialogueStrings, NPCTransform);
            hasSpoken = true;
        }
    }
}

[System.Serializable]

public class dialogueStrings
{
    public string text;//npc says
    public bool isEnd;//is last line?

    [Header("Branch")]
    public bool isQuestion;
    public string answerOption1;
    public string answerOption2;
    public int option1IndexJump;
    public int option2IndexJump;

    [Header("Trigger Events")]
    public UnityEvent startDialogueEvent;
    public UnityEvent endDialogueEvent;

}
