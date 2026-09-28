using RimWorld;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_KoshmishArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;

            if (map == null)
                return false;

            PawnKindDef pawnKind =
                DefDatabase<PawnKindDef>.GetNamedSilentFail(
                    "Koshmish"
                );

            if (pawnKind == null)
            {
                Log.Error(
                    "[AzamPrime] Не найден PawnKindDef Koshmish."
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

            ConfigureKoshmish(pawn);

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
                "Кощмищ прибыл",
                "Кощмищ появился в вашей колонии.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }

        private void ConfigureKoshmish(Pawn pawn)
        {
            pawn.Name =
                new NameSingle("Кощмищ");

            pawn.gender =
                Gender.Male;

            pawn.ageTracker.AgeBiologicalTicks =
                13L * 3600000L;

            pawn.ageTracker.AgeChronologicalTicks =
                13L * 3600000L;

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
                "Apparel_Tshirt"
            );

            GiveWeapon(
                pawn,
                "Gun_Autopistol"
            );

            ClearGeneratedInventory(pawn);

            GiveInventoryItemByName(
                pawn,
                "BlocksGranite",
                4
            );
        }

        private void SetBackstories(Pawn pawn)
        {
            BackstoryDef childhood =
                DefDatabase<BackstoryDef>.GetNamedSilentFail(
                    "Kekelands_KoshmishChildhood"
                );

            BackstoryDef adulthood =
                DefDatabase<BackstoryDef>.GetNamedSilentFail(
                    "Kekelands_KoshmishAdulthood"
                );

            if (childhood != null)
                pawn.story.Childhood = childhood;
            else
                Log.Error(
                    "[AzamPrime] Не найдена детская биография Кощмища."
                );

            if (adulthood != null)
                pawn.story.Adulthood = adulthood;
            else
                Log.Error(
                    "[AzamPrime] Не найдена взрослая биография Кощмища."
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
                    "[AzamPrime] Не найден Kekelands_Old для Кощмища."
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

            AddTrait(pawn, "TooSmart", 0);
            AddTrait(pawn, "SpeedOffset", 1);
            AddTrait(pawn, "Disturbing", 0);
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
                    "[AzamPrime] Для Кощмища не найден TraitDef: "
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
                8,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Melee,
                4,
                Passion.None
            );

            // Строительство < 6.
            SetSkill(
                pawn,
                SkillDefOf.Construction,
                Rand.RangeInclusive(0, 5),
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Mining,
                8,
                Passion.None
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
                1,
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
                16,
                Passion.Major
            );

            SetSkill(
                pawn,
                SkillDefOf.Medicine,
                3,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Social,
                18,
                Passion.Major
            );

            SetSkill(
                pawn,
                SkillDefOf.Intellectual,
                6,
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

            // Худое телосложение.
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
                    "[AzamPrime] Не найдена одежда Кощмища: "
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
                    "[AzamPrime] Не найдено оружие Кощмища: "
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
                    "[AzamPrime] Не найден предмет Кощмища: "
                    + defName
                );

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