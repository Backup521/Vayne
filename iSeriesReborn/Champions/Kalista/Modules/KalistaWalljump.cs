﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DZLib.Logging;
using iSeriesReborn.Utility;
using iSeriesReborn.Utility.Geometry;
using iSeriesReborn.Utility.ModuleHelper;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace iSeriesReborn.Champions.Kalista.Modules
{
    class KalistaWalljump : IModule
    {
        private float LastMovementTick = 0f;
        public string GetName()
        {
            return "Kalista_Walljump";
        }

        public ModuleType GetModuleType()
        {
            return ModuleType.OnUpdate;
        }

        public bool ShouldRun()
        {
            return MenuExtensions.GetItemValue<KeyBind>("iseriesr.kalista.misc.walljump").Active;
        }

        public void Run()
        {
            if ((iSRGeometry.IsOverWall(ObjectManager.Player.ServerPosition, Game.CursorPos)
                && iSRGeometry.GetWallLength(ObjectManager.Player.ServerPosition, Game.CursorPos) >= (35f)) && (Variables.spells[SpellSlot.Q].IsReady()))
            {
                MoveToLimited(iSRGeometry.GetFirstWallPoint(ObjectManager.Player.ServerPosition, Game.CursorPos));
            }
            else
            {
                MoveToLimited(Game.CursorPos);
            }

                var dir = ObjectManager.Player.ServerPosition.To2D() + ObjectManager.Player.Direction.To2D().Perpendicular() * (ObjectManager.Player.BoundingRadius * 1.10f);
                var oppositeDir = ObjectManager.Player.ServerPosition.To2D() + ObjectManager.Player.Direction.To2D().Perpendicular() * -(ObjectManager.Player.BoundingRadius * 2f);
                var Extended = Game.CursorPos;
                if (dir.IsWall() && iSRGeometry.IsOverWall(ObjectManager.Player.ServerPosition, Extended)
                    && Variables.spells[SpellSlot.Q].IsReady()
                    && iSRGeometry.GetWallLength(ObjectManager.Player.ServerPosition, Extended) <= (280f - 65f / 2f))
                {
                    Variables.spells[SpellSlot.Q].Cast(oppositeDir);
                }
        }

        public void MoveToLimited(Vector3 where)
        {
            if (Game.Time - LastMovementTick < 800f)
            {
                return;
            }
            LastMovementTick = Game.Time;

            ObjectManager.Player.IssueOrder(GameObjectOrder.MoveTo, where);
        }
    }
}
