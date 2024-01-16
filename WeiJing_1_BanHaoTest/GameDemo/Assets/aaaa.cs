using UnityEngine;
using System.Collections;

public class aaaa : MonoBehaviour {

    public Terrain _terrain;
	// Use this for initialization
	void Start () {
        TerrainData terrainData = new TerrainData();

        terrainData.heightmapResolution = _terrain.terrainData.heightmapResolution;
        terrainData.size = _terrain.terrainData.size;
        terrainData.wavingGrassAmount = _terrain.terrainData.wavingGrassAmount;
        terrainData.wavingGrassSpeed = _terrain.terrainData.wavingGrassSpeed;
        terrainData.wavingGrassStrength = _terrain.terrainData.wavingGrassStrength;
        terrainData.wavingGrassTint = _terrain.terrainData.wavingGrassTint;
        terrainData.detailPrototypes = _terrain.terrainData.detailPrototypes;
        terrainData.treeInstances = _terrain.terrainData.treeInstances;
        terrainData.treePrototypes = _terrain.terrainData.treePrototypes;
        terrainData.alphamapResolution = _terrain.terrainData.alphamapResolution;
        terrainData.baseMapResolution = _terrain.terrainData.baseMapResolution;
        terrainData.splatPrototypes = _terrain.terrainData.splatPrototypes;

        float[,] heights = _terrain.terrainData.GetHeights(0, 0, _terrain.terrainData.heightmapResolution,
            _terrain.terrainData.heightmapResolution);
        terrainData.SetHeights(0, 0, heights);

        float[, ,] alphaMap = _terrain.terrainData.GetAlphamaps(0, 0, _terrain.terrainData.alphamapWidth, _terrain.terrainData.alphamapHeight);
        Terrain terrain = new Terrain();
        terrain.terrainData.SetAlphamaps(0, 0, alphaMap);
        GameObject _newTerrainObj;

        _newTerrainObj = Terrain.CreateTerrainGameObject(terrainData);
        _newTerrainObj.name = "TerrainNew111";  



	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
