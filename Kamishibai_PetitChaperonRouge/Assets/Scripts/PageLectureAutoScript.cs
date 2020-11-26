using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageLectureAutoScript : MonoBehaviour
{
    private Transform camTransform;
    private LivreManagement livreManagement_script;

    //Animation Changement page
    private float posXStart;
    public RectTransform pageRectTransform;

    //Ambiance Musique
    [HideInInspector] public AudioClip ambiancePage;

    //Image de la page
    public Image imagePage;

    // AudioSource utilisée
    private AudioSource audioSourcePage;

    // AudioClips
    [HideInInspector] public AudioClip pisteFR;
    [HideInInspector] public AudioClip pisteEN;
    [HideInInspector] public AudioClip pisteGe;

    //Bool
    [HideInInspector] public bool isLectureLaunch;


    // Start is called before the first frame update
    void Start()
    {
        camTransform = GameObject.Find("Main Camera").transform;
        livreManagement_script = camTransform.GetComponent<LivreManagement>();

        //AfficheNumPage();
        audioSourcePage = livreManagement_script.sonsIndependants_AudioSource;

        
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLectureLaunch)
        {
            StartCoroutine(LectureAutomatiqueDeroulement());
            isLectureLaunch = true;
        }
    }


    //public Text numPageText;

    /*void AfficheNumPage()
    {
        int numeroPage = livreManagement_script.nbPagesLivre - numPageText.transform.parent.parent.GetSiblingIndex();
        numPageText.text = numeroPage.ToString();
    }*/



    IEnumerator LectureAutomatiqueDeroulement()
    {
        yield return new WaitForSeconds(livreManagement_script.tempsWaitLecture);

        if (!audioSourcePage.isPlaying && !livreManagement_script.isSonPause)
        {
            if (livreManagement_script.langue == "FR")
            {
                audioSourcePage.PlayOneShot(pisteFR);
            }
            else if (livreManagement_script.langue == "EN")
            {
                audioSourcePage.PlayOneShot(pisteEN);
            }
            else if (livreManagement_script.langue == "Ge")
            {
                audioSourcePage.PlayOneShot(pisteGe);
            }
        }
        while (audioSourcePage.isPlaying || livreManagement_script.isSonPause)
        {
            yield return new WaitForSeconds(LivreManagement.deltaTime);
        }

        yield return new WaitForSeconds(livreManagement_script.tempsWaitLecture);
        TournerPagePlus();

    }




    // Tourner Page+
    private bool canDesactive = false;
    public void TournerPagePlus()
    {
        int currentPage = this.transform.GetSiblingIndex();
        int nextPage = livreManagement_script.nbPagesLivre - currentPage + 1;

        if (currentPage > 0)
        {
            if ((nextPage < 6 && livreManagement_script.appliDemo) || !livreManagement_script.appliDemo)
            {
                if (this.transform.parent.GetChild(currentPage - 1).name == this.transform.name)
                {
                    this.transform.parent.GetChild(currentPage - 1).gameObject.SetActive(true);
                    livreManagement_script.currentPageLectAuto = currentPage - 1;
                    livreManagement_script.dataCharged = true;
                    livreManagement_script.SaveLivre();

                    StartCoroutine(TournerPagePlusAnim(currentPage));

                }
            }
            else if (livreManagement_script.appliDemo && nextPage >= 6)
            {
                //PopUp Achète
                livreManagement_script.panelPopUpEnceinte.SetActive(true);
                livreManagement_script.transform.GetComponent<LangageScript>().textPopUpEnceinte.text = livreManagement_script.transform.GetComponent<LangageScript>()._AchetePopUp;
                livreManagement_script.transform.GetComponent<LangageScript>().textPopUpEnceinte.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 40);
                livreManagement_script.panelPopUpEnceinte.transform.Find("PanelAchats").gameObject.SetActive(true);
                livreManagement_script.panelPopUpEnceinte.transform.Find("PanelDontAsk").gameObject.SetActive(false);
            }
        }
        else if (currentPage == 0)
        {
            if (this.transform.parent.GetChild(livreManagement_script.nbPagesLivre - 1).name == this.transform.name)
            {
                //this.transform.parent.GetChild(livreManagement_script.nbPagesLivre - 1).gameObject.SetActive(true);
                livreManagement_script.currentPageLectAuto = livreManagement_script.nbPagesLivre - 1;
                livreManagement_script.dataCharged = true;
                livreManagement_script.SaveLivre();
                camTransform.GetComponent<Fonctions_Utiles>().RetourMenuPrincipal();
                for (int ii = 0; ii < livreManagement_script.nbPagesLivre; ii++)
                {
                    if (ii == livreManagement_script.nbPagesLivre - 1)
                    {
                        this.transform.parent.GetChild(ii).gameObject.SetActive(true);
                    }
                    else
                    {
                        this.transform.parent.GetChild(ii).gameObject.SetActive(false);
                    }
                }
            }
        }

        camTransform.GetComponent<Fonctions_Utiles>().DeselectEventSystem(livreManagement_script.monEvent);
    }

    //Animation Tourner Page +
    IEnumerator TournerPagePlusAnim(int currentPage_)
    {
        float speedTournePage = livreManagement_script.speedTournePage;
        posXStart = pageRectTransform.anchoredPosition.x;
        this.transform.parent.GetChild(currentPage_ - 1).GetComponent<PageLectureAutoScript>().isLectureLaunch = false;
        livreManagement_script.inTransitionPage = true;

        if (this.transform.parent.GetChild(currentPage_ - 1).GetComponent<PageLectureAutoScript>().ambiancePage != ambiancePage)
        {
            StartCoroutine(TransitionDeuxAmbiances(currentPage_, currentPage_ - 1));
        }
        else
        {
            canDesactive = true;
        }


        if (livreManagement_script.animSensReverse)
        {
            //Anim Sens 1
            while (pageRectTransform.anchoredPosition.x < Screen.width + posXStart + 10)
            {
                pageRectTransform.anchoredPosition = new Vector2(pageRectTransform.anchoredPosition.x + LivreManagement.deltaTime * speedTournePage, pageRectTransform.anchoredPosition.y);
                yield return new WaitForSeconds(LivreManagement.deltaTime);
            }
        }
        else
        {
            //Anim Sens 2
            while (pageRectTransform.anchoredPosition.x > -posXStart - 10)
            {
                pageRectTransform.anchoredPosition = new Vector2(pageRectTransform.anchoredPosition.x - LivreManagement.deltaTime * speedTournePage, pageRectTransform.anchoredPosition.y);
                yield return new WaitForSeconds(LivreManagement.deltaTime);
            }
        }

        while (!canDesactive)
        {
            yield return new WaitForSeconds(LivreManagement.deltaTime);
            if (canDesactive)
            {
                break;
            }
        }


        this.gameObject.SetActive(false);
        isLectureLaunch = false;
        livreManagement_script.inTransitionPage = false;
        pageRectTransform.anchoredPosition = new Vector2(posXStart, pageRectTransform.anchoredPosition.y);
        canDesactive = false;
        yield return null;
    }


    //Fade du changement de son
    IEnumerator TransitionDeuxAmbiances(int currentPage_, int newPage)
    {
        //On passe la musique en cours sur l'audio temporaire et on diminue progressivement le volume
        livreManagement_script.ambianceGeneraleAudioTemp.clip = this.transform.parent.GetChild(currentPage_).GetComponent<PageLectureAutoScript>().ambiancePage;
        livreManagement_script.ambianceGeneraleAudioTemp.time = livreManagement_script.ambianceGenarale_AudioSource.time;
        livreManagement_script.ambianceGeneraleAudioTemp.Play();
        StartCoroutine(livreManagement_script.FadeMoins_Volume(livreManagement_script.ambianceGeneraleAudioTemp, livreManagement_script.ambianceGenarale_AudioSource.volume, livreManagement_script.timeFadeChangePage, 0));


        //On donne la nouvelle musique à l'audio principale et augmente progressivement le volume
        livreManagement_script.ambianceGenarale_AudioSource.clip = this.transform.parent.GetChild(newPage).GetComponent<PageLectureAutoScript>().ambiancePage;
        livreManagement_script.ambianceGenarale_AudioSource.Play();
        StartCoroutine(livreManagement_script.FadePlus_Volume(livreManagement_script.ambianceGenarale_AudioSource, 0, livreManagement_script.timeFadeChangePage, livreManagement_script.sliderSonAmbiance.value));


        while (livreManagement_script.ambianceGeneraleAudioTemp.volume > 0)
        {
            yield return new WaitForSeconds(LivreManagement.deltaTime);
            if (livreManagement_script.ambianceGeneraleAudioTemp.volume <= 0)
            {
                livreManagement_script.ambianceGeneraleAudioTemp.volume = 0;
                canDesactive = true;
                break;
            }
        }
        yield return null;

    }

}
