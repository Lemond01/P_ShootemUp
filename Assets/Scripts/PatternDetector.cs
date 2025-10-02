using System;
using UnityEngine;

public class PatternDetector : MonoBehaviour
{
    public Elements[] _inputElements;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputElements = new Elements[0];
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            AddToArray(Elements.Red);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AddToArray(Elements.Blue);
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            AddToArray(Elements.Left);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            AddToArray(Elements.Up);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            AddToArray(Elements.Right);
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            AddToArray(Elements.Down);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            StartCoroutine(PatternGenerator.Instance.ViewElements());
        }
    }

    private void AddToArray(Elements element)
    {
        if(_inputElements.Length == 0)
        {
            Array.Resize(ref _inputElements, _inputElements.Length + 1);

            _inputElements[0] = element;

            StartCoroutine(PatternGenerator.Instance.LookForMatches(element, _inputElements.Length - 1));

            return;
        }

        Array.Resize(ref _inputElements, _inputElements.Length + 1);

        _inputElements[_inputElements.Length - 1] = element;

        StartCoroutine(PatternGenerator.Instance.LookForMatches(element, _inputElements.Length - 1));
    }

    public void ClearArray()
    {
        Array.Resize(ref _inputElements, 0);
    }
}
