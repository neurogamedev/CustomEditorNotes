using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class CustomAction : MonoBehaviour
{
    // In case you want multiple sections and divide their properties in the inspector using a drop-down menu.
    public enum ActionType
    {
        ChatBubble,
        Move
    }

    public ActionType actionType;

    // Type = ChatBubble

    public ChatBubbleParams chatBubbleParams;

    [System.Serializable]
    public class ChatBubbleParams
    {
        public GameObject chatBubble;
        public string text;
        public CustomAction onChatBubbleComplete;

        public void ExecuteAction()
        {
            print("Chat bubble says : '" + text + "'");
        }
    }

    // Type = Move

    public MoveParams moveParams;

    [System.Serializable]
    public class MoveParams
    {
        public GameObject character;
        public Vector3 movePosition;
        public CustomAction onReachedPosition;

        public void ExecuteAction()
        {
            print("Character " + character.name + " is now moving to " + movePosition.ToString() + ".");
        }
    }

    // This is going to become much more interesting in outside scripts.
    // Check the format for each ExecuteAction() in every ActionParams(). It's nested, you see...
    public void ExecuteAction()
    {
        switch(actionType)
        {
            case ActionType.ChatBubble:
                chatBubbleParams.ExecuteAction(); 
                break;

            case ActionType.Move:
                moveParams.ExecuteAction();
                break;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        CustomAction customAction = this;
        Handles.Label(customAction.transform.position, customAction.actionType.ToString());

        switch(actionType)
        {
            // Quite frankly, this is just a filler. Never mind what is going on here.
            case ActionType.ChatBubble:
                if(chatBubbleParams.chatBubble != null)
                {
                    Handles.DrawDottedLine(
                        customAction.transform.position,
                        chatBubbleParams.chatBubble.transform.position,
                        10f
                        );

                    GUIStyle style = new GUIStyle();
                    style.normal.textColor = Color.green;
                    style.alignment = TextAnchor.MiddleCenter;
                    Handles.Label(chatBubbleParams.chatBubble.transform.position, chatBubbleParams.text, style);
                }
                break;

            //  This is the interesting part I want to save for later.
            case ActionType.Move:
                if (moveParams.character != null)
                {
                    Handles.DrawDottedLine(
                        customAction.transform.position,
                        moveParams.character.transform.position,
                        10f
                        );

                    Handles.color = Color.cyan;
                    Handles.DrawLine(
                        moveParams.character.transform.position, 
                        moveParams.movePosition
                        );

                    Vector3 direction = (moveParams.movePosition - moveParams.character.transform.position).normalized;
                    Handles.color = Color.cyan;
                    Handles.SphereHandleCap(
                        0,
                        moveParams.character.transform.position,
                        Quaternion.FromToRotation(moveParams.character.transform.forward, direction),
                        0.15f,
                        EventType.Repaint
                        );

                    Handles.color = Color.cyan;
                    Handles.ConeHandleCap(
                        0,
                        (0.5f * moveParams.movePosition) + (0.5f * moveParams.character.transform.position),
                        Quaternion.FromToRotation(moveParams.character.transform.forward, direction),
                        0.15f,
                        EventType.Repaint
                        );

                    Handles.color = Color.cyan;
                    Handles.ConeHandleCap(
                        0,
                        (0.95f * moveParams.movePosition) + (0.05f * moveParams.character.transform.position), // The arrowhead kinda lands right in the middle of the transform, this equation brings it back just a little so that the tip lands on the transform.
                        Quaternion.FromToRotation(moveParams.character.transform.forward, direction),
                        0.2f,
                        EventType.Repaint
                        );

                    GUIStyle style = new GUIStyle();
                    style.normal.textColor = Color.cyan;
                    style.alignment = TextAnchor.MiddleCenter;
                    Handles.Label(customAction.moveParams.character.transform.position + new Vector3(0f, 0.25f, 0f), "From here...", style);
                    Handles.Label(customAction.moveParams.movePosition + new Vector3(0f, 0.25f, 0f), "...to Here", style);
                }
                break;
        }

    }
#endif

}
