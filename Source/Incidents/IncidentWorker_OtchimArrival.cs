using RimWorld;
using UnityEngine;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_OtchimArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;

            if (map == null)
                return false;

            PawnKindDef pawnKind =
                DefDatabase<PawnKindDef>.GetNamedSilentFail("Otchim");

            if (pawnKind == null)
            {
                Log.Error("[AzamPrime] Не найден PawnKindDef Otchim.");
                return false;
            }

            Pawn pawn =
                PawnGenerator.GeneratePawn(
                    pawnKind,
                    Faction.OfPlayer
                );

            if (pawn == null)
                return false;

            ConfigureOtchim(pawn);

            IntVec3 spawnCell;

            if (!CellFinder.TryFindRandomEdgeCellWith(
                c => map.reachability.CanReachColony(c),
                map,
                CellFinder.EdgeRoadChance_Neutral,
                out spawnCell))
            {
                return false;
            }

            GenSpawn.Spawn(
                pawn,
                spawnCell,
                map
            );

            Find.LetterStack.ReceiveLetter(
                "Отчим прибыл",
                "Отчим появился в вашей колонии.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }

        private void ConfigureOtchim(Pawn pawn)
        {
            pawn.Name =
                new NameSingle("Отчим");

            pawn.gender =
                Gender.Male;

            pawn.ageTracker.AgeBiologicalTicks =
                15L * 3600000L;

            pawn.ageTracker.AgeChronologicalTicks =
                15L * 3600000L;

            SetBackstories(pawn);
            SetXenotype(pawn);
            SetTraits(pawn);
            SetSkills(pawn);
            SetAppearance(pawn);

            ClearGeneratedApparel(pawn);

            // Шапка-носок.
            GiveApparel(
                pawn,
                "Apparel_Tuque"
            );

            // Куртка.
            GiveApparel(
                pawn,
                "Apparel_Jacket"
            );

            GiveThrowingKnives(pawn);

            ClearGeneratedInventory(pawn);

            GiveInventoryItem(
                pawn,
                ThingDefOf.ComponentIndustrial,
                2
            );

            GiveInventoryItem(
                pawn,
                ThingDefOf.MedicineHerbal,
                1
            );
        }

        private void SetBackstories(Pawn pawn)
        {
            BackstoryDef childhood =
                DefDatabase<BackstoryDef>.GetNamedSilentFail(
                    "Kekelands_OtchimChildhood"
                );

            BackstoryDef adulthood =
                DefDatabase<BackstoryDef>.GetNamedSilentFail(
                    "Kekelands_OtchimAdulthood"
                );

            if (childhood != null)
                pawn.story.Childhood = childhood;
            else
                Log.Error(
                    "[AzamPrime] Не найдена детская биография Отчима."
                );

            if (adulthood != null)
                pawn.story.Adulthood = adulthood;
            else
                Log.Error(
                    "[AzamPrime] Не найдена взрослая биография Отчима."
                );
        }

        private void SetXenotype(Pawn pawn)
        {
            XenotypeDef xenotype =
                DefDatabase<XenotypeDef>.GetNamedSilentFail(
                    "Kekelands_Cuckold"
                );

            if (xenotype == null)
            {
                Log.Error(
                    "[AzamPrime] Не найден Kekelands_Cuckold для Отчима."
                );

                return;
            }

            pawn.genes?.SetXenotype(xenotype);
        }

        private void SetTraits(Pawn pawn)
        {
            if (pawn.story?.traits == null)
                return;

            pawn.story.traits.allTraits.Clear();

            // Нытик.
            AddTrait(
                pawn,
                "Kekelands_Whiner",
                0
            );

            // Медленная обучаемость.
            AddTrait(
                pawn,
                "FastLearner",
                -1
            );

            // Пессимист.
            AddTrait(
                pawn,
                "NaturalMood",
                -1
            );

            // Хлюпик.
            AddTrait(
                pawn,
                "Wimp",
                0
            );
        }

        private void AddTrait(
            Pawn pawn,
            string defName,
            int degree)
        {
            TraitDef traitDef =
                DefDatabase<TraitDef>.GetNamedSilentFail(
                    defName
                );

            if (traitDef == null)
            {
                Log.Warning(
                    "[AzamPrime] Для Отчима не найден TraitDef: "
                    + defName
                );

                return;
            }

            pawn.story.traits.GainTrait(
                new Trait(
                    traitDef,
                    degree
                )
            );
        }

        private void SetSkills(Pawn pawn)
        {
            SetSkill(
                pawn,
                SkillDefOf.Shooting,
                1,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Melee,
                0,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Construction,
                3,
                Passion.Minor
            );

            SetSkill(
                pawn,
                SkillDefOf.Mining,
                10,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Cooking,
                4,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Plants,
                0,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Animals,
                20,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Crafting,
                0,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Artistic,
                1,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Medicine,
                0,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Social,
                2,
                Passion.None
            );

            // "Умственный труд -" пока трактуем как 0.
            SetSkill(
                pawn,
                SkillDefOf.Intellectual,
                0,
                Passion.None
            );
        }

        private void SetSkill(
            Pawn pawn,
            SkillDef def,
            int level,
            Passion passion)
        {
            SkillRecord skill =
                pawn.skills.GetSkill(def);

            skill.Level =
                level;

            skill.passion =
                passion;
        }

        private void SetAppearance(Pawn pawn)
        {
            if (pawn.story == null)
                return;

            // Худой.
            pawn.story.bodyType =
                BodyTypeDefOf.Thin;

            // Короткие волосы.
            string[] shortHairDefs =
            {
                "CrewCut",
                "Buzzcut",
                "Shaved"
            };

            foreach (string defName in shortHairDefs)
            {
                HairDef hair =
                    DefDatabase<HairDef>.GetNamedSilentFail(
                        defName
                    );

                if (hair != null)
                {
                    pawn.story.hairDef =
                        hair;

                    break;
                }
            }
        }

        private void ClearGeneratedApparel(Pawn pawn)
        {
            pawn.apparel?.DestroyAll();
        }

        private void GiveApparel(
            Pawn pawn,
            string defName)
        {
            ThingDef def =
                DefDatabase<ThingDef>.GetNamedSilentFail(
                    defName
                );

            if (def == null)
            {
                Log.Warning(
                    "[AzamPrime] Не найдена одежда Отчима: "
                    + defName
                );

                return;
            }

            Apparel apparel =
                ThingMaker.MakeThing(def)
                as Apparel;

            if (apparel != null)
                pawn.apparel.Wear(apparel);
        }

        private void GiveThrowingKnives(Pawn pawn)
        {
            /*
             * Если у тебя метательные ножи идут из
             * Vanilla Weapons Expanded, их defName
             * обычно VWE_Throwing_Knives.
             */
            ThingDef weaponDef =
                DefDatabase<ThingDef>.GetNamedSilentFail(
                    "VWE_Throwing_Knives"
                );

            if (weaponDef == null)
            {
                Log.Warning(
                    "[AzamPrime] Не найдены метательные ножи VWE_Throwing_Knives."
                );

                return;
            }

            if (pawn.equipment?.Primary != null)
            {
                pawn.equipment.DestroyEquipment(
                    pawn.equipment.Primary
                );
            }

            ThingWithComps weapon =
                ThingMaker.MakeThing(
                    weaponDef
                ) as ThingWithComps;

            if (weapon != null)
            {
                pawn.equipment.AddEquipment(
                    weapon
                );
            }
        }

        private void ClearGeneratedInventory(Pawn pawn)
        {
            pawn.inventory?.innerContainer
                .ClearAndDestroyContents();
        }

        private void GiveInventoryItem(
            Pawn pawn,
            ThingDef def,
            int count)
        {
            if (pawn.inventory == null ||
                def == null)
            {
                return;
            }

            Thing thing =
                ThingMaker.MakeThing(def);

            thing.stackCount =
                count;

            pawn.inventory.innerContainer
                .TryAdd(thing);
        }
    }
}