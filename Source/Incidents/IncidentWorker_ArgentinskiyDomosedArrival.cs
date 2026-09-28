using RimWorld;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_ArgentinskiyDomosedArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;

            if (map == null)
                return false;

            PawnKindDef pawnKind =
                DefDatabase<PawnKindDef>.GetNamedSilentFail(
                    "ArgentinskiyDomosed"
                );

            if (pawnKind == null)
            {
                Log.Error(
                    "[AzamPrime] Не найден PawnKindDef ArgentinskiyDomosed."
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

            ConfigurePawn(pawn);

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
                "Аргентинский домосед прибыл",
                "Аргентинский домосед появился в вашей колонии.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }

        private void ConfigurePawn(Pawn pawn)
        {
            pawn.Name =
                new NameSingle("Аргентинский домосед");

            pawn.gender =
                Gender.Male;

            pawn.ageTracker.AgeBiologicalTicks =
                16L * 3600000L;

            pawn.ageTracker.AgeChronologicalTicks =
                16L * 3600000L;

            SetBackstories(pawn);
            SetXenotype(pawn);
            SetTraits(pawn);
            SetSkills(pawn);

            ClearGeneratedApparel(pawn);

            GiveApparel(
                pawn,
                "Apparel_Cape",
                null
            );

            GiveApparel(
                pawn,
                "Apparel_BasicShirt",
                DefDatabase<ThingDef>.GetNamed("Hyperweave")
            );

            GiveApparel(
                pawn,
                "Apparel_Beret",
                null
            );

            GiveApparel(
                pawn,
                "Apparel_FlakVest",
                null
            );

            GiveShotgun(pawn);

            ClearGeneratedInventory(pawn);

            GiveInventoryItem(
                pawn,
                ThingDefOf.MedicineIndustrial,
                2
            );

            GiveInventoryItem(
                pawn,
                ThingDefOf.WakeUp,
                1
            );
        }

        private void SetBackstories(Pawn pawn)
        {
            BackstoryDef childhood =
                DefDatabase<BackstoryDef>.GetNamedSilentFail(
                    "Kekelands_ArgentinskiyDomosedChildhood"
                );

            BackstoryDef adulthood =
                DefDatabase<BackstoryDef>.GetNamedSilentFail(
                    "Kekelands_ArgentinskiyDomosedAdulthood"
                );

            if (childhood != null)
                pawn.story.Childhood = childhood;
            else
                Log.Error(
                    "[AzamPrime] Не найдена детская биография Аргентинского домоседа."
                );

            if (adulthood != null)
                pawn.story.Adulthood = adulthood;
            else
                Log.Error(
                    "[AzamPrime] Не найдена взрослая биография Аргентинского домоседа."
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
                    "[AzamPrime] Не найден Kekelands_Cuckold для Аргентинского домоседа."
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

            // Сильный иммунитет.
            AddTrait(
                pawn,
                "Immunity",
                1
            );

            // Трезвенник.
            AddTrait(
                pawn,
                "DrugDesire",
                -1
            );

            // Жизнелюбие.
            AddTrait(
                pawn,
                "NaturalMood",
                2
            );

            // Вселяет тревогу.
            AddTrait(
                pawn,
                "Disturbing",
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
                    "[AzamPrime] Для Аргентинского домоседа не найден TraitDef: "
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
            SetSkill(pawn, SkillDefOf.Shooting, 3, Passion.None);
            SetSkill(pawn, SkillDefOf.Melee, 3, Passion.None);
            SetSkill(pawn, SkillDefOf.Construction, 8, Passion.Minor);
            SetSkill(pawn, SkillDefOf.Mining, 6, Passion.None);
            SetSkill(pawn, SkillDefOf.Cooking, 7, Passion.None);
            SetSkill(pawn, SkillDefOf.Plants, 5, Passion.None);
            SetSkill(pawn, SkillDefOf.Animals, 2, Passion.None);
            SetSkill(pawn, SkillDefOf.Crafting, 5, Passion.None);
            SetSkill(pawn, SkillDefOf.Artistic, 4, Passion.None);
            SetSkill(pawn, SkillDefOf.Medicine, 1, Passion.None);
            SetSkill(pawn, SkillDefOf.Social, 6, Passion.Minor);
            SetSkill(pawn, SkillDefOf.Intellectual, 8, Passion.Major);
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
                    "[AzamPrime] Не найдена одежда Аргентинского домоседа: "
                    + defName
                );

                return;
            }

            Apparel apparel =
                ThingMaker.MakeThing(
                    apparelDef,
                    stuff
                ) as Apparel;

            if (apparel != null)
                pawn.apparel.Wear(apparel);
        }

        private void GiveShotgun(Pawn pawn)
        {
            ThingDef weaponDef =
                DefDatabase<ThingDef>.GetNamedSilentFail(
                    "Gun_PumpShotgun"
                );

            if (weaponDef == null)
            {
                Log.Warning(
                    "[AzamPrime] Не найден Gun_PumpShotgun."
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

            if (weapon == null)
                return;

            CompQuality quality =
                weapon.TryGetComp<CompQuality>();

            quality?.SetQuality(
                QualityCategory.Good,
                ArtGenerationContext.Colony
            );

            pawn.equipment.AddEquipment(
                weapon
            );
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