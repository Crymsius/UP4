using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTreeNode {

	public int variant { get; set;}

	public int player { get; set; }
    public int depth { get; set; }
    public int depthMax { get; set; }

    public Matrix position { get; set; }
    public Atlas myAtlas { get; set; }
    public Coord gravity { get; set; }
    public List<string> typePlayers;

    public float score { get; set; }
    public bool considered { get; set; }
    
    public Dictionary<Coord, DecisionTreeNode> children;

    public DecisionTreeNode(int _player, int _depth, int _depthMax, Matrix _position, Atlas _myAtlas, 
        Coord _gravity, float _score, Dictionary<Coord, DecisionTreeNode> _children, List<string> _typePlayers)
    {
        player = _player;
        depth = _depth;
        depthMax = _depthMax;
        position = _position;
        myAtlas = _myAtlas;
        gravity = _gravity;
        score = _score;
        children = _children;
        typePlayers = _typePlayers;

        considered = true;
		variant = VariantSelector.variant;
    }

	public IEnumerator AssignDeployment(){
		yield return new WaitForSeconds (0f);
		DeploymentTree ();
	}
    public void DeploymentTree() { // Fonction récursive mettant à jour l'arbre de décision.
		if (depth == depthMax)
            /*do nothing*/
            ;
		else if (children.Count == 0 && !position.isVictory) { // On ne considère la suite que si la position actuelle n'est pas une position de victoire
			List<Coord> playables = new List<Coord> (); // enregistrement de tous les coups jouables
			for (int i = 0; i < position.hDim; i++)
				for (int j = 0; j < position.vDim; j++)
					if (isPlayable (new Coord (i, j), gravity))
						playables.Add (new Coord (i, j));
			
			bool existsVictory = false;

			foreach (Coord play in playables) { // Création des nouveaux noeuds fils
				// On commence par effectuer le play et vérifier si des triggers sont traversés.
				children.Add (play, new DecisionTreeNode (1 - player, depth + 1, depthMax, new Matrix (position, myAtlas), myAtlas, 
					gravity, 0, new Dictionary<Coord, DecisionTreeNode> (), typePlayers));
				if (variant == 0)
					PlayThenTriggered_BastienVersion (play);
				else
					TriggeredUntilPlayed_RomainVersion (play);

				//Puis on attribue un score au plateau. Si une puissance 4 est repérée, on la signale.
				children [play].score = children [play].position.MeasureScore (children [play].player);
				if (children [play].position.isVictory)
					existsVictory = true;
			}

			foreach (Coord play in playables) {
				if (!existsVictory || (existsVictory && children [play].position.isVictory)) {
					if (depth == 0)
						GameObject.Find ("IAHandler").GetComponent<IAMain> ().StartCoroutine (children [play].AssignDeployment ());
					else
						children [play].DeploymentTree ();
				} else
					children [play].considered = false;
			}
		} else
			foreach (Coord key in children.Keys)
				if (children [key].considered) {
					if (depth == 0)
						GameObject.Find ("IAHandler").GetComponent<IAMain> ().StartCoroutine (children [key].AssignDeployment ());
					else
						children [key].DeploymentTree ();
				}
    }
	public void PlayThenTriggered_BastienVersion(Coord play){
		children[play].position.values[play] = 1 - player;
		List<int> crossed = children[play].position.CheckTriggers(play, gravity);
		foreach (int trigger in crossed)
			ExecuteTrigger66 (play, trigger);
	}
	public void TriggeredUntilPlayed_RomainVersion(Coord play){
		Coord currentPlay = play, nextPlay;
		bool next = true;
		while (myAtlas.gridDictionary [currentPlay].trigger.isTrigger && myAtlas.gridDictionary [currentPlay].trigger.triggerType != 3 && next) {
			ExecuteTrigger66 (play, myAtlas.gridDictionary [currentPlay].trigger.triggerType);
			nextPlay = children [play].position.CheckNextFloorOrTrigger (currentPlay, children [play].gravity);
			next = nextPlay != currentPlay;
			currentPlay = nextPlay;
		}
		children[play].position.values[currentPlay] = 1 - player;
		if(myAtlas.gridDictionary [currentPlay].trigger.isTrigger && next) // la seule possibilité est la présence d'un reset gravity
			ExecuteTrigger66 (play, 3);
	}

	public void ExecuteTrigger66(Coord play, int _trigger){
		switch (_trigger)
		{
		case 0:
			children[play].gravity = new Coord(-children[play].gravity.y, children[play].gravity.x);
			break;
		case 1:
			children[play].gravity = new Coord(children[play].gravity.y, -children[play].gravity.x);
			break;
		case 2:
			children[play].gravity = new Coord(-children[play].gravity.x, -children[play].gravity.y);
			break;
		case 3:
			children[play].position.ResetGravity(children[play].gravity);
			break;
		default:
			break;
		}
	}

    public bool isPlayable(Coord cell, Coord gravity) {
        bool result = false;
        if (gravity == new Coord(0, -1))
            result = result || myAtlas.gridDictionary[cell].walls.wally;
        else if (gravity == new Coord(1, 0))
            result = result || myAtlas.gridDictionary[cell].walls.wallx;
        else if (gravity == new Coord(0, 1))
            result = result || (myAtlas.gridDictionary.ContainsKey(cell + gravity) && myAtlas.gridDictionary[cell + gravity].walls.wally);
        else
            result = result || (myAtlas.gridDictionary.ContainsKey(cell + gravity) && myAtlas.gridDictionary[cell + gravity].walls.wallx);
		
		if (variant == 1) // on ajoute le jeu au-dessus / en dessous des triggers dans la variante Romaing
			result = result || (myAtlas.gridDictionary [cell].trigger.isTrigger);

        return position.values[cell] == -1 && (result || !position.values.ContainsKey(cell + gravity) || position.values[cell + gravity] != -1);
    }

    public void MAJ_Scores() // Fonction récursive mettant à jour les scores de chaque coup
    {
        if (depth == depthMax || children.Count == 0 || position.isVictory) // le score est instantané si l'on est sur une feuille
            score = (float)position.MeasureScore(player);

        else
        {
            int numberConsidered = 0;
            float result = 0;
            foreach (Coord key in children.Keys)
                if (children[key].considered)
                {
                    children[key].MAJ_Scores();
                    result += children[key].score;
                    numberConsidered++;
                }
            score = (float)position.MeasureScore(player) - 1f / 3f * result / Mathf.Max(1, numberConsidered);
        }
    }

    public void Maj_Depth(int d) // Fonction récursive mettant à jour la profondeur de chaque noeud
    {
        depth = d;

        foreach (Coord c in children.Keys)
            children[c].Maj_Depth(d + 1);
    }

    public int HowManyNodes() { // Fonction récursive de comptage permettant de savoir combien de noeuds sont actuellement considérés
        if (children.Count == 0)
            return 1;

        else
        {
            int result = 1;
            foreach (Coord key in children.Keys)
                result += children[key].HowManyNodes();
            return result;
        }
    }
}
