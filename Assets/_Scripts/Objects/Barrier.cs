using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BarrierMode
{
    NoPlayer,
    NoCube,
    Neither
}
public class Barrier : MonoBehaviour
{
    public BarrierMode mode;
    public Material NoPlayer;
    public Material NoCube;
    private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        switch (mode)
        {
            case BarrierMode.NoPlayer:
                gameObject.layer = 11;
                meshRenderer.material = NoPlayer;
                break;
            case BarrierMode.NoCube:
                gameObject.layer = 12;
                meshRenderer.material = NoCube;
                break;
            case BarrierMode.Neither:
                break;
            default:
                break;
        }
        //Noplayer
        Physics.IgnoreLayerCollision(3, 11, true);
        //NoCube
        Physics.IgnoreLayerCollision(6, 12, true);
    }

    public void ChangeLayer()
    {
        switch (mode)
        {
            case BarrierMode.NoPlayer:
                gameObject.layer = 12;
                mode = BarrierMode.NoCube;
                meshRenderer.material = NoCube;
                break;
            case BarrierMode.NoCube:
                gameObject.layer = 11;
                mode = BarrierMode.NoPlayer;
                meshRenderer.material = NoPlayer;
                break;
            case BarrierMode.Neither:
                break;
            default:
                break;
        }
    }
}
