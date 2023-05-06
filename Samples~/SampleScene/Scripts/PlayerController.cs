using System.Collections;
using GameEvents.Channels;
using GameEventsSystem.Runtime;
using UnityEngine;

public class PlayerController : BaseSubscriber
{
    [SerializeField] private float playerSpeed;
    [SerializeField] private float hp;
    [SerializeField] private int winConditionScore;

    private CharacterController _controller;
    private int _score;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        StartCoroutine(LifespanCoroutine());
    }

    [GameplayChannel.OnItemCollected]
    private void UpdateScore(ItemType itemType, int itemCost)
    {
        _score += itemCost;
        hp += itemCost;
        Debug.Log($"Collected item {itemType}");

        if (_score >= winConditionScore)
        {
            StopAllCoroutines();
            GameplayChannel.OnGameWin?.Invoke(_score);
        }
    }

    private IEnumerator LifespanCoroutine()
    {
        while (hp > 0)
        {
            hp--;
            yield return new WaitForSeconds(1);
        }
        
        GameplayChannel.OnGameOver?.Invoke(_score);
    }

    private void Update()
    {
        var move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        _controller.Move(move * (Time.deltaTime * playerSpeed));
    }
}
