using RimWorld;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_DedNaPovodkeArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = (Map)parms.target;

            PawnKindDef pawnKind =
                DefDatabase<PawnKindDef>.GetNamed("Kekelands_DedNaPovodke");

            PawnGenerationRequest request = new PawnGenerationRequest(
                kind: pawnKind,
                faction: Faction.OfPlayer,
                context: PawnGenerationContext.NonPlayer,
                forceGenerateNewPawn: true,
                fixedGender: Gender.Male,
                forcedXenotype: DefDatabase<XenotypeDef>.GetNamed("Kekelands_Cuckold")
            );

            Pawn pawn = PawnGenerator.GeneratePawn(request);

            if (pawn == null)
                return false;

            // Имя
            pawn.Name = new NameSingle("Дед на поводке");

            // Возраст — 19 лет
            pawn.ageTracker.AgeBiologicalTicks = 19L * 3600000L;
            pawn.ageTracker.AgeChronologicalTicks = 19L * 3600000L;

            // Пол и тело
            pawn.gender = Gender.Male;
            pawn.story.bodyType = BodyTypeDefOf.Male;

            // -------------------------------------------------
            // БИОГРАФИИ
            // -------------------------------------------------

            // Назначаем заданные биографии по стабильным defName.
            BackstoryDef childhood = DefDatabase<BackstoryDef>.GetNamedSilentFail(
                "Kekelands_DedNaPovodkeChildhood");

            BackstoryDef adulthood = DefDatabase<BackstoryDef>.GetNamedSilentFail(
                "Kekelands_DedNaPovodkeAdulthood");

            if (childhood != null)
                pawn.story.Childhood = childhood;
            else
                Log.Warning("Kekelands: не удалось найти биографию 'Дитя лесов'.");

            if (adulthood != null)
                pawn.story.Adulthood = adulthood;
            else
                Log.Warning("Kekelands: не удалось найти биографию 'Житель поселения'.");

            // -------------------------------------------------
            // ТРЕЙТЫ
            // -------------------------------------------------

            pawn.story.traits.allTraits.Clear();

            // Жизнелюбие
            pawn.story.traits.GainTrait(
                new Trait(
                    DefDatabase<TraitDef>.GetNamed("NaturalMood"),
                    0
                )
            );

            // Доброта
            pawn.story.traits.GainTrait(
                new Trait(
                    DefDatabase<TraitDef>.GetNamed("Kind")
                )
            );

            // Психическая устойчивость
            pawn.story.traits.GainTrait(
                new Trait(
                    DefDatabase<TraitDef>.GetNamed("PsychicSensitivity"),
                    -1
                )
            );

            // Хорошая память
            pawn.story.traits.GainTrait(
                new Trait(
                    DefDatabase<TraitDef>.GetNamed("GreatMemory")
                )
            );

            // -------------------------------------------------
            // НАВЫКИ
            // -------------------------------------------------

            pawn.skills.GetSkill(SkillDefOf.Shooting).Level = 8;
            pawn.skills.GetSkill(SkillDefOf.Melee).Level = 6;
            pawn.skills.GetSkill(SkillDefOf.Construction).Level = 7;
            pawn.skills.GetSkill(SkillDefOf.Mining).Level = 6;
            pawn.skills.GetSkill(SkillDefOf.Cooking).Level = 10;
            pawn.skills.GetSkill(SkillDefOf.Plants).Level = 10;
            pawn.skills.GetSkill(SkillDefOf.Animals).Level = 5;
            pawn.skills.GetSkill(SkillDefOf.Crafting).Level = 18;
            pawn.skills.GetSkill(SkillDefOf.Artistic).Level = 5;
            pawn.skills.GetSkill(SkillDefOf.Medicine).Level = 3;
            pawn.skills.GetSkill(SkillDefOf.Social).Level = 8;
            pawn.skills.GetSkill(SkillDefOf.Intellectual).Level = 8;

            // -------------------------------------------------
            // СТРАСТИ
            // -------------------------------------------------

            pawn.skills.GetSkill(SkillDefOf.Crafting).passion =
                Passion.Minor;

            pawn.skills.GetSkill(SkillDefOf.Construction).passion =
                Passion.Minor;

            pawn.skills.GetSkill(SkillDefOf.Mining).passion =
                Passion.Minor;

            // -------------------------------------------------
            // ВНЕШНОСТЬ
            // -------------------------------------------------

            HairDef hair =
                DefDatabase<HairDef>.GetNamedSilentFail("Buzzcut") ??
                DefDatabase<HairDef>.GetNamedSilentFail("CrewCut") ??
                DefDatabase<HairDef>.GetNamedSilentFail("Shaved");

            if (hair != null)
                pawn.story.hairDef = hair;

            BeardDef beard =
                DefDatabase<BeardDef>.GetNamedSilentFail("Short") ??
                DefDatabase<BeardDef>.GetNamedSilentFail("Bristly");

            if (beard != null)
                pawn.style.beardDef = beard;

            // -------------------------------------------------
            // ОДЕЖДА
            // -------------------------------------------------

            pawn.apparel.DestroyAll();

            // Бронежилет
            ThingDef flakVestDef =
                DefDatabase<ThingDef>.GetNamedSilentFail("Apparel_FlakVest");

            if (flakVestDef != null)
            {
                Apparel flakVest =
                    (Apparel)ThingMaker.MakeThing(flakVestDef);

                pawn.apparel.Wear(flakVest);
            }

            // Плащ
            ThingDef capeDef =
                DefDatabase<ThingDef>.GetNamedSilentFail("Apparel_Cape");

            if (capeDef != null)
            {
                ThingDef capeStuff =
                    GenStuff.RandomStuffByCommonalityFor(capeDef);

                Apparel cape =
                    (Apparel)ThingMaker.MakeThing(capeDef, capeStuff);

                pawn.apparel.Wear(cape);
            }

            // Рубашка из элтекса
            ThingDef eltexShirtDef =
                DefDatabase<ThingDef>.GetNamedSilentFail("Apparel_PsyfocusShirt");

            if (eltexShirtDef != null)
            {
                Apparel eltexShirt =
                    (Apparel)ThingMaker.MakeThing(eltexShirtDef);

                pawn.apparel.Wear(eltexShirt);
            }

            // -------------------------------------------------
            // ОРУЖИЕ
            // -------------------------------------------------

            ThingDef shotgunDef =
                DefDatabase<ThingDef>.GetNamedSilentFail("Gun_Shotgun");

            if (shotgunDef != null)
            {
                Thing shotgun =
                    ThingMaker.MakeThing(shotgunDef);

                pawn.equipment.AddEquipment(
                    (ThingWithComps)shotgun
                );
            }

            // -------------------------------------------------
            // ВЕЩИ
            // -------------------------------------------------

            ThingDef mealDef =
                DefDatabase<ThingDef>.GetNamedSilentFail("MealSimple");

            if (mealDef != null)
            {
                Thing meals = ThingMaker.MakeThing(mealDef);
                meals.stackCount = 2;

                pawn.inventory.innerContainer.TryAdd(meals);
            }

            // -------------------------------------------------
            // ПОЯВЛЕНИЕ
            // -------------------------------------------------

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

            // -------------------------------------------------
            // ПИСЬМО
            // -------------------------------------------------

            Find.LetterStack.ReceiveLetter(
                "Прибытие Деда на поводке",
                "Дед на поводке прибыл в колонию.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }
    }
}