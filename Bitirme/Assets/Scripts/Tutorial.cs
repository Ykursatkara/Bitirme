using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    private int tutorial_level = 0;
    [SerializeField] Text TutorialText;
    void Update()
    {
        if(Input.GetKeyDown("w") && tutorial_level == 1)
        {
            TutorialText.text = "Press w twice while in the air to double jump";
            tutorial_level++;
        }
        else if((Input.GetKeyDown("a") || Input.GetKeyDown("d")) && tutorial_level == 0)
        {
            TutorialText.text = "Press the w key to jump";
            tutorial_level++;
        }
    }
}
