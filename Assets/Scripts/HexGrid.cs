using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour 
{

	public int _width = 6;
	public int _height = 6;

	public Color _defaultColor = Color.white;

	public HexCell _cellPrefab;
	public Text _cellLabelPrefab;

	HexCell[] m_cells;

	Canvas m_gridCanvas;
	HexMesh m_hexMesh;

	void Awake () 
	{
		m_gridCanvas = GetComponentInChildren<Canvas>();
		m_hexMesh = GetComponentInChildren<HexMesh>();

		m_cells = new HexCell[_height * _width];

		for (int z = 0, i = 0; z < _height; z++) {
			for (int x = 0; x < _width; x++) {
				CreateCell(x, z, i++);
			}
		}
	}

	void Start ()
	{
		m_hexMesh.Triangulate(m_cells);
	}

	public void ColorCell (Vector3 position, Color color)
	{
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index = coordinates.X + coordinates.Z * _width + coordinates.Z / 2;
		HexCell cell = m_cells[index];
		cell.color = color;
		m_hexMesh.Triangulate(m_cells);
	}

	void CreateCell (int x, int z, int i) 
	{
		Vector3 position;
		position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
		position.y = 0f;
		position.z = z * (HexMetrics.outerRadius * 1.5f);

		HexCell cell = m_cells[i] = Instantiate<HexCell>(_cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		cell.color = _defaultColor;

		Text label = Instantiate<Text>(_cellLabelPrefab);
		label.rectTransform.SetParent(m_gridCanvas.transform, false);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();
	}
}