using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [Header("Timer Settings")]
    public float startTime = 10f; // Main countdown start value

    [Header("UI Reference")]
    public TextMeshPro timerText; // Assign your 3D TextMeshPro in the Inspector

    [Header("Target Object")]
    // Backwards-compatible single object. Prefer using `targets` array below.
    public GameObject objectToDisable; // The GameObject that will be disabled when timer ends

    [System.Serializable]
    public class TargetObject
    {
        public GameObject obj;
        [Tooltip("If true the object will be enabled when timer reaches zero; if false it will be disabled.")]
        public bool enableOnZero = true;
    }

    [Header("Objects to toggle on timer end")]
    [Tooltip("Add entries and tick whether each should be enabled or disabled when the countdown reaches zero.")]
    public TargetObject[] targets;

    [Tooltip("If true, the enable/disable actions will run only once when the timer reaches zero.")]
    public bool runActionsOnce = true;
    private bool actionsPerformed = false;

    [Header("Pre-countdown Settings")]
    public AudioClip preCountdownSound; // Sound played on 3..2..1
    private AudioSource audioSource;
    private int preCountdown = 3; // 3,2,1 before main countdown
    private bool preCountdownActive = true;

    public float currentTime;
    private float preCountdownTimer = 3f; // 1 second interval

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        currentTime = startTime;

        // Show initial pre-countdown number (3) immediately
        if (timerText != null)
        {
            timerText.text = preCountdown.ToString();
        }
    }

    void Update()
    {
        if (preCountdownActive)
        {
            HandlePreCountdown();
        }
        else
        {
            HandleMainCountdown();
        }
    }

    void HandlePreCountdown()
    {
        preCountdownTimer -= Time.deltaTime;

        if (preCountdownTimer <= 0f)
        {
            // Play sound at each step
            if (preCountdownSound != null)
            {
                audioSource.PlayOneShot(preCountdownSound);
            }

            preCountdown--; // move to next number
            preCountdownTimer = 1f; // reset 1 second delay

            if (preCountdown > 0)
            {
                // Show next countdown number
                if (timerText != null)
                {
                    timerText.text = preCountdown.ToString();
                }
            }
            else
            {
                // Finished pre-countdown → switch to main countdown
                preCountdownActive = false;
                UpdateTimerDisplay();
            }
        }
    }

    void HandleMainCountdown()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0) currentTime = 0;

            UpdateTimerDisplay();
        }
        else
        {
            // When timer hits zero, perform configured actions (once if desired).
            if (!actionsPerformed)
            {
                // Preserve original single-object behaviour for compatibility
                if (objectToDisable != null && objectToDisable.activeSelf)
                {
                    objectToDisable.SetActive(false);
                }

                // Apply targets array actions
                if (targets != null)
                {
                    for (int i = 0; i < targets.Length; i++)
                    {
                        var t = targets[i];
                        if (t == null || t.obj == null) continue;
                        t.obj.SetActive(t.enableOnZero);
                    }
                }

                actionsPerformed = runActionsOnce;
            }
        }
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            // Show remaining time as whole seconds
            timerText.text = Mathf.CeilToInt(currentTime).ToString();
        }
    }
}
