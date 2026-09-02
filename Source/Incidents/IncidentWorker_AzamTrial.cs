using RimWorld;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_AzamTrial : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;

            if (map == null)
                return false;

            float roll = Rand.Value;

            // 30% — только затмение
            if (roll < 0.30f)
            {
                TriggerEclipse(map);
            }

            // 45% — только рейд
            else if (roll < 0.75f)
            {
                TriggerRaid(map);
            }

            // 25% — затмение + рейд
            else
            {
                TriggerEclipse(map);
                TriggerRaid(map);
            }

            Find.LetterStack.ReceiveLetter(
                "Азам ноет",
                "Азаму не понравилось, что бромо снова зарейдили. жди сват.",
                LetterDefOf.ThreatBig,
                new LookTargets(map.Center, map)
            );

            return true;
        }


        private void TriggerEclipse(Map map)
        {
            IncidentDef eclipse =
                DefDatabase<IncidentDef>.GetNamed("Eclipse");

            IncidentParms eclipseParms =
                StorytellerUtility.DefaultParmsNow(
                    IncidentCategoryDefOf.Misc,
                    map
                );

            eclipse.Worker.TryExecute(eclipseParms);
        }


        private void TriggerRaid(Map map)
        {
            IncidentParms raidParms =
                StorytellerUtility.DefaultParmsNow(
                    IncidentCategoryDefOf.ThreatBig,
                    map
                );

            IncidentDefOf.RaidEnemy.Worker.TryExecute(raidParms);
        }
    }
}