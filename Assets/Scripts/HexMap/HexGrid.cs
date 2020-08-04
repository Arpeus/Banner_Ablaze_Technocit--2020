
using UnityEngine.UI;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
	public int _width = 6;
	public int _height = 6;

	public Text _cellLabelPrefab;
	Canvas m_gridCanvas;

	public HexCell cellPrefab;

	HexCell[] cells;

	void Awake()
	{
		m_gridCanvas = GetComponentInChildren<Canvas>();

		cells = new HexCell[_height * _width];

		for (int z = 0, i = 0; z < _height; z++)
		{
			for (int x = 0; x < _width; x++)
			{
				CreateCell(x, z, i++);
			}
		}
	}

	void CreateCell(int x, int z, int i)
	{
		Vector3 position;
		position.x = x * 10f;
		position.y = 0f;
		position.z = z * 10f;

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
	}
}
