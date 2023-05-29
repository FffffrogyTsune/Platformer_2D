using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class Locale_Selector : MonoBehaviour
{
    private bool active = false;

    // Start is called before the first frame update
    void Start()
    {
        int id = PlayerPrefs.GetInt("LocaleKey", 0);
        ChangeLocale(id);
    }

    public void ChangeLocale(int locale_id)
    {
        if (active == true) return;
        StartCoroutine(SetLocale(locale_id));
    }

    IEnumerator SetLocale(int locale_id)
    {
        active = true;
        yield return LocalizationSettings.InitializationOperation;
        PlayerPrefs.SetInt("LocaleKey", locale_id);
        active = false;
    }
}
