using CapstoneProj.GridSystem;

namespace CapstoneProj.ScreenSystem
{
    public class TopScreenAnimation : ScreenAnimation
    {
        protected override void InputToTileDetector_OnTileWithBombDetected(object sender, InputToTileDetector.OnTileWithBombDetectedEventArgs e)
            => animatorInternal.SetTrigger(MINIMIZE);
    }
}