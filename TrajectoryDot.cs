using JetBrains.Annotations;
using UnityEngine;

public class TrajectoryDot : MonoBehaviour
{
    public GameObject dotPrefab;
    public int numberOfDots = 20;
    private GameObject[] dots;
    public float timeStep = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dots = new GameObject[numberOfDots];
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab);
            dots[i].SetActive(false);
        }

    }

    // Update is called once per frame
    public void ShowDots(Vector2 velocity, Vector2 startPos )

    {
        for (int i = 0; i < numberOfDots; i++)
        {
            float time = i * timeStep;
            Vector2 position = startPos +
                               velocity * time +
                               0.5f * Physics2D.gravity * time * time;

            dots[i].transform.position = position;
            dots[i].SetActive(true);
        }
    }
          public void HideDots()
    {
        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i].SetActive(false);
        }
    }
    }

