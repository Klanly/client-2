using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPathFinder : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        //1为可行走点 0为阻挡点
        //PathFinder mPathFinder = new PathFinder(grids);
        //mPathFinder.Formula = HeuristicFormula.Manhattan;
        //mPathFinder.Diagonals = true;
        //mPathFinder.HeavyDiagonals = false;
        //mPathFinder.HeuristicEstimate = 2;
        //mPathFinder.PunishChangeDirection = false;
        //mPathFinder.TieBreaker = false;
        //mPathFinder.SearchLimit = 50000;
        //mPathFinder.ReopenCloseNodes = true;
        //List<PathFinderNode> path = mPathFinder.FindPath(new Point(0, 0), new Point(5, 5));

       
    }
    SceneInfo sceneInfo;
    private void TT()
    {
        sceneInfo = ConfigManager.GetSceneConfigInfo("test_scene", true);
        sceneInfo.ParseGrids();
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            TT();
        }
    }


}
