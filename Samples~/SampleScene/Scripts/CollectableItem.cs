using GameEvents.Channels;
using UnityEngine;

public class CollectableItem : MonoBehaviour
{
    [SerializeField] private ItemType type;
    [SerializeField] private int itemCost;

    private const float TurnSpeed = 50f;
    private const float BounceSpeed = 0.5f;
    private bool _isBouncingUp = true;

    private void Update()
    {
        Bounce();
        Rotate();
    }

    private void Bounce()
    {
        if (_isBouncingUp)
        {
            var newPosition = transform.position;
            newPosition.y += BounceSpeed * Time.deltaTime;
            transform.position = newPosition;
            if (transform.position.y >= 2)
            {
                _isBouncingUp = false;
            }
        }
        else
        {
            var newPosition = transform.position;
            newPosition.y -= BounceSpeed * Time.deltaTime;
            transform.position = newPosition;
            if (transform.position.y <= 1)
            {
                _isBouncingUp = true;
            }
        }
    }

    private void Rotate()
    {
        var currentRotation = transform.rotation.eulerAngles;
        currentRotation.y += TurnSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(currentRotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        GameplayChannel.OnItemCollected?.Invoke(type, itemCost);
        Destroy(gameObject);
    }
}

public enum ItemType
{
    Sphere,
    Cube
}