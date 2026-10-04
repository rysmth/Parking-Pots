using UnityEngine;
using UnityEngine.SceneManagement;

// Ryan Smith Character Select Script
public class CharacterSelect : MonoBehaviour
{
    [Header("Character Information")]
    public GameObject[] characterModels;

    [Header("Current Character")]
    public static int activeCharacter;
    private GameObject activeCharacterModel;
    public GameObject activeCharacterModelPosition;

    [Header("Locked Visual")]
    public GameObject officeCostumeLock;
    public GameObject chefCostumeLock;
    public GameObject vikingCostumeLock;


    //-----------------------------------Start is called once upon creation-------------------------
    private void Start()
    {
        activeCharacter = 0; // Default So The Player Cannot Progress With No Model
        activeCharacterModel = Instantiate(characterModels[activeCharacter], activeCharacterModelPosition.transform); // Spawns The Model As A Visual Indicator That Their Input Has Been Understood

        // Repeated Code To Check If Unlocked Costumes, If So The Icon/s Are Removed.
        if (PlayerPrefs.GetInt("officeCharacterState") == 1)
        { 
            officeCostumeLock.SetActive(false);
        }

        if (PlayerPrefs.GetInt("chefCharacterState") == 1)
        {
            chefCostumeLock.SetActive(false);
        }

        if (PlayerPrefs.GetInt("vikingCharacterState") == 1)
        {
            vikingCostumeLock.SetActive(false);
        }

        if (PlayerPrefs.GetInt("Collectables") >= 10)
        {
            PlayerPrefs.SetInt("vikingCharacterState", 1);
            vikingCostumeLock.SetActive(false);
        }
    }

    //-----------------------------------Change Visual Indicator & Where Costume Is Finalised-------------------------
    public void SelectCharacter() // This Occurs Once The Button Is Pressed To Progress
    {
        SceneManager.LoadScene(1);
    }
    private void ChangeModel() // Avoids Repeated Code
    {
        Destroy(activeCharacterModel); // Removes Previous Model
        activeCharacterModel = Instantiate(characterModels[activeCharacter], activeCharacterModelPosition.transform); // Places The Chosen Model
    }

    //-----------------------------------Costume Variants-------------------------
    public void NormalCharacter() // Public So The Button Can Trigger It 
    {
        activeCharacter = 0; // This Costume Will Never Be Locked So It Simply Changes The Value Without Any Checks
        ChangeModel();
    }

    public void OfficeCharacter()
    {
        if (PlayerPrefs.GetInt("officeCharacterState") == 1) // Only changes the player's model if they have beaten the level that would change the value from 0 to 1 (locked to Unlocked)
        {
            activeCharacter = 1; // Changes The Value To The One That Will Correspond With The Correct Model In The Management Script 
            ChangeModel();
        }

        else
        {
            return; // Simply Does Nothing If The Player Has Not Unlocked It
        }
    }

    // Repeated Code For The Other Options
    public void ChefCharacter()
    {
        if (PlayerPrefs.GetInt("chefCharacterState") == 1)
        {
            activeCharacter = 2;
            ChangeModel();
        }

        else
        { 
            return; 
        }
    }
    public void VikingCharacter()
    {
        if (PlayerPrefs.GetInt("vikingCharacterState") == 1)
        {
            activeCharacter = 3;
            ChangeModel();
        }

        else
        {
            return;
        }
    }
}