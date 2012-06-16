using MTV3D65;

namespace Sandbox
{
    public partial class FrmMain
    {
        /// <summary>
        /// FPS type camera.
        /// </summary>
        /// <param name="mouseX">Mouse X position.</param>
        /// <param name="mouseY">Mouse Y position.</param>
        private void FPSCamera(float mouseX, float mouseY, float scroll, bool reactToKeys)
        {
            float mousespeed = Settings.CameraMouseSpeed;
            float speed = Settings.CameraMoveSpeed;
            float quickSpeed = Settings.CameraRunSpeed;
            float scrollSpeed = Settings.CameraScrollSpeed * scroll;

            core.Camera.RotateY(mouseX*(mousespeed*2));
            core.Camera.SetLookAt(
                core.Camera.GetLookAt().x,
                core.Camera.GetLookAt().y - (mouseY*((mousespeed*2)/100)),
                core.Camera.GetLookAt().z);
            core.Camera.MoveRelative(scrollSpeed, 0, 0, true);

            if (reactToKeys)
            {
                if (core.Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_LEFTSHIFT))
                {
                    speed *= quickSpeed;
                }

                if (core.Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_W))
                {
                    core.Camera.MoveRelative(speed, 0, 0, true);
                }

                if (core.Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_S))
                {
                    core.Camera.MoveRelative(-speed, 0, 0, true);
                }

                if (core.Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_A))
                {
                    core.Camera.MoveRelative(0, 0, -speed, true);
                }

                if (core.Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_D))
                {
                    core.Camera.MoveRelative(0, 0, speed, true);
                }

                if (core.Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_E))
                {
                    core.Camera.MoveRelative(0, speed / 2, 0, true);
                }

                if (core.Input.IsKeyPressed(CONST_TV_KEY.TV_KEY_Q))
                {
                    core.Camera.MoveRelative(0, -(speed / 2), 0, true);
                }
            }
        }
    }
}