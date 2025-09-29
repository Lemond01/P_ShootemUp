using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PatternGenerator : MonoBehaviour
{
    public static PatternGenerator Instance { get; private set; }

    [SerializeField]
    private PatternDetector _player;

    private List<Pattern> _patterns = new List<Pattern>();

    private List<EnemyRef> _enemiesList = new List<EnemyRef>();
    private List<EnemyRef> _matchingPatterns = new List<EnemyRef>();

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddEnemyToList(EnemyTest enemy, Image image, Color defaultColor, string defaultText, TextMeshProUGUI text, Pattern pattern)
    {
        _enemiesList.Add(new EnemyRef(enemy, image, defaultColor, defaultText, text, pattern));
    }

    public IEnumerator ViewElements()
    {
        for(int i = 0; i < _enemiesList.Count; i++)
        {
            for (int j = 0; j < _enemiesList[i]._pattern.arrows.Length; j++)
            {
                Debug.Log($"Arr: {i}, {_enemiesList[i]._pattern.arrows[j].ToString()}");
            }
        }

        yield return null;
    }

    public IEnumerator LookForMatches(Elements element, int index)
    {
        if(index == 0)
        {
            for (int i = 0; i < _enemiesList.Count; i++)
            {
                //Debug.Log($"i: {i}, index: {index}, cnt: {_enemiesList.Count}");
                //Debug.Log($"{_enemiesList[i]._pattern.arrows[index].ToString()}");

                if (_enemiesList[i]._pattern.arrows[index] == element)
                {
                    //Debug.Log($"{index}, {_enemiesList[i]._pattern.arrows[index].ToString()}");
                    //Debug.Log("Imagen correcta");

                    _enemiesList[i]._image.color = Color.white;

                    _matchingPatterns.Add(_enemiesList[i]);

                    //else
                    //{
                    //    Debug.Log("Flecha correcta");

                    //    string c = "<color=red>" + _enemiesList[i]._text.textInfo.characterInfo[index - 1].character.ToString() + "</color>";

                    //    _enemiesList[i]._text.text = c;
                    //}
                }
                //else
                //{
                //    _player.ClearArray();
                //}
            }

            yield break;
        }

        for(int i = 0; i < _matchingPatterns.Count; i++)
        {
            if (_matchingPatterns[i]._pattern.arrows[index] == element)
            {
                //Debug.Log("Flecha correcta");

                HighlighText(_matchingPatterns[i], index);

                Debug.Log($"{_matchingPatterns[i]._pattern.arrows.Length - 1} | {index}");

                if (_matchingPatterns[i]._pattern.arrows.Length - 1 == index)
                {
                    Debug.Log("patron completo");

                    EnemyTest enemy = _matchingPatterns[i]._enemy;
                    
                    _matchingPatterns.Remove(_matchingPatterns[i]);

                    enemy.RemoveFromGame();

                    _player.ClearArray();
                }
            }
            //else
            //{
            //    ClearPattern(_matchingPatterns[i]);
            //    _matchingPatterns.Remove(_matchingPatterns[i]);
            //}
        }

        yield break;
    }

    private void HighlighText(EnemyRef enemyRef, int index)
    {
        char[] arrowsToHighlight = new char[index];
        char[] normalArrows = new char[enemyRef._text.textInfo.characterCount - (index - 1)];

        for (int i = 0; i < index; i++)
        {
            arrowsToHighlight[i] = enemyRef._text.textInfo.characterInfo[i].character;
            //Debug.Log(enemyRef._text.textInfo.characterInfo[i].character);
        }

        for (int i = index; i < enemyRef._text.textInfo.characterCount; i++)
        {
            normalArrows[i - (index)] = enemyRef._text.textInfo.characterInfo[i].character;
        }

        string highlightedText = "<color=red>" + new string(arrowsToHighlight) + "</color>" + new string(normalArrows);

        enemyRef._text.text = highlightedText;
    }

    private void ClearPattern(EnemyRef enemyRef)
    {
        enemyRef._image. color = enemyRef._defaultColor;
        enemyRef._text.text = enemyRef._defaultText;
    }

    public Pattern CreatePattern(int _patternSize)
    {
        //Variant variant = (Variant)Random.Range(0, 2);

        Elements[] arrows = new Elements[_patternSize];
        char[] characters = new char[_patternSize - 1];

        arrows[0] = (Elements)Random.Range(0, 2);

        for (int i = 1; i < _patternSize; i++)
        {
            arrows[i] = (Elements)Random.Range(2, 6);

            switch(arrows[i])
            {
                case Elements.Left:
                    characters[i - 1] = '\u2190';
                    break;

                case Elements.Up:
                    characters[i - 1] = '\u2191';
                    break;

                case Elements.Right:
                    characters[i - 1] = '\u2192';
                    break;

                case Elements.Down:
                    characters[i - 1] = '\u2193';
                    break;
            }
        }

        Pattern pattern = new Pattern(arrows, characters);

        if (_patterns.Count == 0)
        {
            _patterns.Add(pattern);
            
            return pattern;
        }

        for(int i = 0; i < _patterns.Count; i++)
        {
            if(pattern.Equals(_patterns[i]))
            {
                CreatePattern(_patternSize);
            }
        }

        return pattern;
    }
}
