using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class MoveOrRotate : MonoBehaviour
{
    public Transform tweakedObj;
    public UnityEvent hasBrain;
    public Vector3 rotateAxis;
    public float time;
    public bool isActive;
    public int lvl;
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.lvls[lvl-1])
        {
            GetComponent<Interactable>().onInteract.Invoke();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnInteract()
    {
        if (GameManager.Instance.brains[lvl-1])
        {
            if (!isActive)
            {
                isActive = true;
                GameManager.Instance.lvls[lvl - 1] = true;
                tweakedObj.DOBlendableLocalRotateBy(rotateAxis, time);
                hasBrain.Invoke();
            }
        }
        
    }
    
}
