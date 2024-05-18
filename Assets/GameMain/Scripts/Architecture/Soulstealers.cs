using System;
using GameMain.Scripts.Model;
using QFramework;

namespace GameMain.Scripts
{
    public class Soulstealers : Architecture<Soulstealers>
    {
        protected override void Init()
        {
            RegisterModel(new PlayerModel());
            RegisterModel(new TaskModel());
        }
    }
}