using RimWorld;
using UnityEngine;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_SuperhuyArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;

            if (map == null)
                return false;

            PawnKindDef pawnKind =
                DefDatabase<PawnKindDef>.GetNamedSilentFail(
                    "Superhuy"
                );

            if (pawnKind == null)
            {
                Log.Error(
                    "[AzamPrime] Не найден PawnKindDef Superhuy."
                );

                return false;
            }

            Pawn pawn = PawnGenerator.GeneratePawn(
                pawnKind,
                Faction.OfPlayer
            );

            if (pawn == null)
                return false;

            ConfigureSuperhuy(pawn);

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
                "Суперхуй прибыл",
                "Суперхуй появился в вашей колонии.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }

        private void ConfigureSuperhuy(Pawn pawn)
        {
            pawn.Name =
                new NameSingle("Суперхуй");

            pawn.gender =
                Gender.Male;

            pawn.ageTracker.AgeBiologicalTicks =
                19L * 3600000L;

            pawn.ageTracker.AgeChronologicalTicks =
                19L * 3600000L;

            SetXenotype(pawn);
            SetTraits(pawn);
            SetSkillsAndPassions(pawn);
            SetAppearance(pawn);

            ClearGeneratedApparel(pawn);

            // Вязаная шапка.
            GiveApparel(
                pawn,
                "Apparel_Tuque",
                null
            );

            // Обычные штаны.
            GiveApparel(
                pawn,
                "Apparel_Pants",
                null
            );

            // Рубашка из альтекса.
            GiveApparel(
                pawn,
                "Apparel_BasicShirt",
                ThingDefOf.Hyperweave
            );

            // Плащ.
            GiveApparel(
                pawn,
                "Apparel_Cape",
                null
            );

            // Дубина.
            GiveWeapon(
                pawn,
                "MeleeWeapon_Club"
            );

            ClearGeneratedInventory(pawn);

            // 2 блока мрамора.
            GiveInventoryItemByName(
                pawn,
                "BlocksMarble",
                2
            );

            // 1 химтопливо.
            GiveInventoryItem(
                pawn,
                ThingDefOf.Chemfuel,
                1
            );

            // 3 пива.
            GiveInventoryItem(
                pawn,
                ThingDefOf.Beer,
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
                    "[AzamPrime] Не найден Kekelands_Old для Суперхуя."
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

            // Неприятный голос.
            AddTrait(
                pawn,
                "AnnoyingVoice",
                0
            );

            // Интерес к химии.
            AddTrait(
                pawn,
                "DrugDesire",
                1
            );

            // Медлительность.
            AddTrait(
                pawn,
                "SpeedOffset",
                -1
            );

            // Медленная обучаемость.
            AddTrait(
                pawn,
                "FastLearner",
                -1
            );

            // Лёгкая неврастения.
            AddTrait(
                pawn,
                "Neurotic",
                1
            );

            // Биоконсерватор.
            AddTrait(
                pawn,
                "BodyPurist",
                0
            );

            // Сова.
            AddTrait(
                pawn,
                "NightOwl",
                0
            );

            // Хорошая память.
            AddTrait(
                pawn,
                "GreatMemory",
                0
            );

            // Оккультист.
            AddTrait(
                pawn,
                "Occultist",
                0
            );

            // Уродство.
            AddTrait(
                pawn,
                "Beauty",
                -1
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
                    "[AzamPrime] Для Суперхуя не найден TraitDef: "
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
                0,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Melee,
                13,
                Passion.Major
            );

            SetSkill(
                pawn,
                SkillDefOf.Construction,
                8,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Mining,
                15,
                Passion.Major
            );

            SetSkill(
                pawn,
                SkillDefOf.Cooking,
                2,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Plants,
                3,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Animals,
                0,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Crafting,
                4,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Artistic,
                1,
                Passion.Minor
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

            SetSkill(
                pawn,
                SkillDefOf.Intellectual,
                14,
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

            // Чёрные волосы.
            pawn.story.HairColor =
                Color.black;

            /*
             * Борода.
             * Пробуем несколько названий, чтобы
             * не ломать персонажа из-за одного defName.
             */
            string[] beardDefs =
            {
                "FullBeard",
                "Beard_Full",
                "Bushy"
            };

            foreach (string defName in beardDefs)
            {
                BeardDef beard =
                    DefDatabase<BeardDef>.GetNamedSilentFail(
                        defName
                    );

                if (beard != null)
                {
                    pawn.style.beardDef =
                        beard;

                    break;
                }
            }
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
                    "[AzamPrime] Не найдена одежда Суперхуя: "
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
                    "[AzamPrime] Не найдено оружие Суперхуя: "
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
                    "[AzamPrime] Не найден предмет Суперхуя: "
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
    }
}