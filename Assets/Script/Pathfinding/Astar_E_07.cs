using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astar_E_07 : MonoBehaviour//인공지능 길찿기 용 a*알고리즘 그리드
{
    public Vector2 gridWorldsize;
    public float nodeRadius;
  public int focus_point;
    float nodeDiameter;
    public int gridsizeX;
    public int gridsizeY;
    public int e_size_x;
    public int e_size_y;
    node[,] grid;
    public List<node> path;
    // Start is called before the first frame update
    void Start()
    {
        nodeDiameter = nodeRadius * 2;
        gridsizeX = Mathf.RoundToInt(gridWorldsize.x / nodeDiameter);
        gridsizeY = Mathf.RoundToInt(gridWorldsize.y / nodeDiameter);
        createGrid();
    }
    void createGrid()//일정한 크기의 노드로 그리드 맵을 생성
    {
        //gridsize,gridsizeYX 그리드맵의 노드 구성 수
        //gridWorldsize 노드의 크기
        grid = new node[gridsizeX, gridsizeY];
        Vector2 worlddBottomLeft = transform.position - Vector3.right * gridWorldsize.x / 2 - Vector3.up * gridWorldsize.y / 2;
        Vector2 worldpoint;
        bool obs = false;
        for (int x = 0; x < gridsizeX; x++)
        {
            for (int y = 0; y < gridsizeY; y++)
            {

                worldpoint = worlddBottomLeft + Vector2.right * (x * nodeDiameter + nodeRadius) + Vector2.up * (y * nodeDiameter + nodeRadius);
                Collider2D box = Physics2D.OverlapBox(worldpoint, new Vector2(nodeRadius, nodeRadius), 0);//box의 충돌로 통과 가능 여부 체크
                if (box != null)
                {

                    if (box.gameObject.layer == 12)//통과가 불가능한 노드
                    {
                        obs = true;
                    }
                    else
                    {
                        obs = false;
                    }
                }
                else
                {
                    obs = false;
                }
                
                    grid[x, y] = new node(obs, worldpoint, x, y);
                
               
                
            }
        }
       for(int x = 0; x < gridsizeX; x++)//적 오브젝트의 크기의 문제로 벽에 끼이는 경우를 막기 위해 벽 주변 노드를 탐색 경로에 거치지 않도록 한다
        {
            for(int y = 0; y < gridsizeY; y++)
            {
                if (grid[x, y].obstacle)
                {
                    //e_size_x,e_size_x의 갯수 만큼 벽 주변 노드를 이동경로에 넣는 것을 금지한다
                    obs = false;
                    for (int x_ = 0; x_ < e_size_x; x_++)
                    {
                        for (int y_ = 0; y_ < e_size_y; y_++)
                        {
                            if (x - x_ < 0 || x + x_ >= gridsizeX || y - y_ < 0 || y + y_ >= gridsizeY)
                            {
                                continue;
                            }
                            else
                            {
                                grid[x + x_, y + y_].no_path = true;
                                grid[x + x_, y - y_].no_path = true;
                                grid[x - x_, y + y_].no_path = true;
                                grid[x - x_, y - y_].no_path = true;
                               
                            }
                        }
                    }
                }
            }
        }
     
    }
    public bool isground(node nd)//현재 선택된 노드가 그리드의 가장자리 인지를 체크한다(가장자리 일 경우 그리드 밖을 벗어나는 것을 막는다)
    {//천장
        if (nd.gridY - 1 >= 0)
        {
            node a = grid[nd.gridX, nd.gridY - 1];
            if (a.obstacle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public bool isceiling(node nd)
    {//바닥
        if (nd.gridY + 1 < gridsizeY)
        {
            node a = grid[nd.gridX, nd.gridY + 1];
            if (a.obstacle)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    public List<node> getneighbornode(node nd)//선택된 노드의 주변 노드를 탐색한다
    {
        List<node> neighbor = new List<node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0) continue;
                int c_x = nd.gridX + x;
                int c_y = nd.gridY + y;

                if (c_x >= 0 && c_x < gridsizeX && c_y >= 0 && c_y < gridsizeY)
                {
                    neighbor.Add(grid[c_x, c_y]);
                }
            }
        }
        return neighbor;
    }
    public node GetNodeFromWorldPoint(Vector2 WorldPosition)//선택된 position의 의 위치를 노드의 위치로 반환한다
    {
        float percentX = (WorldPosition.x + gridWorldsize.x / 2) / gridWorldsize.x;
        float percentY = (WorldPosition.y + gridWorldsize.y / 2) / gridWorldsize.y;
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridsizeX - 1) * percentX);
        int y = Mathf.RoundToInt((gridsizeY - 1) * percentY);
        return grid[x, y];
    }


    private void OnDrawGizmos()//그리드 맵을 시각화
    {

        Gizmos.DrawWireCube(transform.position, new Vector2(gridWorldsize.x, gridWorldsize.y));
        if (grid != null)
        {
            foreach (node n in grid)
            {
                Gizmos.color = (n.no_path) ? Color.black : Color.white;
               // Gizmos.color = (n.obstacle) ? Color.black : Color.white;
                if (path != null)
                    if (path.Contains(n))
                        Gizmos.color = Color.yellow;
                Gizmos.DrawCube(n.pos, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }
}
