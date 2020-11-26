using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryManagement : MonoBehaviour
{
    [HideInInspector] public int numberPieces = 8;
    public GameObject prefabPiece;
    private GameObject[] puzzlePieces;
    public Transform panelPuzzle;
    public Sprite[] spriteFacePuzzle;
    public List<Sprite> listeSpritePuzzle;

    public int pieceOnClick;
    public GameObject firstPieceClicked;

    [HideInInspector] public int nbPoints;

    public GameObject textFinish;

    //Position y = 60 ou -60
    //Position x0 = -180, +60 ensuite 

    // Start is called before the first frame update
    void Start()
    {
        CreationMemoryGame();
        
    }


    public void CreationMemoryGame()
    {
        textFinish.SetActive(false);
        for (int ii = 0; ii < spriteFacePuzzle.Length; ii++)
        {
            listeSpritePuzzle.Add(spriteFacePuzzle[ii]);
            listeSpritePuzzle.Add(spriteFacePuzzle[ii]);
        }

        puzzlePieces = new GameObject[numberPieces];
        //Instanciation des pièces
        for (int ii = 0; ii < numberPieces; ii++)
        {
            puzzlePieces[ii] = Instantiate(prefabPiece, panelPuzzle);
            if (ii < numberPieces / 2)
            {
                puzzlePieces[ii].transform.localPosition = new Vector3(-180 + ii * 120, 60, 0);
            }
            else
            {
                puzzlePieces[ii].transform.localPosition = new Vector3(-180 + (ii - numberPieces / 2) * 120, -60, 0);
            }
            int spriteToAdd = Random.RandomRange(0, listeSpritePuzzle.Count);
            puzzlePieces[ii].GetComponent<PieceMemory>().spriteFaceHidden = listeSpritePuzzle[spriteToAdd];
            listeSpritePuzzle.Remove(listeSpritePuzzle[spriteToAdd]);
        }
    }


    public void RaZ_MemoryPuzzle()
    {
        nbPoints = 0;
        for (int ii = 0;ii< puzzlePieces.Length; ii++)
        {
            Destroy(puzzlePieces[ii].gameObject);
        }

        CreationMemoryGame();
    }
}
