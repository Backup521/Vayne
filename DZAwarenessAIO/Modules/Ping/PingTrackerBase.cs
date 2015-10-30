﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClipperLib;
using DZAwarenessAIO.Utility;
using DZAwarenessAIO.Utility.Logs;
using DZAwarenessAIO.Utility.MenuUtility;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Geometry = DZAwarenessAIO.Utility.Geometry;
using Color = System.Drawing.Color;

namespace DZAwarenessAIO.Modules.Ping
{
    /// <summary>
    /// The Ranges Tracking Class
    /// </summary>
    class PingTrackerBase : ModuleBase
    {
        /// <summary>
        /// Creates the menu.
        /// </summary>
        public override void CreateMenu()
        {
            try
            {
                var RootMenu = Variables.Menu;
                var moduleMenu = new Menu("Ping Tracker", "dz191.dza.ping");
                {
                    moduleMenu.AddBool("dz191.dza.ping.show", "Show name near pings");
                    RootMenu.AddSubMenu(moduleMenu);
                }
            }
            catch (Exception e)
            {
                LogHelper.AddToLog(new LogItem("Ping_Base", e));
            }

        }

        /// <summary>
        /// Initializes the events.
        /// </summary>
        public override void InitEvents()
        {
            try
            {
                Game.OnPing += OnPing;
            }
            catch (Exception e)
            {
                LogHelper.AddToLog(new LogItem("Ping_Base", e));
            }
        }

        void OnPing(GamePingEventArgs args)
        {
            if (!args.Source.IsMe && ShouldRun() && (args.Source is Obj_AI_Hero))
            {
                var pingType = args.PingType;
                var srcHero = args.Source as Obj_AI_Hero;
                if (pingType == PingCategory.Normal)
                {

                    var textObject = new Render.Text(
                        srcHero.ChampionName,
                        new Vector2(
                            Drawing.WorldToScreen(args.Position.To3D()).X,
                            Drawing.WorldToScreen(args.Position.To3D()).Y + 15), 17, SharpDX.Color.White)
                    {
                        PositionUpdate = () => new Vector2(
                            Drawing.WorldToScreen(args.Position.To3D()).X,
                            Drawing.WorldToScreen(args.Position.To3D()).Y + 30),
                        Centered = true
                    };
                    textObject.Add(0);
                    LeagueSharp.Common.Utility.DelayAction.Add(1000, () =>
                    {
                        textObject.Remove();
                    });
                }
            }
        }

        /// <summary>
        /// Gets the type of the module.
        /// </summary>
        /// <returns></returns>
        public override ModuleTypes GetModuleType()
        {
            return ModuleTypes.General;
        }

        /// <summary>
        /// Shoulds the module run.
        /// </summary>
        /// <returns></returns>
        public override bool ShouldRun()
        {
            return MenuExtensions.GetItemValue<bool>("dz191.dza.ping.show");
        }

        /// <summary>
        /// Called On Update
        /// </summary>
        public override void OnTick() { }
    }
}
