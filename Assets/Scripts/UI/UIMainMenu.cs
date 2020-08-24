using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMainMenu : MonoBehaviour
{
    [SerializeField] GameObject _objNewGameMenu;
    [SerializeField] GameObject _objOptionMenu;
    [SerializeField] GameObject _objHelpMenu;
    [SerializeField] GameObject _objExitMenu;

    public void OnNewGameCLick()
    {
        gameObject.SetActive(false);

        _objNewGameMenu.SetActive(true);
        _objOptionMenu.SetActive(false);
        _objHelpMenu.SetActive(false);
    }

    public void OnOptionClick()
    {
        gameObject.SetActive(false);

        _objNewGameMenu.SetActive(false);
        _objOptionMenu.SetActive(true);
        _objHelpMenu.SetActive(false);
        
    }

    public void OnHelpClick()
    {
        gameObject.SetActive(false);

        _objNewGameMenu.SetActive(false);        
        _objOptionMenu.SetActive(false);
        _objHelpMenu.SetActive(true);
    }


    public void OnExitClick()
    {
        Application.Quit();
    }
}
