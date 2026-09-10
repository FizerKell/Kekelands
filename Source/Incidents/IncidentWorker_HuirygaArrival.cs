using RimWorld;
using UnityEngine;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_HuirygaArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;

            if (map == null)
                return false;

            PawnKindDef pawnKind =
                DefDatabase<PawnKindDef>.GetNamedSilentFail("Huiryga");

            if (pawnKind == null)
            {
                Log.Error("[AzamPrime] Не найден PawnKindDef Huiryga.");
                return false;
            }

            Pawn pawn = PawnGenerator.GeneratePawn(
                pawnKind,
                Faction.OfPlayer
            );

            if (pawn == null)
                return false;

            ConfigureHuiryga(pawn);

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
                "Хуирыга прибыл",
                "Хуирыга появился в вашей колонии.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }

        private void ConfigureHuiryga(Pawn pawn)
        {
            pawn.Name = new NameSingle("Хуирыга");

            pawn.gender = Gender.Male;

            pawn.ageTracker.AgeBiologicalTicks =
                19L * 3600000L;

            pawn.ageTracker.AgeChronologicalTicks =
                19L * 3600000L;

            SetXenotype(pawn);
            SetBodyType(pawn);
            SetTraits(pawn);
            SetSkillsAndPassions(pawn);
            SetHair(pawn);

            ClearGeneratedApparel(pawn);

            GiveBlackJacket(pawn);
            GiveApparel(pawn, "Apparel_SimpleHelmet");
            GiveApparel(pawn, "Apparel_Shades");

            GiveAutopistol(pawn);

            ClearGeneratedInventory(pawn);

            GiveInventoryItem(
                pawn,
                ThingDefOf.SmokeleafJoint,
                10
            );

            GiveInventoryItem(
                pawn,
                ThingDefOf.GoJuice,
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
                    "[AzamPrime] Не найден ксенотип Kekelands_Old для Хуирыги."
                );

                return;
            }

            pawn.genes?.SetXenotype(xenotype);
        }

        // -------------------------
        // ТЕЛО
        // -------------------------

        private void SetBodyType(Pawn pawn)
        {
            if (pawn.story != null)
            {
                pawn.story.bodyType =
                    BodyTypeDefOf.Male;
            }
        }

        // -------------------------
        // ТРЕЙТЫ
        // -------------------------

        private void SetTraits(Pawn pawn)
        {
            if (pawn.story?.traits == null)
                return;

            pawn.story.traits.allTraits.Clear();

            // Доброта.
            AddTrait(
                pawn,
                "Kind",
                0
            );

            // Интерес к химии.
            AddTrait(
                pawn,
                "DrugDesire",
                1
            );

            // Медленная обучаемость.
            AddTrait(
                pawn,
                "FastLearner",
                -1
            );

            // Оптимист.
            AddTrait(
                pawn,
                "NaturalMood",
                1
            );

            // Психическая глухота.
            AddTrait(
                pawn,
                "PsychicSensitivity",
                -2
            );

            // Трудолюбие.
            AddTrait(
                pawn,
                "Industriousness",
                1
            );

            // Хлюпик.
            AddTrait(
                pawn,
                "Wimp",
                0
            );

            // Невероятная красота.
            AddTrait(
                pawn,
                "Beauty",
                2
            );

            // Горе от ума.
            AddTrait(
                pawn,
                "TooSmart",
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
                    "[AzamPrime] Для Хуирыги не найден TraitDef: "
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

            /*
             * Строительство должно быть:
             *
             * > 4
             * < 8
             *
             * Поэтому случайно выбираем 5, 6 или 7.
             */
            pawn.skills
                .GetSkill(SkillDefOf.Construction)
                .Level = Rand.RangeInclusive(5, 7);

            SetSkillLevel(
                pawn,
                SkillDefOf.Mining,
                6
            );

            SetSkillLevel(
                pawn,
                SkillDefOf.Cooking,
                7
            );

            SetSkillLevel(
                pawn,
                SkillDefOf.Plants,
                5
            );

            SetSkillLevel(
                pawn,
                SkillDefOf.Animals,
                6
            );

            SetSkillLevel(
                pawn,
                SkillDefOf.Crafting,
                9
            );

            SetSkillLevel(
                pawn,
                SkillDefOf.Artistic,
                3
            );

            SetSkillLevel(
                pawn,
                SkillDefOf.Medicine,
                1
            );

            SetSkillLevel(
                pawn,
                SkillDefOf.Social,
                13
            );

            SetSkillLevel(
                pawn,
                SkillDefOf.Intellectual,
                2
            );

            /*
             * Ближний и дальний бой ты не указал,
             * поэтому их уровень оставляем случайным,
             * каким его сделал PawnGenerator.
             */

            // Сначала убираем случайные страсти.
            foreach (SkillRecord skill in pawn.skills.skills)
            {
                skill.passion = Passion.None;
            }

            // Общение — две страсти.
            pawn.skills
                .GetSkill(SkillDefOf.Social)
                .passion = Passion.Major;

            // Ремесло — две страсти.
            pawn.skills
                .GetSkill(SkillDefOf.Crafting)
                .passion = Passion.Major;
        }

        private void SetSkillLevel(
            Pawn pawn,
            SkillDef def,
            int level)
        {
            SkillRecord skill =
                pawn.skills.GetSkill(def);

            if (skill != null)
            {
                skill.Level = level;
            }
        }

        // -------------------------
        // ВОЛОСЫ
        // -------------------------

        private void SetHair(Pawn pawn)
        {
            if (pawn.story == null)
                return;

            /*
             * Пробуем несколько коротких ванильных
             * причёсок. Берём первую найденную.
             */
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
                    pawn.story.hairDef = hair;
                    return;
                }
            }

            Log.Warning(
                "[AzamPrime] Не удалось найти короткую причёску для Хуирыги."
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
            string defName)
        {
            ThingDef apparelDef =
                DefDatabase<ThingDef>.GetNamedSilentFail(
                    defName
                );

            if (apparelDef == null)
            {
                Log.Warning(
                    "[AzamPrime] Не найдена одежда Хуирыги: "
                    + defName
                );

                return;
            }

            Apparel apparel =
                ThingMaker.MakeThing(apparelDef)
                as Apparel;

            if (apparel == null)
                return;

            pawn.apparel.Wear(apparel);
        }

        private void GiveBlackJacket(Pawn pawn)
        {
            ThingDef jacketDef =
                DefDatabase<ThingDef>.GetNamedSilentFail(
                    "Apparel_Jacket"
                );

            if (jacketDef == null)
            {
                Log.Warning(
                    "[AzamPrime] Не найден Apparel_Jacket."
                );

                return;
            }

            Apparel jacket =
                ThingMaker.MakeThing(jacketDef)
                as Apparel;

            if (jacket == null)
                return;

            CompColorable colorable =
                jacket.TryGetComp<CompColorable>();

            if (colorable != null)
            {
                jacket.SetColor(
                    new Color(
                        0.05f,
                        0.05f,
                        0.05f
                    ),
                    true
                );
            }

            pawn.apparel.Wear(jacket);
        }

        // -------------------------
        // ОРУЖИЕ
        // -------------------------

        private void GiveAutopistol(Pawn pawn)
        {
            ThingDef weaponDef =
                DefDatabase<ThingDef>.GetNamedSilentFail(
                    "Gun_Autopistol"
                );

            if (weaponDef == null)
            {
                Log.Error(
                    "[AzamPrime] Не найден Gun_Autopistol."
                );

                return;
            }

            if (pawn.equipment?.Primary != null)
            {
                pawn.equipment.DestroyEquipment(
                    pawn.equipment.Primary
                );
            }

            ThingWithComps pistol =
                ThingMaker.MakeThing(weaponDef)
                as ThingWithComps;

            if (pistol == null)
                return;

            // Качество: хорошее.
            CompQuality quality =
                pistol.TryGetComp<CompQuality>();

            if (quality != null)
            {
                quality.SetQuality(
                    QualityCategory.Good,
                    ArtGenerationContext.Colony
                );
            }

            pawn.equipment.AddEquipment(
                pistol
            );
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