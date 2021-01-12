using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class PageLivreScript : MonoBehaviour
{
    private Transform camTransform;
    private LivreManagement livreManagement_script;


    //Panel Son Indépendant
    public GameObject panelSonIndep;
    public bool isPanelSonIndep;
    public int nbSonsIndep;
    public GameObject sonIndepPrefab;
    [HideInInspector] public GameObject[] sonsIndep;


    //Bouton Page+ Page-
    public GameObject pageP_GO;
    public GameObject pageM_GO;


    //Animation Changement page
    private float posXStart;
    public RectTransform pageRectTransform;

    //Ambiance Musique
    [HideInInspector] public AudioClip ambiancePage;

    public Sprite iconeExit;



    // Start is called before the first frame update
    void Start()
    {
        camTransform = GameObject.Find("Main Camera").transform;
        livreManagement_script = camTransform.GetComponent<LivreManagement>();

        //Panel Son Indep
        if (isPanelSonIndep)
        {
            panelSonIndep.SetActive(true);
        } else if (!isPanelSonIndep)
        {
            panelSonIndep.SetActive(false);
        }

        //Bouton Page+ Page-
        if (transform.GetSiblingIndex() == livreManagement_script.nbPagesLivre - 1)
        //if(transform.GetSiblingIndex() == 0)
        {
            //Changer par un symbole de retour au menu livre
            pageM_GO.SetActive(false);
        }
        if (transform.GetSiblingIndex() == 0)
        //if (transform.GetSiblingIndex() == camTransform.GetComponent<LivreManagement>().nbPagesLivre - 1)
        {
            //Changer par un symbole de retour au menu livre
            //pageP_GO.SetActive(false);
            pageP_GO.GetComponent<Image>().sprite = iconeExit;
            //pageP_GO.transform.GetChild(0).GetComponent<Text>().text = "P1";
        }

        AfficheNumPage();
    }





    void Update()
    {
        SonsIndependantsInteractability();
        TournePageInteractability();
    }


    public Text numPageText;

    void AfficheNumPage()
    {
        int numeroPage = livreManagement_script.nbPagesLivre - numPageText.transform.parent.parent.GetSiblingIndex();
        numPageText.text = numeroPage.ToString();
    }



    // Tourner Page+
    private bool canDesactive = false;

    public void TournerPagePlus()
    {
        int currentPage = this.transform.GetSiblingIndex();

        int nextPage = livreManagement_script.nbPagesLivre - currentPage + 1;

        if ((nextPage < 6 && livreManagement_script.appliDemo) || !livreManagement_script.appliDemo)
        {
            senstourne = 1;
            if (currentPage > 0)
            {
                if (this.transform.parent.GetChild(currentPage - 1).name == this.transform.name)
                {
                    this.transform.parent.GetChild(currentPage - 1).gameObject.SetActive(true);
                    livreManagement_script.currentPage = currentPage - 1;
                    livreManagement_script.dataCharged = true;
                    livreManagement_script.SaveLivre();

                    StartCoroutine(TournerPagePlusAnim(currentPage));

                }
            }
            else if (currentPage == 0)
            {
                if (this.transform.parent.GetChild(livreManagement_script.nbPagesLivre - 1).name == this.transform.name)
                {
                    //this.transform.parent.GetChild(livreManagement_script.nbPagesLivre - 1).gameObject.SetActive(true);
                    livreManagement_script.currentPage = livreManagement_script.nbPagesLivre - 1;
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
                    //StartCoroutine(TournerPageMoinsAnim(currentPage));
                }
            }

            
        } else if (livreManagement_script.appliDemo && nextPage >= 6)
        {
            //PopUp Achète
            livreManagement_script.panelPopUpEnceinte.SetActive(true);
            livreManagement_script.transform.GetComponent<LangageScript>().textPopUpEnceinte.text = livreManagement_script.transform.GetComponent<LangageScript>()._AchetePopUp;
            livreManagement_script.transform.GetComponent<LangageScript>().textPopUpEnceinte.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 40);
            livreManagement_script.panelPopUpEnceinte.transform.Find("PanelAchats").gameObject.SetActive(true);
            livreManagement_script.panelPopUpEnceinte.transform.Find("PanelDontAsk").gameObject.SetActive(false);
            livreManagement_script.transform.GetComponent<LangageScript>().textPopUpEnceinte.transform.SetAsLastSibling();
        }
        camTransform.GetComponent<Fonctions_Utiles>().DeselectEventSystem(livreManagement_script.monEvent);
    }

    //Animation Tourner Page +
    IEnumerator TournerPagePlusAnim(int currentPage_)
    {
        float speedTournePage = livreManagement_script.speedTournePage;
        posXStart = pageRectTransform.anchoredPosition.x;
        this.transform.Find("ButtonPagePlus").gameObject.SetActive(false);
        livreManagement_script.inTransitionPage = true;
        this.transform.parent.GetChild(currentPage_ - 1).transform.Find("ButtonPagePlus").gameObject.SetActive(false);

        bool boutonMoinsNewPageActif = false;
        if(this.transform.parent.GetChild(currentPage_ - 1).transform.Find("ButtonPageMoins").gameObject.activeSelf) {
            boutonMoinsNewPageActif = true;
            this.transform.parent.GetChild(currentPage_ - 1).transform.Find("ButtonPageMoins").gameObject.SetActive(false);
        }

        bool boutonMoinsActif = false;
        if (this.transform.Find("ButtonPageMoins").gameObject.activeSelf)
        {
            boutonMoinsActif = true;
            this.transform.Find("ButtonPageMoins").gameObject.SetActive(false);
        }

        if (isPanelSonIndep)
        {
            panelSonIndep.SetActive(false);
        }

        if(this.transform.parent.GetChild(currentPage_ - 1).GetComponent<PageLivreScript>().isPanelSonIndep)
        {
            this.transform.parent.GetChild(currentPage_ - 1).GetComponent<PageLivreScript>().panelSonIndep.SetActive(false);
        }

        if (this.transform.parent.GetChild(currentPage_ - 1).GetComponent<PageLivreScript>().ambiancePage != ambiancePage)
        {
            StartCoroutine(TransitionDeuxAmbiances(currentPage_, currentPage_ - 1));
        } else
        {
            canDesactive = true;
        }


        if (!livreManagement_script.animSensReverse)
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
        this.transform.parent.GetChild(currentPage_ - 1).transform.Find("ButtonPagePlus").gameObject.SetActive(true);

        if (boutonMoinsNewPageActif)
        {
            this.transform.parent.GetChild(currentPage_ - 1).transform.Find("ButtonPageMoins").gameObject.SetActive(true);
        }
        if (boutonMoinsActif)
        {
            this.transform.Find("ButtonPageMoins").gameObject.SetActive(true);
        }
        this.transform.Find("ButtonPagePlus").gameObject.SetActive(true);

        if (isPanelSonIndep)
        {
            panelSonIndep.SetActive(true);
        }

        if (this.transform.parent.GetChild(currentPage_ - 1).GetComponent<PageLivreScript>().isPanelSonIndep)
        {
            this.transform.parent.GetChild(currentPage_ - 1).GetComponent<PageLivreScript>().panelSonIndep.SetActive(true);
        }


        /*while (!canDesactive)
        {
            yield return new WaitForSeconds(LivreManagement.deltaTime);
            if (canDesactive)
            {
                break;
            }
        }*/
        this.gameObject.SetActive(false);
        livreManagement_script.inTransitionPage = false;
        pageRectTransform.anchoredPosition = new Vector2(posXStart, pageRectTransform.anchoredPosition.y);
        canDesactive = false;
        yield return null;
    }


    //Fade du changement de son
    IEnumerator TransitionDeuxAmbiances(int currentPage_,int newPage)
    {
        //On passe la musique en cours sur l'audio temporaire et on diminue progressivement le volume
        livreManagement_script.ambianceGeneraleAudioTemp.clip = this.transform.parent.GetChild(currentPage_).GetComponent<PageLivreScript>().ambiancePage;
        livreManagement_script.ambianceGeneraleAudioTemp.time = livreManagement_script.ambianceGenarale_AudioSource.time;
        livreManagement_script.ambianceGeneraleAudioTemp.Play();
        StartCoroutine(livreManagement_script.FadeMoins_Volume(livreManagement_script.ambianceGeneraleAudioTemp, livreManagement_script.ambianceGenarale_AudioSource.volume, livreManagement_script.timeFadeChangePage, 0));


        //On donne la nouvelle musique à l'audio principale et augmente progressivement le volume
        livreManagement_script.ambianceGenarale_AudioSource.clip = this.transform.parent.GetChild(newPage).GetComponent<PageLivreScript>().ambiancePage;
        livreManagement_script.ambianceGenarale_AudioSource.Play();
        StartCoroutine(livreManagement_script.FadePlus_Volume(livreManagement_script.ambianceGenarale_AudioSource,0, livreManagement_script.timeFadeChangePage, newPage));


        while (livreManagement_script.ambianceGeneraleAudioTemp.volume > 0)
        {
            yield return new WaitForSeconds(LivreManagement.deltaTime);
            if(livreManagement_script.ambianceGeneraleAudioTemp.volume <= 0)
            {
                livreManagement_script.ambianceGeneraleAudioTemp.volume = 0;
                canDesactive = true;
                break;
            }
        }
        yield return null;

    }




    // Tourner Page-
    int senstourne;
    public void TournerPageMoins()
    {
        senstourne = -1;
        int currentPage = this.transform.GetSiblingIndex();
        if (currentPage < livreManagement_script.nbPagesLivre - 1)
        {
            if (this.transform.parent.GetChild(currentPage + 1).name == this.transform.name)
            {

                livreManagement_script.currentPage = currentPage + 1;
                livreManagement_script.dataCharged = true;
                livreManagement_script.SaveLivre();
                StartCoroutine(TournerPageMoinsAnim(currentPage));
            }
        }

        camTransform.GetComponent<Fonctions_Utiles>().DeselectEventSystem(livreManagement_script.monEvent);
    }



    //Animation Tourner Page -
    IEnumerator TournerPageMoinsAnim(int currentPage_)
    {
        livreManagement_script.inTransitionPage = true;
        float speedTournePage = livreManagement_script.speedTournePage;
        int numPage = currentPage_ + 1;
        if(currentPage_ == 0 && senstourne == 1)
        {
            numPage = livreManagement_script.nbPagesLivre - 1;
        }


        posXStart = pageRectTransform.anchoredPosition.x;
        this.transform.parent.GetChild(numPage).gameObject.SetActive(true);

        if (!livreManagement_script.animSensReverse)
        {
            //Anim Sens 1
            this.transform.parent.GetChild(numPage).GetComponent<PageLivreScript>().pageRectTransform.anchoredPosition = new Vector2(Screen.width + posXStart + 10, pageRectTransform.anchoredPosition.y);
        }
        else
        {
            //Anim Sens 2
            this.transform.parent.GetChild(numPage).GetComponent<PageLivreScript>().pageRectTransform.anchoredPosition = new Vector2(-posXStart - 10, pageRectTransform.anchoredPosition.y);
        }

        this.transform.Find("ButtonPageMoins").gameObject.SetActive(false);
        this.transform.Find("ButtonPagePlus").gameObject.SetActive(false);
        if (isPanelSonIndep)
        {
            panelSonIndep.SetActive(false);
        }

        bool boutonMoinsNewPageActif = false;
        if (this.transform.parent.GetChild(numPage).transform.Find("ButtonPageMoins").gameObject.activeSelf)
        {
            boutonMoinsNewPageActif = true;
            this.transform.parent.GetChild(numPage).transform.Find("ButtonPageMoins").gameObject.SetActive(false);
        }
        this.transform.parent.GetChild(numPage).transform.Find("ButtonPagePlus").gameObject.SetActive(false);
        if (this.transform.parent.GetChild(numPage).GetComponent<PageLivreScript>().isPanelSonIndep)
        {
            this.transform.parent.GetChild(numPage).GetComponent<PageLivreScript>().panelSonIndep.SetActive(false);
        }


        if (this.transform.parent.GetChild(numPage).GetComponent<PageLivreScript>().ambiancePage != ambiancePage)
        {
            StartCoroutine(TransitionDeuxAmbiances(currentPage_, numPage));
        } else
        {
            canDesactive = true;
        }


        if (!livreManagement_script.animSensReverse)
        {
            //Anim Sens 1
            while (this.transform.parent.GetChild(numPage).GetComponent<PageLivreScript>().pageRectTransform.anchoredPosition.x > posXStart)
            {
                this.transform.parent.GetChild(numPage).GetComponent<PageLivreScript>().pageRectTransform.anchoredPosition = new Vector2(this.transform.parent.GetChild(numPage).GetComponent<PageLivreScript>().pageRectTransform.anchoredPosition.x - LivreManagement.deltaTime * speedTournePage, this.transform.parent.GetChild(numPage).GetComponent<PageLivreScript>().pageRectTransform.anchoredPosition.y);
                yield return new WaitForSeconds(LivreManagement.deltaTime);
                if(this.transform.parent.GetChild(numPage).GetComponent<PageLivreScript>().pageRectTransform.anchoredPosition.x <= posXStart)
                {
                    this.transform.parent.GetChild(numPage).GetComponent<PageLivreScript>().pageRectTransform.anchoredPosition = new Vector2(posXStart, this.transform.parent.GetChild(numPage).GetComponent<PageLivreScript>().pageRectTransform.anchoredPosition.y);
                    break;
                }
            }
        }
        else
        {
            //Anim Sens 2
            while (this.transform.parent.GetChild(numPage).GetComponent<PageLivreScript>().pageRectTransform.anchoredPosition.x < posXStart)
            {
                this.transform.parent.GetChild(numPage).GetComponent<PageLivreScript>().pageRectTransform.anchoredPosition = new Vector2(this.transform.parent.GetChild(numPage).GetComponent<PageLivreScript>().pageRectTransform.anchoredPosition.x + LivreManagement.deltaTime * speedTournePage, this.transform.parent.GetChild(numPage).GetComponent<PageLivreScript>().pageRectTransform.anchoredPosition.y);
                yield return new WaitForSeconds(LivreManagement.deltaTime);
                if (this.transform.parent.GetChild(numPage).GetComponent<PageLivreScript>().pageRectTransform.anchoredPosition.x >= posXStart)
                {
                    this.transform.parent.GetChild(numPage).GetComponent<PageLivreScript>().pageRectTransform.anchoredPosition = new Vector2(posXStart, this.transform.parent.GetChild(numPage).GetComponent<PageLivreScript>().pageRectTransform.anchoredPosition.y);
                    break;
                }
            }
        }

        this.transform.Find("ButtonPageMoins").gameObject.SetActive(true);
        this.transform.Find("ButtonPagePlus").gameObject.SetActive(true);
        if (isPanelSonIndep)
        {
            panelSonIndep.SetActive(true);
        }



        if (boutonMoinsNewPageActif)
        {
            this.transform.parent.GetChild(numPage).transform.Find("ButtonPageMoins").gameObject.SetActive(true);
        }
        this.transform.parent.GetChild(numPage).transform.Find("ButtonPagePlus").gameObject.SetActive(true);
        if (this.transform.parent.GetChild(numPage).GetComponent<PageLivreScript>().isPanelSonIndep)
        {
            this.transform.parent.GetChild(numPage).GetComponent<PageLivreScript>().panelSonIndep.SetActive(true);
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
        livreManagement_script.inTransitionPage = false;
        pageRectTransform.anchoredPosition = new Vector2(posXStart, pageRectTransform.anchoredPosition.y);
        canDesactive = false;
        yield return null;
    }


    //Sons Indépendants Interactability 
    public void SonsIndependantsInteractability()
    {
        if (isPanelSonIndep)
        {
            if (!livreManagement_script.isSonPause && !sonsIndep[0].GetComponent<Button>().interactable)
            {
                for(int ii = 0; ii < sonsIndep.Length; ii++)
                {
                    sonsIndep[ii].GetComponent<Button>().interactable = true;
                }
            } else if(livreManagement_script.isSonPause && sonsIndep[0].GetComponent<Button>().interactable)
            {
                for (int ii = 0; ii < sonsIndep.Length; ii++)
                {
                    sonsIndep[ii].GetComponent<Button>().interactable = false;
                }
            }
        }
    }


    // Tourne Page Interactability
    public void TournePageInteractability()
    {
        if (pageP_GO.activeSelf)
        {
            if(!livreManagement_script.isSonPause && !pageP_GO.GetComponent<Button>().interactable)
            {
                pageP_GO.GetComponent<Button>().interactable = true;
            } else if (livreManagement_script.isSonPause && pageP_GO.GetComponent<Button>().interactable)
            {
                pageP_GO.GetComponent<Button>().interactable = false;
            }
        }

        if (pageM_GO.activeSelf)
        {
            if (!livreManagement_script.isSonPause && !pageM_GO.GetComponent<Button>().interactable)
            {
                pageM_GO.GetComponent<Button>().interactable = true;
            }
            else if (livreManagement_script.isSonPause && pageM_GO.GetComponent<Button>().interactable)
            {
                pageM_GO.GetComponent<Button>().interactable = false;
            }
        }
    }

}
