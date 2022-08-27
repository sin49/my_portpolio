using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding_E_07 : MonoBehaviour//a*알고리즘을 이용한 7번 적의 길찿기 인공지능
{
    
    Astar_E_07 grid;
    E_07_AI E_ai;
    Rigidbody2D rgd;
    public Transform start_pos;
    public Transform Target_pos;
  
    bool onground = false;
    bool atceiling = false;
    private void Start()
    {
        rgd = this.GetComponent<Rigidbody2D>();
        grid = GameObject.FindGameObjectWithTag("astar").GetComponent<Astar_E_07>();
        E_ai = GetComponent<E_07_AI>();
        start_pos = this.transform;
        Target_pos = this.transform.GetComponent<Unit>().Player.transform;
    }
   public void find_not_stuckpath(node nd)//길찿기 경로 중 이동 중 끼일 수 있는 부분에서는 반대 방향으로 강하게 밀어내서 경로를 재설정하게 유도한다
    {
        List<node> i = grid.getneighbornode(nd);
        for(int n = 0; n < i.Count; n++)
        {
            if (i[n].no_path)
            {
                Vector2 dir = (i[n].pos-nd.pos);
                rgd.AddForce(-1*dir.normalized * 3,ForceMode2D.Impulse);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        FindPath(start_pos.position + (Vector3.down * grid.focus_point), Target_pos.position+ (Vector3.down * grid.focus_point));
    }
    void FindPath(Vector2 StartPos, Vector2 TargetPos)//startnode에서 targetnode까지의 길찿기 경로를 만든다
    {
        node startnode = grid.GetNodeFromWorldPoint(StartPos);
        node targetnode = grid.GetNodeFromWorldPoint(TargetPos);
       
        List<node> openList = new List<node>();
        HashSet<node> closeList = new HashSet<node>();
        openList.Add(startnode);

        while (openList.Count > 0)//현재 노드의 위치를 시작으로 targetnode까지의 퇴단 경로를 구한다
        {
            node currentnode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].F < currentnode.F || openList[i].F == currentnode.F && openList[i].H < currentnode.H)
                {
                    currentnode = openList[i];
                }
            }
            openList.Remove(currentnode);
            closeList.Add(currentnode);
            if (currentnode == targetnode)
            {
                RetracePath(startnode, targetnode);
                return;
            }
            foreach (node n in grid.getneighbornode(currentnode))//(최단 경로 탐색 중 장애물 노드는 경로는 처리하지 않는다)
            {
                if (n.obstacle || n.no_path || closeList.Contains(n))
                    continue;

                int newcurrentToNeighborCost = currentnode.G + GetDistanceCost(currentnode, n);
                if (newcurrentToNeighborCost < n.G || !openList.Contains(n))//길찿기 경로의 거리를 계산 해서 최소 거리가 나오도록 경로를 수정한다
                {
                    n.G = newcurrentToNeighborCost;
                    n.H = GetDistanceCost(n, targetnode);
                    n.Pnode = currentnode;
                    if (!openList.Contains(n))
                        openList.Add(n);
                }
            }
        }
    }
   
    void RetracePath(node startnode, node endnode)//완성된 리스트를 뒤집어서 적의 이동 경로로 사용한다
    {
        List<node> path = new List<node>();
        node currentnode = endnode;
        while (currentnode != startnode)
        {
            path.Add(currentnode);
            currentnode = currentnode.Pnode;
        }
        path.Reverse();
        grid.path = path;
        E_ai.path = path;
    }

    int GetDistanceCost(node nodeA, node nodeB)// 거리를 계산한다
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }
}

