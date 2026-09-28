using RimWorld;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_SaphirArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;

            if (map == null)
                return false;

            PawnKindDef pawnKind =
                DefDatabase<PawnKindDef>.GetNamedSilentFail("Saphir");

            if (pawnKind == null)
            {
                Log.Error("[AzamPrime] Не найден PawnKindDef Saphir.");
                return false;
            }

            Pawn pawn =
                PawnGenerator.GeneratePawn(
                    pawnKind,
                    Faction.OfPlayer
                );

            if (pawn == null)
                return false;

            ConfigureSaphir(pawn);

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
                "Сапфир прибыл",
                "Сапфир появился в вашей колонии.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }

        private void ConfigureSaphir(Pawn pawn)
        {
            pawn.Name =
                new NameSingle("Сапфир");

            pawn.gender =
                Gender.Male;

            // Возраст пока не фиксируем — ты его не указал.

            SetBackstories(pawn);
            SetXenotype(pawn);
            SetTraits(pawn);
            SetSkillsAndPassions(pawn);
            SetAppearance(pawn);

            ClearGeneratedApparel(pawn);

            GiveApparel(
                pawn,
                "Apparel_Cape"
            );

            GiveApparel(
                pawn,
                "Apparel_Tuque"
            );

            // Винтовка.
            GiveWeapon(
                pawn,
                "Gun_BoltActionRifle"
            );

            ClearGeneratedInventory(pawn);

            GiveInventoryItem(
                pawn,
                ThingDefOf.Gold,
                3
            );

            GiveInventoryItem(
                pawn,
                ThingDefOf.MedicineIndustrial,
                1
            );

            GiveInventoryItem(
                pawn,
                ThingDefOf.Plasteel,
                10
            );
        }

        private void SetBackstories(Pawn pawn)
        {
            BackstoryDef childhood =
                DefDatabase<BackstoryDef>.GetNamedSilentFail(
                    "Kekelands_SaphirChildhood"
                );

            BackstoryDef adulthood =
                DefDatabase<BackstoryDef>.GetNamedSilentFail(
                    "Kekelands_SaphirAdulthood"
                );

            if (childhood != null)
                pawn.story.Childhood = childhood;
            else
                Log.Error("[AzamPrime] Не найдена детская биография Сапфира.");

            if (adulthood != null)
                pawn.story.Adulthood = adulthood;
            else
                Log.Error("[AzamPrime] Не найдена взрослая биография Сапфира.");
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
                    "[AzamPrime] Не найден Kekelands_Old для Сапфира."
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

            // Завистливость.
            AddTrait(
                pawn,
                "Jealous",
                0
            );

            // Оптимист.
            AddTrait(
                pawn,
                "NaturalMood",
                1
            );

            // Сова.
            AddTrait(
                pawn,
                "NightOwl",
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
                    "[AzamPrime] Для Сапфира не найден TraitDef: "
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
            SetSkill(pawn, SkillDefOf.Shooting, 5, Passion.None);
            SetSkill(pawn, SkillDefOf.Melee, 3, Passion.None);
            SetSkill(pawn, SkillDefOf.Construction, 5, Passion.None);
            SetSkill(pawn, SkillDefOf.Mining, 5, Passion.None);
            SetSkill(pawn, SkillDefOf.Cooking, 6, Passion.None);
            SetSkill(pawn, SkillDefOf.Plants, 5, Passion.Minor);
            SetSkill(pawn, SkillDefOf.Animals, 0, Passion.None);
            SetSkill(pawn, SkillDefOf.Crafting, 3, Passion.None);
            SetSkill(pawn, SkillDefOf.Artistic, 6, Passion.None);
            SetSkill(pawn, SkillDefOf.Medicine, 5, Passion.None);
            SetSkill(pawn, SkillDefOf.Social, 5, Passion.Minor);
            SetSkill(pawn, SkillDefOf.Intellectual, 10, Passion.Major);
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

        private void SetAppearance(Pawn pawn)
        {
            if (pawn.story == null)
                return;

            pawn.story.bodyType =
                BodyTypeDefOf.Male;

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
            ThingDef apparelDef =
                DefDatabase<ThingDef>.GetNamedSilentFail(
                    defName
                );

            if (apparelDef == null)
            {
                Log.Warning(
                    "[AzamPrime] Не найдена одежда Сапфира: "
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
                    "[AzamPrime] Не найдено оружие Сапфира: "
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