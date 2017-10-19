using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour {
    [SerializeField] private UnityEngine.UI.Slider DifficultSlider;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("Difficult"))
            PlayerPrefs.SetFloat("Difficult", 0.25f);
        SomeValues.StartFrom = PlayerPrefs.GetFloat("Difficult");

        if (SomeValues.StartFrom == 1) DifficultSlider.value = 3;
        else  DifficultSlider.value = SomeValues.StartFrom * 4;
    }

    public void Difficult(UnityEngine.UI.Slider Slider)
    {
        if (Slider.value == 3) SomeValues.StartFrom = 1;
        else SomeValues.StartFrom = Slider.value / 4;

        PlayerPrefs.SetFloat("Difficult", SomeValues.StartFrom);
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
