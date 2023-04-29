using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization;


public class DropDown : MonoBehaviour
{
    public void setSelectedLocale(Locale locale){
        LocalizationSettings.SelectedLocale = locale;
    }
    
    public void DropDownTest(int index){
        switch(index){
            case 0: setSelectedLocale(LocalizationSettings.AvailableLocales.Locales[0]); break;
            case 1: setSelectedLocale(LocalizationSettings.AvailableLocales.Locales[1]); break;
            case 2: setSelectedLocale(LocalizationSettings.AvailableLocales.Locales[2]); break;
            default:
                setSelectedLocale(LocalizationSettings.AvailableLocales.Locales[0]); break;
        }
    }
}
