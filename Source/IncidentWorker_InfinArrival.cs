using RimWorld;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_BromoArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;

            if (map == null)
                return false;

            PawnKindDef pawnKind =
                DefDatabase<PawnKindDef>.GetNamedSilentFail("Infin");

            if (pawnKind == null)
            {
                Log.Error("[AzamPrime] Не найден PawnKindDef Инфин.");
                return false;
            }

            Pawn pawn = PawnGenerator.GeneratePawn(
                pawnKind,
                Faction.OfPlayer
            );

            if (pawn == null)
                return false;

            IntVec3 spawnCell;

            if (!CellFinder.TryFindRandomEdgeCellWith(
                c => map.reachability.CanReachColony(c),
                map,
                CellFinder.EdgeRoadChance_Neutral,
                out spawnCell))
            {
                Log.Warning(
                    "[AzamPrime] Не удалось найти точку появления Инфина."
                );

                return false;
            }

            GenSpawn.Spawn(
                pawn,
                spawnCell,
                map
            );

            Find.LetterStack.ReceiveLetter(
                "Инфин прибыл",
                "Инфин появился в вашей колонии.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }
    }
}