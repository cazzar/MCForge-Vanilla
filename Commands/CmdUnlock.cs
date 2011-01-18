using System;
using System.IO;
using System.Collections.Generic;
namespace MCForge
{
    public class CmdUnlock : Command
    {
        // The command's name, in all lowercase.  What you'll be putting behind the slash when using it.
        public override string name { get { return "unlock"; } }

        // Command's shortcut (please take care not to use an existing one, or you may have issues.
        public override string shortcut { get { return "ul"; } }

        // Determines which submenu the command displays in under /help.
        public override string type { get { return "other"; } }

        // Determines whether or not this command can be used in a museum.  Block/map altering commands should be made false to avoid errors.
        public override bool museumUsable { get { return false; } }

        // Determines the command's default rank.  Valid values are:
        // LevelPermission.Nobody, LevelPermission.Banned, LevelPermission.Guest
        // LevelPermission.Builder, LevelPermission.AdvBuilder, LevelPermission.Operator, LevelPermission.Admin
        public override LevelPermission defaultRank { get { return LevelPermission.Operator; } }

        // This is where the magic happens, naturally.
        // p is the player object for the player executing the command.  message is everything after the command invocation itself.
        public override void Use(Player p, string message)
        {
            if (!Directory.Exists("text/lockdown"))
            {
                p.SendMessage("Could not locate the folder creating one now.");
                Directory.CreateDirectory("text/lockdown");
                Directory.CreateDirectory("text/lockdown/map");
                p.SendMessage("Added the settings for the command");
            }
            string[] param = message.Split(' ');
            {
                if (param.Length == 2)
                {
                    if (param[0] == "map")
                    {
                        if (File.Exists("text/lockdown/map/" + param[1] + ""))
                        {
                            File.Delete("text/lockdown/map/" + param[1] + "");
                            Player.SendMessage(p, "the map " + param[1] + " have been unlocked");
                        }
                        else
                            Player.SendMessage(p, "the map " + param[1] + " is not locked or does not exist");
                    }
                }
                if (param.Length == 2) // shift number to the next lockdown command
                {
                    if (param[0] == "player")
                    {
                        Player who = Player.Find(param[1]);
                        {
                            if (who != null)
                            {
                                if (who.jailed)
                                {
                                    if (p != null) if (who.group.Permission >= p.group.Permission) { Player.SendMessage(p, "Cannot lock down someone of equal or greater rank."); return; }
                                    if (who.level != p.level) Command.all.Find("goto").Use(who, p.level.name);
                                    Player.GlobalDie(who, false);
                                    who.jailed = false;
                                    Player.GlobalChat(p, who.color + who.name + Server.DefaultColor + " have been unlocked!", true);
                                    return;
                                }
                                else Player.SendMessage(p, "the player " + param[1] + " is not locked down"); return;
                            }
                            else Player.SendMessage(p, "there is no such player online!"); return;
                        }
                    }

                }
            }
        }

        public override void Help(Player p)
        {
            Player.SendMessage(p, " use /unlock map <mapname> to unlock it (open it).");
            Player.SendMessage(p, " use /unlock player <playername> to unlock player."); return;
        }
    }
}