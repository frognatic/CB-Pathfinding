using System;
using System.Collections.Generic;
using Managers;
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
            
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var hit, 100)) 
                return;

            foreach (MovingUnitsMono mono in movingUnitsMonoList) 
                mono.UpdatePath(hit);
        }

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