using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Algorithms;
using DG;
using DG.Tweening;
public class TestPathFinder : MonoBehaviour
{
    SceneInfo sceneInfo;
    Camera myCamera;

    public GameObject testPlayer;

    // Use this for initialization
    void Start ()
    {

        


        testPlayer = GameObject.Find("player");
        myCamera = Camera.main;
        //sceneInfo = ConfigManager.GetSceneConfigInfo("test_scene", true);
        //sceneInfo.ParseGrids();

        LightmapData[] lightmapDatas = LightmapSettings.lightmaps;
        for (int i = 0; i < lightmapDatas.Length; ++i)
        {
            LightmapData lightmapData = lightmapDatas[i];
            Debug.Log("lightmapData.lightmapColor.name:"+ lightmapData.lightmapColor.name);
            Debug.Log("lightmapData.lightmapDir.name:" + lightmapData.lightmapDir.name);
            
        }
    }

    private Point sPoint;
    private Point ePoint;
    private void TT(Vector3 startPoint,Vector3 endPoint)
    {
        if (sPoint == null)
        {
            sPoint = new Point(0,0);
        }
        if (ePoint == null)
        {
            ePoint = new Point(0, 0);
        }
        Vector2 s1 = sceneInfo.PixelToGrid(startPoint.x, startPoint.z);
        Vector2 s2 = sceneInfo.PixelToGrid(endPoint.x, endPoint.z);

        sPoint.X = (int)s1.x;
        sPoint.Y = (int)s1.y;
        ePoint.X = (int)s2.x;
        ePoint.Y = (int)s2.y;

        // 1为可行走点 0为阻挡点
        PathFinder mPathFinder = new PathFinder(sceneInfo.Grids);
        mPathFinder.Formula = HeuristicFormula.Manhattan;
        mPathFinder.Diagonals = true;
        mPathFinder.HeavyDiagonals = false;
        mPathFinder.HeuristicEstimate = 2;
        mPathFinder.PunishChangeDirection = false;
        mPathFinder.TieBreaker = false;
        mPathFinder.SearchLimit = 50000;
        mPathFinder.ReopenCloseNodes = true;
        List<PathFinderNode> path = mPathFinder.FindPath(sPoint, ePoint);
        
        if (path != null && path.Count > 0)
        {
            Debug.Log(path.Count);
            int length = path.Count;
            for (int i = 0; i < length; ++i)
            {
                PathFinderNode pathFinderNode = path[i];
                
                Debug.Log(pathFinderNode.X + "/" + pathFinderNode.Y);
            }
            if (walker == null)
            {
                walker = new Walker();
            }
            walker.player = testPlayer;
            walker.path = path;
            walker.sceneInfo = sceneInfo;
            walker.DoWalk();
        }
        
    }

    private Walker walker;
    private GameObject targetGo;


    void Update()
    {
        TimerManager.Update(Time.deltaTime);
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 position = Input.mousePosition;
            
            //定义射线
            Ray mRay = myCamera.ScreenPointToRay(position);
            RaycastHit mHit;
            //判断射线是否击中地面
            if (Physics.Raycast(mRay, out mHit))
            {
                if (mHit.collider.gameObject.tag == GameObjectTags.Terrain)
                {
                    //获取目标坐标
                    if (targetGo == null)
                    {
                        Object obj = Resources.Load("Sphere");
                        targetGo = GameObject.Instantiate(obj) as GameObject;
                    }
                    targetGo.transform.position = mHit.point;

                    TT(testPlayer.transform.position, mHit.point);
                }
            }
            
        }
    }


}


public class Walker
{
    public GameObject player;
    public List<PathFinderNode> path;
    private int index = 0;
    private PathFinderNode pathFinderNode;
    private CharacterController characterController;
    public SceneInfo sceneInfo;
    private Vector3 targetPos;
    private TimerInfo timerInfo;


    public void DoWalk()
    {
        if (path == null || player == null) return;
        if (characterController == null)
        {
            characterController = player.GetComponent<CharacterController>();
        }
        if (characterController == null) return;
        
        pathFinderNode = path[index];
        targetPos = sceneInfo.GridToPixel(pathFinderNode.X, pathFinderNode.Y);
        player.transform.LookAt(targetPos);
        timerInfo = TimerManager.AddHandler(OnUpdateHandler);
    }
    
    private void OnUpdateHandler(float del)
    {
        if (characterController != null)
        {
            float distance = Vector3.Distance(targetPos, player.transform.position);
            Debug.Log(distance);
            if (distance <= 0.1f)
            {
                index++;
                if (index < path.Count)
                {
                    pathFinderNode = path[index];
                    targetPos = sceneInfo.GridToPixel(pathFinderNode.X, pathFinderNode.Y);
                    player.transform.LookAt(targetPos);
                }
                else
                {
                    TimerManager.RemoveHandler(timerInfo);
                }
            }
            else
            {
                characterController.SimpleMove(player.transform.forward);
            }
        }
    }

}
