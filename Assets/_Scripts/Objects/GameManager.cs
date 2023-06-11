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

    // Start is called before the first frame update
    private void Awake()
    {
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
        
    }
    public void SwitchLevel(int level)
    {
        currentLvl = level;
        SceneManager.LoadScene(level);
    }
    
}
