using RimWorld;
using UnityEngine;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_VergilyushaArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;

            if (map == null)
                return false;

            PawnKindDef pawnKind =
                DefDatabase<PawnKindDef>.GetNamedSilentFail(
                    "Vergilyusha"
                );

            if (pawnKind == null)
            {
                Log.Error(
                    "[AzamPrime] Не найден PawnKindDef Vergilyusha."
                );

                return false;
            }

            Pawn pawn = PawnGenerator.GeneratePawn(
                pawnKind,
                Faction.OfPlayer
            );

            if (pawn == null)
                return false;

            ConfigureVergilyusha(pawn);

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

            // Спавним принадлежащую игроку лошадь рядом.
            SpawnHorse(
                pawn,
                map
            );

            Find.LetterStack.ReceiveLetter(
                "Вергилюша прибыл",
                "Вергилюша появился в вашей колонии вместе с лошадью.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }

        private void ConfigureVergilyusha(Pawn pawn)
        {
            pawn.Name =
                new NameSingle("Вергилюша");

            pawn.gender =
                Gender.Male;

            pawn.ageTracker.AgeBiologicalTicks =
                15L * 3600000L;

            pawn.ageTracker.AgeChronologicalTicks =
                15L * 3600000L;

            SetXenotype(pawn);
            SetTraits(pawn);
            SetSkillsAndPassions(pawn);
            SetAppearance(pawn);

            ClearGeneratedApparel(pawn);

            // Штаны.
            GiveApparel(
                pawn,
                "Apparel_Pants",
                null
            );

            // Обычная рубашка.
            GiveApparel(
                pawn,
                "Apparel_BasicShirt",
                null
            );

            // Простой шлем из пластали.
            GiveApparel(
                pawn,
                "Apparel_SimpleHelmet",
                ThingDefOf.Plasteel
            );

            // Бронежилет.
            GiveApparel(
                pawn,
                "Apparel_FlakVest",
                null
            );

            // Длинный меч.
            GiveWeapon(
                pawn,
                "MeleeWeapon_LongSword"
            );

            ClearGeneratedInventory(pawn);

            // Психолист.
            GiveInventoryItemByName(
                pawn,
                "PsychoidLeaves",
                1
            );

            // 3 пеммикана.
            GiveInventoryItem(
                pawn,
                ThingDefOf.Pemmican,
                3
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
                    "[AzamPrime] Не найден Kekelands_Old для Вергилюши."
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

            // Бегун.
            AddTrait(
                pawn,
                "SpeedOffset",
                1
            );

            // Тяжёлая неврастения.
            AddTrait(
                pawn,
                "Neurotic",
                2
            );

            // Хорошая память.
            AddTrait(
                pawn,
                "GreatMemory",
                0
            );

            // Жизнелюбие.
            AddTrait(
                pawn,
                "NaturalMood",
                2
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
                    "[AzamPrime] Для Вергилюши не найден TraitDef: "
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
                SkillDefOf.Melee,
                7,
                Passion.Minor
            );

            SetSkill(
                pawn,
                SkillDefOf.Shooting,
                3,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Construction,
                2,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Mining,
                4,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Cooking,
                6,
                Passion.Minor
            );

            SetSkill(
                pawn,
                SkillDefOf.Plants,
                4,
                Passion.Minor
            );

            SetSkill(
                pawn,
                SkillDefOf.Animals,
                13,
                Passion.Major
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
                4,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Medicine,
                2,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Social,
                3,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Intellectual,
                4,
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

            if (skill == null)
                return;

            skill.Level =
                level;

            skill.passion =
                passion;
        }

        // -------------------------
        // ВНЕШНОСТЬ
        // -------------------------

        private void SetAppearance(Pawn pawn)
        {
            if (pawn.story == null)
                return;

            // Тонкое телосложение.
            pawn.story.bodyType =
                BodyTypeDefOf.Thin;

            // Чёрные волосы.
            pawn.story.HairColor =
                Color.black;

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

                    return;
                }
            }

            Log.Warning(
                "[AzamPrime] Не найдена короткая причёска для Вергилюши."
            );
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
                    "[AzamPrime] Не найдена одежда Вергилюши: "
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

            pawn.apparel.Wear(
                apparel
            );
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
                    "[AzamPrime] Не найдено оружие Вергилюши: "
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

        private void GiveInventoryItemByName(
            Pawn pawn,
            string defName,
            int count)
        {
            ThingDef def =
                DefDatabase<ThingDef>.GetNamedSilentFail(
                    defName
                );

            if (def == null)
            {
                Log.Warning(
                    "[AzamPrime] Не найден предмет Вергилюши: "
                    + defName
                );

                return;
            }

            GiveInventoryItem(
                pawn,
                def,
                count
            );
        }

        // -------------------------
        // ЛОШАДЬ
        // -------------------------

        private void SpawnHorse(
            Pawn owner,
            Map map)
        {
            PawnKindDef horseKind =
                DefDatabase<PawnKindDef>.GetNamedSilentFail(
                    "Horse"
                );

            if (horseKind == null)
            {
                Log.Warning(
                    "[AzamPrime] Не найден PawnKindDef Horse."
                );

                return;
            }

            Pawn horse =
                PawnGenerator.GeneratePawn(
                    horseKind,
                    Faction.OfPlayer
                );

            if (horse == null)
                return;

            IntVec3 horseCell =
                CellFinder.RandomClosewalkCellNear(
                    owner.Position,
                    map,
                    3
                );

            GenSpawn.Spawn(
                horse,
                horseCell,
                map
            );
        }
    }
}