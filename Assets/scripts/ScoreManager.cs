using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

/// <summary>
/// ScoreManager handles scoring, displays results, and saves/loads player data using JSON.
/// Attach this to a GameObject in your scene. Requires a CountdownTimer somewhere in the scene.
/// </summary>
public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance; // Singleton

    private int totalScore = 0;

    [Header("3D Text (World Space)")]
    public TextMeshPro scoreText; // drag your 3D TMP object here

    [Header("Final Score Sound")]
    public AudioClip finalScoreSound;

    [Header("Tags to Track for Hits")]
    [Tooltip("List the collider tags you want to track (ordered). Default: one, two, 3")]
    public string[] trackedTags = new string[] { "Ethio Telecome", "Telebirr", "Zemen Gebeya" };

    [Header("Player ID Text (3D)")]
    [Tooltip("Assign a 3D TextMeshPro object that contains the player ID text.")]
    public TextMeshPro playerIdText; // drag 3D TextMeshPro object here in Inspector

    [Header("Debug")]
    [Tooltip("When true the final-format score display will be shown even while the timer is running (for debugging).")]
    public bool forceShowFinalFormat = false;

    [Header("Per-Tag Display Toggles")]
    [Tooltip("Toggle whether each tracked tag is shown in the final display. Array aligns with trackedTags.")]
    public bool[] displayTrackedTag = null;

    // Internal
    private Dictionary<string, int> hitCounts = new Dictionary<string, int>();
    private AudioSource audioSource;
    private bool finalDisplayed = false;

    // File path for JSON save
    private string saveFilePath;

    [System.Serializable]
    public class PlayerData
    {
        public string playerId;
        public int score;
    }

    private void Awake()
    {
        // Setup singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Setup audio source
        audioSource = gameObject.AddComponent<AudioSource>();

        // Initialize hit counts
        hitCounts.Clear();
        if (trackedTags != null)
        {
            foreach (var t in trackedTags)
            {
                if (!string.IsNullOrEmpty(t) && !hitCounts.ContainsKey(t))
                    hitCounts[t] = 0;
            }
        }

    // Ensure displayTrackedTag array matches trackedTags length
    SyncDisplayTagArray();

    }

    // Ensure the displayTrackedTag array aligns with trackedTags length
    private void SyncDisplayTagArray()
    {
        if (trackedTags == null)
        {
            displayTrackedTag = null;
            return;
        }

        if (displayTrackedTag == null || displayTrackedTag.Length != trackedTags.Length)
        {
            bool[] newArr = new bool[trackedTags.Length];
            for (int i = 0; i < newArr.Length; i++) newArr[i] = true; // default: show
            if (displayTrackedTag != null)
            {
                // copy old values into new array where possible
                for (int i = 0; i < Mathf.Min(displayTrackedTag.Length, newArr.Length); i++)
                    newArr[i] = displayTrackedTag[i];
            }
            displayTrackedTag = newArr;
        }

        // Setup JSON save path
        saveFilePath = Path.Combine(Application.persistentDataPath, "player_score.json");
    }

    private void Update()
    {
        CountdownTimer timer = FindObjectOfType<CountdownTimer>();
        if (timer == null || scoreText == null) return;

        // Normal runtime display: simple score while timer > 0, unless forced to show final format for debugging.
        if (timer.currentTime > 0f && !forceShowFinalFormat)
        {
            scoreText.text = "Score: " + totalScore.ToString();
        }
        else
        {
            // Show final-style details. If timer actually ended, save once; if forced via debug toggle, don't auto-save.
            ShowFinalScore();

            if (timer.currentTime <= 0f && !finalDisplayed)
            {
                SaveScore();
                finalDisplayed = true;
            }
        }
    }

    /// <summary>
    /// Add to the player's score by points.
    /// </summary>
    public void AddScore(int points)
    {
        totalScore += points;
    }

    /// <summary>
    /// Register a hit for a given tag.
    /// </summary>
    public void RegisterHit(string tag, int points = 0)
    {
        if (string.IsNullOrEmpty(tag)) return;

        if (hitCounts.ContainsKey(tag))
        {
            hitCounts[tag]++;
        }
        else
        {
            hitCounts[tag] = 1;
        }

        if (points != 0) AddScore(points);
    }

    /// <summary>
    /// Show final results: Player ID, score, hits per tag, and previous saved score.
    /// </summary>
    private void ShowFinalScore()
    {
        if (scoreText == null) return;

    string playerId = playerIdText != null ? playerIdText.text : "Unknown";

        // Load previous data
        PlayerData prevData = LoadScore();

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("Player: " + playerId);
        sb.AppendLine("Final Score: " + totalScore.ToString());
        sb.AppendLine("Hits:");

        if (trackedTags != null && trackedTags.Length > 0)
        {
            for (int i = 0; i < trackedTags.Length; i++)
            {
                var t = trackedTags[i];
                if (string.IsNullOrEmpty(t)) continue;

                // Respect the per-tag display toggle if provided
                if (displayTrackedTag != null && i < displayTrackedTag.Length && !displayTrackedTag[i])
                    continue;

                int count = hitCounts.ContainsKey(t) ? hitCounts[t] : 0;
                // Show as: tag xN (e.g. one x3) or show x0 if zero
                sb.AppendLine(t + " x" + count.ToString());
            }
        }

        if (prevData != null)
        {
            sb.AppendLine("");
            sb.AppendLine("Previous Player: " + prevData.playerId);
            sb.AppendLine("Previous Score: " + prevData.score);
        }

        scoreText.text = sb.ToString();

        if (finalScoreSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(finalScoreSound);
        }
    }

    /// <summary>
    /// Save the current playerId and score to JSON.
    /// </summary>
    private void SaveScore()
    {
    string playerId = playerIdText != null ? playerIdText.text : "Unknown";

        PlayerData data = new PlayerData
        {
            playerId = playerId,
            score = totalScore
        };

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, json);

        Debug.Log($"Saved score for '{playerId}': {totalScore}");
    }

    /// <summary>
    /// Load previously saved score from JSON (if exists).
    /// </summary>
    private PlayerData LoadScore()
    {
        if (!File.Exists(saveFilePath)) return null;

        string json = File.ReadAllText(saveFilePath);
        return JsonUtility.FromJson<PlayerData>(json);
    }
}
