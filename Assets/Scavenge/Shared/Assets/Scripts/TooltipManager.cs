using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TooltipState
{
    UP,
    DISPLAYING_TEXT,
    DOWN,
    OFF
}

public class TooltipManager : MonoBehaviour
{
    public GameObject tooltip;
    public float speed = 0.0016f;
    private Text text;
    private float time;
    private TooltipState state;
    private float elapsed;
    private float ydrop = 0.142f;
    private float currenty = 0.0f;

    void Start()
    {
        text = tooltip.GetComponentInChildren<Text>();
    }

    public TooltipState GetState()
    {
        return this.state;
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }

    public void Send(float time)
    {
        elapsed = 0.0f;

        tooltip.SetActive(true);
        this.time = time;
        state = TooltipState.DOWN;
    }

    void Update()
    {
        Vector3 ttransform = tooltip.transform.position;

        switch (state) {
            case TooltipState.DOWN:
                if (currenty < ydrop) {
                    ttransform.y -= speed;
                    currenty += speed;
                }
                else {
                    state = TooltipState.DISPLAYING_TEXT;
                }
                break;
            case TooltipState.UP:
                if (currenty > 0.0f) {
                    ttransform.y += speed;
                    currenty -= speed;
                }
                else {
                    state = TooltipState.OFF;
                    tooltip.SetActive(false);
                }
                break;
            case TooltipState.DISPLAYING_TEXT:
                elapsed += Time.deltaTime;
                if (elapsed >= time) {
                    state = TooltipState.UP;
                }
                break;
            case TooltipState.OFF: // should never happen
                break;
        }
        tooltip.transform.position = ttransform;
        // Keep ghost distance relative to camera. As you approach the ghost, it shall move away at the same rate.
    }
}
