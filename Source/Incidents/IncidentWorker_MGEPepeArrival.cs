using System;
using RimWorld;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_MGEPepeArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;
            if (map == null)
                return false;

            PawnKindDef pawnKind = DefDatabase<PawnKindDef>.GetNamedSilentFail("MGEPepe");
            if (pawnKind == null)
            {
                Log.Error("[AzamPrime] Не найден PawnKindDef МГЕ Пепе.");
                return false;
            }

            Pawn pawn = PawnGenerator.GeneratePawn(pawnKind, Faction.OfPlayer);
            if (pawn == null)
                return false;

            ConfigureMGEPepe(pawn);

            IntVec3 spawnCell;
            if (!CellFinder.TryFindRandomEdgeCellWith(
                c => map.reachability.CanReachColony(c),
                map,
                CellFinder.EdgeRoadChance_Neutral,
                out spawnCell))
            {
                return false;
            }

            GenSpawn.Spawn(pawn, spawnCell, map);

            Find.LetterStack.ReceiveLetter(
                "МГЕ Пепе прибыл",
                "МГЕ Пепе появился в вашей колонии.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }

        private void ConfigureMGEPepe(Pawn pawn)
        {
            pawn.Name = new NameSingle("МГЕ Пепе");
            pawn.gender = Gender.Male;

            pawn.ageTracker.AgeBiologicalTicks = 29L * 3600000L;
            pawn.ageTracker.AgeChronologicalTicks = 29L * 3600000L;

            SetXenotype(pawn);
            SetBodyType(pawn);
            SetHair(pawn);
            SetTraits(pawn);
            SetSkillsAndPassions(pawn);

            ClearGeneratedApparel(pawn);
            GiveApparel(pawn, "Apparel_CowboyHat", null);
            GiveApparel(pawn, "Apparel_Vest", DefDatabase<ThingDef>.GetNamedSilentFail("Hyperweave"));
            GiveApparel(pawn, "Apparel_Duster", null);

            GiveWeapon(pawn, "MeleeWeapon_Longsword", ThingDefOf.Steel);

            ClearGeneratedInventory(pawn);
            GiveInventoryItem(pawn, ThingDefOf.ComponentIndustrial, 4);
            GiveInventoryItem(pawn, DefDatabase<ThingDef>.GetNamedSilentFail("Neutroamine"), 1);
        }

        private void SetXenotype(Pawn pawn)
        {
            XenotypeDef xenotype = DefDatabase<XenotypeDef>.GetNamedSilentFail("Kekelands_Cuckold");
            if (xenotype == null)
            {
                Log.Error("[AzamPrime] Не найден ксенотип Kekelands_Cuckold для МГЕ Пепе.");
                return;
            }

            pawn.genes?.SetXenotype(xenotype);
        }

        private void SetBodyType(Pawn pawn)
        {
            if (pawn.story != null)
                pawn.story.bodyType = BodyTypeDefOf.Hulk;
        }

        private void SetHair(Pawn pawn)
        {
            if (pawn.story == null)
                return;

            HairDef hair = DefDatabase<HairDef>.GetNamedSilentFail("Shaved");
            if (hair != null)
                pawn.story.hairDef = hair;
        }

        private void SetTraits(Pawn pawn)
        {
            if (pawn.story?.traits == null)
                return;

            pawn.story.traits.allTraits.Clear();

            AddTrait(pawn, "Gay", 0);
            AddTrait(pawn, "Beauty", 1);             // красота
            AddTrait(pawn, "FastLearner", 1);        // быстрая обучаемость
            AddTrait(pawn, "GreatMemory", 0);        // хорошая память
            AddTrait(pawn, "Industriousness", 1);    // трудолюбие
            AddTrait(pawn, "Immunity", -1);          // слабый иммунитет
            AddTrait(pawn, "SpeedOffset", -1);       // медлительность
        }

        private void AddTrait(Pawn pawn, string defName, int degree)
        {
            TraitDef traitDef = DefDatabase<TraitDef>.GetNamedSilentFail(defName);
            if (traitDef == null)
            {
                Log.Warning("[AzamPrime] Для МГЕ Пепе не найден TraitDef: " + defName);
                return;
            }

            pawn.story.traits.GainTrait(new Trait(traitDef, degree));
        }

        private void SetSkillsAndPassions(Pawn pawn)
        {
            if (pawn.skills == null)
                return;

            SetSkill(pawn, SkillDefOf.Construction, 11);
            EnsureSkillAtLeast(pawn, SkillDefOf.Plants, 5, 10);
            SetSkill(pawn, SkillDefOf.Social, 8);
            SetSkill(pawn, SkillDefOf.Medicine, 13);
            SetSkill(pawn, SkillDefOf.Melee, 10);
            SetSkill(pawn, SkillDefOf.Shooting, 3);
            SetSkill(pawn, SkillDefOf.Crafting, 8);
            SetSkill(pawn, SkillDefOf.Artistic, 14);

            foreach (SkillRecord skill in pawn.skills.skills)
                skill.passion = Passion.None;

            pawn.skills.GetSkill(SkillDefOf.Construction).passion = Passion.Minor;
            pawn.skills.GetSkill(SkillDefOf.Medicine).passion = Passion.Minor;
            pawn.skills.GetSkill(SkillDefOf.Melee).passion = Passion.Major;
            pawn.skills.GetSkill(SkillDefOf.Artistic).passion = Passion.Major;
        }

        private void SetSkill(Pawn pawn, SkillDef skillDef, int level)
        {
            pawn.skills.GetSkill(skillDef).Level = level;
        }

        private void EnsureSkillAtLeast(Pawn pawn, SkillDef skillDef, int minLevel, int randomMax)
        {
            SkillRecord skill = pawn.skills.GetSkill(skillDef);
            if (skill.Level < minLevel)
                skill.Level = Rand.RangeInclusive(minLevel, randomMax);
        }

        private void ClearGeneratedApparel(Pawn pawn)
        {
            pawn.apparel?.DestroyAll();
        }

        private void GiveApparel(Pawn pawn, string defName, ThingDef stuff)
        {
            ThingDef apparelDef = DefDatabase<ThingDef>.GetNamedSilentFail(defName);
            if (apparelDef == null)
            {
                Log.Warning("[AzamPrime] Не найдена одежда МГЕ Пепе: " + defName);
                return;
            }

            Apparel apparel = ThingMaker.MakeThing(apparelDef, stuff) as Apparel;
            if (apparel != null)
                pawn.apparel.Wear(apparel);
        }

        private void GiveWeapon(Pawn pawn, string defName, ThingDef stuff)
        {
            ThingDef weaponDef = DefDatabase<ThingDef>.GetNamedSilentFail(defName);
            if (weaponDef == null)
            {
                Log.Warning("[AzamPrime] Не найдено оружие МГЕ Пепе: " + defName);
                return;
            }

            if (pawn.equipment?.Primary != null)
                pawn.equipment.DestroyEquipment(pawn.equipment.Primary);

            ThingWithComps weapon = ThingMaker.MakeThing(weaponDef, stuff) as ThingWithComps;
            if (weapon != null)
                pawn.equipment.AddEquipment(weapon);
        }

        private void ClearGeneratedInventory(Pawn pawn)
        {
            pawn.inventory?.innerContainer.ClearAndDestroyContents();
        }

        private void GiveInventoryItem(Pawn pawn, ThingDef def, int count)
        {
            if (pawn.inventory == null || def == null)
                return;

            Thing thing = ThingMaker.MakeThing(def);
            thing.stackCount = count;
            pawn.inventory.innerContainer.TryAdd(thing);
        }
    }
}
