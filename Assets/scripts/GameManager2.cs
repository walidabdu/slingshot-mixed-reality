using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;  // For handling UI elements like buttons

public class GameManager2 : MonoBehaviour
{
    public TextMeshProUGUI timerText;   // Timer TextMeshPro UI element
    public TextMeshProUGUI scoreText;   // Score TextMeshPro UI element
    public TextMeshProUGUI gameOverText; // Game Over TextMeshPro UI element
    public Button startButton;          // Start button
    public GameObject anotherGameManager; // Another GameManager GameObject to activate

    private float countdownTime = 60f;  // Timer in seconds
    private int score = 0;              // Player score
    private bool isGameActive = false;  // Check if the game is active

    // Start is called before the first frame update
    void Start()
    {
        // Initially set game over text and another GameManager inactive
        gameOverText.gameObject.SetActive(false);
        anotherGameManager.SetActive(false);

        // Show the start button at the beginning
        startButton.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        // If the game is active, update the timer
        if (isGameActive)
        {
            UpdateTimer();
        }
    }

    // Method to start the game when the start button is pressed
    public void StartGame()
    {
        // Reset the timer and score
        countdownTime = 60f;
        score = 0;
        UpdateScore(0);

        // Set the game state as active
        isGameActive = true;

        // Activate another GameManager and deactivate start button
        anotherGameManager.SetActive(true);
        startButton.gameObject.SetActive(false);
        gameOverText.gameObject.SetActive(false);
    }

    // Method to handle the countdown timer
    void UpdateTimer()
    {
        if (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;
            timerText.text = "Time: " + Mathf.FloorToInt(countdownTime);  // Display time as an integer
        }
        else
        {
            EndGame(); // End the game when the timer reaches zero
        }
    }

    // Method to end the game when the timer finishes
    void EndGame()
    {
        isGameActive = false;

        // Show Game Over text and the start button
        gameOverText.gameObject.SetActive(true);
        startButton.gameObject.SetActive(true);
    }

    // Method to update the score
    public void UpdateScore(int points)
    {
        score += points;
        scoreText.text = "Score: " + score;
    }

    // Method to handle collision and score update when "ball" collides with "chips"
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is between a ball and chips
        if (collision.gameObject.CompareTag("ball") && gameObject.CompareTag("chips"))
        {
            // Update score when a ball hits chips
            UpdateScore(10);
        }
    }
}
