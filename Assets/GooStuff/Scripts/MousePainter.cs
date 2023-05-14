using UnityEngine;

public class MousePainter : MonoBehaviour{
    public Camera cam;
    [Space]
    public bool mouseSingleClick;
    [Space]
    public Color paintColor;
    
    public float radius = 1;
    public float strength = 1;
    public float hardness = 1;
    public float drawRange = 5;
    Texture2D tex;
    Texture text;
    RenderTexture rText;
    Color maskColor;
    void Update(){

        bool click;
        click = mouseSingleClick ? Input.GetMouseButtonDown(0) : Input.GetMouseButton(0);

        if (click){
            Vector3 position = Input.mousePosition;
            Ray ray = cam.ScreenPointToRay(position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, drawRange)){
                Debug.DrawRay(ray.origin, hit.point - ray.origin, Color.red);
                transform.position = hit.point;
                Paintable p = hit.collider.GetComponent<Paintable>();
                if(p != null){
                    PaintManager.instance.paint(p, hit.point, radius, hardness, strength, paintColor);
                    rText = null;
                    rText = hit.collider.GetComponent<Paintable>().getMask();
                    text= hit.collider.GetComponent<MeshRenderer>().material.GetTexture("Texture2D_41271c3c5f484ca2a435c65087a81705");
                    tex = rText.toTexture2D();
                    maskColor = tex.GetPixel(Mathf.FloorToInt(hit.textureCoord.x * text.width), Mathf.FloorToInt(hit.textureCoord.y * text.height));
                }
            }
        }
    }

}
public static class ExtensionMethod
{
    public static Texture2D toTexture2D(this RenderTexture rTex)
    {
        Texture2D tex = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
        var old_rt = RenderTexture.active;
        RenderTexture.active = rTex;

        tex.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
        tex.Apply();

        RenderTexture.active = old_rt;
        return tex;
    }
}
