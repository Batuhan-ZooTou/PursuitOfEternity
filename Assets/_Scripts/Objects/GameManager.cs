using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public UnityEvent OnTrigger;
    public int currentLvl;
    public List<bool> lvls;
    public List<bool> brains;
    public List<bool> GooTypes;
    public List<bool> Notes;
    public GameObject panel;

    // Start is called before the first frame update
    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        if (Instance==null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if(Instance!=this)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            panel.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
    }
    public void SwitchLevel(int level)
    {
        currentLvl = level;
        SceneManager.LoadScene(level);
    }
    public void OpenGUI()
    {
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }
    public void CloseGUI()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
    }



}
