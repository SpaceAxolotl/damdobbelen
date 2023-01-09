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

    public void Init(bool isOffset)
    {
        _renderer.material.color = isOffset ? _offsetColor : _baseColor;
        targetColor = _renderer.material.color;
    }

    private void OnMouseEnter()
    {
        _renderer.material.color = _highlightcolor;
    }

    private void OnMouseExit()
    {
        _renderer.material.color = targetColor;
    }

}
