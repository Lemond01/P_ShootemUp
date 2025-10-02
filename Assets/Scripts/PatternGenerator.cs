using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PatternGenerator : MonoBehaviour
{
    public static PatternGenerator Instance { get; private set; }

    [SerializeField]
    private PatternDetector _player;

    private List<Pattern> _patterns = new List<Pattern>();

    [SerializeField]
    private List<EnemyRef> _enemiesList = new List<EnemyRef>();
    [SerializeField]
    private List<EnemyRef> _matchingVariants = new List<EnemyRef>();
    [SerializeField]
    private List<EnemyRef> _matchingArrows = new List<EnemyRef>();

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
        Debug.Log(index);
        if(index == 0)
        {
            for (int i = 0; i < _enemiesList.Count; i++)
            {
                if (_enemiesList[i]._pattern.arrows[index] == element)
                {
                    _enemiesList[i]._image.color = Color.white;

                    _matchingVariants.Add(_enemiesList[i]);
                }
            }

            yield break;
        }

        bool matches = false;

        for (int i = 0; i < _matchingVariants.Count; i++)
        {
            if (_matchingVariants[i]._pattern.arrows[index] == element)
            {
                HighlighText(_matchingVariants[i], index);

                if (!_matchingArrows.Contains(_matchingVariants[i]))
                    _matchingArrows.Add(_matchingVariants[i]);

                matches = true;

                if (_matchingVariants[i]._pattern.arrows.Length - 1 == index)
                {
                    EnemyTest enemy = _matchingVariants[i]._enemy;


                    _enemiesList.Remove(_matchingVariants[i]);
                    _matchingVariants.Remove(_matchingVariants[i]);

                    enemy.RemoveFromGame();

                    _player.ClearArray();
                }
            }
        }

        if (!matches)
            _player.ClearArray();

        _matchingVariants = ExcludeElementsAndCleanItems(_matchingVariants, _matchingArrows);

        _matchingArrows.Clear();

        yield break;
    }

    private List<EnemyRef> ExcludeElementsAndCleanItems(List<EnemyRef> originalList, List<EnemyRef> itemsToCompare)
    {
        List<EnemyRef> itemsToClean = originalList.Except(itemsToCompare).ToList();

        foreach (EnemyRef item in itemsToClean)
        {
            ClearPattern(item);
        }

        return originalList.Intersect(itemsToCompare).ToList();
    }

    private void HighlighText(EnemyRef enemyRef, int index)
    {
        char[] arrowsToHighlight = new char[index];
        char[] normalArrows = new char[enemyRef._text.textInfo.characterCount - (index - 1)];

        for (int i = 0; i < index; i++)
        {
            arrowsToHighlight[i] = enemyRef._text.textInfo.characterInfo[i].character;
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
