using RimWorld;
using System;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_KamArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;

            PawnKindDef kamKind = DefDatabase<PawnKindDef>.GetNamed("Kekelands_Kam");

            PawnGenerationRequest request = new PawnGenerationRequest(
                kind: kamKind,
                faction: Faction.OfPlayer,
                context: PawnGenerationContext.NonPlayer,
                forceGenerateNewPawn: true,
                fixedGender: Gender.Male,
                forcedXenotype: DefDatabase<XenotypeDef>.GetNamed("Kekelands_Old")
            );

            Pawn pawn = PawnGenerator.GeneratePawn(request);

            if (pawn == null)
                return false;

            // Имя
            pawn.Name = new NameSingle("Кам");

            // Возраст
            pawn.ageTracker.AgeBiologicalTicks = 25L * 3600000L;
            pawn.ageTracker.AgeChronologicalTicks = 25L * 3600000L;

            // Зрелость — специальная биография с отключённым умственным трудом
            BackstoryDef adultBackstory =
                DefDatabase<BackstoryDef>.GetNamedSilentFail("Kekelands_KamAdulthood");

            if (adultBackstory != null)
                pawn.story.Adulthood = adultBackstory;

            // Убираем случайно сгенерированные трейты
            pawn.story.traits.allTraits.Clear();

            // Жизнелюбие
            pawn.story.traits.GainTrait(
                new Trait(
                    DefDatabase<TraitDef>.GetNamed("NaturalMood"),
                    2
                )
            );

            // Оптимист
            // NaturalMood — спектральный TraitDef, поэтому одновременно
            // жизнелюбие и оптимист технически являются одной веткой.
            // Поэтому здесь оставляем именно жизнелюбие.
            //
            // Если нужны ОБА одновременно, RimWorld будет считать это
            // конфликтующими степенями одного TraitDef.

            // Психическая глухота
            pawn.story.traits.GainTrait(
                new Trait(
                    DefDatabase<TraitDef>.GetNamed("PsychicSensitivity"),
                    -2
                )
            );

            // Короткие волосы
            HairDef hair =
                DefDatabase<HairDef>.GetNamedSilentFail("Buzzcut") ??
                DefDatabase<HairDef>.GetNamedSilentFail("CrewCut") ??
                DefDatabase<HairDef>.GetNamedSilentFail("Shaved");

            if (hair != null)
                pawn.story.hairDef = hair;

            // Мужской тип тела
            pawn.story.bodyType = BodyTypeDefOf.Male;

            // Нож
            ThingDef knifeDef =
                DefDatabase<ThingDef>.GetNamedSilentFail("MeleeWeapon_Knife");

            if (knifeDef != null)
            {
                Thing knife = ThingMaker.MakeThing(knifeDef);
                pawn.equipment.AddEquipment((ThingWithComps)knife);
            }

            // Поиск точки появления
            IntVec3 spawnCell;

            if (!CellFinder.TryFindRandomEdgeCellWith(
                c => map.reachability.CanReachColony(c),
                map,
                CellFinder.EdgeRoadChance_Neutral,
                out spawnCell))
            {
                spawnCell = map.Center;
            }

            GenSpawn.Spawn(pawn, spawnCell, map, WipeMode.Vanish);

            // Письмо
            Find.LetterStack.ReceiveLetter(
                "Прибытие Кама",
                "Кам прибыл в колонию.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }
    }
}