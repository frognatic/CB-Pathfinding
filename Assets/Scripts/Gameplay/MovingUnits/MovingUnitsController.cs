using System;
using System.Collections.Generic;
using System.Linq;
using Managers;
using Pathfinding;
using UnityEngine;
using UnityEngine.EventSystems;
using Utility;

namespace Gameplay.MovingUnits
{
    public class MovingUnitsController : MonoBehaviour
    {
        [SerializeField] private MovingUnitsMono movingUnitsPrefab;
        [SerializeField] private Transform movingUnitsContent;

        private readonly List<MovingUnitsMono> movingUnitsMonoList = new();
        
        private Camera mainCamera;

        private RaycastHit destinationHit;
        private MovingUnitsMono leader;

        private void Awake() => mainCamera = Camera.main;

        private void OnEnable()
        {
            MovingUnitsManager.OnChangeLeader += OnChangeLeaderResponse;
            MovingUnitsManager.OnCreateUnits += SpawnUnitsMono;
        }

        private void OnDisable()
        {
            MovingUnitsManager.OnChangeLeader -= OnChangeLeaderResponse;
            MovingUnitsManager.OnCreateUnits -= SpawnUnitsMono;
        }

        private void Update()
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;
            
            HandleInput();
        }
        
        private void HandleInput()
        {
            if (!Input.GetMouseButtonDown(0)) 
                return;

            leader = GetLeader;
            if (leader == null)
                return;
            
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out destinationHit, 100)) 
                return;

            MoveUnits();
        }

        private void MoveUnits()
        {
            MoveLeader();
            MoveFollowers();
        }

        private void MoveLeader() => leader.SetPathToDestination(destinationHit);

        private void MoveFollowers()
        {
            List<MovingUnitsMono> unitsWithoutLeader = movingUnitsMonoList.Where(x => !x.IsLeader).ToList();
            List<Vector3> followersDestinations = CalculateFollowersDestinations(unitsWithoutLeader.Count);
            
            for (int i = 0; i < unitsWithoutLeader.Count; i++)
            {
                MovingUnitsMono mono = unitsWithoutLeader[i];
                mono.SetPathToDestination(followersDestinations[i]);
            }
        }

        private List<Vector3> CalculateFollowersDestinations(int pathAmount)
        {
            PathfindingNode leaderNode = leader.GetNode(destinationHit.point);
            return PathfindingManager.Instance.GetFollowersDestinations(leaderNode, pathAmount);
        }

        private MovingUnitsMono GetLeader => movingUnitsMonoList.Find(x => x.IsLeader);
        
        private void SpawnUnitsMono()
        {
            Clear();
            foreach (MovingUnit moveUnit in MovingUnitsManager.Instance.MovingList)
            {
                MovingUnitsMono movingUnitMono = Instantiate(movingUnitsPrefab, movingUnitsContent);
                movingUnitMono.Init(moveUnit);
                movingUnitsMonoList.Add(movingUnitMono);
            }
        }

        private void Clear()
        {
            movingUnitsMonoList.Clear();
            movingUnitsContent.DestroyImmediateAllChildren();
        }

        private void OnChangeLeaderResponse(IMovingUnits movingUnit) =>
            movingUnitsMonoList.ForEach(x => x.MarkAsLeader(movingUnit));
    }
}