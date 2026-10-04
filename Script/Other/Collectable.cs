using TMPro;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    [Header("Text Mesh Pro For Collectable Amount")]
    public TMP_Text collectableCountText;

    private void Awake()
    {
        PlayerPrefs.GetInt("Collectables");
        collectableCountText.text = PlayerPrefs.GetInt("Collectables").ToString();
    }
}