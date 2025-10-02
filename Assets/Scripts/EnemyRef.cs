using TMPro;
using UnityEngine;
using UnityEngine.UI;

public struct EnemyRef
{
    public EnemyRef(EnemyTest enemy, Image image, Color defaultColor, string defaultText, TextMeshProUGUI text, Pattern pattern)
    {
        _enemy = enemy;
        _image = image;
        _defaultColor = defaultColor;
        _text = text;
        _defaultText = defaultText;
        _pattern = pattern;
    }

    public EnemyTest _enemy;
    public Image _image;
    public Color _defaultColor;
    public TextMeshProUGUI _text;
    public string _defaultText;
    public Pattern _pattern;
}
