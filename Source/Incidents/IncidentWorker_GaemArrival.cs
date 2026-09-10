using RimWorld;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_GaemArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;

            if (map == null)
                return false;

            PawnKindDef pawnKind =
                DefDatabase<PawnKindDef>.GetNamedSilentFail("Gaem");

            if (pawnKind == null)
            {
                Log.Error("[AzamPrime] Не найден PawnKindDef Gaem.");
                return false;
            }

            Pawn pawn = PawnGenerator.GeneratePawn(
                pawnKind,
                Faction.OfPlayer
            );

            if (pawn == null)
                return false;

            ConfigureGaem(pawn);

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
                "Гаем прибыл",
                "Гаем появился в вашей колонии.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }

        private void ConfigureGaem(Pawn pawn)
        {
            pawn.Name = new NameSingle("Гаем");
            pawn.gender = Gender.Male;

            pawn.ageTracker.AgeBiologicalTicks =
                17L * 3600000L;

            pawn.ageTracker.AgeChronologicalTicks =
                17L * 3600000L;

            SetXenotype(pawn);
            SetTraits(pawn);
            SetSkillsAndPassions(pawn);
            SetAppearance(pawn);

            ClearGeneratedApparel(pawn);

            GiveApparel(
                pawn,
                "Apparel_Pants",
                null
            );

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

            // Обычные очки.
            GiveApparel(
                pawn,
                "Apparel_Glasses",
                null
            );

            GiveTomahawk(pawn);

            ClearGeneratedInventory(pawn);

            // 1 пачка лекарственных трав.
            GiveInventoryItem(
                pawn,
                ThingDefOf.MedicineHerbal,
                1
            );

            // 1 бодрин.
            GiveInventoryItem(
                pawn,
                ThingDefOf.WakeUp,
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
                    "[AzamPrime] Не найден Kekelands_Old для Гаема."
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

            // Нытик — наш кастомный.
            AddTrait(
                pawn,
                "Kekelands_Whiner",
                0
            );

            // Непонятый творец.
            AddTrait(
                pawn,
                "TorturedArtist",
                0
            );

            // Нервозность.
            AddTrait(
                pawn,
                "Nerves",
                1
            );

            /*
             * Забывчивость.
             * Если этого TraitDef нет в твоей сборке,
             * просто увидим предупреждение в логе.
             */
            AddTrait(
                pawn,
                "Forgetful",
                0
            );

            // Лучик счастья — Anomaly.
            AddTrait(
                pawn,
                "Joyous",
                0
            );

            // Обжора.
            AddTrait(
                pawn,
                "Gourmand",
                0
            );

            // Пессимист.
            AddTrait(
                pawn,
                "NaturalMood",
                -1
            );

            // Сильный иммунитет.
            AddTrait(
                pawn,
                "Immunity",
                1
            );

            // Эффективный сон.
            AddTrait(
                pawn,
                "QuickSleeper",
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
                    "[AzamPrime] Для Гаема не найден TraitDef: "
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
                5,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Melee,
                6,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Construction,
                1,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Mining,
                1,
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
                2,
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
                0,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Artistic,
                10,
                Passion.Major
            );

            SetSkill(
                pawn,
                SkillDefOf.Medicine,
                8,
                Passion.Minor
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
                5,
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

                    return;
                }
            }

            Log.Warning(
                "[AzamPrime] Не найдена короткая причёска для Гаема."
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
                    "[AzamPrime] Не найдена одежда Гаема: "
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
        // ТОМАГАВК
        // -------------------------

        private void GiveTomahawk(Pawn pawn)
        {
            /*
             * Томагавк не относится к стандартному набору
             * старых Core-оружий, поэтому ищем несколько
             * возможных DefName.
             */
            string[] possibleDefs =
            {
                "MeleeWeapon_Tomahawk",
                "Tomahawk"
            };

            ThingDef weaponDef = null;

            foreach (string defName in possibleDefs)
            {
                weaponDef =
                    DefDatabase<ThingDef>.GetNamedSilentFail(
                        defName
                    );

                if (weaponDef != null)
                    break;
            }

            if (weaponDef == null)
            {
                Log.Warning(
                    "[AzamPrime] Не найден томагавк для Гаема."
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