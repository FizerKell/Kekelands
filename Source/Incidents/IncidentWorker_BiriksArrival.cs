using RimWorld;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_BiriksArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;

            if (map == null)
                return false;

            PawnKindDef pawnKind =
                DefDatabase<PawnKindDef>.GetNamedSilentFail(
                    "Biriks"
                );

            if (pawnKind == null)
            {
                Log.Error(
                    "[AzamPrime] Не найден PawnKindDef Biriks."
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

            ConfigureBiriks(pawn);

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
                "Бирикс прибыл",
                "Бирикс появился в вашей колонии.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }

        private void ConfigureBiriks(Pawn pawn)
        {
            pawn.Name =
                new NameSingle("Бирикс");

            pawn.gender =
                Gender.Male;

            pawn.ageTracker.AgeBiologicalTicks =
                42L * 3600000L;

            pawn.ageTracker.AgeChronologicalTicks =
                42L * 3600000L;

            SetBackstories(pawn);
            SetXenotype(pawn);
            SetTraits(pawn);
            SetSkillsAndPassions(pawn);

            ClearGeneratedApparel(pawn);

            // Роскошная мантия.
            GiveApparel(
                pawn,
                "Apparel_RobeRoyal",
                null
            );

            // Рубашка из альтекса.
            GiveApparel(
                pawn,
                "Apparel_BasicShirt",
                DefDatabase<ThingDef>.GetNamed(
                    "Hyperweave"
                )
            );

            // Коронет.
            GiveApparel(
                pawn,
                "Apparel_Coronet",
                ThingDefOf.Gold
            );

            // Пистолет-пулемёт.
            GiveWeapon(
                pawn,
                "Gun_MachinePistol"
            );

            ClearGeneratedInventory(pawn);

            // 10 золота.
            GiveInventoryItem(
                pawn,
                ThingDefOf.Gold,
                10
            );

            // 1 вкусное блюдо.
            GiveInventoryItem(
                pawn,
                ThingDefOf.MealFine,
                1
            );
        }

        private void SetBackstories(Pawn pawn)
        {
            BackstoryDef childhood =
                DefDatabase<BackstoryDef>.GetNamedSilentFail(
                    "Kekelands_BiriksChildhood"
                );

            BackstoryDef adulthood =
                DefDatabase<BackstoryDef>.GetNamedSilentFail(
                    "Kekelands_BiriksAdulthood"
                );

            if (childhood != null)
                pawn.story.Childhood = childhood;
            else
                Log.Error(
                    "[AzamPrime] Не найдена детская биография Бирикса."
                );

            if (adulthood != null)
                pawn.story.Adulthood = adulthood;
            else
                Log.Error(
                    "[AzamPrime] Не найдена взрослая биография Бирикса."
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
                    "[AzamPrime] Не найден Kekelands_Cuckold для Бирикса."
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

            // Усердие.
            AddTrait(
                pawn,
                "Industriousness",
                2
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
                    "[AzamPrime] Для Бирикса не найден TraitDef: "
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
                11,
                Passion.Major
            );

            SetSkill(
                pawn,
                SkillDefOf.Melee,
                3,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Construction,
                4,
                Passion.None
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
                10,
                Passion.Minor
            );

            SetSkill(
                pawn,
                SkillDefOf.Plants,
                4,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Animals,
                3,
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
                8,
                Passion.Minor
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
                10,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Intellectual,
                11,
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
                    "[AzamPrime] Не найдена одежда Бирикса: "
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
                    "[AzamPrime] Не найдено оружие Бирикса: "
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