using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Analytics;


public class LangageScript : MonoBehaviour
{

    // Textes du Menu Principal
    public Text textTitre;
    public Text textLivreBouton;
    public Text textContenuPedagogique;
    public Text textCreditsBouton;
    public Text textQuitter;
    public Text textLectureAutoBouton;
    public Text textPopUpEnceinte;
    public Text textDontAskAgain;
    [HideInInspector] public string _TextePopUpEnceinte;
    [HideInInspector] public string _AchetePopUp;

    // Textes Panel Recommencer
    public Text textRecommencer;
    public Text textOui;
    public Text textNon;
    public Text textRecommencerLectAuto;
    public Text textOuiLectAuto;
    public Text textNonLectAuto;


    // Textes du Panel Pause
    public Text textSliderSonAmbiance;
    public Text textSliderSonIndep;
    public Text textBoutonChoixLangue;
    public Text textVolume;
    //public Text textReInitParam;


    // Textes du Livre
    [HideInInspector] public string[] textesPagesLivres;
    [HideInInspector] public string[] textesPagesLegendes;
    [HideInInspector] public string creditsPageTexte;


    // Textes des Crédits
    public Text textCredits;
    public Text textMerci;
    public Text textInCreditsSonoDeck;
    public Text textInCreditsCalicephale;
    //public Text textInCredits2;
    public Text textCreditsRetour;

    // EventSystem
    private EventSystem myeventSystem;
    public BaseEventData event_Data;

    // Textes panneau "Acheter"
    public Text textAcheter;
    public Text textRetourAcheter;

    // Start is called before the first frame update
    void Start()
    {
        myeventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();

        //Textes pages livres
        textesPagesLivres = new string[this.GetComponent<LivreManagement>().nbPagesLivre];
        textesPagesLegendes = new string[this.GetComponent<LivreManagement>().nbPagesLivre];

        if (this.GetComponent<LivreManagement>().langue == "FR")
        {
            SetTextesFr();
        } else if(this.GetComponent<LivreManagement>().langue == "EN")
        {
            SetTextesEn();
        } else if(this.GetComponent<LivreManagement>().langue == "GE")
        {
            // !!!!!!! Mettre ALLEMAND !!!!!!!
            SetTextesGe();
        }
    }


    public void SetTextesFr()
    {
        //Changement de langue
        this.GetComponent<LivreManagement>().langue = "FR";

        // Textes du Menu Principal
        textTitre.text = "Titre du Livre";
        textLivreBouton.text = "le Livre\ndu Conteur";
        textLivreBouton.fontSize = 65;
        textContenuPedagogique.text = "Contenu Pédagogique";
        textCreditsBouton.text = "Crédits";
        textQuitter.text = "Quitter";
        //textPopUpEnceinte.text = "Nous vous conseillons d'utiliser un casque ou des enceintes pour profiter au mieux de l'expérience sonore.";
        _TextePopUpEnceinte = "Nous vous conseillons d'utiliser un casque ou des enceintes pour profiter au mieux de l'expérience sonore.";
        _AchetePopUp = "Merci d'avoir profité \n de l'extrait de ce Kamishibai !!";
        textDontAskAgain.text = "Ne plus afficher";

        // Textes du Panel Pause
        textVolume.text = "Volumes";
        textSliderSonAmbiance.text = "Ambiance";
        textSliderSonIndep.text = "Bruitages";
        textBoutonChoixLangue.text = "Choix Langue";
        //textReInitParam.text = "Rénitialiser\nParamètres";

        // Textes du Livre (A remplir pour chaque livre en fonction du nombre de pages)
        textesPagesLivres[0] = "Il était une fois une petite fille que tout le monde appréciait. Sa grand-mère lui faisait plein de cadeaux. Un jour elle lui offrit un joli bonnet tout rouge tricoté avec amour. La petite fille l’adorait, qu’il pleuve ou qu’il vente elle ne le quittait jamais. Alors tout le monde l’appela Le Petit Chaperon Rouge.";
        textesPagesLivres[1] = "Un matin, sa maman lui demande :\n" +
            "– Grand-mère est malade, elle doit garder le lit, peux-tu lui apporter ce panier, j’y ai mis les galettes qu’elle préfère et une bouteille de vin <color=red> (1) </color>.\n" +
            "– J’y vais tout de suite, maman.\n" +
            "– Et surtout, Petit Chaperon Rouge ne t’écarte pas de la route.\n" +
            "– Bien sûr, salut Maman !\n" +
            "Mais à peine la porte fermée <color=red> (2) </color>, la fillette aperçoit, bondissant près du grand arbre, un lièvre au pelage tout blanc, aux yeux tout rouges... <color=red> (3) </color>";
        textesPagesLivres[2] = "...et aux grandes dents <color=red> (1) </color> ! Il allait vers la forêt, tout excité en parlant très fort d’un grand magicien aux talents extraordinaires.";
        textesPagesLivres[3] = "Curieuse, Petit Chaperon Rouge suit le capucin. Ils s’enfoncent au cœur de la forêt, loin de la route. Enfin ils rencontrent le Grand magicien, debout dans une clairière à côté de son grand chapeau noir. Quelques animaux font la queue devant l’enchanteur, mais notre ami resquille.\n" +
            "– Salut petit lièvre blanc, aimerais-tu que je fasse un tour de magie pour toi ?\n" +
            "– J’aimerais..., j’aimerais me trouver dans un immense champs de carottes, oranges et craquantes !\n " +
            "Le lièvre ferme les yeux, le Grand magicien lève sa baguette, dessine trois cercles et soudain d’une voix forte déclame :\n" +
            "– ABRACA... <color=red> (1) </color>";
        textesPagesLivres[4] = "...DABRA ! <color=red> (1) </color>";
        textesPagesLivres[5] = "Le petit lièvre disparaît sous les applaudissement des animaux de la forêt <color=red> (1) </color>. Il ne reste pas le moindre poil blanc.\n" +
            "– Au suivant ! Salut petit renard orange, quel tour de magie aimerais-tu que je fasse pour toi ?\n" +
            "– Je voudrais..., je voudrais me trouver au milieu d’un poulailler, avec plein de poules dodues !\n " +
            "Le renard ferme les yeux, le Grand magicien lève sa baguette, dessine trois cercles et d’une voix forte déclame :\n" +
            "– ABRACA... <color=red> (2) </color>";
        textesPagesLivres[6] = "...DABRA ! <color=red> (1) </color>";
        textesPagesLivres[7] = "Le petit renard disparaît, Petit Chaperon Rouge applaudit aussi <color=red> (1) </color>. Il ne reste pas le moindre poil orange.\n" +
            "– Au suivant ! Salut petit oiseau noir, quel tour de magie aimerais-tu que je fasse pour toi ?\n" +
            "– Je souhaiterais..., je souhaiterais voyager sur la lune, plus haut que tous les autres oiseaux <color=red> (2) </color> !\n" +
            "Le petit oiseau ferme les yeux, le Grand magicien lève sa baguette, dessine trois cercles et d’une voix forte déclame :\n" +
            "– ABRACA... <color=red> (3) </color>";
        textesPagesLivres[8] = "...DABRA ! <color=red> (1) </color>";
        textesPagesLivres[9] = "L’oiseau disparaît sous les yeux du Petit Chaperon Rouge fascinée, il ne reste pas une petite plume noire. Curieuse elle regarde le Grand magicien tout poilu, qu’elle n’avait jamais rencontré et il lui semblait normal, gentil. Elle le salue avec chaleur. Le grand débraillé à la queue touffue au sourire roublard lui demande :\n" +
            "– Salut petite fille toute rouge, toi aussi tu souhaites que je fasse un tour de magie ?\n" +
            "– S’il vous plaît..., s’il vous plaît, monsieur le magicien, envoyez-moi chez ma grand-mère, elle est malade et je lui apporte ce panier avec des galettes et du vin <color=red> (1) </color>.\n" +
            "– Oh! C’est mignon tout plein. Alors vite je vais exhausser ton vœux, tu es prête ?\n" +
            "La petite fille ferme les yeux, la baguette magique se lève, s’agite et une voix caverneuse éructe :\n" +
            "– ABRACA... <color=red> (2) </color>";
        textesPagesLivres[10] = "...DABRA ! <color=red> (1) </color>";
        textesPagesLivres[11] = "Le Petit Chaperon Rouge et le Grand magicien ont disparu, il ne reste pas le moindre bout de laine rouge, pas le moindre petit poil gris.\n" +
            "Petit Chaperon Rouge n’est pas arrivé chez sa grand-mère, elle est là, au milieu d’un grand champs de fleurs <color=red> (1) </color>. Mais ou est le grand magicien ?...";
        textesPagesLivres[12] = "... là, tout près d’une maison, celle de la grand-mère.\n" +
            "Et il use de tout ses talents, fait disparaître la vieille <color=red> (1) </color>, mets ses habits et puis confortablement s’installe dans son lit pour attendre le Petit Chaperon Rouge, un sourire gourmand sur les lèvres.";
        textesPagesLivres[13] = "La petite fille arrive, joyeuse, c’est la maison de Grand-mère, elle entre. <color=red> (1) </color>";
        textesPagesLivres[14] = "Comme elle a changée :\n" +
            "– Bonjour Mamie, comment vas-tu ? Mais, comme tu as de grands yeux !\n" +
            "– C’est pour mieux te voir, mon enfant !\n" +
            "Grand-mère a une voix bizarre, la même voix que le grand débraillé à la queue touffue et au sourire roublard.";
        textesPagesLivres[15] = "– Mamie, comme tu as de grandes oreilles !\n" +
            "– C’est pour mieux t’entendre ma petite fille.\n" +
            "– Ta bouche, comme tu as une grande bouche !\n" +
            "– C’est pour mieux énoncer mes sortilèges.";
        textesPagesLivres[16] = "La voix est tonitruante. La petite fille est terrorisée. Par magie elle disparaît instantanément. <color=red> (1) </color>";
        textesPagesLivres[17] = "Mais la voix est si forte qu’un chasseur qui passait par là s’interroge :\n" +
            "– Bizarre, je ne savais pas que la vieille s’était mise à la magie.\n" +
            "Il s’approche de la maison <color=red> (1) </color>, toque une fois, deux fois à la porte, vite fait tomber la chevillette, entre <color=red> (2) </color>, et tout de suite comprend la situation.\n" +
            "Discrètement il prend la baguette magique, et tel un magicien expérimenté la lève, fait les trois cercleset crie :\n" +
            "– ABRACA... <color=red> (3) </color>";
        textesPagesLivres[18] = "...DABRA ! <color=red> (1) </color>";
        textesPagesLivres[19] = "Aussitôt, le loup grand magicien disparaît, et, la grand-mère un peu endormie, Petit Chaperon Rouge un peu engourdie, le renard et le lièvre tout tremblotants sortent du chapeau magique. Ils se mettent à danser, heureux d’être à nouveau libres.\n" +
            "Qui pense au petit oiseau et au grand loup ?\n \n" +
            "Le chasseur, intimidé, prononce encore quelques formules magiques qu’il avait entendu dans sa jeunesse :";
        textesPagesLivres[20] = "– ABRACADABRA, ABRACADABRA ! <color=red> (1) </color>\n" +
            "Le Grand débraillé à la queue touffue ne réapparut jamais. Parfois au fond du chapeau on entend son rire roublard, surtout les nuits de pleine lune <color=red> (2) </color>.";

        creditsPageTexte = "Le Petit Chaperon rouge et le Magicien | Takacs Marie | Callicéphale 2015";

        // Textes de la légende des images des pages (A remplir pour chaque livre en fonction du nombre de pages)
        textesPagesLegendes[0] = "";
        textesPagesLegendes[1] = "Tirez doucement sur la carte !";
        textesPagesLegendes[2] = "";
        textesPagesLegendes[3] = "Gardez une pause avant de tirer la carte !";
        textesPagesLegendes[4] = "";
        textesPagesLegendes[5] = "Gardez une pause avant de tirer la carte !";
        textesPagesLegendes[6] = "";
        textesPagesLegendes[7] = "Gardez une pause avant de tirer la carte !";
        textesPagesLegendes[8] = "";
        textesPagesLegendes[9] = "Gardez une pause avant de tirer la carte !";
        textesPagesLegendes[10] = "";
        textesPagesLegendes[11] = "";
        textesPagesLegendes[12] = "";
        textesPagesLegendes[13] = "";
        textesPagesLegendes[14] = "";
        textesPagesLegendes[15] = "";
        textesPagesLegendes[16] = "";
        textesPagesLegendes[17] = "Gardez une pause avant de tirer la carte !";
        textesPagesLegendes[18] = "";
        textesPagesLegendes[19] = "";
        textesPagesLegendes[20] = "Laisser un temps avant de terminer l'histoire !";

        if (this.GetComponent<LivreManagement>().pageLivreArray.Length > 0)
        {
            for (int ii = 0; ii < textesPagesLivres.Length; ii++)
            {
                this.GetComponent<LivreManagement>().pageLivreArray[ii].transform.Find("TextPage").GetComponent<Text>().text = textesPagesLivres[ii];
                this.GetComponent<LivreManagement>().pageLivreArray[ii].transform.Find("ImagePageLiseret").Find("ImagePage").Find("TextLegendeImage").GetComponent<Text>().text = textesPagesLegendes[ii];
                this.GetComponent<LivreManagement>().pageLivreArray[ii].transform.Find("TextCredits").GetComponent<Text>().text = creditsPageTexte;
            }
        }

        // Textes des Crédits
        textCredits.text = "Crédits";
        textMerci.text = "Merci d'avoir profité de ce Kamishibaï !!";
        textInCreditsSonoDeck.text = "Application développée par SONODECK :\n\n\n\n" +
            "Florian Costes (Environnement sonore) \n" +
            "Romain Rainaud (Programmation)\n\n" +
            "Avec la voix de Ninon Juniet \n\n\n" +
            "Icônes : \n\n ";
        textInCreditsCalicephale.text = "Kamishibai disponible aux éditions Callicéphale";
        //textInCredits2.text = "Retrouvez-nous sur :";
        textCreditsRetour.text = "Retour";

        // Textes Panel Recommencer
        textRecommencer.text = "Voulez-vous reprendre à la dernière page sauvegardée ?";
        textOui.text = "Oui";
        textNon.text = "Non";
        textRecommencerLectAuto.text = textRecommencer.text;
        textOuiLectAuto.text = textOui.text;
        textNonLectAuto.text = textNon.text;

        // Texte Lecture Auto
        textLectureAutoBouton.text = "le Livre\nLu";

        //Depart
        if (myeventSystem.GetComponent<EventSystem>().currentSelectedGameObject != null)
        {
            if (myeventSystem.GetComponent<EventSystem>().currentSelectedGameObject.transform.parent.name == "Panel_LangueDepart")
            {
                if (!this.GetComponent<LivreManagement>().panelPause.activeSelf)
                {
                    this.GetComponent<LivreManagement>().panelPause.SetActive(true);
                }
                myeventSystem.GetComponent<EventSystem>().currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().buttonChoixLangue.GetComponent<Button>().interactable = true;
            }
        }


        //Acheter Panel
        textAcheter.text = "Débloquer le \ncontenu complet";
        textRetourAcheter.text = "Retour";
    }

    public void SetTextesEn()
    {
        //Changement de langue
        this.GetComponent<LivreManagement>().langue = "EN";

        // Textes du Menu Principal
        textTitre.text = "Book Title";
        textLivreBouton.text = "Storyteller\nBook";
        textLivreBouton.fontSize = 65;
        textContenuPedagogique.text = "Pedagogic Content";
        textCreditsBouton.text = "Credits";
        textQuitter.text = "Quit";

        // Textes du Panel Pause
        textVolume.text = "Volumes";
        textSliderSonAmbiance.text = "Ambient Sound";
        textSliderSonIndep.text = "Sound effects";
        textBoutonChoixLangue.text = "Language Choice";
        //textReInitParam.text = "Renitialize\nParameters";

        // Textes du Livre (A remplir pour chaque livre en fonction du nombre de pages)
        textesPagesLivres[0] = "Hello, I am a book and you are on the first page.";
        textesPagesLivres[1] = "Hi, I am still a book and you are on the second page.";
        textesPagesLivres[2] = "I change the text because this one sucks !!";
        textesPagesLivres[3] = "";
        textesPagesLivres[4] = "";
        textesPagesLivres[5] = "";
        textesPagesLivres[6] = "";
        textesPagesLivres[7] = "";
        textesPagesLivres[8] = "";
        textesPagesLivres[9] = "";
        textesPagesLivres[10] = "";
        textesPagesLivres[11] = "";
        textesPagesLivres[12] = "";
        textesPagesLivres[13] = "";
        textesPagesLivres[14] = "";
        textesPagesLivres[15] = "";
        textesPagesLivres[16] = "";
        textesPagesLivres[17] = "";
        textesPagesLivres[18] = "";
        textesPagesLivres[19] = "";
        textesPagesLivres[20] = "";

        creditsPageTexte = "Little Red Riding Hood and the wizard | Takacs Marie | Calicéphale 2015";

        // Textes de la légende des images des pages (A remplir pour chaque livre en fonction du nombre de pages)
        textesPagesLegendes[0] = "";
        textesPagesLegendes[1] = "Tirez doucement sur la carte !";
        textesPagesLegendes[2] = "";
        textesPagesLegendes[3] = "Gardez une pause avant de tirer la carte !";
        textesPagesLegendes[4] = "";
        textesPagesLegendes[5] = "Gardez une pause avant de tirer la carte !";
        textesPagesLegendes[6] = "";
        textesPagesLegendes[7] = "Gardez une pause avant de tirer la carte !";
        textesPagesLegendes[8] = "";
        textesPagesLegendes[9] = "Gardez une pause avant de tirer la carte !";
        textesPagesLegendes[10] = "";
        textesPagesLegendes[11] = "";
        textesPagesLegendes[12] = "";
        textesPagesLegendes[13] = "";
        textesPagesLegendes[14] = "";
        textesPagesLegendes[15] = "";
        textesPagesLegendes[16] = "";
        textesPagesLegendes[17] = "Gardez une pause avant de tirer la carte !";
        textesPagesLegendes[18] = "";
        textesPagesLegendes[19] = "";
        textesPagesLegendes[20] = "Marquez la fin de l'histoire !";

        if (this.GetComponent<LivreManagement>().pageLivreArray.Length > 0)
        {
            for (int ii = 0; ii < textesPagesLivres.Length; ii++)
            {
                this.GetComponent<LivreManagement>().pageLivreArray[ii].transform.Find("TextPage").GetComponent<Text>().text = textesPagesLivres[ii];
                this.GetComponent<LivreManagement>().pageLivreArray[ii].transform.Find("ImagePageLiseret").Find("ImagePage").Find("TextLegendeImage").GetComponent<Text>().text = textesPagesLegendes[ii];
                this.GetComponent<LivreManagement>().pageLivreArray[ii].transform.Find("TextCredits").GetComponent<Text>().text = creditsPageTexte;
            }
        }

        // Textes des Crédits
        textCredits.text = "Credits";
        textMerci.text = "Thanks to have benefited of that Kamishibaï!!";
        textInCreditsSonoDeck.text = "Kamishibaï edited by Calicéphale editions.\n\n" +
            "Application developped by SONODECK :\n" +
            "Florian Costes (Sound environment) and Romain Rainaud (Programmation).\n\n\n\n" +
            "Other Calicephale editions' Kamishibaï on: \n\n" +
            "Icons : \n\n ";
        //textInCredits2.text = "Find us on:";
        textCreditsRetour.text = "Back";


        // Textes Panel Recommencer
        textRecommencer.text = "Do you want to continue from the last saved page?";
        textOui.text = "Yes";
        textNon.text = "No";
        textRecommencerLectAuto.text = textRecommencer.text;
        textOuiLectAuto.text = textOui.text;
        textNonLectAuto.text = textNon.text;

        // Texte Lecture Auto
        textLectureAutoBouton.text = "Read\nBook";

        // Depart
        if (myeventSystem.GetComponent<EventSystem>().currentSelectedGameObject != null)
        {
            if (myeventSystem.GetComponent<EventSystem>().currentSelectedGameObject.transform.parent.name == "Panel_LangueDepart")
            {
                if (!this.GetComponent<LivreManagement>().panelPause.activeSelf)
                {
                    this.GetComponent<LivreManagement>().panelPause.SetActive(true);
                }
                myeventSystem.GetComponent<EventSystem>().currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().buttonChoixLangue.GetComponent<Button>().interactable = true;
            }
        }
    }




    public void SetTextesGe()
    {
        //Changement de langue
        this.GetComponent<LivreManagement>().langue = "Ge";

        // Textes du Menu Principal
        textTitre.text = "Titel des Buches";
        textLivreBouton.text = "das Buch\nGeschichtenerzählers";
        textLivreBouton.fontSize = 45;
        textContenuPedagogique.text = "Lehrinhalte";
        textCreditsBouton.text = "Credits";
        textQuitter.text = "Leave";

        // Textes du Panel Pause
        textVolume.text = "Volumen";
        textSliderSonAmbiance.text = "Ambient Sound";
        textSliderSonIndep.text = "Soundeffekte";
        textBoutonChoixLangue.text = "Wahl Sprache";
        //textReInitParam.text = "Renitialize\nParameters";

        // Textes du Livre (A remplir pour chaque livre en fonction du nombre de pages)
        textesPagesLivres[0] = "Es war einmal ein kleines Mädchen, das von allen geliebt wurde. Und besonders seine Großmutter machte ihm viele, viele Geschenke. Eines Tages gab sie ihm ein wunderschönes, rotes Mützchen, das sie liebevoll selbst gestrickt hatte. Das kleine Mädchen mochte die Mütze sehr und trug sie immer, ob es nun regnete oder stürmte. Also nannten es alle Rotkäppchen.";
        textesPagesLivres[1] = "Eines Morgens sprach die Mutter: \n" +
            "- Großmutter ist krank und liegt im Bett. Kannst du ihr diesen Korb bringen? Ich habe ihr Kuchen eingepackt, den sie so mag, und eine Flasche Wein.\n" +
            "- Ich gehe sofort, Mama. \n" +
            "- Doch pass’ auf, Rotkäppchen, verlass’ niemals den Weg! \n" +
            "- Aber nein, Mama! Bis bald! \n" +
            "Sobald es die Tür hinter sich zugezogen hatte, sah das kleine Mädchen in der Nähe eines Baumes einen Hasen mit schneeweißem Fell, roten Augen ... (1)";
        textesPagesLivres[2] = "... und zwei großen Zähnen! Er lief ganz aufgeregt Richtung Wald und sprach laut von einem großen Zauberer mit ungeheuren Kräften.";
        textesPagesLivres[3] = "Neugierig geworden folgte Rotkäppchen dem Hasen tief in den Wald, weit ab vom Weg. Nach einiger Zeit stießen sie tatsächlich auf einer Lichtung auf den Großen Zauberer: hoch aufgerichtet mit seinem großen schwarzen Hut in der einen und seinem Zauberstab in der anderen Hand. Einige Tiere hatten sich bereits vor dem Zauberer aufgereiht, doch unser Freund trat direkt auf ihn zu.\n" +
            "- Hallo, kleiner weißer Hase. Soll ich einen Zaubertrick für dich machen?\n" +
            "- Oh ja!, ich wäre gerne ... wäre gerne ...  in einem riesigen Feld voller Möhren, ganz orangefarben und knackig!\n" +
            "Der Hase schloss die Augen, der Große Zauberer hob seinen Zauberstab in die Höhe, ließ ihn dreimal kreisen und sprach jäh mit lauter Stimme: Abraka ... (1)";
        textesPagesLivres[4] = "... dabra! (1)";
        textesPagesLivres[5] = "Und schwuppdiwupp verschwand der kleine Hase unter dem Applaus (1) der Tiere des Waldes. Kein einziges seiner weißen Haare blieb mehr übrig.\n" +
            "- Auf ein Neues ...! Holla, kleiner roter Fuchs,  welchen Zaubertrick soll ich denn für dich machen?\n" +
            "- Och, ich ..., ich wäre gern ..., inmitten eines Hühnerstalls, mit lauter gutgenährten Hühnern!\n" +
            "Der Fuchs schloss die Augen, der Große Zauberer hob seinen Zauberstab in die Höhe, ließ ihn dreimal kreisen und sprach mit lauter Stimme: Abraka ... (2)";
        textesPagesLivres[6] = "... dabra! (1)";
        textesPagesLivres[7] = "Und schwuppdiwupp verschwand der kleine Fuchs, sodass auch Rotkäppchen klatschte (1). Kein einziges seiner roten Haare blieb mehr übrig.\n" +
            "- Auf ein Neues ...! Hallo, hübsches, kleines Vögelchen, was soll ich denn für dich zaubern?\n" +
            "- Oh, ich würde gerne ..., ich würde gerne ..., bis zum Mond fliegen, höher als alle anderen Vögel!\n" +
            "Der kleine Vogel schloss die Augen, der Große Zauberer hob seinen Zauberstab in die Höhe, ließ ihn dreimal kreisen und sprach mit lauter Stimme: Abraka ... (2)";
        textesPagesLivres[8] = "... dabra! (1)";
        textesPagesLivres[9] = "Und schwuppdiwupp verschwand der kleine Vogel unter den Augen des ganz erstaunten Rotkäppchens. Keine einzige seiner schwarzen Federn blieb mehr übrig.\n" +
            "Fasziniert blickte das kleine Mädchen auf den über und über behaarten Großen Zauberer. Es hatte ihn noch nie zuvor getroffen und er schien nett zu sein, und gar nicht so unheimlich. Es grüßte ihn freundlich. Der große, etwas zerzauste Zauberer mit buschigem Schwanz und einem listigen Lächeln sprach:\n" +
            "- Hallo, kleines Mädchen mit dem roten Käppchen, möchtest du auch, dass ich einen Zaubertrick nur für dich mache?\n" +
            "- Bitte ..., bitte, Herr Zauberer, zaubern Sie mich zum Haus meiner Großmutter, sie ist krank und ich bringe ihr einen Korb mit ihrem Lieblingskuchen und einer Flasche Wein.\n" +
            "- Oh! Das ist eine Kleinigkeit. Rasch! Ich werde dir deinen Wunsch erfüllen, bist du bereit?\n" +
            "Rotkäppchen schloss die Augen, der Zauberstab hob sich, kreiste dreimal und mit tiefer Stimme ertönte es: Abraka ... (1)";
        textesPagesLivres[10] = "... dabra! (1)";
        textesPagesLivres[11] = "Rotkäppchen mitsamt dem Großen Zauberer war verschwunden. Kein einziger roter Wollfaden, kein ein-ziges graues Schwanzhaar blieb zurück. Doch zum Hause seiner Großmutter ist Rotkäppchen nicht gelangt ... Dort, inmitten einer Blumenwiese ist es gelandet. Und der Zauberer, wo war der ...?";
        textesPagesLivres[12] = "Na da!, direkt neben dem Haus!, dem Haus der Großmutter (1). Und er nutzte all seine Zauberkraft, drückte die Klinke herunter, öffnete die Tür, ließ die Großmutter ver-schwinden, zog sich ihre Kleider an und setzte sich dann ganz bequem ins Bett, um auf Rotkäppchen zu warten,... ein gieriges Lächeln auf den Lippen ...";
        textesPagesLivres[13] = "Das kleine Mädchen kam fröhlich heran. Das war ja das Haus der Großmutter! Es öffnete die Tür und trat ein. (1)";
        textesPagesLivres[14] = "Aber wie Großmutter sich verändert hatte ...!\n" +
            "- Guten Tag, Großmama, wie geht es dir ...? Doch warum hast du denn so große Augen?\n" +
            "- Damit ich dich besser sehen kann, mein Kind!\n" +
            "Großmutter hatte eine merkwürdige Stimme, die gleiche Stimme wie der große Zerzauste mit dem buschigen Schwanz und dem listigen Lächeln.";
        textesPagesLivres[15] = "- Großmutter, was hast du für große Ohren?\n" +
            "- Damit ich dich besser hören kann, mein kleines Mädchen!\n" +
            "- Aber dein Mund, was hast du für einen großen Mund!\n" +
            "- Das ist ..., damit ich ..., ... meine Zaubersprüche besser sagen kann!";
        textesPagesLivres[16] = "Die Stimme klang wie Donnerhall. Das kleine Mädchen war ganz starr vor Schreck. Auf magische Weise verschwand es augenblicklich. (1)";
        textesPagesLivres[17] = "Die Stimme war so laut, dass sich ein Jäger, der an dem Haus vorbeikam, wunderte (1).\n" +
            "- Seltsam, ich wusste nicht, dass die alte Dame Zauberei betreibt.\n" +
            "Er näherte sich dem Haus, klopfte einmal, klopfte zweimal, öffnete die Tür, trat ein und verstand sofort was geschehen war. Vorsichtig nahm er den Zauberstab und wie ein erfahrener Zauberer hob er ihn in die Höhe, ließ ihn dreimal kreisen und rief:\n" +
            "- Abraka ... (1)";
        textesPagesLivres[18] = "... dabra! (1)";
        textesPagesLivres[19] = "Augenblicklich verschwand der große Zauberwolf und die Großmutter, noch ein wenig schläfrig, das Rotkäppchen, noch ein wenig erstarrt, der Fuchs und der am ganzen Körper zitternde Hase sprangen aus dem Zauberhut. Vor lauter Glück darüber, wieder frei zu sein, fingen sie an zu tanzen.Wer denkt da noch an den kleinen Vogel und den großen Wolf? Der etwas eingeschüchterte Jäger jedenfalls murmelte weiterhin noch einige Zaubersprüche, die er in sei-ner Jugend gehört hatte: ";
        textesPagesLivres[20] = "- Abrakadabra ..., Abrakadabra ...! (1)\n" +
            "Der große Zerzauste aber mit dem buschigen Schwanz tauchte niemals wieder auf. Manchmal kann man jedoch vom Grunde seines Hutes sein listiges Lachen hören, besonders in den Nächten des Vollmonds.";

        creditsPageTexte = "Little Red Riding Hood and the wizard | Takacs Marie | Calicéphale 2015";

        // Textes de la légende des images des pages (A remplir pour chaque livre en fonction du nombre de pages)
        textesPagesLegendes[0] = "";
        textesPagesLegendes[1] = "Ziehen Sie die Karte vorsichtig heraus!";
        textesPagesLegendes[2] = "";
        textesPagesLegendes[3] = "Machen Sie eine Pause, bevor Sie die Karte ziehen!";
        textesPagesLegendes[4] = "";
        textesPagesLegendes[5] = "Machen Sie eine Pause, bevor Sie die Karte ziehen!";
        textesPagesLegendes[6] = "";
        textesPagesLegendes[7] = "Machen Sie eine Pause, bevor Sie die Karte ziehen!";
        textesPagesLegendes[8] = "";
        textesPagesLegendes[9] = "Machen Sie eine Pause, bevor Sie die Karte ziehen!";
        textesPagesLegendes[10] = "";
        textesPagesLegendes[11] = "";
        textesPagesLegendes[12] = "";
        textesPagesLegendes[13] = "";
        textesPagesLegendes[14] = "";
        textesPagesLegendes[15] = "";
        textesPagesLegendes[16] = "";
        textesPagesLegendes[17] = "Machen Sie eine Pause, bevor Sie die Karte ziehen!";
        textesPagesLegendes[18] = "";
        textesPagesLegendes[19] = "";
        textesPagesLegendes[20] = "Markieren Sie das Ende der Geschichte!";

        if (this.GetComponent<LivreManagement>().pageLivreArray.Length > 0)
        {
            for (int ii = 0; ii < textesPagesLivres.Length; ii++)
            {
                this.GetComponent<LivreManagement>().pageLivreArray[ii].transform.Find("TextPage").GetComponent<Text>().text = textesPagesLivres[ii];
                this.GetComponent<LivreManagement>().pageLivreArray[ii].transform.Find("ImagePageLiseret").Find("ImagePage").Find("TextLegendeImage").GetComponent<Text>().text = textesPagesLegendes[ii];
                this.GetComponent<LivreManagement>().pageLivreArray[ii].transform.Find("TextCredits").GetComponent<Text>().text = creditsPageTexte;
            }
        }

        // Textes des Crédits
        textCredits.text = "Credits";
        textMerci.text = "Vielen Dank, dass Sie diesen Kamishibai nutzen!!";
        textInCreditsSonoDeck.text = "Kamishibai herausgegeben von Editions Calicéphale.\n\n" +
            "Von SONODECK entwickelte Anwendung:\n" +
            "Florian Costes (Klangumgebung) und Romain Rainaud (Programmierung).\n\n\n\n" +
            "Andere Kamishibai-Ausgaben von Calicéphale über: \n\n" +
            "Symbols: \n\n ";
        //textInCredits2.text = "Finden Sie uns auf:";
        textCreditsRetour.text = "Rückkehr";


        // Textes Panel Recommencer
        textRecommencer.text = "Möchten Sie zur zuletzt gespeicherten Seite zurückkehren?";
        textOui.text = "Ja";
        textNon.text = "Nein";
        textRecommencerLectAuto.text = textRecommencer.text;
        textOuiLectAuto.text = textOui.text;
        textNonLectAuto.text = textNon.text;

        // Texte Lecture Auto
        textLectureAutoBouton.text = "das Buch\nGelesen";

        // Depart
        if (myeventSystem.GetComponent<EventSystem>().currentSelectedGameObject != null)
        {
            if (myeventSystem.GetComponent<EventSystem>().currentSelectedGameObject.transform.parent.name == "Panel_LangueDepart")
            {
                if (!this.GetComponent<LivreManagement>().panelPause.activeSelf)
                {
                    this.GetComponent<LivreManagement>().panelPause.SetActive(true);
                }
                myeventSystem.GetComponent<EventSystem>().currentSelectedGameObject.transform.parent.gameObject.SetActive(false);
                GameObject.Find("Main Camera").GetComponent<LivreManagement>().buttonChoixLangue.GetComponent<Button>().interactable = true;
            }
        }
    }





    public void OuvreLien(string monUrl)
    {
        Application.OpenURL(monUrl);
    }

    public void AddTrackerData(string nomData)
    {
        AnalyticsResult analyticsResults = Analytics.CustomEvent(nomData);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
