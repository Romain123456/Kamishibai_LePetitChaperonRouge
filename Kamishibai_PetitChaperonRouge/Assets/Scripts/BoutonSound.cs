using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BoutonSound : MonoBehaviour
{
	public AudioSource monAudioSource;
	public AudioClip selectBoutonSon;

	public void PlaySound () {
        if (!GameObject.Find("Main Camera").GetComponent<LivreManagement>().dataCharged ||
            (GameObject.Find("Main Camera").GetComponent<LivreManagement>().dataCharged && GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent.currentSelectedGameObject.name != "Button_Livre" && GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent.currentSelectedGameObject.name != "Button_LectureAuto") || 
            (GameObject.Find("Main Camera").GetComponent<LivreManagement>().dataCharged && ((GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent.currentSelectedGameObject.name == "Button_Livre" && GameObject.Find("Main Camera").GetComponent<LivreManagement>().currentPage == GameObject.Find("Main Camera").GetComponent<LivreManagement>().nbPagesLivre - 1) || (GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent.currentSelectedGameObject.name == "Button_LectureAuto") && GameObject.Find("Main Camera").GetComponent<LivreManagement>().currentPageLectAuto == GameObject.Find("Main Camera").GetComponent<LivreManagement>().nbPagesLivre - 1) ) )
        {
            monAudioSource.PlayOneShot(selectBoutonSon);
        }
	}
  
}
