using Qualcomm.Snapdragon.Spaces.Samples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace Haunt
{
    public class EndGameState : IGameState
    {
        private GameController controller;
        public EndGameState(GameController controller)
        {
            this.controller = controller;
        }

        public void Start()
        {
            Text title = GameObject.FindGameObjectWithTag("GameTextTitle").GetComponent<Text>();
            Text body = GameObject.FindGameObjectWithTag("GameTextBody").GetComponent<Text>();

            GameResults results = controller.GetGameResults();
            if (results.win)
            {
                title.text = "Congratulations!";

                string fmtstr = "You spared {0} souls with {1:0.0}s left.";
                if (results.soulCount == 1)
                {
                    fmtstr = "You spared {0} soul with {1:0.0}s left.";
                }

                body.text = string.Format(fmtstr, results.soulCount, results.time);
            }
            else
            {
                title.text = "Haunted!";
                body.text = string.Format("You spared {0} {1} before Gerald haunted you.", results.soulCount, results.soulCount == 1 ? "soul" : "souls");
            }

            TooltipManager ttman = controller.GetTooltipManager();
            ttman.SetText("The haunt is over, return to the start to view your results.");
            ttman.Send(4.0f);
        }

        public void Awake()
        {
        }

        public void OnTriggerAction(InputAction.CallbackContext ctx)
        {

        }

        public void CreateSoul()
        {

        }

        public void Update()
        {

        }

        public void End()
        {

        }
    }
}