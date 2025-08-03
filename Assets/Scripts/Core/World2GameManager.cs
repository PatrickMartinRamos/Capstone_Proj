using CapstoneProj.MiscSystem;
using CapstoneProj.ScriptableObjectSystem;
using UnityEngine;

namespace CapstoneProj.CoreSystem
{
    public class World2GameManager : SingletonBehaviour<World2GameManager>
    {
        [SerializeField] private int level = 1;
        [SerializeField] private StageListSO _stageListSO;
        private StageSO _stageSO;

        protected override void Awake()
        {
            base.Awake();

            SetStageSO();
        }

        public void SetStageSO()
            => _stageSO = _stageListSO.GetStageSOByLevel(level);

        public StageSO GetStageSO()
            => _stageSO;
    }
}