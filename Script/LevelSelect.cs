using UnityEngine;

// Ryan Joshua Smith Level Select Script
public class LevelSelect : MonoBehaviour
{
    [Header("Level Preview")]
    [SerializeField] GameObject[] levelModel;
    private GameObject activeModel;
    [SerializeField] GameObject activeModelPosition;

    [Header("Level Index")]
    public static int activeLevel;

    [Header("Change Canvas")]
    [SerializeField] GameObject levelSelectCanvas;
    [SerializeField] GameObject characterSelectCanvas;

    [Header("Management")]
    public static int selectedLevel;

    [Header("Sound Design")]
    [SerializeField] AudioSource selectSFX;


    private void Awake()
    {
        activeLevel = 0; // Default Is The First Level So Player Cannot Progress Without A Level
        activeModel = Instantiate(levelModel[activeLevel], activeModelPosition.transform); // Places The Model Corresponding To The Value At That Specific Location With That Specific Size etc.
    }

    public void SelectLevel()
    {
        selectSFX.Play();

        selectedLevel = activeLevel;

        if (levelSelectCanvas.activeSelf) // First Canvas Is On By Default, But Unnecessary Check As There Is No Back Button But Could Easily Be Implemented By Using This Method Too 
        {
            // Awkwardly Hides Previous Menu and Model To Then Show Next Menu
            levelSelectCanvas.SetActive(false);
            characterSelectCanvas.SetActive(true);
            activeModel.SetActive(false);
        }

        else // If It's not Shown, Do this instead (Make's it Shown)
        {
            levelSelectCanvas.SetActive(true);
            characterSelectCanvas.SetActive(false);
            activeModel.SetActive(true);
        }
    }

    public void NextLevel()
    {
        activeLevel = (activeLevel + 1) % levelModel.Length; // Moves Array Index Up Before Going Back To 0
        RepeatInstantiate();
    }

    public void PreviousLevel()
    {
        activeLevel = (activeLevel - 1 + levelModel.Length) % levelModel.Length; // Avoids Potential Breaks
        RepeatInstantiate();
    }

    private void RepeatInstantiate() // Avoids Repeating Code
    {
        selectSFX.Play(); // Audio Indicator of Input
        Destroy(activeModel); // Removes previous Model
        activeModel = Instantiate(levelModel[activeLevel], activeModelPosition.transform); // Places new Model To Indicate To The player that they've Changed Level
    }
}