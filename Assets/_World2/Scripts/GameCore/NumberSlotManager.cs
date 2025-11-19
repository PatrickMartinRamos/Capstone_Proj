using System;

namespace Stellarfarer
{
    public class NumberSlotManager : SingletonBehaviour<NumberSlotManager>
    {
        public event Func<bool> OnTryExtractMidpoint;
        public event Func<bool> OnTryExtractDistance;

        public bool TryExtractMidpoint()
            => OnTryExtractMidpoint?.Invoke() ?? false;

        public bool TryExtractDistance()
            => OnTryExtractDistance?.Invoke() ?? false;
    }
}