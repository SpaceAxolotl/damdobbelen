using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] public Color _baseColor, _offsetColor;
    [SerializeField] public MeshRenderer _renderer;
    [SerializeField] public Color _highlightcolor;
    private Color targetColor;

    // public Color colormemory;

    private void Start()
    {
        targetColor = _renderer.material.color; //onthoudt de originele kleur van de tile for later use.
    }

    public void Init(bool isOffset)
    {
        _renderer.material.color = isOffset ? _offsetColor : _baseColor; //als offset waar is, geef offsetkleur mee aan het object. anders geef de basecolor mee.
        targetColor = _renderer.material.color; //onthoudt de originele kleur van de tile for later use.
    }

    private void OnMouseEnter()
    {
        _renderer.material.color = _highlightcolor; //verander kleur van de tile naar highlightkleur.
    }

    private void OnMouseExit()
    {
        _renderer.material.color = targetColor; //on mouse exit, geeft de originele kleur mee
    }

}
