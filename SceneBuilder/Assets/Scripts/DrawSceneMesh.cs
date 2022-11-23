using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//绘制场景网格
public class DrawSceneMesh : MonoBehaviour
{
    #region 字段
    //整个网格的大小
    public float MeshSize = 10;
    //单个网格的大小
    public float CellSize = 0.5f;
    //网格颜色
    public Color LineColor;
    //网格材质
    public Material LineMat;
    //网格中心点
    public Vector3 MeshCenter = Vector3.zero;

    //网格线的起始与终止点
    List<Vector3[]> m_linePoints = new List<Vector3[]>();
    #endregion

    #region unity回调
    // Start is called before the first frame update
    void Start()
    {
        Initialized();
    }

    //绘制网格
    void OnPostRender()
    {
        LineMat.SetPass(0);

        GL.PushMatrix();

        GL.Begin(GL.LINES);
        for (int i = 0; i < m_linePoints.Count; i++)
        {
            GL.Vertex(m_linePoints[i][0]);
            GL.Vertex(m_linePoints[i][1]);
        }
        GL.End();

        GL.PopMatrix();
    }

    #endregion

    #region 方法
    void Initialized()
    {
        m_linePoints.Clear();

        LineMat.SetColor("_Color", LineColor);

        //计算单排网格数量
        int cellCount = (int)(MeshSize / CellSize);

        //计算横线的顶点
        for (int i = 0; i < cellCount/2; i++)
        {
            //左上
            Vector3[] points1 = new Vector3[2];
            points1[0] = new Vector3(MeshCenter.x, MeshCenter.y, MeshCenter.z + CellSize * i);
            points1[1] = new Vector3(MeshCenter.x - MeshSize / 2, MeshCenter.y, MeshCenter.z + CellSize * i);

            //左下
            Vector3[] points2 = new Vector3[2];
            points2[0] = new Vector3(MeshCenter.x, MeshCenter.y, MeshCenter.z - CellSize * i);
            points2[1] = new Vector3(MeshCenter.z - MeshSize / 2, MeshCenter.y, MeshCenter.z - CellSize * i);

            //右上
            Vector3[] points3 = new Vector3[2];
            points3[0] = new Vector3(MeshCenter.x, MeshCenter.y, MeshCenter.z + CellSize * i);
            points3[1] = new Vector3(MeshCenter.x + MeshSize / 2, MeshCenter.y, MeshCenter.z + CellSize * i);

            //右下
            Vector3[] points4 = new Vector3[2];
            points4[0] = new Vector3(MeshCenter.x, MeshCenter.y, MeshCenter.z - CellSize * i);
            points4[1] = new Vector3(MeshCenter.z + MeshSize / 2, MeshCenter.y, MeshCenter.z - CellSize * i);

            m_linePoints.Add(points1);
            m_linePoints.Add(points2);
            m_linePoints.Add(points3);
            m_linePoints.Add(points4);
        }

        //计算竖线的顶点
        for (int i = 0; i < cellCount/2; i++)
        {
            //左上
            Vector3[] points1 = new Vector3[2];
            points1[0] = new Vector3(MeshCenter.x - CellSize * i, MeshCenter.y, MeshCenter.z);
            points1[1] = new Vector3(MeshCenter.x - CellSize * i, MeshCenter.y, MeshCenter.z+ MeshSize / 2);

            //左下
            Vector3[] points2 = new Vector3[2];
            points2[0] = new Vector3(MeshCenter.x - CellSize * i, MeshCenter.y, MeshCenter.z);
            points2[1] = new Vector3(MeshCenter.x - CellSize * i, MeshCenter.y, MeshCenter.z - MeshSize / 2);

            //右上
            Vector3[] points3 = new Vector3[2];
            points3[0] = new Vector3(MeshCenter.x + CellSize * i, MeshCenter.y, MeshCenter.z);
            points3[1] = new Vector3(MeshCenter.x + CellSize * i, MeshCenter.y, MeshCenter.z + MeshSize / 2);

            //右下
            Vector3[] points4 = new Vector3[2];
            points4[0] = new Vector3(MeshCenter.x + CellSize * i, MeshCenter.y, MeshCenter.z);
            points4[1] = new Vector3(MeshCenter.x + CellSize * i, MeshCenter.y, MeshCenter.z - MeshSize / 2);

            m_linePoints.Add(points1);
            m_linePoints.Add(points2);
            m_linePoints.Add(points3);
            m_linePoints.Add(points4);
        }
    }
    #endregion

}
