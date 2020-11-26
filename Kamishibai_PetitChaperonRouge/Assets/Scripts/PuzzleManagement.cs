using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PuzzleManagement : MonoBehaviour
{
    public Sprite imageBasePuzzle;
    public GameObject puzzleBaseGO;
    private int nbPiecesX = 4;
    private int nbPiecesY = 4;
    public GameObject piecePrefab;
    private GameObject[,] puzzlePieces;
    private Vector2[,] pos0Puzzle;
    private int[] indPosProche = new int[2];
    public Sprite[] spritePieces;
    private int[,] indPosProcheActual;

    PointerEventData monEventData;
    EventSystem myEventSystem;

    public GameObject panelMain;
    private Vector2 sizePanelMain;

    private float largeurPiece;
    private float longueurPiece;

    private GameObject pieceToMove;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Starting());
        largeurPiece = piecePrefab.GetComponent<RectTransform>().sizeDelta.x;
        longueurPiece = piecePrefab.GetComponent<RectTransform>().sizeDelta.y;
    }

    IEnumerator Starting()
    {
        yield return new WaitForSeconds(LivreManagement.deltaTime);
        monEventData = EventSystem.current.gameObject.GetComponent<StandAloneInputModuleCustom>().GetLastPointerEventDataPublic(-1);
        myEventSystem = GameObject.Find("Main Camera").GetComponent<LivreManagement>().monEvent;

        yield return new WaitForSeconds(1);

        puzzleBaseGO.GetComponent<Image>().sprite = imageBasePuzzle;
        puzzlePieces = new GameObject[nbPiecesX, nbPiecesY];
        pos0Puzzle = new Vector2[nbPiecesX, nbPiecesY];

        sizePanelMain = new Vector2(panelMain.GetComponent<RectTransform>().sizeDelta.x, panelMain.GetComponent<RectTransform>().sizeDelta.y);

        int spriteNum = 0;
        for (int jj = 0; jj < puzzlePieces.GetLength(0); jj++)
        {
            for (int ii = 0; ii < puzzlePieces.GetLength(1); ii++)
            {
                puzzlePieces[ii, jj] = Instantiate(piecePrefab, puzzleBaseGO.transform);
                puzzlePieces[ii, jj].GetComponent<RectTransform>().sizeDelta = new Vector2(puzzleBaseGO.GetComponent<RectTransform>().sizeDelta.x / nbPiecesX, puzzleBaseGO.GetComponent<RectTransform>().sizeDelta.y / nbPiecesY);
                //Changer SizeDelta de la sousPiece
                puzzlePieces[ii, jj].transform.GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(puzzleBaseGO.GetComponent<RectTransform>().sizeDelta.x / nbPiecesX, puzzleBaseGO.GetComponent<RectTransform>().sizeDelta.y / nbPiecesY);
                puzzlePieces[ii, jj].transform.GetChild(0).GetChild(0).GetComponent<RectTransform>().sizeDelta = new Vector2(puzzleBaseGO.GetComponent<RectTransform>().sizeDelta.x / nbPiecesX, puzzleBaseGO.GetComponent<RectTransform>().sizeDelta.y / nbPiecesY);

                puzzlePieces[ii, jj].GetComponent<RectTransform>().anchoredPosition = new Vector2(ii * puzzleBaseGO.GetComponent<RectTransform>().sizeDelta.x / nbPiecesX, (puzzlePieces.GetLength(0) - jj) * puzzleBaseGO.GetComponent<RectTransform>().sizeDelta.y / nbPiecesY);
                puzzlePieces[ii, jj].transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = spritePieces[spriteNum];
                puzzlePieces[ii, jj].transform.name = "PiecePuzzle" + spriteNum;
                spriteNum++;
                puzzlePieces[ii, jj].transform.parent = puzzlePieces[ii, jj].transform.parent.parent;
                pos0Puzzle[ii, jj] = puzzlePieces[ii, jj].GetComponent<RectTransform>().anchoredPosition;
            }
        }


        DesordreStarting();
    }

    // Update is called once per frame
    void Update()
    {

        Touch touch = Input.touches[0];

        if (touch.phase == TouchPhase.Began)
        {
            pieceToMove = myEventSystem.currentSelectedGameObject.gameObject;
        }

        if (touch.phase == TouchPhase.Moved && pieceToMove.CompareTag("PiecePuzzle"))
        {
            pieceToMove.GetComponent<RectTransform>().anchoredPosition = new Vector3((touch.position.x / 2.1f) - (largeurPiece / 2), (touch.position.y / 2.1f) + (longueurPiece / 2), 0);
        }

        if (touch.phase == TouchPhase.Ended)
        {
            pieceToMove = null;
            CheckVictory();
            if (isWin)
            {
                WinFonction();
            }
        }

    }


    public void WinFonction()
    {
        Vector2 minPosProche = new Vector2(1000, 1000);

        for (int jj = 0; jj < pos0Puzzle.GetLength(0); jj++)
        {
            for (int ii = 0; ii < pos0Puzzle.GetLength(1); ii++)
            {
                puzzlePieces[ii, jj].GetComponent<RectTransform>().anchoredPosition = pos0Puzzle[ii, jj];
                puzzlePieces[ii, jj].GetComponent<Button>().interactable = false;
            }

        }
    }




    bool isWin = false;
    int nbPoints = 0;
    public void CheckVictory()
    {
        isWin = false;
        nbPoints = 0;
        //On parcourt toutes les pièces. On teste pour chaque pièce quelle est l'indice de sa position proche actuelle
        for (int jj = 0; jj < puzzlePieces.GetLength(0); jj++)
        {
            for (int ii = 0; ii < puzzlePieces.GetLength(1); ii++)
            {
                //int ii = 0;
                //int jj = 0;
                Vector2 diffPos = new Vector2(1000, 1000);
                int kk = 0;
                int tt = 0;
                for (int mm = 0; mm < pos0Puzzle.GetLength(0); mm++)
                {
                    for (int ll = 0; ll < pos0Puzzle.GetLength(1); ll++)
                    {
                        if (Mathf.Abs(pos0Puzzle[ll, mm].x - puzzlePieces[ii, jj].GetComponent<RectTransform>().anchoredPosition.x) < diffPos.x)
                        {
                            diffPos.x = Mathf.Abs(pos0Puzzle[ll, mm].x - puzzlePieces[ii, jj].GetComponent<RectTransform>().anchoredPosition.x);
                            kk = ll;
                        }
                        if (Mathf.Abs(pos0Puzzle[ll, mm].y - puzzlePieces[ii, jj].GetComponent<RectTransform>().anchoredPosition.y) < diffPos.y)
                        {
                            diffPos.y = Mathf.Abs(pos0Puzzle[ll, mm].y - puzzlePieces[ii, jj].GetComponent<RectTransform>().anchoredPosition.y);
                            tt = mm;
                        }
                    }
                }
                if (kk == ii && tt == jj)
                {
                    nbPoints++;
                }
            }
        }
        if(nbPoints == spritePieces.Length)
        {
            isWin = true;
        } else
        {
            isWin = false;
        }
    }



    

    public void DesordreStarting()
    {
        List<Vector2> indHasard = new List<Vector2>();
        for (int jj = 0; jj < puzzlePieces.GetLength(0); jj++)
        {
            for (int ii = 0; ii < puzzlePieces.GetLength(1); ii++)
            {
                indHasard.Add(new Vector2(ii, jj));
            }
        }


        float maxValueEclat = 70;
        float minValueEclat = 50;
        for (int jj = 0; jj < puzzlePieces.GetLength(0); jj++)
        {
            for (int ii = 0; ii < puzzlePieces.GetLength(1); ii++)
            {
                int ll = Random.RandomRange(0,indHasard.Count);
                Vector2 eclatement = new Vector2();
                if((int)indHasard[ll].x < nbPiecesX / 2)
                {
                    eclatement.x = Random.RandomRange(-maxValueEclat, -minValueEclat);
                } else
                {
                    eclatement.x = Random.RandomRange(minValueEclat, maxValueEclat);
                }
                if ((int)indHasard[ll].y >= nbPiecesY /2)
                {
                    eclatement.y = Random.RandomRange(-maxValueEclat, -minValueEclat);
                } else
                {
                    eclatement.y = Random.RandomRange(minValueEclat, maxValueEclat);
                }

                puzzlePieces[ii, jj].GetComponent<RectTransform>().anchoredPosition = pos0Puzzle[(int)indHasard[ll].x, (int)indHasard[ll].y]+eclatement;
                indHasard.Remove(indHasard[ll]);
            }
        }
    }



    public void RaZ_Puzzle()
    {
        for (int jj = 0; jj < puzzlePieces.GetLength(0); jj++)
        {
            for (int ii = 0; ii < puzzlePieces.GetLength(1); ii++)
            {
                Destroy(puzzlePieces[ii, jj]);
            }
        }

        StartCoroutine(Starting());
    }
}
