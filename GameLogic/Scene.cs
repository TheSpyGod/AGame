using System;
using System.Windows.Forms;
using Adventure;
using System.Numerics;

namespace AGConsole.Engine.Render
{
    public class Scene
    {
        private OpenWrld OW;
        private ActorModel.Actor player;
        private ActorModel.Actor enemy;

        public Scene(Control control)
        {
            OW = new OpenWrld(control);
            InitializeScene();
        }

        private void InitializeScene()
        {
            player = new ActorModel.Actor { Location = new Vector2(1, 1), Health = 200, IsAlive = true };
            enemy = new ActorModel.Actor { Location = new Vector2(20, 20), Health = 200, IsAlive = true };
            OW.SetActors(player, enemy);
            OW.UpdateDrawing();
        }

        public void HandleInput(Keys key)
        {
            if (!OW.HandlePlayerMovement(key) && key == Keys.Escape || key == Keys.Left || key == Keys.Down || key == Keys.Up || key == Keys.Right)
            {
                if (enemy.IsAlive) OW.HandleEnemyTurn();
                OW.UpdateDrawing();
            }
        }
    }
}
