using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MeliMeloManager : MonoBehaviour
{
    public string[] motsListe;
    private List<string> motsListeTemp;
    //public int[] motsListeSyllabes;
    //private List<int> motsListeSyllabesTemp;

    public Text[] motsToFind = new Text[3];
    //private int[] motsToFindSyllabes;
    public GameObject textAreaPrefab;
    public GameObject imageCaseVidePrefab;

    public GameObject pieceSyllabePrefab;
    private List<GameObject> pieceSyllabeListe;

    public Vector2[] positionStartListe;
    private List<Vector2> positionStartListeTemp;

    PointerEventData monEventData;
    EventSystem myEventSystem;

    float largeurPanel;
    float longueurPanel;

    private GameObject selectedPiece;

    public int nbPoints = 0;
    private int nbPointsMax;

    public GameObject panelVictoire;

    // Start is called before the first frame update
    void Start()
    {
        CreationMots();
        StartCoroutine(DataEvent());

        largeurPanel = this.GetComponent<RectTransform>().sizeDelta.x;
        longueurPanel = this.GetComponent<RectTransform>().sizeDelta.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            if (touch.phase == TouchPhase.Began)
            {
                selectedPiece = myEventSystem.currentSelectedGameObject.gameObject;
            }
            if (touch.phase == TouchPhase.Moved && selectedPiece.CompareTag("PieceMeliMelo"))
            {
                selectedPiece.GetComponent<RectTransform>().anchoredPosition = new Vector3((touch.position.x / 3), (touch.position.y / 3), 0);
            }
            if (touch.phase == TouchPhase.Ended)
            {
                CheckPlacePiece(selectedPiece.transform);
                CheckSyllabePiecePlace(selectedPiece.transform);
                selectedPiece = null;
            }
        }
    }

    void CheckPlacePiece(Transform pieceToCheck)
    {
        Vector3 positionPiece = pieceToCheck.transform.position;
        bool checkY = false;
        if (pieceToCheck.position.x >= -6.5f && pieceToCheck.position.x < -4.5f)
        {
            pieceToCheck.GetComponent<PieceMeliMelo>().indiceX = 1;         //Gauche
            positionPiece.x = -5.6f;
            checkY = true;
        }
        else if (pieceToCheck.position.x >= -4.5f && pieceToCheck.position.x < -2.5f)
        {
            pieceToCheck.GetComponent<PieceMeliMelo>().indiceX = 2;          //Centre
            positionPiece.x = -3.65f;
            checkY = true;
        }
        else if (pieceToCheck.position.x >= -2.5f && pieceToCheck.position.x < -0.5f)
        {
            pieceToCheck.GetComponent<PieceMeliMelo>().indiceX = 3;         //Droite
            positionPiece.x = -1.75f;
            checkY = true;
        }

        if (checkY)
        {
            if (pieceToCheck.position.y >= -2.5f && pieceToCheck.position.y < -0.5f)
            {
                pieceToCheck.GetComponent<PieceMeliMelo>().indiceY = 1;         //En bas
                positionPiece.y = -1.35f;
            }
            else if (pieceToCheck.position.y >= -0.5f && pieceToCheck.position.y < 1.5f)
            {
                pieceToCheck.GetComponent<PieceMeliMelo>().indiceY = 2;         //Centre
                positionPiece.y = 0.67f;
            }
            else if (pieceToCheck.position.y >= 1.5f && pieceToCheck.position.y < 3.5f)
            {
                pieceToCheck.GetComponent<PieceMeliMelo>().indiceY = 3;         //En haut
                positionPiece.y = 2.72f;
            }
        }
        pieceToCheck.position = positionPiece;
    }



    void CheckSyllabePiecePlace(Transform pieceToCheck)
    {
        //Debug.Log(pieceToCheck.GetComponent<PieceMeliMelo>().indiceX);
        /*if (pieceToCheck.GetComponent<PieceMeliMelo>().indiceY - 1 > 0)
        {
            Debug.Log(motsToFind[pieceToCheck.GetComponent<PieceMeliMelo>().indiceY - 1].text);
        }*/

        if (pieceToCheck.GetComponent<PieceMeliMelo>().indiceX - 1 >= 0)
        {
            if (pieceToCheck.GetComponent<PieceMeliMelo>().indiceX - 1 < motsToFind[pieceToCheck.GetComponent<PieceMeliMelo>().indiceY - 1].transform.childCount)
            {
                
                if(motsToFind[pieceToCheck.GetComponent<PieceMeliMelo>().indiceY - 1].transform.GetChild(pieceToCheck.GetComponent<PieceMeliMelo>().indiceX - 1).GetComponent<ImageVideSyllabe>().syllabeImage == selectedPiece.transform.GetChild(0).GetComponent<Text>().text)
                {
                    //Debug.Log(motsToFind[pieceToCheck.GetComponent<PieceMeliMelo>().indiceY - 1].transform.GetChild(pieceToCheck.GetComponent<PieceMeliMelo>().indiceX - 1).GetComponent<ImageVideSyllabe>().syllabeImage);
                    //motsToFind[pieceToCheck.GetComponent<PieceMeliMelo>().indiceY - 1].transform.GetChild(pieceToCheck.GetComponent<PieceMeliMelo>().indiceX - 1).GetComponent<ImageVideSyllabe>().isBienPlace = true;
                    selectedPiece.GetComponent<Button>().interactable = false;
                    nbPoints++;
                    Debug.Log("Bonne pioche !");
                } else
                {
                    Debug.Log("Loupé !!");
                }
            }
        }

        pieceToCheck.GetComponent<PieceMeliMelo>().indiceX = 0;
        pieceToCheck.GetComponent<PieceMeliMelo>().indiceY = 0;

        if(nbPoints == nbPointsMax)
        {
            Debug.Log("Gagné !!!");
            panelVictoire.transform.SetAsLastSibling();
            panelVictoire.SetActive(true);
        }
        
    }


    void CreationMots()
    {
        panelVictoire.SetActive(false);


        motsListeTemp = new List<string>();
        for(int ii = 0; ii < motsListe.Length; ii++)
        {
            motsListeTemp.Add(motsListe[ii]);

        }

        pieceSyllabeListe = new List<GameObject>();

        positionStartListeTemp = new List<Vector2>();
        for(int ii = 0; ii < positionStartListe.Length; ii++)
        {
            positionStartListeTemp.Add(positionStartListe[ii]);
        }



        for(int ii = 0; ii < motsToFind.Length; ii++)
        {
            GameObject textArea = Instantiate(textAreaPrefab, this.transform);
            textArea.transform.SetAsFirstSibling();
            textArea.GetComponent<RectTransform>().anchoredPosition = new Vector2(154, 80+ (ii * 80));
            motsToFind[ii] = textArea.GetComponent<Text>();
            int choixMot = Random.RandomRange(0, motsListeTemp.Count);
            motsToFind[ii].text = motsListeTemp[choixMot];

            int motsToFindSyllabes = NombreSyllabesToFind(motsToFind[ii].text);

            for (int jj = 0; jj < motsToFindSyllabes; jj++)
            {
                GameObject newPiece = Instantiate(pieceSyllabePrefab, this.transform);
                newPiece.transform.GetChild(0).GetComponent<Text>().text = FindSyllabes(motsToFind[ii].text, jj);
                int positionChoix = Random.RandomRange(0, positionStartListeTemp.Count);
                newPiece.GetComponent<RectTransform>().anchoredPosition = positionStartListeTemp[positionChoix];
                positionStartListeTemp.Remove(positionStartListeTemp[positionChoix]);

                GameObject pieceVide = Instantiate(imageCaseVidePrefab, textArea.transform);
                pieceVide.GetComponent<RectTransform>().anchoredPosition = new Vector2(188 * (jj )+400, 65);
                pieceVide.GetComponent<RectTransform>().localScale = Vector3.one * 2.5f;
                pieceVide.GetComponent<ImageVideSyllabe>().syllabeImage = newPiece.transform.GetChild(0).GetComponent<Text>().text;

                nbPointsMax++;
            }
            motsListeTemp.Remove(motsListeTemp[choixMot]);
        }
    }



    int NombreSyllabesToFind(string _MonMot)
    {
        int nbSyllabes = 0;
        if (_MonMot == "Lapin" || _MonMot == "Renard" || _MonMot == "Chasseur"|| _MonMot == "Maison")
        {
            nbSyllabes = 2;
        }      
        else if (_MonMot == "Chaperon")
        {
            nbSyllabes = 3;
        }
      
        return nbSyllabes;
    }


    string FindSyllabes(string _MonMot,int _Syllabe)
    {
        string maSyllabe= "";
        if(_MonMot == "Lapin")
        {
            if(_Syllabe == 0)
            {
                maSyllabe = "La";
            } else if(_Syllabe == 1)
            {
                maSyllabe = "Pin";
            }
        } else if(_MonMot == "Renard")
        {
            if (_Syllabe == 0)
            {
                maSyllabe = "Re";
            }
            else if (_Syllabe == 1)
            {
                maSyllabe = "Nard";
            }
        }
        else if (_MonMot == "Chaperon")
        {
            if (_Syllabe == 0)
            {
                maSyllabe = "Cha";
            }
            else if (_Syllabe == 1)
            {
                maSyllabe = "Pe";
            }
            else if (_Syllabe == 2)
            {
                maSyllabe = "Ron";
            }
        }
        else if (_MonMot == "Chasseur")
        {
            if (_Syllabe == 0)
            {
                maSyllabe = "Cha";
            }
            else if (_Syllabe == 1)
            {
                maSyllabe = "Sseur";
            }
        }
        else if (_MonMot == "Maison")
        {
            if (_Syllabe == 0)
            {
                maSyllabe = "Mai";
            }
            else if (_Syllabe == 1)
            {
                maSyllabe = "Son";
            }
        }
        return maSyllabe;
    }


    IEnumerator DataEvent()
    {
        yield return new WaitForSeconds(Time.deltaTime);
        monEventData = EventSystem.current.gameObject.GetComponent<StandAloneInputModuleCustom>().GetLastPointerEventDataPublic(-1);
        myEventSystem = EventSystem.current.GetComponent<EventSystem>();
    }


    IEnumerator CoroutineRestartMeliMelo()
    {
        for (int ii = 0; ii < this.transform.childCount; ii++)
        {
            if (this.transform.GetChild(ii).name != "TextTitre" && this.transform.GetChild(ii).name != "ButtonRecommencerMeliMelo" && this.transform.GetChild(ii).name != "PanelVictoire" && this.transform.GetChild(ii).name != "ButtonRetour")
            {
                Destroy(this.transform.GetChild(ii).gameObject,Time.deltaTime);
            }
        }

        yield return new WaitForSeconds(0.5f);
        nbPoints = 0;
        nbPointsMax = 0;
        CreationMots();
    }

    public void RestartMeliMelo()
    {
        StartCoroutine(CoroutineRestartMeliMelo());
    }

}
