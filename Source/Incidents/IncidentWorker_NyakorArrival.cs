using RimWorld;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_NyakorArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;

            if (map == null)
                return false;

            PawnKindDef pawnKind =
                DefDatabase<PawnKindDef>.GetNamedSilentFail(
                    "Nyakor"
                );

            if (pawnKind == null)
            {
                Log.Error(
                    "[AzamPrime] Не найден PawnKindDef Nyakor."
                );

                return false;
            }

            Pawn pawn =
                PawnGenerator.GeneratePawn(
                    pawnKind,
                    Faction.OfPlayer
                );

            if (pawn == null)
                return false;

            ConfigureNyakor(pawn);

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
                "Някор прибыл",
                "Някор появился в вашей колонии.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }

        private void ConfigureNyakor(Pawn pawn)
        {
            pawn.Name =
                new NameSingle("Някор");

            pawn.gender =
                Gender.Male;

            pawn.ageTracker.AgeBiologicalTicks =
                19L * 3600000L;

            pawn.ageTracker.AgeChronologicalTicks =
                19L * 3600000L;

            SetBackstories(pawn);
            SetXenotype(pawn);
            SetTraits(pawn);
            SetSkillsAndPassions(pawn);

            ClearGeneratedApparel(pawn);

            // Рубашка.
            GiveApparel(
                pawn,
                "Apparel_BasicShirt"
            );

            // Бронештаны.
            GiveApparel(
                pawn,
                "Apparel_FlakPants"
            );

            // Лабораторный халат.
            GiveApparel(
                pawn,
                "Apparel_LabCoat"
            );

            // Автоматический дробовик.
            GiveWeapon(
                pawn,
                "Gun_ChainShotgun"
            );

            ClearGeneratedInventory(pawn);

            // 3 пива.
            GiveInventoryItem(
                pawn,
                ThingDefOf.Beer,
                3
            );
        }

        // -------------------------
        // БИОГРАФИИ
        // -------------------------

        private void SetBackstories(Pawn pawn)
        {
            BackstoryDef childhood =
                DefDatabase<BackstoryDef>.GetNamedSilentFail(
                    "Kekelands_NyakorChildhood"
                );

            BackstoryDef adulthood =
                DefDatabase<BackstoryDef>.GetNamedSilentFail(
                    "Kekelands_NyakorAdulthood"
                );

            if (childhood != null)
                pawn.story.Childhood = childhood;
            else
                Log.Error(
                    "[AzamPrime] Не найдена детская биография Някора."
                );

            if (adulthood != null)
                pawn.story.Adulthood = adulthood;
            else
                Log.Error(
                    "[AzamPrime] Не найдена взрослая биография Някора."
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
                    "[AzamPrime] Не найден Kekelands_Old для Някора."
                );

                return;
            }

            pawn.genes?.SetXenotype(
                xenotype
            );
        }

        // -------------------------
        // ТРЕЙТЫ
        // -------------------------

        private void SetTraits(Pawn pawn)
        {
            if (pawn.story?.traits == null)
                return;

            pawn.story.traits.allTraits.Clear();

            // Аккуратный стрелок.
            AddTrait(
                pawn,
                "ShootingAccuracy",
                1
            );

            // Бегун.
            AddTrait(
                pawn,
                "SpeedOffset",
                1
            );

            // Доброта.
            AddTrait(
                pawn,
                "Kind",
                0
            );

            // Красота.
            AddTrait(
                pawn,
                "Beauty",
                1
            );

            // Снобизм.
            AddTrait(
                pawn,
                "Greedy",
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
                    "[AzamPrime] Для Някора не найден TraitDef: "
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
            SetSkill(
                pawn,
                SkillDefOf.Shooting,
                8,
                Passion.Major
            );

            SetSkill(
                pawn,
                SkillDefOf.Melee,
                10,
                Passion.Major
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
                2,
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
                5,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Animals,
                2,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Crafting,
                6,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Artistic,
                3,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Medicine,
                5,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Social,
                4,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Intellectual,
                8,
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

        // -------------------------
        // ОДЕЖДА
        // -------------------------

        private void ClearGeneratedApparel(Pawn pawn)
        {
            pawn.apparel?.DestroyAll();
        }

        private void GiveApparel(
            Pawn pawn,
            string defName)
        {
            ThingDef apparelDef =
                DefDatabase<ThingDef>.GetNamedSilentFail(
                    defName
                );

            if (apparelDef == null)
            {
                Log.Warning(
                    "[AzamPrime] Не найдена одежда Някора: "
                    + defName
                );

                return;
            }

            Apparel apparel =
                ThingMaker.MakeThing(
                    apparelDef
                ) as Apparel;

            if (apparel != null)
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
                Log.Warning(
                    "[AzamPrime] Не найдено оружие Някора: "
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

            thing.stackCount =
                count;

            pawn.inventory.innerContainer
                .TryAdd(thing);
        }
    }
}