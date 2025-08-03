using CapstoneProj.EnumSystem;
using UnityEngine;

namespace CapstoneProj.ScriptableObjectSystem
{
    public class BombSO : ScriptableObject
    {
        public string BombName;
        public BombType BombType;
        public Sprite BombSprite;
        public Transform BombPrefab;
        public ExplosionType ExplosionType;
        public DefusalType DefusalType;
    }
}