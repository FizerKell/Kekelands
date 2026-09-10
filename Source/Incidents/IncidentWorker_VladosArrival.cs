using RimWorld;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_VladosArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;

            if (map == null)
                return false;

            PawnKindDef pawnKind =
                DefDatabase<PawnKindDef>.GetNamedSilentFail("Vlados");

            if (pawnKind == null)
            {
                Log.Error("[AzamPrime] Не найден PawnKindDef Vlados.");
                return false;
            }

            Pawn pawn = PawnGenerator.GeneratePawn(
                pawnKind,
                Faction.OfPlayer
            );

            if (pawn == null)
                return false;

            ConfigureVlados(pawn);

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
                "Владос прибыл",
                "Владос появился в вашей колонии.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }

        private void ConfigureVlados(Pawn pawn)
        {
            pawn.Name = new NameSingle("Владос");
            pawn.gender = Gender.Male;

            pawn.ageTracker.AgeBiologicalTicks =
                19L * 3600000L;

            pawn.ageTracker.AgeChronologicalTicks =
                19L * 3600000L;

            SetXenotype(pawn);
            SetTraits(pawn);
            SetSkillsAndPassions(pawn);

            ClearGeneratedApparel(pawn);

            GiveApparel(
                pawn,
                "Apparel_CowboyHat",
                null
            );

            GiveApparel(
                pawn,
                "Apparel_Pants",
                null
            );

            GiveApparel(
                pawn,
                "Apparel_BasicShirt",
                ThingDefOf.Hyperweave
            );

            GiveApparel(
                pawn,
                "Apparel_FlakVest",
                null
            );

            GiveWeapon(
                pawn,
                "Gun_SMG"
            );

            ClearGeneratedInventory(pawn);

            GiveInventoryItem(
                pawn,
                ThingDefOf.Cloth,
                4
            );

            // Обычная кожа.
            ThingDef leather =
                DefDatabase<ThingDef>.GetNamedSilentFail(
                    "Leather_Plain"
                );

            if (leather != null)
            {
                GiveInventoryItem(
                    pawn,
                    leather,
                    6
                );
            }

            GiveInventoryItem(
                pawn,
                ThingDefOf.MealFine,
                1
            );
        }

        // -------------------------
        // РАСА
        // -------------------------

        private void SetXenotype(Pawn pawn)
        {
            XenotypeDef xenotype =
                DefDatabase<XenotypeDef>.GetNamedSilentFail(
                    "Kekelands_Old"
                );

            if (xenotype == null)
            {
                Log.Error(
                    "[AzamPrime] Не найден Kekelands_Old для Владоса."
                );

                return;
            }

            pawn.genes?.SetXenotype(xenotype);
        }

        // -------------------------
        // ТРЕЙТЫ
        // -------------------------

        private void SetTraits(Pawn pawn)
        {
            if (pawn.story?.traits == null)
                return;

            pawn.story.traits.allTraits.Clear();

            // Меланхолик / сильная отрицательная ветка настроения.
            AddTrait(
                pawn,
                "NaturalMood",
                -2
            );

            // Трезвенник.
            AddTrait(
                pawn,
                "DrugDesire",
                -1
            );

            // Хорошая память.
            AddTrait(
                pawn,
                "GreatMemory",
                0
            );

            // Отшельничество.
            AddTrait(
                pawn,
                "Recluse",
                0
            );

            // Забывчивость.
            AddTrait(
                pawn,
                "Forgetful",
                0
            );

            // Быстрая обучаемость.
            AddTrait(
                pawn,
                "FastLearner",
                1
            );

            // Ловкость.
            AddTrait(
                pawn,
                "Nimble",
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
                    "[AzamPrime] Для Владоса не найден TraitDef: "
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

        // -------------------------
        // НАВЫКИ
        // -------------------------

        private void SetSkillsAndPassions(Pawn pawn)
        {
            if (pawn.skills == null)
                return;

            SetSkill(
                pawn,
                SkillDefOf.Shooting,
                13,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Melee,
                4,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Construction,
                4,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Mining,
                8,
                Passion.Major
            );

            SetSkill(
                pawn,
                SkillDefOf.Cooking,
                5,
                Passion.Minor
            );

            SetSkill(
                pawn,
                SkillDefOf.Plants,
                6,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Animals,
                3,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Crafting,
                5,
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
                4,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Social,
                6,
                Passion.Minor
            );

            SetSkill(
                pawn,
                SkillDefOf.Intellectual,
                15,
                Passion.Minor
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

            skill.Level = level;
            skill.passion = passion;
        }

        // -------------------------
        // ОДЕЖДА
        // -------------------------

        private void ClearGeneratedApparel(Pawn pawn)
        {
            pawn.apparel?.DestroyAll();
        }

        private void GiveApparel(
            Pawn pawn,
            string defName,
            ThingDef stuff)
        {
            ThingDef apparelDef =
                DefDatabase<ThingDef>.GetNamedSilentFail(
                    defName
                );

            if (apparelDef == null)
            {
                Log.Warning(
                    "[AzamPrime] Не найдена одежда Владоса: "
                    + defName
                );

                return;
            }

            Apparel apparel =
                ThingMaker.MakeThing(
                    apparelDef,
                    stuff
                ) as Apparel;

            if (apparel == null)
                return;

            pawn.apparel.Wear(apparel);
        }

        // -------------------------
        // ОРУЖИЕ
        // -------------------------

        private void GiveWeapon(
            Pawn pawn,
            string defName)
        {
            ThingDef weaponDef =
                DefDatabase<ThingDef>.GetNamedSilentFail(
                    defName
                );

            if (weaponDef == null)
            {
                Log.Error(
                    "[AzamPrime] Не найдено оружие Владоса: "
                    + defName
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

        // -------------------------
        // ИНВЕНТАРЬ
        // -------------------------

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

            thing.stackCount = count;

            pawn.inventory.innerContainer
                .TryAdd(thing);
        }
    }
}