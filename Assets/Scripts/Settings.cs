using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {
    private void Start()
    {
        SomeValues.StartFrom = 0.25f;
    }

    public void Dificult(UnityEngine.UI.Slider Slider)
    {
        if (Slider.value == 3) SomeValues.StartFrom = 1;
        else SomeValues.StartFrom = Slider.value / 4;
    }

    //public void InLineCount(UnityEngine.UI.Slider Slider)
    //{
    //    GameManager.SomeValues.InLineLength = (int)Slider.value;
    //}

    public void BackToGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main_Scene");
    }
}
