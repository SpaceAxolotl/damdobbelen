using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] public Color _baseColor, _offsetColor;
    [SerializeField] public MeshRenderer _renderer;
    [SerializeField] public Color _highlightcolor;
    public Color colormemory;

    public void Init(bool isOffset)
    {
        _renderer.material.color = isOffset ? _offsetColor : _baseColor;
       
    }

    private void OnMouseEnter()
    {
        Debug.Log(_renderer.material.color);
        colormemory = _renderer.material.color;
        // _highlight.SetActive(true);
        _renderer.material.color = _highlightcolor;
        
    }

    public void OnMouseExit()
    {
        _renderer.material.color = colormemory;
        // _highlight.SetActive(false);
        // _renderer.material.color = GetComponent<Renderer.material.color>();
        Debug.Log(colormemory);
            
    }

}
