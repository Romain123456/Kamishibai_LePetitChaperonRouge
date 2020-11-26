using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LivreData
{
    public float volSonAmbiance;
    public float volSonsIndep;
    public int currentPage;
    public string langue;
    public int currentPageLectAuto;
    public bool isDontAskAgain;
 
    public LivreData(LivreManagement livreScript)
    {
        volSonAmbiance = livreScript.volSonAmbiance;
        volSonsIndep = livreScript.volSonsIndep;
        currentPage = livreScript.currentPage;
        langue = livreScript.langue;
        currentPageLectAuto = livreScript.currentPageLectAuto;
        isDontAskAgain = livreScript.isDontAskAgain;
    }
}
