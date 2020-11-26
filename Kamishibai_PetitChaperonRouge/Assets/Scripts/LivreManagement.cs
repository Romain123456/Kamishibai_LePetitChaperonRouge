using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;

public class LivreManagement : MonoBehaviour
{
    static public float deltaTime;
    public bool appliDemo;

    //Changer nom du livre pour chaque livre !
    static public string nomSave = "/chaperonrouge.ksh";

    public Button livreButton;
    public Button livrePanelRecommOUI;
    public Button livrePanelRecommNON;
    private Fonctions_Utiles scriptFonction;
    public GameObject panelLivre;
    public GameObject emptyBoutonsReglages;
    public GameObject panelMenuPrincipal;
    public GameObject panelLangueDepart;
    public GameObject panelPopUpEnceinte;

    public Sprite tickNoSprite;
    public Sprite tickYesSprite;
    public bool isDontAskAgain;
    public Image DontAskAgainImage;


    //Prefab PageLivre
    public GameObject pageLivrePrefab;
    //Nombre de pages du livre
    public int nbPagesLivre;
    [HideInInspector] public GameObject[] pageLivreArray;

    //AudioSource de l'ambiance Générale
    public AudioSource ambianceGenarale_AudioSource;
    public AudioSource ambianceGeneraleAudioTemp;


    //AudioSource des sons indépendant
    public AudioSource sonsIndependants_AudioSource;
    public AudioClip ambianceMenuPrincipal;
    public AudioClip[] ambianceLivre;

    //Image de la page du livre
    public Sprite[] imagePage;
    public float speedTournePage;

    //Liste des sons/sprites du livre
    public AudioClip[] listeSonsLivre;
    public Sprite[] listeSpriteLivre;               //Mettre même indice de son et de sprite correspondant

    //Génération Son Indépendant
    private int compteurCodeSonIndep = 0;

    //Animation PageTourne
    //public GameObject imagePageTourne;

    //Retour Menu
    [HideInInspector] public GameObject contenuOuvert;


    //Présence d'un contenu pédagogique ?
    public bool isContenuPedagogique;
    public GameObject contenuPedagogiquePanel;
    public GameObject contenuPedagogiqueGO;
    public GameObject livreGO;


    //Crédits
    public Button creditsBouton;
    public GameObject creditPanel;


    // Animations page livre
    public bool animSensReverse;
    public float timeFadeChangePage;
    public float timeFadeOpenLivre;




    //Lecture Automatique
    public bool isLectureAuto;
    public GameObject lectureAutoGO;
    public GameObject panelLectureAuto;
    [HideInInspector] public GameObject[] pageLectAutoArray;
    public GameObject pageLectAutoPrefab;
    public Button lectAutoRecommOUI;
    public Button lectAutoRecommNON;
    public AudioClip[] pistesLectAutoFR;
    public AudioClip[] pistesLectAutoEN;
    public AudioClip[] pistesLectAutoGe;
    public float tempsWaitLecture;


    //EventSystem
    [HideInInspector] public EventSystem monEvent;
    [HideInInspector] public PointerEventData monEventData;

    //Ecran noir
    public GameObject ecranNoir;
    [HideInInspector] public Image ecranNoirImage;


    //Panel Contenu Pédagogique
    public GameObject panelMemory;
    public GameObject panelPuzzle;
    public GameObject panelMeliMelo;


    // Fade de l'écran noir
    void Awake()
    {
        deltaTime = 1/30;
        ecranNoirImage = ecranNoir.GetComponent<Image>();
        StartCoroutine(FadeMoinsEcranNoir(ecranNoirImage, 0.5f));

        QualitySettings.vSyncCount = 0;  // VSync must be disabled
        Application.targetFrameRate = 30;

        ecranNoir.transform.SetAsLastSibling();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    public IEnumerator FadeMoinsEcranNoir(Image fadeMoinsImage,float speedFade)
    {
        float alpha = fadeMoinsImage.color.a;
        yield return new WaitForSeconds(1);
        //Desactivation du popup des enceintes
        panelPopUpEnceinte.SetActive(false);

        while (fadeMoinsImage.color.a > 0)
        {
            alpha -= deltaTime/speedFade;
            fadeMoinsImage.color = new Color(fadeMoinsImage.color.r, fadeMoinsImage.color.g, fadeMoinsImage.color.b, alpha);
            yield return new WaitForSeconds(deltaTime);
            if (fadeMoinsImage.color.a <= 0)
            {
                fadeMoinsImage.color = new Color(fadeMoinsImage.color.r, fadeMoinsImage.color.g, fadeMoinsImage.color.b, 0);
                fadeMoinsImage.transform.gameObject.SetActive(false);
                break;
            }
        }

    }

    


    // Start is called before the first frame update
    void Start()
    {
        monEvent = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        
        // Accueil : Accès vers le livre
        scriptFonction = this.GetComponent<Fonctions_Utiles>();
        livreButton.onClick.AddListener(delegate { scriptFonction.LivreBouton(panelLivre); });
        livrePanelRecommOUI.onClick.AddListener(delegate { scriptFonction.LivrePanelRecommencerOUI(panelLivre); });
        livrePanelRecommNON.onClick.AddListener(delegate { scriptFonction.LivrePanelRecommencerNON(panelLivre); });

        lectureAutoGO.GetComponent<Button>().onClick.AddListener(delegate { scriptFonction.LectureAutoBouton(panelLectureAuto); });
        lectAutoRecommOUI.onClick.AddListener(delegate { scriptFonction.LectureAutoPanelRecommencerOUI(panelLectureAuto); });
        lectAutoRecommNON.onClick.AddListener(delegate { scriptFonction.LectureAutoPanelRecommencerNON(panelLectureAuto); });

        //Contenu Pédagogique et Lecture Auto
        CheckContenuPedagogiqueLectureAuto();

        // Instantiation des pages du livre
        InstanciationLivre();

        // Instantiation des pages de lecture du livre auto
        if (isLectureAuto)
        {
            InstanciationLectureAuto();
        }

        ambianceGenarale_AudioSource.clip = ambianceMenuPrincipal;
        ambianceGenarale_AudioSource.Play();


        //Chargement des données
        //Slider value et son = donnée chargée !
        
        LoadLivre();
        PanelLangueGeneration();
        // Button Langue Tableau
        /*buttonLangues = new Button[buttonLangueEmpty.transform.childCount];
        for (int ii=0; ii < buttonLangueEmpty.transform.childCount; ii++)
        {
            buttonLangues[ii] = buttonLangueEmpty.transform.GetChild(ii).GetComponent<Button>();
        }*/

        StartCoroutine(DesactiveCreditContenuPedag());
    }


    public void InstanciationLivre()
    {
        if (nbPagesLivre > 0)
        {
            //Attribution Méthode Button Panel Pause
            //buttonSonPlus.onClick.AddListener(delegate { PanelPauseMonteSon(); });
            //buttonSonMoins.onClick.AddListener(delegate { PanelPauseBaisseSon(); });
            //buttonSonMute.onClick.AddListener(delegate { PanelPauseSonMute(); });
            buttonSonPause.onClick.AddListener(delegate { PanelPauseSonPause(); });

            pageLivreArray = new GameObject[nbPagesLivre];
            for (int ii = 0; ii < nbPagesLivre; ii++)
            {
                pageLivreArray[ii] = (GameObject)Instantiate(pageLivrePrefab, panelLivre.transform);
                pageLivreArray[ii].GetComponent<ScalePositionUI>().ReSizeScaleUI();
                pageLivreArray[ii].GetComponent<PageLivreScript>().isPanelSonIndep = livre.isPanelSonIndep[ii];
                //pageLivreArray[ii].transform.Find("TextPage").GetComponent<Text>().text = this.GetComponent<LangageScript>().textesPagesLivres[ii];

                //Affectation de la fonctionalité pause au texte
                pageLivreArray[ii].transform.Find("TextPage").GetComponent<Button>().onClick.AddListener(delegate { PanelPauseSonPause(); });


                //Ambiance musique 
                pageLivreArray[ii].GetComponent<PageLivreScript>().ambiancePage = ambianceLivre[ii];

                //Image de la page
                pageLivreArray[ii].transform.Find("ImagePageLiseret").Find("ImagePage").GetComponent<Image>().sprite = imagePage[ii];

                //Sons indés
                if (pageLivreArray[ii].GetComponent<PageLivreScript>().isPanelSonIndep)
                {
                    pageLivreArray[ii].GetComponent<PageLivreScript>().nbSonsIndep = livre.nbSonsIndep[ii];
                    if (pageLivreArray[ii].GetComponent<PageLivreScript>().nbSonsIndep > 0)
                    {
                        pageLivreArray[ii].GetComponent<PageLivreScript>().sonsIndep = new GameObject[pageLivreArray[ii].GetComponent<PageLivreScript>().nbSonsIndep];
                        for (int jj = 0; jj < pageLivreArray[ii].GetComponent<PageLivreScript>().nbSonsIndep; jj++)
                        {
                            if (ii == (int)livre.codesSonIndep[compteurCodeSonIndep].x && jj == (int)livre.codesSonIndep[compteurCodeSonIndep].y)
                            {
                                pageLivreArray[ii].GetComponent<PageLivreScript>().sonsIndep[jj] = (GameObject)Instantiate(pageLivreArray[ii].GetComponent<PageLivreScript>().sonIndepPrefab, pageLivreArray[ii].GetComponent<PageLivreScript>().panelSonIndep.transform);
                                if(pageLivreArray[ii].GetComponent<PageLivreScript>().nbSonsIndep == 1)
                                {
                                    pageLivreArray[ii].GetComponent<PageLivreScript>().sonsIndep[jj].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0, -9.0f, 0);
                                } else if(pageLivreArray[ii].GetComponent<PageLivreScript>().nbSonsIndep > 1)
                                {
                                    pageLivreArray[ii].GetComponent<PageLivreScript>().sonsIndep[jj].GetComponent<RectTransform>().anchoredPosition3D = new Vector3(-35.0f + (jj * 70.0f), -9.0f, 0);
                                }
                                pageLivreArray[ii].GetComponent<PageLivreScript>().sonsIndep[jj].GetComponent<BoutonSonIndepScript>().sonIndepClip = listeSonsLivre[(int)livre.codesSonIndep[compteurCodeSonIndep].z];
                                pageLivreArray[ii].GetComponent<PageLivreScript>().sonsIndep[jj].GetComponent<Image>().sprite = listeSpriteLivre[(int)livre.codesSonIndep[compteurCodeSonIndep].z];
                                pageLivreArray[ii].GetComponent<PageLivreScript>().sonsIndep[jj].transform.GetChild(0).GetComponent<Text>().text = (livre.codesSonIndep[compteurCodeSonIndep].y+1).ToString();
                                compteurCodeSonIndep++;
                                
                            }
                            else
                            {
                                Debug.Log("Error pour la page : " + ii + " et le son : " + jj + ". Corrige codeSonIndep GROS NAZE !!");
                            }
                        }
                    }
                }
                else
                {
                    pageLivreArray[ii].GetComponent<PageLivreScript>().nbSonsIndep = 0;
                }



                if (ii == 0)
                {
                    pageLivreArray[ii].SetActive(true);
                }
                else
                {
                    pageLivreArray[ii].SetActive(false);
                }
                pageLivreArray[ii].transform.SetAsFirstSibling();
            }


            /*imagePageTourne.transform.SetAsLastSibling();
            pageTourneAnimator = imagePageTourne.GetComponent<Animator>();

            imagePageTourne.SetActive(false);*/
            panelPause.transform.SetAsLastSibling();
            panelPopUpEnceinte.transform.SetAsLastSibling();
            ecranNoir.transform.SetAsLastSibling();
        }

    }

    IEnumerator DesactiveCreditContenuPedag()
    {
        yield return new WaitForSeconds(1);
        creditPanel.SetActive(false);
        contenuPedagogiquePanel.SetActive(false);
        panelMemory.SetActive(false);
        panelPuzzle.SetActive(false);
    }


    public void InstanciationLectureAuto()
    {
        if (nbPagesLivre > 0)
        {
            pageLectAutoArray = new GameObject[nbPagesLivre];

            for (int ii = 0; ii < nbPagesLivre; ii++)
            {
                pageLectAutoArray[ii] = (GameObject)Instantiate(pageLectAutoPrefab, panelLectureAuto.transform);
                pageLectAutoArray[ii].GetComponent<ScalePositionUI>().ReSizeScaleUI();

                //Ambiance musique 
                pageLectAutoArray[ii].GetComponent<PageLectureAutoScript>().ambiancePage = ambianceLivre[ii];

                //Image de la page
                pageLectAutoArray[ii].transform.Find("ImagePage").GetComponent<Image>().sprite = imagePage[ii];

                //Affectation de la fonctionalité pause à l'image
                pageLectAutoArray[ii].transform.Find("ImagePage").GetComponent<Button>().onClick.AddListener(delegate { CachePanelPause(); });

                //Piste Audio
                pageLectAutoArray[ii].GetComponent<PageLectureAutoScript>().pisteEN = pistesLectAutoEN[ii];
                pageLectAutoArray[ii].GetComponent<PageLectureAutoScript>().pisteFR = pistesLectAutoFR[ii];
                pageLectAutoArray[ii].GetComponent<PageLectureAutoScript>().pisteGe = pistesLectAutoGe[ii];

                if (ii == 0)
                {
                    pageLectAutoArray[ii].SetActive(true);
                }
                else
                {
                    pageLectAutoArray[ii].SetActive(false);
                }
                pageLectAutoArray[ii].transform.SetAsFirstSibling();
            }
        }

    }

    public void CachePanelPause()
    {
        if (panelPause.GetComponent<RectTransform>().localScale.x > 0)
        {
            panelPause.GetComponent<RectTransform>().localScale = new Vector2(0, 0);
        } else
        {
            panelPause.GetComponent<RectTransform>().localScale = new Vector2(1, 1);
            StartCoroutine(CachePanelPauseCoroutine());
        }
    }

    public IEnumerator CachePanelPauseCoroutine()
    {
        float chrono = 0;
        monEventData = EventSystem.current.gameObject.GetComponent<StandAloneInputModuleCustom>().GetLastPointerEventDataPublic(-1);
        while (chrono < 3)
        {
            yield return new WaitForSeconds(deltaTime);
            chrono += deltaTime;
            if (isSonPause || monEvent.currentSelectedGameObject != null && ( (monEvent.currentSelectedGameObject.name == "SliderSonIndependant" && monEventData.eligibleForClick) || (monEvent.currentSelectedGameObject.name == "SliderSonAmbiance" && monEventData.eligibleForClick) ))
            {
                chrono = 0;
            }
            Debug.Log(chrono);
            if(panelPause.GetComponent<RectTransform>().localScale.x == 0 || chrono >= 3 )
            {
                break;
            }
        }
        panelPause.GetComponent<RectTransform>().localScale = new Vector2(0, 0);
    }





    public void SaveLivre()
    {
        SaveSystem.SaveLivre(this);
    }


    [HideInInspector] public bool dataCharged;
    public void LoadLivre()
    {
        LivreData data = SaveSystem.LoadLivre();
        if (data != null)
        {
            panelLangueDepart.SetActive(true);
            sliderSonAmbiance.value = data.volSonAmbiance;

            sliderSonIndependant.value = data.volSonsIndep;

            currentPage = data.currentPage;
            currentPageLectAuto = data.currentPageLectAuto;
            langue = data.langue;
            dataCharged = true;
            panelPause.SetActive(true);
            panelLangueDepart.SetActive(false);
            isDontAskAgain = data.isDontAskAgain;
            //Destroy(panelLangueDepart);
        } else
        {
            panelLangueDepart.SetActive(true);
            panelPause.SetActive(false);
        }
        //ambianceGenarale_AudioSource.volume = sliderSonAmbiance.value;
        StartCoroutine(FadePlus_Volume(ambianceGenarale_AudioSource,0,2, sliderSonAmbiance.value));
        sonsIndependants_AudioSource.volume = sliderSonIndependant.value;

    }


    public IEnumerator FadePlus_Volume(AudioSource monAudioSource,float volBase,float tpsFade,float volTarget)
    {
        monAudioSource.volume = volBase;
        float stepVol = Time.fixedDeltaTime * volTarget / tpsFade;

        while(monAudioSource.volume < volTarget)
        {
            monAudioSource.volume += stepVol;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            if(monAudioSource.volume >= volTarget)
            {
                monAudioSource.volume = volTarget;
                break;
            }
        }
    }


    public IEnumerator FadeMoins_Volume(AudioSource monAudioSource, float volBase, float tpsFade, float volTarget)
    {
        monAudioSource.volume = volBase;
        float stepVol = Time.fixedDeltaTime * volBase / tpsFade;

        while (monAudioSource.volume > volTarget)
        {
            monAudioSource.volume -= stepVol;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            if (monAudioSource.volume <= volTarget)
            {
                monAudioSource.volume = volTarget;
                break;
            }
        }
    }



    // Update is called once per frame
    void Update()
    {
        deltaTime = Time.smoothDeltaTime;

        //ControleAnimationPageTourne();
        SonAmbianceInteractability();
        SonIndependantInteractability();
        EvolutionVariablesSaveSystem();
        BoutonsPanelPauseInteractability();

        //Sécurité panel pause interactability
        PanelPauseSecurity();
    }


    private int nbPageLivreActive = 0;
    void PanelPauseSecurity()
    {
        if (panelLivre.activeSelf)
        {
            nbPageLivreActive = 0;
            for(int ii = 0; ii < panelLivre.transform.childCount; ii++)
            {
                if (panelLivre.transform.GetChild(ii).gameObject.activeSelf)
                {
                    nbPageLivreActive++;
                }
            }
            
            if(nbPageLivreActive==1 && !buttonSonPause.interactable)
            {
                inTransitionPage = false;
                buttonSonPause.interactable = true;
            }
        }

        if (panelLectureAuto.activeSelf)
        {
            for (int ii = 0; ii < panelLectureAuto.transform.childCount; ii++)
            {
                if (panelLectureAuto.transform.GetChild(ii).gameObject.activeSelf)
                {
                    nbPageLivreActive++;
                }
            }

            if (nbPageLivreActive == 1 && !buttonSonPause.interactable)
            {
                inTransitionPage = false;
                buttonSonPause.interactable = true;
            }
        }
    }


    // Présence d'un contenu pédagogique ?
    private int nbButtonOtherContents = 0;
    void CheckContenuPedagogiqueLectureAuto()
    {
        if (isContenuPedagogique)
        {
            nbButtonOtherContents++;
        }
        if (isLectureAuto)
        {
            nbButtonOtherContents++;
        }

        if(nbButtonOtherContents == 0)
        {
            lectureAutoGO.SetActive(false);
            contenuPedagogiqueGO.SetActive(false);
            livreGO.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -20);
            livreGO.GetComponent<ScalePositionUI>().positionBase.x = 502;
        }

        if(nbButtonOtherContents == 1)
        {
            livreGO.GetComponent<ScalePositionUI>().positionBase.x = 312;
            livreGO.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(155, -20);
            if(isContenuPedagogique && !isLectureAuto)
            {
                contenuPedagogiqueGO.SetActive(true);
                contenuPedagogiqueGO.GetComponent<ScalePositionUI>().positionBase.x = 692;
                lectureAutoGO.SetActive(false);
            } else if(!isContenuPedagogique && isLectureAuto)
            {
                contenuPedagogiqueGO.SetActive(false);
                lectureAutoGO.SetActive(true);
                lectureAutoGO.GetComponent<ScalePositionUI>().positionBase.x = 692;
                lectureAutoGO.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-225, -27);
            }
        }

        if(nbButtonOtherContents == 2)
        {
            livreGO.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(252, -20);
            livreGO.GetComponent<ScalePositionUI>().positionBase.x = 240;

            contenuPedagogiqueGO.SetActive(true);
            contenuPedagogiqueGO.GetComponent<ScalePositionUI>().positionBase.x = 502;

            lectureAutoGO.SetActive(true);
            lectureAutoGO.GetComponent<ScalePositionUI>().positionBase.x = 742;
            lectureAutoGO.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-252, -20);
        }
    }


    //Bouton Panel Pause
    //public Button buttonSonPlus;
    //public Button buttonSonMoins;
    //public Button buttonSonMute;
    //public Sprite spriteSonOk;
    //public Sprite spriteSonOff;
    private float currentSound;
    //private bool isMute = false;
    public Button buttonSonPause;
    public Sprite spriteSonPlay;
    public Sprite spriteSonPause;
    [HideInInspector] public bool isSonPause = false;
    public GameObject panelPause;
   

    //Slider SonAmbiance
    public Slider sliderSonAmbiance;

    public void SonAmbianceGestion()
    {
        ambianceGenarale_AudioSource.volume = sliderSonAmbiance.value;
        //ambianceGeneraleAudioTemp.volume = sliderSonAmbiance.value;
    }

    public void SonAmbianceInteractability()
    {
        if (!isSonPause && !sliderSonAmbiance.interactable)
        {
            sliderSonAmbiance.interactable = true;
        } else if(isSonPause && sliderSonAmbiance.interactable)
        {
            sliderSonAmbiance.interactable = false;
        }
    }



    // Retour Menu Principal + Langue
    public Button buttonMenuPrincipal;
    public GameObject buttonChoixLangue;
    //public GameObject buttonReInitParam;
    [HideInInspector] public bool inTransitionPage;
    public void BoutonsPanelPauseInteractability()
    {
        if (contenuOuvert != null && contenuOuvert != creditPanel)
        {
            /*if (buttonReInitParam.activeSelf)
            {
                buttonReInitParam.SetActive(false);
            }*/

            if (buttonChoixLangue.activeSelf)
            {
                buttonChoixLangue.SetActive(false);
            }

            if (!inTransitionPage)
            {
                buttonSonPause.interactable = true;
            } else
            {
                buttonSonPause.interactable = false;
            }
            if (!isSonPause && buttonMenuPrincipal.interactable)
            {
                buttonMenuPrincipal.interactable = false;
            }

            else if (isSonPause && !buttonMenuPrincipal.interactable)
            {
                buttonMenuPrincipal.interactable = true;
            }
        } else if(contenuOuvert == null || contenuOuvert == creditPanel)
        {
            buttonMenuPrincipal.interactable = false;
            buttonSonPause.interactable = false;
            /*if (!buttonReInitParam.activeSelf)
            {
                buttonReInitParam.SetActive(true);
            }*/
            if (!buttonChoixLangue.activeSelf && nbLanguesDispo > 1 && !livreButton.transform.GetChild(1).gameObject.activeSelf && !lectureAutoGO.transform.GetChild(1).gameObject.activeSelf)
            {
                buttonChoixLangue.SetActive(true);
            }
            if(livreButton.transform.GetChild(1).gameObject.activeSelf || lectureAutoGO.transform.GetChild(1).gameObject.activeSelf)
            {
                buttonChoixLangue.SetActive(false);
            }
        }
    }



    //Slider SonIndépendants
    public Slider sliderSonIndependant;

    public void SonIndependantGestion()
    {
        sonsIndependants_AudioSource.volume = sliderSonIndependant.value;
    }

    public void SonIndependantInteractability()
    {
        if (!isSonPause && !sliderSonIndependant.interactable)
        {
            sliderSonIndependant.interactable = true;
        }
        else if (isSonPause && sliderSonIndependant.interactable)
        {
            sliderSonIndependant.interactable = false;
        }
    }

    public void PanelPauseSonPause()
    {
        if (!isSonPause)
        {
            isSonPause = true;
            ambianceGenarale_AudioSource.Pause();
            ambianceGeneraleAudioTemp.Pause();
            sonsIndependants_AudioSource.Pause();
            buttonSonPause.gameObject.GetComponent<Image>().sprite = spriteSonPlay;
        }
        else if (isSonPause)
        {
            isSonPause = false;
            ambianceGenarale_AudioSource.UnPause();
            ambianceGeneraleAudioTemp.UnPause();
            sonsIndependants_AudioSource.UnPause();
            buttonSonPause.gameObject.GetComponent<Image>().sprite = spriteSonPause;
        }

        scriptFonction.DeselectEventSystem(monEvent);
    }





    // Variables pour le système de sauvegarde
    [HideInInspector] public float volSonAmbiance;
    [HideInInspector] public float volSonsIndep;
    [HideInInspector] public int currentPage;
    [HideInInspector] public string langue;
    [HideInInspector] public int currentPageLectAuto;

    public void EvolutionVariablesSaveSystem()
    {
        volSonAmbiance = ambianceGenarale_AudioSource.volume;
        volSonsIndep = sonsIndependants_AudioSource.volume;
    }



    // Panel Langue Génération
    [HideInInspector] public int nbLanguesDispo;
    public void PanelLangueGeneration()
    {
        //On compte le nombre de langue disponibles
        if (livre.isFrDispo)
        {
            nbLanguesDispo++;
        }
        if (livre.isEnDispo)
        {
            nbLanguesDispo++;
        }
        if (livre.isGeDispo)
        {
            nbLanguesDispo++;
        }

        // Création du panel en fonction du nombre de langues dispo
        if (nbLanguesDispo == 1)
        {
            LangueDispo1();
        } else if(nbLanguesDispo == 2)
        {
            LangueDispo2();
        } else if(nbLanguesDispo == 3)
        {
            LangueDispo3();
        }

    }


    //Nomenclature Child de PanelChoix Langue : 0=FR, 1=EN, 2=GE
    //1 langue dispo
    void LangueDispo1()
    {
        panelLangueDepart.SetActive(false);
        if (livre.isFrDispo)
        {
            langue = "FR";
        } else if (livre.isEnDispo)
        {
            langue = "EN";
        } else if (livre.isGeDispo)
        {
            langue = "GR";
        }
        panelPause.SetActive(true);
        buttonChoixLangue.SetActive(false);
    }
    //2 langues dispos
    void LangueDispo2()
    {
        if (livre.isFrDispo)
        {
            panelLangueDepart.transform.GetChild(0).gameObject.SetActive(true);
            panelLangueDepart.transform.GetChild(0).GetComponent<RectTransform>().anchoredPosition = new Vector2(-170,0);
        }
        if (livre.isEnDispo)
        {
            panelLangueDepart.transform.GetChild(1).gameObject.SetActive(true);
            if(livre.isFrDispo && !livre.isGeDispo)
            {
                panelLangueDepart.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(170, 0);
            } else if(!livre.isFrDispo && livre.isGeDispo)
            {
                panelLangueDepart.transform.GetChild(1).GetComponent<RectTransform>().anchoredPosition = new Vector2(-170, 0);
            }
        }
        if (livre.isGeDispo)
        {
            panelLangueDepart.transform.GetChild(2).gameObject.SetActive(true);
            panelLangueDepart.transform.GetChild(2).GetComponent<RectTransform>().anchoredPosition = new Vector2(170, 0);
        }
    }
    //3 langues dispos
    void LangueDispo3()
    {
        for(int ii = 0; ii < panelLangueDepart.transform.childCount; ii++)
        {
            panelLangueDepart.transform.GetChild(ii).gameObject.SetActive(true);
            panelLangueDepart.transform.GetChild(ii).GetComponent<RectTransform>().anchoredPosition = new Vector2(-277+ii*277, 0);
        }
    }






    public void EraseSaveData()
    {
        string path = Application.persistentDataPath + LivreManagement.nomSave;

        if (File.Exists(path))
        {
            File.SetAttributes(path, FileAttributes.Normal);
            File.Delete(path);
        }
    }


    //Classe Livre
    [System.Serializable]
    public class Livre
    {
        public bool[] isPanelSonIndep;              //Size : nombre de pages, est-ce que la page a un panel de sons indés ?
        public int[] nbSonsIndep;                   //Size : nombre de pages, combien la page a de nombre de sons indés ?
        public Vector3[] codesSonIndep;             //Size : Somme de nbSonsIndep, x = numéro de page, y = numéro du son indé de la page, z = code du son indé y
        //Est-ce que les langues sont dispo ?
        public bool isFrDispo;
        public bool isEnDispo;
        public bool isGeDispo;
    }
    public Livre livre;



    //Achat de l'appli complète
    public void BuyEntireKamishibai()
    {
        IAP_Manager.Instance.BuyEntireKamishibai();
    }

}
