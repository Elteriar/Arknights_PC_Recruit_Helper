
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Arknights_PC_Recruit_Helper.src
{
    public class RecruitTags
    {
        public static string[] allTags = {
            "Caster", "Defender", "Guard", "Medic", "Sniper", "Specialist", "Supporter", "Vanguard",
            "Melee", "Ranged",
            "Starter", "Senior Operator", "Top Operator",
            "AoE", "Crowd Control", "DP-Recovery", "Debuff", "Defense", "DPS", "Elemental", "Fast-Redeploy", "Healing", "Nuker",
            "Robot", "Shift", "Slow", "Soar", "Summon", "Support", "Survival"};
        //Теги Elemental,Soar не ведут к картинке. Надо бы найти их

        public static int maxTagsOnScreen = 5;

        //6★ 1 tag
        public static string topTag = "Top Operator";

        //5★ 1 tag
        public static string seniorTag = "Senior Operator";


        //5★ 2 tags
        public static Dictionary<string, string[]> combofiveStarsTags = new Dictionary<string, string[]>()
        {
            { "Crowd Control",  [ "DP-Recovery", "Melee", "Vanguard","Summon","Supporter","Fast-Redeploy","Specialist","Slow" ] },
            { "Debuff",  [ "AoE", "Supporter", "Fast-Redeploy", "Melee", "Specialist" ] },
            { "Nuker",  [ "Ranged", "Sniper", "AoE", "Caster" ] },
            { "Shift",  [ "Defender", "Defense", "DPS", "Slow" ] },
            { "Specialist",  new[] { "Survival", "Slow" } },
            { "Summon",  new[] { "Supporter"} },
            { "Support",  new[] { "DP-Recovery", "Vanguard", "Supporter", "Survival" } },
            { "DPS",  new[] { "Defender", "Defense", "Supporter", "Healing" } },
            { "Defense",  new[] { "Survival", "Guard", "AoE", "Caster", "Ranged" } },
            { "Survival",  new[] { "Defender", "Supporter" } },
            { "Healing",  new[] { "Caster" } }

        };
        //5★ 3 tags
        public static string[][] threesomeDPSfiveStarsTags = { [ "AoE", "Guard" ], [ "AoE", "Melee" ], [ "Caster", "Slow"] };

        //4★ 1 tag
        public static string[] solofourStarsTags = {
            "Crowd Control", "Debuff", "Fast-Redeploy", "Nuker", "Shift", "Specialist", "Summon", "Support"
        };

        //4★ 2 tags
        public static Dictionary<string, string[]> combofourStarsTags = new Dictionary<string, string[]>()
        {
            { "Slow",  [ "AoE", "Sniper", "DPS", "Guard", "Melee", "Caster", "Healing" ] },
            { "DPS",  [ "AoE" ] },
            { "Survival",  [ "Ranged", "Sniper" ] },
            { "Healing",  ["DP-Recovery", "Vanguard", "Supporter"] },
            { "Ranged",  [ "DP-Recovery", "Vanguard" ] }
        };

        public static string GetBestTags(List<string> currentTags)
        {
            string resultTags = "";

            //6★ 1 tag search
            if(currentTags.Contains(topTag))
                resultTags += topTag + " 6★ \n";
            //5★ 1 tag search
            if (currentTags.Contains(seniorTag))
                resultTags += seniorTag + " 5★ \n";

            //5★ 2 tags search
            foreach (string tag in currentTags)
            {
                if (combofiveStarsTags.TryGetValue(tag, out var subTags))
                {
                    foreach (string tag2 in currentTags)
                    {
                        if (subTags.Contains(tag2))
                        {
                            resultTags += tag + " + " + tag2 + " 5★ \n";
                        }
                    }
                }
            }

            //5★ 3 tags search
            if (currentTags.Contains("DPS"))
            {
                for (int i = 0; i < threesomeDPSfiveStarsTags.Length; i++)
                {
                    if (currentTags.Contains(threesomeDPSfiveStarsTags[i][0]) && currentTags.Contains(threesomeDPSfiveStarsTags[i][1]))
                        resultTags += $"DPS + {threesomeDPSfiveStarsTags[i][0]} + {threesomeDPSfiveStarsTags[i][1]} 5★ \n";
                }
            }

            //4★ 1 tag search
            foreach (var tag in solofourStarsTags)
            {
                if (currentTags.Contains(tag))
                {                
                    resultTags += tag + " 4★ \n";
                }
            }

            //4★ 2 tags search
            foreach (string tag in currentTags)
            {
                if (combofourStarsTags.TryGetValue(tag, out var subTags))
                {
                    foreach (string tag2 in currentTags)
                    {
                        if (subTags.Contains(tag2))
                        {
                            resultTags += tag + " + " + tag2 + " 4★ \n"; 
                        }                        
                    }  
                }
            }

            return resultTags;
        }
    }
}
