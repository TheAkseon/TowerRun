using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTower : MonoBehaviour
{
    [SerializeField] private Human _startHuman;
    [SerializeField] private Transform _distanceChercker;
    [SerializeField] private float _fixationMaxDistance;
    [SerializeField] private BoxCollider _checkColider;

    private List<Human> _humans;

    public event UnityAction<int> HumanAdded;

    private void Start()
    {
        _humans = new List<Human>();
        Vector3 spawnPoint = transform.position;
        _humans.Add(Instantiate(_startHuman, spawnPoint, Quaternion.identity, transform));
        _humans[0].Run();
        HumanAdded.Invoke(_humans.Count);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Human human))
        {
            Tower collisionTower = human.GetComponentInParent<Tower>();
            if (collisionTower != null)
            {
                List<Human> collectedHumans = collisionTower.CollectHuman(_distanceChercker, _fixationMaxDistance);

                if (collectedHumans != null)
                {
                    _humans[0].StopRun();

                    for (int i = collectedHumans.Count - 1; i >= 0; i--)
                    {
                        Human insertHuman = collectedHumans[i];
                        InsertHuman(insertHuman);
                        DisplaceCheckers(insertHuman);
                    }

                    HumanAdded?.Invoke(_humans.Count);
                    _humans[0].Run();
                }
                collisionTower.Break();
            }
        }
    }

    private void InsertHuman(Human collectedHuman)
    {
        _humans.Insert(0, collectedHuman);
        SetHumanPosition(collectedHuman);
    }

    private void SetHumanPosition(Human human)
    {
        human.transform.parent = transform;
        human.transform.localPosition = new Vector3(0, human.transform.localPosition.y, 0);
        human.transform.localRotation = Quaternion.identity;
    }

    private void DisplaceCheckers(Human human)
    {
        float displaceScale = 1.5f;
        Vector3 distanceCheckerNewPosition = _distanceChercker.position;
        distanceCheckerNewPosition.y -= human.transform.localScale.y * displaceScale;
        _distanceChercker.position = distanceCheckerNewPosition;
        _checkColider.center = _distanceChercker.localPosition;
    }
}
