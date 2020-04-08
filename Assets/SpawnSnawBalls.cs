using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSnawBalls : MonoBehaviour
{

    [SerializeField] private GameObject snowBall;
    private GameObject _snowball;
    [SerializeField] private GameObject fish;
    private GameObject _fish;
    [SerializeField] private GameObject SpawnFishPoint;
    [SerializeField] private GameObject SpawnSnawBallPoint;
    List<GameObject> SnowBallList = new List<GameObject>();
    List<GameObject> FishList = new List<GameObject>();

    private bool isCoroutineExecutingSnow = false;
    private bool isCoroutineExecutingFish = false;
    private int countSnowBall = 0;
    private int countFish = 0;
    void Start()
    {
    }

    private void Update()
    {
        if(countSnowBall < 110 && !isCoroutineExecutingSnow)
        {

                _snowball = Instantiate(snowBall) as GameObject;
                _snowball.transform.position = SpawnSnawBallPoint.transform.position;
                SnowBallList.Add(_snowball);
                countSnowBall++;
                StartCoroutine(WaitOneSecondSnowBall());
        }
        if (countFish < 60 && !isCoroutineExecutingFish)
        {

            _fish = Instantiate(fish) as GameObject;
            _fish.transform.position = SpawnFishPoint.transform.position;
            FishList.Add(_fish);
            countFish++;
            StartCoroutine(WaitOneSecondFish());
        }

    }

    IEnumerator WaitOneSecondSnowBall()
    {
        isCoroutineExecutingSnow = true;
        yield return new WaitForSeconds(0.2f);
        isCoroutineExecutingSnow = false;
    }

    IEnumerator WaitOneSecondFish()
    {
        isCoroutineExecutingFish = true;
        yield return new WaitForSeconds(0.2f);
        isCoroutineExecutingFish = false;
    }
}
