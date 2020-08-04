using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class HexMesh : MonoBehaviour 
{

	Mesh m_hexMesh;
	List<Vector3> m_vertices;
	List<Color> m_colors;
	List<int> m_triangles;

	MeshCollider m_meshCollider;

	void Awake () 
	{
		GetComponent<MeshFilter>().mesh = m_hexMesh = new Mesh();
		m_meshCollider = gameObject.AddComponent<MeshCollider>();
		m_hexMesh.name = "Hex Mesh";
		m_vertices = new List<Vector3>();
		m_colors = new List<Color>();
		m_triangles = new List<int>();
	}

	public void Triangulate (HexCell[] cells) 
	{
		m_hexMesh.Clear();
		m_vertices.Clear();
		m_colors.Clear();
		m_triangles.Clear();
		for (int i = 0; i < cells.Length; i++) 
		{
			Triangulate(cells[i]);
		}
		m_hexMesh.vertices = m_vertices.ToArray();
		m_hexMesh.colors = m_colors.ToArray();
		m_hexMesh.triangles = m_triangles.ToArray();
		m_hexMesh.RecalculateNormals();
		m_meshCollider.sharedMesh = m_hexMesh;
	}

	void Triangulate (HexCell cell) 
	{
		Vector3 center = cell.transform.localPosition;
		for (int i = 0; i < 6; i++) 
		{
			AddTriangle(
				center,
				center + HexMetrics.corners[i],
				center + HexMetrics.corners[i + 1]
			);
			AddTriangleColor(cell.color);
		}
	}

	void AddTriangle (Vector3 v1, Vector3 v2, Vector3 v3) 
	{
		int vertexIndex = m_vertices.Count;
		m_vertices.Add(v1);
		m_vertices.Add(v2);
		m_vertices.Add(v3);
		m_triangles.Add(vertexIndex);
		m_triangles.Add(vertexIndex + 1);
		m_triangles.Add(vertexIndex + 2);
	}

	void AddTriangleColor (Color color) 
	{
		m_colors.Add(color);
		m_colors.Add(color);
		m_colors.Add(color);
	}
}