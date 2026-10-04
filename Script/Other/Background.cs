using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    public RawImage background;
    public float _x, _y;
    private void Update()
    {
        background.uvRect = new Rect(background.uvRect.position + new Vector2(_x, _y) * Time.deltaTime, background.uvRect.size);
    }
}