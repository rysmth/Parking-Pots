using UnityEngine;

public class changeCanvas : MonoBehaviour
{
    public GameObject firstCanvas;
    public GameObject secondCanvas;
    public GameObject sceneModel;

    public void ChangeCanvas()
    {
        if (firstCanvas.activeSelf)
        {
            secondCanvas.SetActive(true);
            firstCanvas.SetActive(false);
            sceneModel.SetActive(false);
        }

        else
        {
            secondCanvas.SetActive(false);
            firstCanvas.SetActive(true);
            sceneModel.SetActive(true);
        }
    }
}