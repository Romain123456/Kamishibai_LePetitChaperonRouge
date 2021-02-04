using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Analytics;


public class Fonctions_Utiles : MonoBehaviour {
	//Ce script répertorie toutes les fonctions utilisées un peu partout !
	//A attacher une fois par scène à une caméra (par exemple)



	//Méthode Quitter : permet de quitter le jeu
	public void Quitter(){
        GameObject.Find("Main Camera").GetComponent<LivreManagement>().SaveLivre();
        StartCoroutine(QuitterCoroutine());
	}

    private IEnumerator QuitterCoroutine()
    {
        AudioSource quitterAudio = GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent.currentSelectedGameObject.GetComponent<AudioSource>();
        quitterAudio.PlayOneShot(quitterAudio.clip);
        while (quitterAudio.isPlaying)
        {
            yield return new WaitForSeconds(LivreManagement.deltaTime);
            if (!quitterAudio.isPlaying)
            {
                break;
            }
        }
        Application.Quit();
    }

		

    public void DemarrerContenu(GameObject newContenu)
    {
        StartCoroutine(CoroutDemarrerContenu(newContenu));
    }


    AnalyticsResult analyticsResults;
    IEnumerator CoroutDemarrerContenu(GameObject newContenu)
    {
        if (newContenu == GameObject.Find("Main Camera").GetComponent<LivreManagement>().creditPanel || newContenu == GameObject.Find("Main Camera").GetComponent<LivreManagement>().contenuPedagogiquePanel)
        {
            if(newContenu == GameObject.Find("Main Camera").GetComponent<LivreManagement>().creditPanel)
            {
                analyticsResults = Analytics.CustomEvent("CreditPress");
            } else if(newContenu== newContenu == GameObject.Find("Main Camera").GetComponent<LivreManagement>().contenuPedagogiquePanel)
            {
                analyticsResults = Analytics.CustomEvent("ContenuPedagogique");
            }
            AudioSource playingAudioSource = GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent.currentSelectedGameObject.gameObject.GetComponent<AudioSource>();
            if (playingAudioSource != null)
            {
                while (playingAudioSource.isPlaying)
                {
                    yield return new WaitForSeconds(LivreManagement.deltaTime);
                    if (!playingAudioSource.isPlaying)
                    {
                        GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPause.SetActive(false);
                        break;
                    }
                }
            }
        }
        if (newContenu == GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelLivre)
        {
            analyticsResults = Analytics.CustomEvent("LivreConteur");
        } else if (newContenu == GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelLectureAuto)
        {
            analyticsResults = Analytics.CustomEvent("LivreLu");
        } else if(newContenu == GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelMemory)
        {
            analyticsResults = Analytics.CustomEvent("Memory");
        } else if(newContenu == GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPuzzle)
        {
            analyticsResults = Analytics.CustomEvent("Puzzle");
        }
        else if(newContenu == GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelMeliMelo)
        {
            analyticsResults = Analytics.CustomEvent("MeliMelo");
        }

            GameObject.Find("Main Camera").GetComponent<LivreManagement>().contenuOuvert = newContenu.gameObject;
        newContenu.SetActive(true);
        GameObject.Find("Main Camera").GetComponent<LivreManagement>().emptyBoutonsReglages.SetActive(true);
        GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelMenuPrincipal.SetActive(false);
        DeselectEventSystem(GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent);
    }






    void DesactiveBoutonsPanelLivre()
    {
        for (int ii = 2;ii< GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelMenuPrincipal.transform.childCount; ii++)
        {
            if (GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelMenuPrincipal.transform.GetChild(ii).GetComponent<Button>() != null)
            {
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelMenuPrincipal.transform.GetChild(ii).GetComponent<Button>().interactable = false;
            }
        }
    }

    void ActiveBoutonsPanelLivre()
    {
        for (int ii = 2; ii < GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelMenuPrincipal.transform.childCount; ii++)
        {
            if (GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelMenuPrincipal.transform.GetChild(ii).GetComponent<Button>() != null)
            {
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelMenuPrincipal.transform.GetChild(ii).GetComponent<Button>().interactable = true;
            }
        }
    }

    IEnumerator DemarrageContenuApresLecture(GameObject newContenu)
    {
        if (GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent.currentSelectedGameObject != null)
        {
            AudioSource playingAudioSource = GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent.currentSelectedGameObject.gameObject.GetComponent<AudioSource>();
            if (playingAudioSource != null)
            {
                if (!GameObject.Find("Main Camera").GetComponent<LivreManagement>().isDontAskAgain)
                {
                    GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.SetActive(true);
                    GameObject.Find("Main Camera").transform.GetComponent<LangageScript>().textPopUpEnceinte.transform.SetAsFirstSibling();
                    GameObject.Find("Main Camera").GetComponent<LangageScript>().textPopUpEnceinte.text = GameObject.Find("Main Camera").GetComponent<LangageScript>()._TextePopUpEnceinte;
                    GameObject.Find("Main Camera").GetComponent<LangageScript>().textPopUpEnceinte.GetComponent<RectTransform>().anchoredPosition = new Vector2(0,-42);
                    GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.transform.Find("PanelAchats").gameObject.SetActive(false);
                    GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.transform.Find("PanelDontAsk").gameObject.SetActive(true);
                }
                Debug.Log(playingAudioSource.isPlaying);
                while (playingAudioSource.isPlaying)
                {
                    yield return new WaitForSeconds(LivreManagement.deltaTime);
                    Debug.Log(playingAudioSource.isPlaying);
                    if (!playingAudioSource.isPlaying)
                    {
                        if (!GameObject.Find("Main Camera").GetComponent<LivreManagement>().isDontAskAgain)
                        {
                            yield return new WaitForSeconds(1.5f);
                        }
                        break;
                    }
                }
                DemarrerContenu(newContenu);
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.SetActive(false);
                DeselectEventSystem(GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent);
            } else
            {
                Debug.Log("Pas d'AudioSource");
            }
        } else
        {
            Debug.Log("Pas de Bouton Selectionné");
        }

        yield return null;
    }


    public void LivreBouton(GameObject newContenu)
    {
        GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPause.GetComponent<RectTransform>().localScale = new Vector2(0, 0);
        if (!GameObject.Find("Main Camera").GetComponent<LivreManagement>().dataCharged || GameObject.Find("Main Camera").GetComponent<LivreManagement>().currentPage == GameObject.Find("Main Camera").GetComponent<LivreManagement>().nbPagesLivre-1)
        {
            Debug.Log(GameObject.Find("Main Camera").GetComponent<LivreManagement>().currentPage);
            StartCoroutine(DemarrageContenuApresLecture(newContenu));
            //DemarrerContenu(newContenu);
            
            if (newContenu != GameObject.Find("Main Camera").GetComponent<LivreManagement>().creditPanel)
            {
                GameObject.Find("Main Camera").GetComponent<AudioSource>().clip = GameObject.Find("Main Camera").GetComponent<LivreManagement>().ambianceLivre[0];
                GameObject.Find("Main Camera").GetComponent<AudioSource>().Play();
            }

        } else
        {
            Debug.Log(GameObject.Find("Main Camera").GetComponent<LivreManagement>().currentPage);
            GameObject.Find("Main Camera").GetComponent<LivreManagement>().emptyBoutonsReglages.SetActive(false);
            AudioSource playingAudioSource = GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent.currentSelectedGameObject.gameObject.GetComponent<AudioSource>();
            playingAudioSource.Stop();
            GameObject.Find("Main Camera").GetComponent<LivreManagement>().livreButton.transform.Find("PanelRecommencer").gameObject.SetActive(true);
            GameObject.Find("Main Camera").GetComponent<LivreManagement>().livreButton.transform.SetAsLastSibling();
            DesactiveBoutonsPanelLivre();
            DeselectEventSystem(GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent);
        }
        //DeselectEventSystem(GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent);
    }


    public void LectureAutoBouton(GameObject newContenu)
    {
        GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPause.GetComponent<RectTransform>().localScale = new Vector2(0, 0);
        if (!GameObject.Find("Main Camera").GetComponent<LivreManagement>().dataCharged || GameObject.Find("Main Camera").GetComponent<LivreManagement>().currentPageLectAuto == GameObject.Find("Main Camera").GetComponent<LivreManagement>().nbPagesLivre - 1)
        {
            GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelLectureAuto.transform.GetChild(GameObject.Find("Main Camera").GetComponent<LivreManagement>().nbPagesLivre - 1).GetComponent<PageLectureAutoScript>().isLectureLaunch = false;
            StartCoroutine(DemarrageContenuApresLecture(newContenu));
            if (newContenu != GameObject.Find("Main Camera").GetComponent<LivreManagement>().creditPanel)
            {
                GameObject.Find("Main Camera").GetComponent<AudioSource>().clip = GameObject.Find("Main Camera").GetComponent<LivreManagement>().ambianceLivre[0];
                GameObject.Find("Main Camera").GetComponent<AudioSource>().Play();
            }

        }
        else
        {
            GameObject.Find("Main Camera").GetComponent<LivreManagement>().emptyBoutonsReglages.SetActive(false);
            AudioSource playingAudioSource = GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent.currentSelectedGameObject.gameObject.GetComponent<AudioSource>();
            playingAudioSource.Stop();
            GameObject.Find("Main Camera").GetComponent<LivreManagement>().lectureAutoGO.transform.Find("PanelRecommencer").gameObject.SetActive(true);
            GameObject.Find("Main Camera").GetComponent<LivreManagement>().lectureAutoGO.transform.SetAsLastSibling();
            DesactiveBoutonsPanelLivre();
            DeselectEventSystem(GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent);
        }
    }



    public void LivrePanelRecommencerOUI(GameObject newContenu)
    {
        StartCoroutine(ChargementLivre(newContenu));
    }

    IEnumerator ChargementLivre(GameObject newContenu)
    {
        AudioSource playingAudioSource = GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent.currentSelectedGameObject.gameObject.GetComponent<AudioSource>();

        GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.transform.SetAsLastSibling();

        if (playingAudioSource != null)
        {
            if (!GameObject.Find("Main Camera").GetComponent<LivreManagement>().isDontAskAgain)
            {
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.SetActive(true);
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.transform.Find("PanelAchats").gameObject.SetActive(false);
                GameObject.Find("Main Camera").transform.GetComponent<LangageScript>().textPopUpEnceinte.transform.SetAsFirstSibling();
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.transform.Find("PanelDontAsk").gameObject.SetActive(true);
                GameObject.Find("Main Camera").GetComponent<LangageScript>().textPopUpEnceinte.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -42);
                GameObject.Find("Main Camera").GetComponent<LangageScript>().textPopUpEnceinte.text = GameObject.Find("Main Camera").GetComponent<LangageScript>()._TextePopUpEnceinte;
            }
            while (playingAudioSource.isPlaying)
            {
                yield return new WaitForSeconds(LivreManagement.deltaTime);
                if (!playingAudioSource.isPlaying)
                {
                    if (!GameObject.Find("Main Camera").GetComponent<LivreManagement>().isDontAskAgain)
                    {
                        yield return new WaitForSeconds(1.5f);
                    }
                    GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.SetActive(false);
                    DeselectEventSystem(GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent);
                    break;
                }
            }
        }

        DemarrerContenu(newContenu);
        
        for (int ii = 0; ii < GameObject.Find("Main Camera").GetComponent<LivreManagement>().nbPagesLivre; ii++)
        {
            GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelLivre.transform.GetChild(ii).gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(0.1f);
        for (int ii = GameObject.Find("Main Camera").GetComponent<LivreManagement>().nbPagesLivre-1; ii >= 0; ii--)
        {
            if (ii == GameObject.Find("Main Camera").GetComponent<LivreManagement>().currentPage)
            {
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelLivre.transform.GetChild(ii).gameObject.SetActive(true);

                StartCoroutine(TransitionDeuxAmbiances(GameObject.Find("Main Camera").GetComponent<LivreManagement>().timeFadeOpenLivre, GameObject.Find("Main Camera").GetComponent<LivreManagement>().ambianceGenarale_AudioSource.clip, GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelLivre.transform.GetChild(ii).GetComponent<PageLivreScript>().ambiancePage, GameObject.Find("Main Camera").GetComponent<LivreManagement>().currentPage));

            }
            else
            {
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelLivre.transform.GetChild(ii).gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(0.1f);
        }

        GameObject.Find("Main Camera").GetComponent<LivreManagement>().livreButton.transform.Find("PanelRecommencer").gameObject.SetActive(false);
        DeselectEventSystem(GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent);
        ActiveBoutonsPanelLivre();
    }


    public void LivrePanelRecommencerNON(GameObject newContenu)
    {
        StartCoroutine(CoroutLivreRecommencerNON(newContenu));
    }


    IEnumerator CoroutLivreRecommencerNON(GameObject newContenu)
    {

        AudioSource playingAudioSource = GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent.currentSelectedGameObject.gameObject.GetComponent<AudioSource>();

        GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.transform.SetAsLastSibling();

        if (playingAudioSource != null)
        {
            if (!GameObject.Find("Main Camera").GetComponent<LivreManagement>().isDontAskAgain)
            {
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.SetActive(true);
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.transform.Find("PanelAchats").gameObject.SetActive(false);
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.transform.Find("PanelDontAsk").gameObject.SetActive(true);
                GameObject.Find("Main Camera").transform.GetComponent<LangageScript>().textPopUpEnceinte.transform.SetAsFirstSibling();
                GameObject.Find("Main Camera").GetComponent<LangageScript>().textPopUpEnceinte.text = GameObject.Find("Main Camera").GetComponent<LangageScript>()._TextePopUpEnceinte;
                GameObject.Find("Main Camera").GetComponent<LangageScript>().textPopUpEnceinte.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -42);
            }
            while (playingAudioSource.isPlaying)
            {
                yield return new WaitForSeconds(LivreManagement.deltaTime);
                if (!playingAudioSource.isPlaying)
                {
                    if (!GameObject.Find("Main Camera").GetComponent<LivreManagement>().isDontAskAgain)
                    {
                        yield return new WaitForSeconds(1.5f);
                    }
                    GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.SetActive(false);
                    DeselectEventSystem(GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent);
                    break;
                }
            }
        }



        DemarrerContenu(newContenu);
        for (int ii = 0; ii < GameObject.Find("Main Camera").GetComponent<LivreManagement>().nbPagesLivre; ii++)
        {
            if (ii == GameObject.Find("Main Camera").GetComponent<LivreManagement>().nbPagesLivre - 1)
            {
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelLivre.transform.GetChild(ii).gameObject.SetActive(true);

                StartCoroutine(TransitionDeuxAmbiances(GameObject.Find("Main Camera").GetComponent<LivreManagement>().timeFadeOpenLivre, GameObject.Find("Main Camera").GetComponent<LivreManagement>().ambianceGenarale_AudioSource.clip, GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelLivre.transform.GetChild(ii).GetComponent<PageLivreScript>().ambiancePage,ii));

            }
            else
            {
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelLivre.transform.GetChild(ii).gameObject.SetActive(false);
            }
        }
        ActiveBoutonsPanelLivre();
        GameObject.Find("Main Camera").GetComponent<LivreManagement>().livreButton.transform.Find("PanelRecommencer").gameObject.SetActive(false);
        DeselectEventSystem(GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent);
    }


    //Lecture Auto Reprise
    public void LectureAutoPanelRecommencerOUI(GameObject newContenu)
    {
        StartCoroutine(ChargementLectureAuto(newContenu));
    }

    IEnumerator ChargementLectureAuto(GameObject newContenu)
    {
        AudioSource playingAudioSource = GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent.currentSelectedGameObject.gameObject.GetComponent<AudioSource>();

        GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.transform.SetAsLastSibling();

        if (playingAudioSource != null)
        {
            if (!GameObject.Find("Main Camera").GetComponent<LivreManagement>().isDontAskAgain)
            {
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.SetActive(true);
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.transform.Find("PanelAchats").gameObject.SetActive(false);
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.transform.Find("PanelDontAsk").gameObject.SetActive(true);
                GameObject.Find("Main Camera").transform.GetComponent<LangageScript>().textPopUpEnceinte.transform.SetAsFirstSibling();
                GameObject.Find("Main Camera").GetComponent<LangageScript>().textPopUpEnceinte.text = GameObject.Find("Main Camera").GetComponent<LangageScript>()._TextePopUpEnceinte;
                GameObject.Find("Main Camera").GetComponent<LangageScript>().textPopUpEnceinte.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -42);
            }
            while (playingAudioSource.isPlaying)
            {
                yield return new WaitForSeconds(LivreManagement.deltaTime);
                if (!playingAudioSource.isPlaying)
                {
                    if (!GameObject.Find("Main Camera").GetComponent<LivreManagement>().isDontAskAgain)
                    {
                        yield return new WaitForSeconds(1.5f);
                    }
                    GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.SetActive(false);
                    DeselectEventSystem(GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent);
                    break;
                }
            }
        }


        DemarrerContenu(newContenu);


        for (int ii = 0; ii < GameObject.Find("Main Camera").GetComponent<LivreManagement>().nbPagesLivre; ii++)
        {
            GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelLectureAuto.transform.GetChild(ii).gameObject.SetActive(true);
            GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelLectureAuto.transform.GetChild(ii).GetComponent<PageLectureAutoScript>().isLectureLaunch = false;
        }

        yield return new WaitForSeconds(0.1f);
        for (int ii = GameObject.Find("Main Camera").GetComponent<LivreManagement>().nbPagesLivre - 1; ii >= 0; ii--)
        {
            GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelLectureAuto.transform.GetChild(ii).GetComponent<PageLectureAutoScript>().isLectureLaunch = false;
            if (ii == GameObject.Find("Main Camera").GetComponent<LivreManagement>().currentPageLectAuto)
            {
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelLectureAuto.transform.GetChild(ii).gameObject.SetActive(true);

                StartCoroutine(TransitionDeuxAmbiances(GameObject.Find("Main Camera").GetComponent<LivreManagement>().timeFadeOpenLivre, GameObject.Find("Main Camera").GetComponent<LivreManagement>().ambianceGenarale_AudioSource.clip, GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelLectureAuto.transform.GetChild(ii).GetComponent<PageLectureAutoScript>().ambiancePage,ii));

            }
            else
            {
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelLectureAuto.transform.GetChild(ii).gameObject.SetActive(false);
            }
            yield return new WaitForSeconds(0.1f);
        }

        GameObject.Find("Main Camera").GetComponent<LivreManagement>().lectureAutoGO.transform.Find("PanelRecommencer").gameObject.SetActive(false);
        ActiveBoutonsPanelLivre();
        DeselectEventSystem(GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent);
    }



    public void LectureAutoPanelRecommencerNON(GameObject newContenu)
    {
        StartCoroutine(CoroutLivreAutoRecommencerNON(newContenu));
    }


    IEnumerator CoroutLivreAutoRecommencerNON(GameObject newContenu)
    {
        AudioSource playingAudioSource = GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent.currentSelectedGameObject.gameObject.GetComponent<AudioSource>();

        GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.transform.SetAsLastSibling();

        if (playingAudioSource != null)
        {
            if (!GameObject.Find("Main Camera").GetComponent<LivreManagement>().isDontAskAgain)
            {
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.SetActive(true);
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.transform.Find("PanelAchats").gameObject.SetActive(false);
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.transform.Find("PanelDontAsk").gameObject.SetActive(true);
                GameObject.Find("Main Camera").transform.GetComponent<LangageScript>().textPopUpEnceinte.transform.SetAsFirstSibling();
                GameObject.Find("Main Camera").GetComponent<LangageScript>().textPopUpEnceinte.text = GameObject.Find("Main Camera").GetComponent<LangageScript>()._TextePopUpEnceinte;
                GameObject.Find("Main Camera").GetComponent<LangageScript>().textPopUpEnceinte.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -42);
            }
            while (playingAudioSource.isPlaying)
            {
                yield return new WaitForSeconds(LivreManagement.deltaTime);
                if (!playingAudioSource.isPlaying)
                {
                    if (!GameObject.Find("Main Camera").GetComponent<LivreManagement>().isDontAskAgain)
                    {
                        yield return new WaitForSeconds(1.5f);
                    }
                    GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPopUpEnceinte.SetActive(false);
                    DeselectEventSystem(GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent);
                    break;
                }
            }
        }


        DemarrerContenu(newContenu);
        for (int ii = 0; ii < GameObject.Find("Main Camera").GetComponent<LivreManagement>().nbPagesLivre; ii++)
        {
            if (ii == GameObject.Find("Main Camera").GetComponent<LivreManagement>().nbPagesLivre - 1)
            {
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelLectureAuto.transform.GetChild(ii).GetComponent<PageLectureAutoScript>().isLectureLaunch = false;
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelLectureAuto.transform.GetChild(ii).gameObject.SetActive(true);

                StartCoroutine(TransitionDeuxAmbiances(GameObject.Find("Main Camera").GetComponent<LivreManagement>().timeFadeOpenLivre, GameObject.Find("Main Camera").GetComponent<LivreManagement>().ambianceGenarale_AudioSource.clip, GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelLectureAuto.transform.GetChild(ii).GetComponent<PageLectureAutoScript>().ambiancePage,ii));

            }
            else
            {
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelLectureAuto.transform.GetChild(ii).gameObject.SetActive(false);
            }
        }
        GameObject.Find("Main Camera").GetComponent<LivreManagement>().lectureAutoGO.transform.Find("PanelRecommencer").gameObject.SetActive(false);

        ActiveBoutonsPanelLivre();
        DeselectEventSystem(GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent);
    }



    //Fade du changement de son
    IEnumerator TransitionDeuxAmbiances(float tpsFade, AudioClip fadeMoinsClip, AudioClip fadePlusClip,int indAmbiance)
    {
        //On passe la musique en cours sur l'audio temporaire et on diminue progressivement le volume
        GameObject.Find("Main Camera").GetComponent<LivreManagement>().ambianceGeneraleAudioTemp.clip = fadeMoinsClip;
            //GameObject.Find("Main Camera").GetComponent<LivreManagement>().ambianceGenarale_AudioSource.clip;
        GameObject.Find("Main Camera").GetComponent<LivreManagement>().ambianceGeneraleAudioTemp.time = GameObject.Find("Main Camera").GetComponent<LivreManagement>().ambianceGenarale_AudioSource.time;
        GameObject.Find("Main Camera").GetComponent<LivreManagement>().ambianceGeneraleAudioTemp.Play();
        StartCoroutine(GameObject.Find("Main Camera").GetComponent<LivreManagement>().FadeMoins_Volume(GameObject.Find("Main Camera").GetComponent<LivreManagement>().ambianceGeneraleAudioTemp, GameObject.Find("Main Camera").GetComponent<LivreManagement>().ambianceGenarale_AudioSource.volume, tpsFade, 0));


        //On donne la nouvelle musique à l'audio principale et augmente progressivement le volume
        GameObject.Find("Main Camera").GetComponent<LivreManagement>().ambianceGenarale_AudioSource.clip = fadePlusClip; //GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelLivre.transform.GetChild(ii).GetComponent<PageLivreScript>().ambiancePage;
        GameObject.Find("Main Camera").GetComponent<LivreManagement>().ambianceGenarale_AudioSource.Play();
        StartCoroutine(GameObject.Find("Main Camera").GetComponent<LivreManagement>().FadePlus_Volume(GameObject.Find("Main Camera").GetComponent<LivreManagement>().ambianceGenarale_AudioSource, 0, tpsFade, indAmbiance));


        while (GameObject.Find("Main Camera").GetComponent<LivreManagement>().ambianceGeneraleAudioTemp.volume > 0)
        {
            yield return new WaitForSeconds(LivreManagement.deltaTime);
            if (GameObject.Find("Main Camera").GetComponent<LivreManagement>().ambianceGeneraleAudioTemp.volume <= 0)
            {
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().ambianceGeneraleAudioTemp.volume = 0;
                break;
            }
        }
        yield return null;

    }


    // Déselection de l'EventSystem
    public void DeselectEventSystem(EventSystem monEvent)
    {
        monEvent.SetSelectedGameObject(null);
    }




    public void RetourMenuPrincipal()
    {
        StartCoroutine(CoroutRetourMenuPrincipal());
    }


    IEnumerator CoroutRetourMenuPrincipal()
    {
        if (GameObject.Find("Main Camera").GetComponent<LivreManagement>().contenuOuvert == GameObject.Find("Main Camera").GetComponent<LivreManagement>().creditPanel || GameObject.Find("Main Camera").GetComponent<LivreManagement>().contenuOuvert == GameObject.Find("Main Camera").GetComponent<LivreManagement>().contenuPedagogiquePanel)
        {
            AudioSource playingAudioSource = null;

            playingAudioSource = GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent.currentSelectedGameObject.gameObject.GetComponent<AudioSource>();

            if (playingAudioSource != null)
            {
                while (playingAudioSource.isPlaying)
                {
                    yield return new WaitForSeconds(LivreManagement.deltaTime);
                    if (!playingAudioSource.isPlaying)
                    {
                        
                        break;
                    }
                }
            }
        }
        

        if (GameObject.Find("Main Camera").GetComponent<LivreManagement>().contenuOuvert != null)
        {
            GameObject.Find("Main Camera").GetComponent<LivreManagement>().contenuOuvert.SetActive(false);
            GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelMenuPrincipal.SetActive(true);
            if (GameObject.Find("Main Camera").GetComponent<LivreManagement>().contenuOuvert != GameObject.Find("Main Camera").GetComponent<LivreManagement>().creditPanel)
            {
                if (GameObject.Find("Main Camera").GetComponent<LivreManagement>().isSonPause)
                {
                    GameObject.Find("Main Camera").GetComponent<LivreManagement>().PanelPauseSonPause();
                }
                StartCoroutine(TransitionDeuxAmbiances(GameObject.Find("Main Camera").GetComponent<LivreManagement>().timeFadeOpenLivre, GameObject.Find("Main Camera").GetComponent<LivreManagement>().ambianceGenarale_AudioSource.clip, GameObject.Find("Main Camera").GetComponent<LivreManagement>().ambianceMenuPrincipal,0));
            }

            GameObject.Find("Main Camera").GetComponent<LivreManagement>().contenuOuvert = null;
            GameObject.Find("Main Camera").GetComponent<LivreManagement>().sonsIndependants_AudioSource.Stop();

        }

        GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPause.SetActive(true);
        GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelPause.GetComponent<RectTransform>().localScale = Vector3.one;
        DeselectEventSystem(GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent);
    }




    // Choix des Langues Affiche Panel 
    public void AfficheChoixLangue()
    {
        if (!GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelLangueDepart.activeSelf)
        {
            GameObject.Find("Main Camera").GetComponent<LivreManagement>().panelLangueDepart.SetActive(true);
            GameObject.Find("Main Camera").GetComponent<LivreManagement>().buttonChoixLangue.GetComponent<Button>().interactable = false;
        }
        DeselectEventSystem(GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent);
    }


    //Méthode Selection_First_Button : Permet de rendre sélectable un bouton dans un menu
    static public void Selection_First_Bouton(BaseEventData mon_event,Button mon_button){
		mon_button.OnSelect(mon_event);
		mon_button.Select ();
	}


	//Methode Debug_Clic_Souris : permet de ne pas déselectionner tous les boutons lorsqu'on clique avec la souris
	static public void Debug_Clic_Souris(BaseEventData mon_event,Button mon_button){
		if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1) || Input.GetMouseButtonDown (2)) {
			Fonctions_Utiles.Selection_First_Bouton (mon_event, mon_button);
		}
	}


	//Methode Back_From_Commandes : permet de quitter l'image affichant les commandes
	static public void Back_From_Commandes(GameObject commandes_image,GameObject retour_image,BaseEventData mon_event,Button mon_button){
		//En appuyant sur le bouton aproprié, on revient à l'ancien écran
		if(Input.GetKeyDown(KeyCode.Backspace) || Input.GetButtonDown("X360_B")){
			retour_image.SetActive (true);
			commandes_image.SetActive (false);
			Fonctions_Utiles.Selection_First_Bouton (mon_event, mon_button);
		}
	}


	//Méthode MoveMenuCommandes : permet le fonctionnement du switch des images du menu commandes
	static public void MoveMenuCommandes (Button button_select_, RectTransform button_devant_, GameObject image_devant_, Button button_deselect_, GameObject image_derriere_, BaseEventData mon_event) {
		Fonctions_Utiles.Selection_First_Bouton (mon_event, button_select_);					//Selection du bouton concerné
		button_devant_.SetSiblingIndex(4);			//Bouton mis devant, NE PAS DIRE QUE L'AUTRE EST DERRIERE. 4 : bon ordre trouvé
		image_devant_.SetActive(true);							//L'image correspondante est active
		button_deselect_.OnDeselect (mon_event);				//Bouton déselectionné
		image_derriere_.SetActive(false);						//Image derrière désactivée
	}


	//Méthode NavigueCommandes : permet de naviguer dans le menu commandes
	/*static public void NavigueCommandes(Button button_select1, RectTransform button_devant1, GameObject image_devant1, Button button_select2, RectTransform button_devant2, GameObject image_devant2, BaseEventData mon_event){
		if (Input.GetKeyDown (KeyCode.LeftArrow) || Input.GetAxis("X360_LeftStick_Hor") < -0.005f) {					//Si on appuie sur la flèche gauche ou joystick gauche vers gauche
			Fonctions_Utiles.MoveMenuCommandes (button_select1,button_devant1,image_devant1,button_select2,image_devant2,mon_event);
		}
		if (Input.GetKeyDown (KeyCode.RightArrow) || Input.GetAxis("X360_LeftStick_Hor") > 0.005f) {					//Si on appuie sur la flèche droite ou joystick gauche vers droite
			Fonctions_Utiles.MoveMenuCommandes(button_select2,button_devant2,image_devant2,button_select1,image_devant1,mon_event);
		}
	}*/


    public void DesactivePanel(GameObject _PanelToDesactive)
    {
        _PanelToDesactive.SetActive(false);
    }



    //Ne plus afficher la popup 
    public void DontAskPopUp()
    {
        if(!GameObject.Find("Main Camera").GetComponent<LivreManagement>().isDontAskAgain)
        {
            GameObject.Find("Main Camera").GetComponent<LivreManagement>().isDontAskAgain = true;
            GameObject.Find("Main Camera").GetComponent<LivreManagement>().DontAskAgainImage.sprite = GameObject.Find("Main Camera").GetComponent<LivreManagement>().tickYesSprite;
        } else
        {
            GameObject.Find("Main Camera").GetComponent<LivreManagement>().isDontAskAgain = false;
            GameObject.Find("Main Camera").GetComponent<LivreManagement>().DontAskAgainImage.sprite = GameObject.Find("Main Camera").GetComponent<LivreManagement>().tickNoSprite;
        }
    }

}
