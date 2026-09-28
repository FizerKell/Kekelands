using RimWorld;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_AlbatrosArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;

            if (map == null)
                return false;

            PawnKindDef pawnKind =
                DefDatabase<PawnKindDef>.GetNamedSilentFail(
                    "Albatros"
                );

            if (pawnKind == null)
            {
                Log.Error(
                    "[AzamPrime] Не найден PawnKindDef Albatros."
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

            ConfigureAlbatros(pawn);

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
                "Альбатрос прибыл",
                "Альбатрос появился в вашей колонии.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }

        private void ConfigureAlbatros(Pawn pawn)
        {
            pawn.Name =
                new NameSingle("Альбатрос");

            pawn.gender =
                Gender.Male;

            pawn.ageTracker.AgeBiologicalTicks =
                20L * 3600000L;

            pawn.ageTracker.AgeChronologicalTicks =
                20L * 3600000L;

            SetBackstories(pawn);
            SetXenotype(pawn);
            SetTraits(pawn);
            SetSkillsAndPassions(pawn);
            SetAppearance(pawn);

            ClearGeneratedApparel(pawn);

            // Броня феникса.
            GiveApparel(
                pawn,
                "Apparel_PhoenixArmor"
            );

            // Меч.
            GiveWeapon(
                pawn,
                "MeleeWeapon_LongSword"
            );

            ClearGeneratedInventory(pawn);

            // 5 компонентов.
            GiveInventoryItem(
                pawn,
                ThingDefOf.ComponentIndustrial,
                5
            );

            // 2 медикамента.
            GiveInventoryItem(
                pawn,
                ThingDefOf.MedicineIndustrial,
                2
            );
        }

        private void SetBackstories(Pawn pawn)
        {
            BackstoryDef childhood =
                DefDatabase<BackstoryDef>.GetNamedSilentFail(
                    "Kekelands_AlbatrosChildhood"
                );

            BackstoryDef adulthood =
                DefDatabase<BackstoryDef>.GetNamedSilentFail(
                    "Kekelands_AlbatrosAdulthood"
                );

            if (childhood != null)
                pawn.story.Childhood = childhood;
            else
                Log.Error(
                    "[AzamPrime] Не найдена детская биография Альбатроса."
                );

            if (adulthood != null)
                pawn.story.Adulthood = adulthood;
            else
                Log.Error(
                    "[AzamPrime] Не найдена взрослая биография Альбатроса."
                );
        }

        private void SetXenotype(Pawn pawn)
        {
            XenotypeDef xenotype =
                DefDatabase<XenotypeDef>.GetNamedSilentFail(
                    "Kekelands_Old"
                );

            if (xenotype == null)
            {
                Log.Error(
                    "[AzamPrime] Не найден Kekelands_Old для Альбатроса."
                );

                return;
            }

            pawn.genes?.SetXenotype(
                xenotype
            );
        }

        private void SetTraits(Pawn pawn)
        {
            if (pawn.story?.traits == null)
                return;

            pawn.story.traits.allTraits.Clear();

            // Обжора.
            AddTrait(
                pawn,
                "Gourmand",
                0
            );

            // Психическая глухота.
            AddTrait(
                pawn,
                "PsychicSensitivity",
                -2
            );

            // Усердие.
            AddTrait(
                pawn,
                "Industriousness",
                2
            );

            // Подземник.
            AddTrait(
                pawn,
                "Undergrounder",
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
                    "[AzamPrime] Для Альбатроса не найден TraitDef: "
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

        private void SetSkillsAndPassions(Pawn pawn)
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
                15,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Construction,
                8,
                Passion.Minor
            );

            SetSkill(
                pawn,
                SkillDefOf.Mining,
                6,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Cooking,
                4,
                Passion.Major
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
                8,
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
                6,
                Passion.None
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
                5,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Intellectual,
                9,
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

            skill.Level =
                level;

            skill.passion =
                passion;
        }

        private void SetAppearance(Pawn pawn)
        {
            if (pawn.story == null)
                return;

            // Толстая форма тела.
            pawn.story.bodyType =
                BodyTypeDefOf.Fat;

            // Без волос.
            HairDef hair =
                DefDatabase<HairDef>.GetNamedSilentFail(
                    "Shaved"
                );

            if (hair != null)
            {
                pawn.story.hairDef =
                    hair;
            }
            else
            {
                Log.Warning(
                    "[AzamPrime] Не найдена причёска Shaved для Альбатроса."
                );
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
            ThingDef apparelDef =
                DefDatabase<ThingDef>.GetNamedSilentFail(
                    defName
                );

            if (apparelDef == null)
            {
                Log.Warning(
                    "[AzamPrime] Не найдена одежда Альбатроса: "
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
                    "[AzamPrime] Не найдено оружие Альбатроса: "
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