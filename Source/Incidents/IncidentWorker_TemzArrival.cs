using RimWorld;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_TemzArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;

            PawnKindDef pawnKind =
                DefDatabase<PawnKindDef>.GetNamed("Kekelands_Temz");

            PawnGenerationRequest request = new PawnGenerationRequest(
                kind: pawnKind,
                faction: Faction.OfPlayer,
                context: PawnGenerationContext.NonPlayer,
                forceGenerateNewPawn: true,
                fixedGender: Gender.Male
            );

            Pawn pawn = PawnGenerator.GeneratePawn(request);

            if (pawn == null)
                return false;

            // Имя
            pawn.Name = new NameSingle("Темз");

            // Всё остальное оставляем стандартной генерации RimWorld:
            // возраст, трейты, навыки, страсти, биографии,
            // одежда, оружие, вещи и внешность.

            IntVec3 spawnCell;

            if (!CellFinder.TryFindRandomEdgeCellWith(
                c => map.reachability.CanReachColony(c),
                map,
                CellFinder.EdgeRoadChance_Neutral,
                out spawnCell))
            {
                spawnCell = map.Center;
            }

            GenSpawn.Spawn(
                pawn,
                spawnCell,
                map,
                WipeMode.Vanish
            );

            Find.LetterStack.ReceiveLetter(
                "Прибытие Темза",
                "Темз прибыл в колонию.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }
    }
}