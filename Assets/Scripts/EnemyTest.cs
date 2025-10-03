using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyTest : MonoBehaviour
{
    public Pattern enemyPattern;
    public TextMeshProUGUI txtArrows;
    public Image imgVariant;
    public int health = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Color color;

        enemyPattern = PatternGenerator.Instance.CreatePattern(6);

        txtArrows.text = new string(enemyPattern.arrowsChar);

        if (enemyPattern.arrows[0] == Elements.Red)
        {
            color = Color.red;
            imgVariant.color = color;

            PatternGenerator.Instance.AddEnemyToList(this, imgVariant, color, txtArrows.text, txtArrows, enemyPattern);
        }
        else if (enemyPattern.arrows[0] == Elements.Blue)
        {
            color = Color.blue;
            imgVariant.color = color;

            PatternGenerator.Instance.AddEnemyToList(this, imgVariant, color, txtArrows.text, txtArrows, enemyPattern);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RemoveFromGame()
    {
        Destroy(gameObject);
    }

    public void TakeDamage()
    {
        health--;
        if (health <= 0)
        {
            RemoveFromGame();
        }
    }
}
