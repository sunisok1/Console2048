using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Console2048
{
    public class GamePack : IGamePack
    {
        [SerializeField] private Scene m_scene;
        public string Name => "Console2048";
        public string Version => "0.1.0";
        public string Icon => "";

        public void Load()
        {
            SceneManager.LoadScene("Game/Console2048/Console2048");
        }
    }
}