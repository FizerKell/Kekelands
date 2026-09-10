using RimWorld;
using UnityEngine;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_GlebaArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;

            if (map == null)
                return false;

            PawnKindDef pawnKind =
                DefDatabase<PawnKindDef>.GetNamedSilentFail("Gleba");

            if (pawnKind == null)
            {
                Log.Error("[AzamPrime] Не найден PawnKindDef Gleba.");
                return false;
            }

            Pawn pawn = PawnGenerator.GeneratePawn(
                pawnKind,
                Faction.OfPlayer
            );

            if (pawn == null)
                return false;

            ConfigureGleba(pawn);

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
                "Глеба прибыл",
                "Глеба появился в вашей колонии.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }

        private void ConfigureGleba(Pawn pawn)
        {
            pawn.Name = new NameSingle("Глеба");

            pawn.gender = Gender.Male;

            pawn.ageTracker.AgeBiologicalTicks =
                16L * 3600000L;

            pawn.ageTracker.AgeChronologicalTicks =
                16L * 3600000L;

            SetXenotype(pawn);
            SetBody(pawn);
            SetTraits(pawn);
            SetSkills(pawn);
            SetAppearance(pawn);

            ClearGeneratedApparel(pawn);

            GiveRedCape(pawn);
            GiveApparel(pawn, "Apparel_FlakPants");

            GiveWeapon(pawn);

            ClearGeneratedInventory(pawn);

            GiveInventoryItem(
                pawn,
                ThingDefOf.MeleeWeapon_Knife,
                1
            );

            GiveInventoryItem(
                pawn,
                ThingDefOf.Gold,
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
                    "[AzamPrime] Не найден ксенотип Kekelands_Old для Глебы."
                );

                return;
            }

            pawn.genes?.SetXenotype(xenotype);
        }

        // -------------------------
        // ВНЕШНОСТЬ
        // -------------------------

        private void SetBody(Pawn pawn)
        {
            if (pawn.story == null)
                return;

            // Женская форма тела при мужском поле.
            pawn.story.bodyType = BodyTypeDefOf.Female;
        }

        private void SetAppearance(Pawn pawn)
        {
            if (pawn.story == null)
                return;

            // Белые волосы.
            pawn.story.HairColor = Color.white;

            // Длинные волосы.
            HairDef hair =
                DefDatabase<HairDef>.GetNamedSilentFail(
                    "LongHair"
                );

            if (hair != null)
            {
                pawn.story.hairDef = hair;
            }
            else
            {
                Log.Warning(
                    "[AzamPrime] Не найдена причёска LongHair для Глебы."
                );
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

            // Железные нервы.
            AddTrait(pawn, "Nerves", -2);

            // Пиромания.
            AddTrait(pawn, "Pyromaniac", 0);

            // Слабый иммунитет.
            AddTrait(pawn, "Immunity", -1);

            // Стрессоустойчивость.
            AddTrait(pawn, "Nerves", -1);

            // Трудолюбие.
            AddTrait(pawn, "Industriousness", 1);

            // Красота.
            AddTrait(pawn, "Beauty", 1);

            // Мазохизм.
            AddTrait(pawn, "Masochist", 0);

            // Непонятый творец.
            AddTrait(pawn, "TorturedArtist", 0);
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
                    "[AzamPrime] Не найден TraitDef для Глебы: "
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

        private void SetSkills(Pawn pawn)
        {
            if (pawn.skills == null)
                return;

            SetSkill(
                pawn,
                SkillDefOf.Melee,
                2,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Shooting,
                10,
                Passion.Minor
            );

            SetSkill(
                pawn,
                SkillDefOf.Construction,
                6,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Cooking,
                6,
                Passion.Major
            );

            SetSkill(
                pawn,
                SkillDefOf.Mining,
                8,
                Passion.None
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
                2,
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
                10,
                Passion.Major
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
                12,
                Passion.None
            );
        }

        private void SetSkill(
            Pawn pawn,
            SkillDef skillDef,
            int level,
            Passion passion)
        {
            SkillRecord skill =
                pawn.skills.GetSkill(skillDef);

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
            string defName)
        {
            ThingDef def =
                DefDatabase<ThingDef>.GetNamedSilentFail(
                    defName
                );

            if (def == null)
            {
                Log.Warning(
                    "[AzamPrime] Не найдена одежда Глебы: "
                    + defName
                );

                return;
            }

            Apparel apparel =
                ThingMaker.MakeThing(def)
                as Apparel;

            if (apparel == null)
                return;

            pawn.apparel.Wear(apparel);
        }

        private void GiveRedCape(Pawn pawn)
        {
            ThingDef capeDef =
                DefDatabase<ThingDef>.GetNamedSilentFail(
                    "Apparel_Cape"
                );

            if (capeDef == null)
            {
                Log.Warning(
                    "[AzamPrime] Не найден Apparel_Cape."
                );

                return;
            }

            Apparel cape =
                ThingMaker.MakeThing(capeDef)
                as Apparel;

            if (cape == null)
                return;

            // Красим плащ в красный,
            // если одежда поддерживает окраску.
            CompColorable colorable =
                cape.TryGetComp<CompColorable>();

            if (colorable != null)
            {
                cape.SetColor(
                    new Color(
                        0.75f,
                        0.05f,
                        0.05f
                    ),
                    true
                );
            }

            pawn.apparel.Wear(cape);
        }

        // -------------------------
        // ОРУЖИЕ
        // -------------------------

        private void GiveWeapon(Pawn pawn)
        {
            ThingDef weaponDef =
                DefDatabase<ThingDef>.GetNamedSilentFail(
                    "Gun_HellcatRifle"
                );

            if (weaponDef == null)
            {
                Log.Error(
                    "[AzamPrime] Не найдена винтовка Gun_HellcatRifle."
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
                ThingMaker.MakeThing(weaponDef)
                as ThingWithComps;

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