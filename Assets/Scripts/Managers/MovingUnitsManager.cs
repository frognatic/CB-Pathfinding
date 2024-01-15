using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gameplay.MovingUnits;
using Managers.Singleton;
using UnityEngine;
using Utility;

namespace Managers
{
    public class MovingUnitsManager : MonoSingleton<MovingUnitsManager>
    {
        public static event Action<IMovingUnits> OnChangeLeader;
        public static event Action OnCreateUnits;

        private const int UnitsToStartAmount = 3;
        private const string UnitDetailsPrefix = "unit_";

        public List<MovingUnit> MovingList { get; } = new();

        public UniTask Initialize()
        {
            SaveManager.Instance.SaveState.movingUnitsState ??= new();
            SetUnits();

            return UniTask.CompletedTask;
        }

        private void SetUnits(bool loadUnits = false)
        {
            MovingList.Clear();

            if (loadUnits)
                LoadsUnitsFromSave();
            else
                CreateNewUnits();

            OnCreateUnits?.Invoke();
        }

        private void CreateNewUnits()
        {
            List<UnitDetails> unitDetailsList = AddressableManager.Instance.GetUnitDetailsList();
            if (unitDetailsList.Count < UnitsToStartAmount)
            {
                Debug.LogError($"You tried to spawn too many units. Add more at UnitDetailsSO Scriptable Object. ");
                return;
            }

            List<MovingUnitsState.UnitData> unitDataList = new();
            List<UnitDetails> randedUnitDetails = unitDetailsList.GetRandomListElements(UnitsToStartAmount);

            for (int i = 0; i < UnitsToStartAmount; i++)
                unitDataList.Add(CreateSingleUnit(randedUnitDetails[i]));

            SaveManager.Instance.SaveState.movingUnitsState.movingUnitsData = unitDataList;
        }

        private MovingUnitsState.UnitData CreateSingleUnit(UnitDetails unitDetails)
        {
            MovingUnitsState.UnitData unitData = new(unitDetails.id, unitDetails.spawnPosition);
            MovingUnit movingUnit = new(unitData, unitDetails);

            MovingList.Add(movingUnit);
            return unitData;
        }

        private void LoadsUnitsFromSave()
        {
            foreach (MovingUnitsState.UnitData unitData in GetSavedUnits)
            {
                UnitDetails unitDetails =
                    AddressableManager.Instance.GetUnitDetails($"{UnitDetailsPrefix}{unitData.id}");
                MovingUnit movingUnit = new(unitData, unitDetails);

                MovingList.Add(movingUnit);
            }
        }

        private List<MovingUnitsState.UnitData> GetSavedUnits =>
            SaveManager.Instance.SaveState.movingUnitsState.movingUnitsData;


        public void LoadMovingUnits()
        {
            SaveManager.Instance.Load();
            SetUnits(loadUnits: true);

            OnCreateUnits?.Invoke();
        }

        public void SetUnitAsLeader(IMovingUnits movingUnits)
        {
            MarkAsDeselected();
            movingUnits.MarkAsLeader();

            OnChangeLeader?.Invoke(movingUnits);
        }

        private void MarkAsDeselected() => MovingList.ForEach(x => x.Unmark());
    }
}