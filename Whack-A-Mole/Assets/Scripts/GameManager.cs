using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<Mole> moles;

    [Header("UI objects")]
    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject gameUI;
    [SerializeField] private GameObject outOfTimeText;
    [SerializeField] private GameObject bombText;
    [SerializeField] private TMPro.TextMeshProUGUI timeText;
    [SerializeField] private TMPro.TextMeshProUGUI scoreText;

    [Header("Hole Layouts")]
    [SerializeField] private GameObject threeHoles;
    [SerializeField] private GameObject sixHoles;
    [SerializeField] private GameObject nineHoles;

    private float startingTime = 30f;

    // Global variables
    private float timeRemaining;
    private HashSet<Mole> currentMoles = new HashSet<Mole>();
    private int score;
    private bool playing = false;

    // public so the button can see it
    public void StartGame()
    {
        // Hide/show ui elements we want and dont want to see
        playButton.SetActive(false);
        outOfTimeText.SetActive(false);
        bombText.SetActive(false);
        gameUI.SetActive(true);
        // hide all visible moles
        for (int i = 0; i < moles.Count; i++)
        {
            moles[i].Hide();
            moles[i].SetIndex(i);
        }
        // remove current game slate
        currentMoles.Clear();
        timeRemaining = startingTime;
        score = 0;
        playing = true;
    }

    void Start() {
        StartGame();
    }
    public void GameOver(int type)
    {
        if (type == 0)
        {
            outOfTimeText.SetActive(true);
        }
        else if (type == 1)
        {
            bombText.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
        // Hide all moles
        foreach (Mole mole in moles)
        {
            mole.StopGame();
        }
        playing = false;
        playButton.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (playing)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                timeRemaining = 0;
                GameOver(0);
            }
            timeText.text = $"{(int)timeRemaining / 60}:{(int)timeRemaining % 60:D2}";
            if (currentMoles.Count <= score / 10)
            {
                // choose a random mole
                int index = Random.Range(0, moles.Count);
                if (!currentMoles.Contains(moles[index]))
                {
                    currentMoles.Add(moles[index]);
                    moles[index].Activate(score / 10);
                }
            }
        }
    }
    public void AddScore(int moleIndex)
    {
        // Add and update score.
        score += 1;
        scoreText.text = $"{score}";
        //scoreText.text = $"{score}";
        // Increase time by a little bit.
        timeRemaining += 1;
        // Remove from active moles.
        currentMoles.Remove(moles[moleIndex]);
    }

    public void Missed(int moleIndex, bool isMole)
    {
        if (isMole)
        {
            // Decrease time by a little bit.
            timeRemaining -= 2;
        }
        // Remove from active moles.
        currentMoles.Remove(moles[moleIndex]);
    }

    void Awake()
    {
        // loads in the number of holes based on the user setting
        switch (MainMenu.holeKey)
        {
            case 0:
            {
                threeHoles.SetActive(true);
                sixHoles.SetActive(false);
                nineHoles.SetActive(false);
                break;
            }
            case 1:
            {
                threeHoles.SetActive(false);
                sixHoles.SetActive(true);
                nineHoles.SetActive(false);
                break;
            }
            case 2:
            {
                threeHoles.SetActive(false);
                sixHoles.SetActive(false);
                nineHoles.SetActive(true);
                break;
            }
            default: break;
        }
        // now with the mole holes loaded in, the game manager can add each mole to the list
        moles = FindObjectsOfType<Mole>().ToList();
    }
}
