using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField] private int _width, _height; //serializefield zorgt ervoor dat je de integer kan veranderen in Unity, zodat je variabelen buiten de code snel kunt veranderen
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private Transform _cam;

    private void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int i = 0; i < _width * _height; i++) //
        {
            int x = i % _width; //% is dat je de waarde rechts van de % haalt van je waarde links van de %. de "remainder" is je uitkomst.
            int y = i / _width; //i gedeeld door breedte.

            var spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity); //instantiate: maak het volgende aan. begint met het aanmaken van een nieuw object (tileprefab) op de volgende vector3(locatie), met de volgende rotatie.
            spawnedTile.name = $"Tile {x} {y}"; //de naam van je nieuwe spawnedtile is de positie waar hij op gespawned wordt.
            spawnedTile.Init((x % 2 == 0) != (y % 2 == 0)); //Deze code zorgt ervoor dat het spel weet welke tile "offset" (oneven) is zodat je de kleur kan veranderen van de oneven tiles.
        }
        _cam.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10); //verandert de camera positie
    }
}
