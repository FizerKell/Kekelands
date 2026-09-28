using RimWorld;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_LamesArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;

            if (map == null)
                return false;

            PawnKindDef pawnKind =
                DefDatabase<PawnKindDef>.GetNamedSilentFail("Lames");

            if (pawnKind == null)
            {
                Log.Error("[AzamPrime] Не найден PawnKindDef Lames.");
                return false;
            }

            Pawn pawn =
                PawnGenerator.GeneratePawn(
                    pawnKind,
                    Faction.OfPlayer
                );

            if (pawn == null)
                return false;

            ConfigureLames(pawn);

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
                "Ламес прибыл",
                "Ламес появился в вашей колонии.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }

        private void ConfigureLames(Pawn pawn)
        {
            pawn.Name =
                new NameSingle("Ламес");

            pawn.gender =
                Gender.Male;

            pawn.ageTracker.AgeBiologicalTicks =
                17L * 3600000L;

            pawn.ageTracker.AgeChronologicalTicks =
                17L * 3600000L;

            SetBackstories(pawn);
            SetXenotype(pawn);
            SetTraits(pawn);
            SetSkillsAndPassions(pawn);
            SetAppearance(pawn);

            ClearGeneratedApparel(pawn);

            GiveApparel(
                pawn,
                "Apparel_Pants"
            );

            GiveApparel(
                pawn,
                "Apparel_BasicShirt"
            );

            GiveApparel(
                pawn,
                "Apparel_FlakVest"
            );

            GiveApparel(
                pawn,
                "Apparel_SimpleHelmet"
            );

            GiveWeapon(
                pawn,
                "Gun_AssaultRifle"
            );

            ClearGeneratedInventory(pawn);

            GiveInventoryItem(
                pawn,
                ThingDefOf.Jade,
                4
            );
        }

        private void SetBackstories(Pawn pawn)
        {
            BackstoryDef childhood =
                DefDatabase<BackstoryDef>.GetNamedSilentFail(
                    "Kekelands_LamesChildhood"
                );

            BackstoryDef adulthood =
                DefDatabase<BackstoryDef>.GetNamedSilentFail(
                    "Kekelands_LamesAdulthood"
                );

            if (childhood != null)
                pawn.story.Childhood = childhood;
            else
                Log.Error("[AzamPrime] Не найдена детская биография Ламеса.");

            if (adulthood != null)
                pawn.story.Adulthood = adulthood;
            else
                Log.Error("[AzamPrime] Не найдена взрослая биография Ламеса.");
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
                    "[AzamPrime] Не найден Kekelands_Cuckold для Ламеса."
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

            // Безупречная память.
            AddTrait(
                pawn,
                "GreatMemory",
                0
            );

            // Доброта.
            AddTrait(
                pawn,
                "Kind",
                0
            );

            // Лучик счастья.
            AddTrait(
                pawn,
                "Joyous",
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
                    "[AzamPrime] Для Ламеса не найден TraitDef: "
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
            SetSkill(pawn, SkillDefOf.Shooting, 8, Passion.None);
            SetSkill(pawn, SkillDefOf.Melee, 10, Passion.None);
            SetSkill(pawn, SkillDefOf.Construction, 11, Passion.None);
            SetSkill(pawn, SkillDefOf.Mining, 8, Passion.None);
            SetSkill(pawn, SkillDefOf.Cooking, 2, Passion.None);
            SetSkill(pawn, SkillDefOf.Plants, 7, Passion.None);
            SetSkill(pawn, SkillDefOf.Animals, 2, Passion.None);
            SetSkill(pawn, SkillDefOf.Crafting, 5, Passion.None);
            SetSkill(pawn, SkillDefOf.Artistic, 6, Passion.Minor);
            SetSkill(pawn, SkillDefOf.Medicine, 6, Passion.None);
            SetSkill(pawn, SkillDefOf.Social, 4, Passion.Minor);
            SetSkill(pawn, SkillDefOf.Intellectual, 7, Passion.Major);
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

            pawn.story.bodyType =
                BodyTypeDefOf.Male;

            string[] mediumHairDefs =
            {
                "Messy",
                "Bob",
                "Ponytail"
            };

            foreach (string defName in mediumHairDefs)
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
                    "[AzamPrime] Не найдена одежда Ламеса: "
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
                    "[AzamPrime] Не найдено оружие Ламеса: "
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