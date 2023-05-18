using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;
public class RandomGlitchEmitter : MonoBehaviour
{
    // Frequency 1/n game ticks
    public int frequency;
    // Length in game ticks
    public int duration;
    // Maximum intensity value to ramp up to
    public float maxIntensity;

    private DigitalGlitch _vfxGlitch;
    private bool doingGlitch;
    private float fromValue;
    private float toValue;
    private int currentTick;
    private float stepInterval;
    private float currentValue;
    private bool glitchLock;
    // Start is called before the first frame update
    void Start()
    {
        glitchLock = false;
        _vfxGlitch = Camera.main.GetComponent<DigitalGlitch>();
    }

    // Update is called once per frame
    void Update()
    {
        if (doingGlitch) {
            ContinueGlitch();
            return;
        }

        doingGlitch = ShouldGlitch();
        if (doingGlitch)
        {
            StartGlitch();
        }
    }

    // Used to fuzz the camera for a given point of time
    public void FuzzScreen(float time)
    {
        StartCoroutine(GlitchOverride(time));
    }

    IEnumerator GlitchOverride(float time)
    {
        glitchLock = true;
        float oldMax = maxIntensity;
        float oldDuration = duration;
        duration = (int) (time / Time.smoothDeltaTime);
        maxIntensity = 1.0f;
        doingGlitch = true;
        StartGlitch();
        while (doingGlitch)
        {
            yield return null;
            ContinueGlitch();
        }

        glitchLock = false;
    }

    bool ShouldGlitch()
    {
        return !glitchLock && Random.Range(1, frequency) == 1;
    }

    void StartGlitch() {
            fromValue = Random.Range(0.001f, 0.01f);
            toValue = Random.Range(0.01f, maxIntensity);
            stepInterval = (toValue-fromValue) / duration;
            currentValue = fromValue;
            _vfxGlitch.intensity = fromValue;
            currentTick = 0;
    }

    void ContinueGlitch() {
        // If we're finished, restart
        if (currentTick == duration) {
            _vfxGlitch.intensity = 0;
            doingGlitch = false;
            return;
        }

        // go back down half way through
        if (currentTick == duration/2) {
            stepInterval *= -1;
        }
        currentValue += stepInterval;
        _vfxGlitch.intensity = currentValue;
        currentTick++;
    }
}