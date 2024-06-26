﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurePlease.Helpers
{
    internal class InternalHelper
    {
        private static Dictionary<string, Dictionary<string, DateTime>> playerCooldowns = new Dictionary<string, Dictionary<string, DateTime>>();
        private static Dictionary<string, Dictionary<string, bool>> autoEnabled = new Dictionary<string, Dictionary<string, bool>>();

        private static List<string> log = new List<string>();

        public static void resetCooldown(string spell, string player)
        {
            lock (playerCooldowns)
            {
                if (spell == null || player == null) { return; }
                if (player == "Helmaru")
                {
                    log.Add($"[{DateTime.Now}] resetCooldown [{player}] [{spell}]");
                }
                if (!playerCooldowns.ContainsKey(spell))
                {
                    playerCooldowns[spell] = new Dictionary<string, DateTime>();
                }
                if(playerCooldowns[spell].ContainsKey(player) && playerCooldowns[spell][player] > new DateTime(1970, 1, 1, 0, 0, 0))
                {
                    log.Add("");
                }
                playerCooldowns[spell][player] = new DateTime(1970, 1, 1, 0, 0, 0);

            }
        }
        public static void resetAllCooldowns()
        {
            playerCooldowns = new Dictionary<string, Dictionary<string, DateTime>>();
        }
        public static void setCooldown(string spell, string player)
        {
            lock (playerCooldowns)
            {
                if (spell == null || player == null) { return; }
                if (player == "Helmaru")
                {
                    log.Add($"[{DateTime.Now}] setCooldown [{player}] [{spell}]");
                }
                if (!playerCooldowns.ContainsKey(spell))
                {
                    playerCooldowns[spell] = new Dictionary<string, DateTime>();
                }
                playerCooldowns[spell][player] = DateTime.Now;
            }
        }

        public static DateTime getCooldown(string spell, string player)
        {
            lock (playerCooldowns)
            {
                if (spell == null || player == null) { return DateTime.Now; }

                if (player == "Helmaru")
                {
                    log.Add($"[{DateTime.Now}] getCooldown [{player}] [{spell}]");
                }
                if (!playerCooldowns.ContainsKey(spell) || !playerCooldowns[spell].ContainsKey(player))
                {
                    resetCooldown(spell, player);
                }
                return playerCooldowns[spell][player];
            }
        }

        public static int getTimeSpanInMinutes(string spell, string player)
        {
            var currentTime = DateTime.Now;
            return Convert.ToInt32((currentTime.Subtract(getCooldown(spell, player))).TotalMinutes);
        }

        public static void setAutoEnable(string spell, string player, bool enable)
        {
            if (spell == null || player == null) { return; }

            if (!autoEnabled.ContainsKey(spell))
            {
                autoEnabled[spell] = new Dictionary<string, bool>();
            }
            if (enable)
            {
                //reset cooldown on activation
                resetCooldown(spell, player);
            }
            autoEnabled[spell][player] = enable;
        }
        public static bool getAutoEnable(string spell, string player)
        {
            if(spell == null || player == null) { return false; }

            if (!autoEnabled.ContainsKey(spell) || !autoEnabled[spell].ContainsKey(player))
            {
                setAutoEnable(spell, player, false);
            }
            return autoEnabled[spell][player];
        }

    }
}
