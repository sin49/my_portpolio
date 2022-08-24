using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding_E_07 : MonoBehaviour//a*�˰����� �̿��� 7�� ���� ��O�� �ΰ�����
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
   public void find_not_stuckpath(node nd)//��O�� ��� �� �̵� �� ���� �� �ִ� �κп����� �ݴ� �������� ���ϰ� �о�� ��θ� �缳���ϰ� �����Ѵ�
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
    void FindPath(Vector2 StartPos, Vector2 TargetPos)//startnode���� targetnode������ ��O�� ��θ� �����
    {
        node startnode = grid.GetNodeFromWorldPoint(StartPos);
        node targetnode = grid.GetNodeFromWorldPoint(TargetPos);
       
        List<node> openList = new List<node>();
        HashSet<node> closeList = new HashSet<node>();
        openList.Add(startnode);

        while (openList.Count > 0)//���� ����� ��ġ�� �������� targetnode������ ��� ��θ� ���Ѵ�
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
            foreach (node n in grid.getneighbornode(currentnode))//(�ִ� ��� Ž�� �� ��ֹ� ���� ��δ� ó������ �ʴ´�)
            {
                if (n.obstacle || n.no_path || closeList.Contains(n))
                    continue;

                int newcurrentToNeighborCost = currentnode.G + GetDistanceCost(currentnode, n);
                if (newcurrentToNeighborCost < n.G || !openList.Contains(n))//��O�� ����� �Ÿ��� ��� �ؼ� �ּ� �Ÿ��� �������� ��θ� �����Ѵ�
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
   
    void RetracePath(node startnode, node endnode)//�ϼ��� ����Ʈ�� ����� ���� �̵� ��η� ����Ѵ�
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

    int GetDistanceCost(node nodeA, node nodeB)// �Ÿ��� ����Ѵ�
    {
        int distX = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int distY = Mathf.Abs(nodeA.gridY - nodeB.gridY);

        if (distX > distY)
            return 14 * distY + 10 * (distX - distY);
        return 14 * distX + 10 * (distY - distX);
    }
}

