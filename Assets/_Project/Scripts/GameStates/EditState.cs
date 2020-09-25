using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UntitledBallGame.GlobalStates;
using UntitledBallGame.UI;

namespace UntitledBallGame.GameStates
{
    public class EditState : GameStateBase
    {
        public EditState(GameplayState parent, GameStateContext context) : base(parent, context)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Context.InputManager.Controls.EditMode.Enable();

            Context.GameUi.EditingScreen.Show();

            var levelItems = Context.LevelItemManager.GetAll();
            
            if (levelItems == null || !levelItems.Any())
                Debug.LogError("No level items found!");
            
            var levelItemDTOs = levelItems.Select(i => 
                new LevelItemDTO
                {
                    Id = i.id,
                    Icon = i.icon
                });
            
            Context.GameUi.EditingScreen.CreateItemButtons(levelItemDTOs);
            Context.GameUi.EditingScreen.ItemSelected += OnItemSelected;
            
            if (Context.BallController.BallPosition != Context.BallSpawner.Position)
            {
                Context.BallController.FreezeBall();
                TeleportBallToSpawn();
            }

            Context.CameraController.TranslateTo(Context.BallController.BallPosition);
        }

        private void OnItemSelected(int itemId)
        {
            Debug.Log($"{itemId} is selected");
            // TODO: logic for dragging item into the world
        }

        public override void Exit()
        {
            base.Exit();

            Context.InputManager.Controls.EditMode.Disable();

            Context.GameUi.EditingScreen.Hide();
        }

        public override void LateUpdate()
        {
            base.LateUpdate();

            Context.CameraController.TranslateBy(Context.InputManager.GetCameraMovement() * Time.deltaTime);

            Vector2 newPos = Context.LevelBounds.GetObjectInBounds(Context.CameraController.CalculateCameraBounds());
            Context.CameraController.TranslateTo(newPos);
        }

        private void TeleportBallToSpawn()
        {
            Context.BallController.BallPosition = Context.BallSpawner.Position;
        }
    }
}