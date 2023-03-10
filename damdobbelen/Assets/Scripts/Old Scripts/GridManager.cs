using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _cam;

    private void Start()
    {
        //GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int i = 0; i < _width * _height; i++)
        {
            int x = i % _width;
            int y = i / _width;

            var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity); //maakt nieuwe tiles aan en geeft ze een naam.
            spawnedTile.name = $"Tile {x} {y}";
            spawnedTile.Init((x % 2 == 0) != (y % 2 == 0)); //verwijst naar code in tile.cs, geeft door of tiles offset zijn of niet. dus de bool die hij meegeeft is true of false
        }
        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10); //zorgt ervoor dat de camera in het midden van het speelveld blijft
    }
}
