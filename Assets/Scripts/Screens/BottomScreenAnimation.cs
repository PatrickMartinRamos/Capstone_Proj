using CapstoneProj.GridSystem;

namespace CapstoneProj.ScreenSystem
{
    public class BottomScreenAnimation : ScreenAnimation
    {
        protected override void InputToTileDetector_OnTileWithBombDetected(object sender, InputToTileDetector.OnTileWithBombDetectedEventArgs e)
        {
            animatorInternal.SetTrigger(MAXIMIZE);

            // TODO: SHOW TILE WITH BOMB
        }
    }
}