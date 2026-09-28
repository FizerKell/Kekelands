using RimWorld;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_NecropolitArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;

            if (map == null)
                return false;

            PawnKindDef pawnKind =
                DefDatabase<PawnKindDef>.GetNamedSilentFail(
                    "Necropolit"
                );

            if (pawnKind == null)
            {
                Log.Error(
                    "[AzamPrime] Не найден PawnKindDef Necropolit."
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

            ConfigureNecropolit(pawn);

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
                "Некрополит прибыл",
                "Некрополит появился в вашей колонии.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }

        private void ConfigureNecropolit(Pawn pawn)
        {
            pawn.Name =
                new NameSingle("Некрополит");

            pawn.gender =
                Gender.Male;

            pawn.ageTracker.AgeBiologicalTicks =
                14L * 3600000L;

            pawn.ageTracker.AgeChronologicalTicks =
                14L * 3600000L;

            SetBackstories(pawn);
            SetXenotype(pawn);
            SetTraits(pawn);
            SetSkillsAndPassions(pawn);

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
                "Neutroamine",
                1
            );

            GiveInventoryItem(
                pawn,
                ThingDefOf.ComponentIndustrial,
                1
            );
        }

        // -------------------------
        // БИОГРАФИЯ
        // -------------------------

        private void SetBackstories(Pawn pawn)
        {
            BackstoryDef childhood =
                DefDatabase<BackstoryDef>.GetNamedSilentFail(
                    "Kekelands_NecropolitChildhood"
                );

            BackstoryDef adulthood =
                DefDatabase<BackstoryDef>.GetNamedSilentFail(
                    "Kekelands_NecropolitAdulthood"
                );

            if (childhood != null)
                pawn.story.Childhood = childhood;
            else
                Log.Error(
                    "[AzamPrime] Не найдена детская биография Некрополита."
                );

            if (adulthood != null)
                pawn.story.Adulthood = adulthood;
            else
                Log.Error(
                    "[AzamPrime] Не найдена взрослая биография Некрополита."
                );
        }

        // -------------------------
        // РАСА
        // -------------------------

        private void SetXenotype(Pawn pawn)
        {
            XenotypeDef xenotype =
                DefDatabase<XenotypeDef>.GetNamedSilentFail(
                    "Kekelands_Cuckold"
                );

            if (xenotype == null)
            {
                Log.Error(
                    "[AzamPrime] Не найден Kekelands_Cuckold для Некрополита."
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

            // Асексуальность.
            AddTrait(
                pawn,
                "Asexual",
                0
            );

            // Восхищается пустотой (Anomaly).
            AddTrait(
                pawn,
                "VoidFascination",
                0
            );

            // Кровожадность.
            AddTrait(
                pawn,
                "Bloodlust",
                0
            );

            // Ленность.
            AddTrait(
                pawn,
                "Industriousness",
                -1
            );

            // Оптимист.
            AddTrait(
                pawn,
                "NaturalMood",
                1
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
                    "[AzamPrime] Для Некрополита не найден TraitDef: "
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
            SetSkill(
                pawn,
                SkillDefOf.Shooting,
                1,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Melee,
                1,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Construction,
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
                SkillDefOf.Cooking,
                6,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Plants,
                13,
                Passion.Major
            );

            SetSkill(
                pawn,
                SkillDefOf.Animals,
                5,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Crafting,
                3,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Artistic,
                1,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Medicine,
                2,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Social,
                6,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Intellectual,
                3,
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
            ThingDef apparelDef =
                DefDatabase<ThingDef>.GetNamedSilentFail(
                    defName
                );

            if (apparelDef == null)
            {
                Log.Warning(
                    "[AzamPrime] Не найдена одежда Некрополита: "
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

        // -------------------------
        // ОРУЖИЕ
        // -------------------------

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
                    "[AzamPrime] Не найдено оружие Некрополита: "
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
                    "[AzamPrime] Не найден предмет Некрополита: "
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