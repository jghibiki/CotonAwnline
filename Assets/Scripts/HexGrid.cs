using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid : MonoBehaviour {

	public int rows = 10;
	public int cols = 10;
	public float cell_radius = 1f;

	public HexCell cellPrefab;

	private HexCell[] cells;

	void Awake(){
		cells = new HexCell[rows * cols];	

		for(int z = 0, i = 0; z < rows; z++){
			for(int x = 0; x < cols; x++){
				CreateCell(x, z, i++);
			}
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void CreateCell(int x, int z, int i){
		Vector3 position;
		position.x = x * cell_radius;
		position.y = 0;
		position.z = z * cell_radius;

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
	}
}
