using RimWorld;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_DraydonArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;

            if (map == null)
                return false;

            PawnKindDef pawnKind =
                DefDatabase<PawnKindDef>.GetNamedSilentFail(
                    "Draydon"
                );

            if (pawnKind == null)
            {
                Log.Error(
                    "[AzamPrime] Не найден PawnKindDef Draydon."
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

            ConfigureDraydon(pawn);

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
                "Дрейдон прибыл",
                "Дрейдон появился в вашей колонии.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }

        private void ConfigureDraydon(Pawn pawn)
        {
            pawn.Name =
                new NameSingle("Дрейдон");

            pawn.gender =
                Gender.Male;

            pawn.ageTracker.AgeBiologicalTicks =
                16L * 3600000L;

            pawn.ageTracker.AgeChronologicalTicks =
                16L * 3600000L;

            SetBackstories(pawn);
            SetXenotype(pawn);
            SetBody(pawn);
            SetTraits(pawn);
            SetSkillsAndPassions(pawn);

            ClearGeneratedApparel(pawn);

            // Бронекуртка.
            GiveApparel(
                pawn,
                "Apparel_FlakJacket"
            );

            // Бронештаны.
            GiveApparel(
                pawn,
                "Apparel_FlakPants"
            );

            // Маска войны.
            GiveApparel(
                pawn,
                "Apparel_WarMask"
            );

            // Гладиус.
            GiveWeapon(
                pawn,
                "MeleeWeapon_Gladius"
            );

            ClearGeneratedInventory(pawn);

            GiveInventoryItem(
                pawn,
                ThingDefOf.Steel,
                10
            );

            GiveInventoryItemByName(
                pawn,
                "BlocksSlate",
                3
            );
        }

        private void SetBackstories(Pawn pawn)
        {
            BackstoryDef childhood =
                DefDatabase<BackstoryDef>.GetNamedSilentFail(
                    "Kekelands_DraydonChildhood"
                );

            BackstoryDef adulthood =
                DefDatabase<BackstoryDef>.GetNamedSilentFail(
                    "Kekelands_DraydonAdulthood"
                );

            if (childhood != null)
                pawn.story.Childhood = childhood;
            else
                Log.Error(
                    "[AzamPrime] Не найдена детская биография Дрейдона."
                );

            if (adulthood != null)
                pawn.story.Adulthood = adulthood;
            else
                Log.Error(
                    "[AzamPrime] Не найдена взрослая биография Дрейдона."
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
                    "[AzamPrime] Не найден Kekelands_Cuckold для Дрейдона."
                );

                return;
            }

            pawn.genes?.SetXenotype(
                xenotype
            );
        }

        private void SetBody(Pawn pawn)
        {
            if (pawn.story != null)
            {
                pawn.story.bodyType =
                    BodyTypeDefOf.Male;
            }
        }

        private void SetTraits(Pawn pawn)
        {
            if (pawn.story?.traits == null)
                return;

            pawn.story.traits.allTraits.Clear();

            // Биоконсерватор.
            AddTrait(
                pawn,
                "BodyPurist",
                0
            );

            // Восхищается пустотой.
            AddTrait(
                pawn,
                "VoidFascination",
                0
            );

            // Лучик счастья.
            AddTrait(
                pawn,
                "Joyous",
                0
            );

            // Непонятый творец.
            AddTrait(
                pawn,
                "TorturedArtist",
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
                    "[AzamPrime] Для Дрейдона не найден TraitDef: "
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
                4,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Melee,
                9,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Construction,
                8,
                Passion.Major
            );

            SetSkill(
                pawn,
                SkillDefOf.Mining,
                9,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Cooking,
                6,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Plants,
                12,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Animals,
                10,
                Passion.None
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
                7,
                Passion.Minor
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
                10,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Intellectual,
                6,
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
            string defName)
        {
            ThingDef def =
                DefDatabase<ThingDef>.GetNamedSilentFail(
                    defName
                );

            if (def == null)
            {
                Log.Warning(
                    "[AzamPrime] Не найдена одежда Дрейдона: "
                    + defName
                );

                return;
            }

            Apparel apparel =
                ThingMaker.MakeThing(
                    def
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
                    "[AzamPrime] Не найдено оружие Дрейдона: "
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
                    "[AzamPrime] Не найден предмет Дрейдона: "
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