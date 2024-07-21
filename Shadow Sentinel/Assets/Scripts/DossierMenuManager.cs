using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class DossierMenuManager : MonoBehaviour
{
    public GameObject dossierPanel;
    public TMP_Text missionTxt;

    // Start is called before the first frame update
    void Start()
    {
        int currentLvl = SceneManager.GetActiveScene().buildIndex;
        switch (currentLvl)
        {
            case 1:
                LevelOneMission();
                break;

            case 2:
                LevelTwoMission();
                break;

            case 3:
                LevelThreeMission();
                break;

            case 4:
                LevelFourMission();
                break;

            case 5:
                LevelFiveMission();
                break;

            default:
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void LevelOneMission()
    {
        missionTxt.text = "Welcome aboard new recruit! You are inducted into Nexus, a clandestine organization operating in a " +
            "cyberpunk future dominated by mega-corporations. Equipped with cutting-edge cybernetics," +
            " neural implants, and stealth technology, you become Cipher, a shadow operative trained" +
            " to navigate the shadows of towering corporate skyscrapers.";


        missionTxt.gameObject.SetActive(true);
    }
    public void LevelTwoMission()
    {
        missionTxt.text = " Your first mission targets an informant hiding within a sprawling corporate" +
            " headquarters. In the heart of a towering mega-city, you infiltrate the heavily fortified " +
            "building, blending into crowds of augmented employees and security drones. Utilizing " +
            "neural hacking and stealth tactics, eliminate the informant and secure vital data " +
            "implicating corporate malpractice.";


        missionTxt.gameObject.SetActive(true);
    }
    public void LevelThreeMission()
    {
        missionTxt.text = " Intelligence lead you to a floating airship dockyard controlled by a powerful " +
            "corporation. Suspended high above the city, these massive airships serve as mobile bases for" +
            " illegal experiments and weapon shipments. Infiltrate the floating complex," +
            "sabotage operations and neutralize the rogue corporate scientist.";

        missionTxt.gameObject.SetActive(true);
    }
    public void LevelFourMission()
    {
        missionTxt.text = "In a bioluminescent mega-structure beneath the ocean's depths, " +
            "you infiltrate a luxurious nightclub frequented by corporate elites and influential figures." +
            "Extract sensitive data stored in encrypted servers and neutralize the target";


        missionTxt.gameObject.SetActive(true);
    }
    public void LevelFiveMission()
    {
        missionTxt.text = "Your final mission takes you to a colossal skyscraper serving as the syndicate's" +
            " fortified headquarters. Orbiting a war-torn planet, this sprawling structure houses elite " +
            "mercenaries and orbital defenses. Evading security checkpoints and engaging in high-altitude " +
            "combat, you breach the skyscraper's defenses and confront the syndicate's leaders in a climactic" +
            " showdown. By dismantling their operations and securing justice, you ensure Nexus's legacy as " +
            "the galaxy's foremost protector against corporate tyranny.";

        missionTxt.gameObject.SetActive(true);
    }
}
