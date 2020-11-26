using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PieceMemory : MonoBehaviour
{
    public Sprite spriteFaceHidden;
    public Sprite spriteFaceShowed;

    public MemoryManagement scriptManager;

    private void Start()
    {
        scriptManager = GameObject.Find("PanelGlobalMemory").GetComponent<MemoryManagement>();
        GetComponent<Image>().sprite = spriteFaceShowed;
        SpriteState mySpriteState;
        mySpriteState.disabledSprite = spriteFaceHidden;
        GetComponent<Button>().spriteState = mySpriteState;
    }


    public IEnumerator CoroutineBouton()
    {
        GetComponent<Button>().interactable = false;
        yield return new WaitForSeconds(1);
        if (scriptManager.firstPieceClicked.GetComponent<PieceMemory>().spriteFaceHidden == GetComponent<PieceMemory>().spriteFaceHidden)
        {
            Destroy(scriptManager.firstPieceClicked);
            Destroy(this.gameObject);
            scriptManager.nbPoints += 2;
            if(scriptManager.nbPoints == scriptManager.numberPieces)
            {
                scriptManager.textFinish.SetActive(true);
            }
        }
        else
        {
            scriptManager.firstPieceClicked.GetComponent<Button>().interactable = true;
            GetComponent<Button>().interactable = true;
            scriptManager.firstPieceClicked = null;
        }
        scriptManager.pieceOnClick = 0;
    }

    public void FonctionBoutonClic()
    {
        scriptManager.pieceOnClick++;
        if (scriptManager.pieceOnClick == 1)
        {
            GetComponent<Button>().interactable = false;
            scriptManager.firstPieceClicked = this.gameObject;
        } else if(scriptManager.pieceOnClick == 2)
        {
            StartCoroutine(CoroutineBouton());
        }
        //Debug.Log(scriptManager.pieceOnClick);
        //
    }
}
