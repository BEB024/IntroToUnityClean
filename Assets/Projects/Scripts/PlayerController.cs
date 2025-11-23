//using System;
using System.Collections;
// using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private float movementX;
    private float movementY;

    [Header("Player Settings")]
    public float speed = 5f;

    [Header("Audio Settings")]
    public AudioSource correctAudioSource;
    public AudioSource wrongAudioSource;

    [Header("UI Elements")]
    public Text scoreText;
    public Text timerText;
    public Text colorText;

    [Header("UI Panels")]
    public GameObject inGameUI;    // Assign "In-Game" panel
    public GameObject gameOverUI;  // Assign "GameOver" panel

    [Header("Game Over Elements")]
    public Text finalScoreText;    // Assign the score text inside GameOver

    private int scoreCounter;
    private float timer = 120f;
    private bool isRunning = true;

    private enum PowerUpType { None, Speed, Time, Double }
    private PowerUpType currentPowerUp = PowerUpType.None;
    private Coroutine activePowerUpRoutine;

    private float normalSpeed;
    private bool timerPaused = false;
    private int baseScorePerPickup = 1000;

    private Color[] colorCycle = { Color.red, Color.blue, Color.green, Color.yellow, new Color(0.6f, 0.3f, 0f) };
    private string[] colorNames = { "Red", "Blue", "Green", "Yellow", "Brown" };
    private int currentColorIndex = 0;
    private Color targetColor;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        normalSpeed = speed;
        targetColor = colorCycle[currentColorIndex];
        UpdateUI();

        // ✅ Ensure only in-game UI shows at start
        inGameUI.SetActive(true);
        gameOverUI.SetActive(false);
    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void FixedUpdate()
    {
        if (!isRunning) return;
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);
    }

    void Update()
    {
        if (isRunning && !timerPaused)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 0f;
                isRunning = false;
                ShowGameOver();
            }
        }

        int minutes = Mathf.FloorToInt(timer / 60);
        int seconds = Mathf.FloorToInt(timer % 60);
        timerText.text = $"Time: {minutes}:{seconds:00}";
    }

    void ShowGameOver()
    {
        inGameUI.SetActive(false);
        gameOverUI.SetActive(true);

        if (finalScoreText != null)
        {
            finalScoreText.text = scoreCounter + " POINTS";
            SaveHighScore(scoreCounter);
        }
        else
            Debug.LogWarning("⚠️ finalScoreText not assigned in Inspector.");

        Time.timeScale = 0f;
    }

    void SaveHighScore(int newScore)
    {
        int oldHigh = PlayerPrefs.GetInt("HighScore0", 0);
        if (newScore > oldHigh)
        {
            PlayerPrefs.SetInt("HighScore0", newScore);
            PlayerPrefs.Save();
            Debug.Log("🏆 New High Score: " + newScore);
        }
    }


    // ------------------ POWER-UP SYSTEM ------------------
    void ActivatePowerUp(PowerUpType type)
    {
        if (activePowerUpRoutine != null)
        {
            StopCoroutine(activePowerUpRoutine);
            ResetPowerUp();
        }

        currentPowerUp = type;
        activePowerUpRoutine = StartCoroutine(PowerUpDuration(type));
    }

    IEnumerator PowerUpDuration(PowerUpType type)
    {
        ApplyPowerUpEffect(type);
        yield return new WaitForSecondsRealtime(10f);
        ResetPowerUp();
    }

    void ApplyPowerUpEffect(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.Speed: speed = normalSpeed * 3f; break;
            case PowerUpType.Time: timerPaused = true; break;
            case PowerUpType.Double: baseScorePerPickup = 2000; break;
        }
    }

    void ResetPowerUp()
    {
        speed = normalSpeed;
        timerPaused = false;
        baseScorePerPickup = 1000;
        currentPowerUp = PowerUpType.None;
        activePowerUpRoutine = null;
    }

    // ------------------ PICKUP / ENEMY SYSTEM ------------------
    void OnTriggerEnter(Collider other)
    {
        if (!isRunning) return;

        if (other.CompareTag("PickUp"))
        {
            HandleColorPickup(other);
            return;
        }

        if (other.CompareTag("SpeedPU"))
        {
            ActivatePowerUp(PowerUpType.Speed);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("TimePU"))
        {
            ActivatePowerUp(PowerUpType.Time);
            other.gameObject.SetActive(false);
        }
        else if (other.CompareTag("DoublePU"))
        {
            ActivatePowerUp(PowerUpType.Double);
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.CompareTag("Door"))
        {
            AnimationManager.Instance.ToggleDoor();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Door"))
        {
            AnimationManager.Instance.ToggleDoor();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isRunning) return;

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Vector3 pushDirection = (transform.position - collision.transform.position).normalized;
            float pushForce = 7f; // how strong the knockback is

            // Apply knockback
            rb.AddForce(pushDirection * pushForce, ForceMode.Impulse);

            // Deduct score
            scoreCounter = Mathf.Max(0, scoreCounter - 500);
            UpdateUI();
        }
    }

    void HandleColorPickup(Collider other)
    {
        Renderer rend = other.GetComponent<Renderer>();
        Color pickupColor = rend != null ? rend.material.color : Color.white;
        bool isCorrect = IsSameColor(pickupColor, targetColor);

        if (isCorrect)
        {
            scoreCounter += baseScorePerPickup;
            NextTargetColor();
            correctAudioSource?.Play();
        }
        else
        {
            scoreCounter = Mathf.Max(0, scoreCounter - 100);
            wrongAudioSource?.Play();
        }

        UpdateUI();
        other.gameObject.SetActive(false);
    }

    void NextTargetColor()
    {
        currentColorIndex = (currentColorIndex + 1) % colorCycle.Length;
        targetColor = colorCycle[currentColorIndex];
    }

    void UpdateUI()
    {
        scoreText.text = "Score: " + scoreCounter;
        colorText.text = "Color: " + colorNames[currentColorIndex];
    }

    bool IsSameColor(Color a, Color b)
    {
        const float tolerance = 0.1f;
        return Mathf.Abs(a.r - b.r) < tolerance &&
               Mathf.Abs(a.g - b.g) < tolerance &&
               Mathf.Abs(a.b - b.b) < tolerance;
    }

}