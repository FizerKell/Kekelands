using System;
using RimWorld;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_LegionerArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;
            if (map == null)
                return false;

            PawnKindDef pawnKind =
                DefDatabase<PawnKindDef>.GetNamedSilentFail("Legioner");

            if (pawnKind == null)
            {
                Log.Error("[AzamPrime] Не найден PawnKindDef Легионера.");
                return false;
            }

            Pawn pawn = PawnGenerator.GeneratePawn(pawnKind, Faction.OfPlayer);
            if (pawn == null)
                return false;

            ConfigureLegioner(pawn);

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
                "Легионер прибыл",
                "Легионер появился в вашей колонии.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }

        private void ConfigureLegioner(Pawn pawn)
        {
            pawn.Name = new NameSingle("Легионер");
            pawn.gender = Gender.Male;

            pawn.ageTracker.AgeBiologicalTicks = 19L * 3600000L;
            pawn.ageTracker.AgeChronologicalTicks = 19L * 3600000L;

            SetXenotype(pawn);
            SetBodyType(pawn);
            SetTraits(pawn);
            SetSkillsAndPassions(pawn);

            ClearGeneratedApparel(pawn);
            GiveApparel(pawn, "Apparel_FlakJacket", null);
            GiveApparel(pawn, "Apparel_FlakPants", null);
            GiveApparel(pawn, "Apparel_SimpleHelmet", ThingDefOf.Plasteel);

            GiveWeapon(pawn, "Gun_SniperRifle");

            ClearGeneratedInventory(pawn);
            GiveInventoryItem(pawn, ThingDefOf.GoJuice, 1);
            GiveInventoryItem(pawn, ThingDefOf.MealFine, 3);
        }

        private void SetXenotype(Pawn pawn)
        {
            XenotypeDef xenotype =
                DefDatabase<XenotypeDef>.GetNamedSilentFail("Kekelands_Old");

            if (xenotype == null)
            {
                Log.Error("[AzamPrime] Не найден ксенотип Kekelands_Old для Легионера.");
                return;
            }

            pawn.genes?.SetXenotype(xenotype);
        }

        private void SetBodyType(Pawn pawn)
        {
            if (pawn.story != null)
                pawn.story.bodyType = BodyTypeDefOf.Hulk;
        }

        private void SetTraits(Pawn pawn)
        {
            if (pawn.story?.traits == null)
                return;

            pawn.story.traits.allTraits.Clear();

            // Беспечный стрелок / Trigger-happy.
            AddTrait(pawn, "ShootingAccuracy", -1);

            // Оптимизм.
            AddTrait(pawn, "NaturalMood", 1);

            // Эффективный сон.
            AddTrait(pawn, "FastSleeper", 0);

            // Медленная обучаемость.
            AddTrait(pawn, "FastLearner", -1);

            // Психическая устойчивость.
            AddTrait(pawn, "PsychicSensitivity", -1);

            // Нервозность.
            AddTrait(pawn, "Neurotic", 1);

            // Ванильный снобизм (Greedy в defName).
            AddTrait(pawn, "Greedy", 0);

            // Ванильный непонятый творец.
            AddTrait(pawn, "TorturedArtist", 0);
        }

        private void AddTrait(Pawn pawn, string defName, int degree)
        {
            TraitDef traitDef = DefDatabase<TraitDef>.GetNamedSilentFail(defName);

            if (traitDef == null)
            {
                Log.Warning("[AzamPrime] Для Легионера не найден TraitDef: " + defName);
                return;
            }

            pawn.story.traits.GainTrait(new Trait(traitDef, degree));
        }

        private void SetSkillsAndPassions(Pawn pawn)
        {
            if (pawn.skills == null)
                return;

            // Неуказанные навыки оставляем такими, какими их сгенерировал PawnGenerator.
            SkillRecord intellectual = pawn.skills.GetSkill(SkillDefOf.Intellectual);
            SkillRecord shooting = pawn.skills.GetSkill(SkillDefOf.Shooting);
            SkillRecord social = pawn.skills.GetSkill(SkillDefOf.Social);
            SkillRecord cooking = pawn.skills.GetSkill(SkillDefOf.Cooking);

            // Условия пользователя: Intellectual > 10, Shooting > 6, Social ровно 10.
            intellectual.Level = Math.Max(intellectual.Level, Rand.RangeInclusive(11, 15));
            shooting.Level = Math.Max(shooting.Level, Rand.RangeInclusive(7, 12));
            social.Level = 10;

            // Страсти задаём только указанным навыкам.
            foreach (SkillRecord skill in pawn.skills.skills)
                skill.passion = Passion.None;

            shooting.passion = Passion.Major;
            intellectual.passion = Passion.Minor;
            cooking.passion = Passion.Minor;
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
                Log.Warning("[AzamPrime] Не найдена одежда Легионера: " + defName);
                return;
            }

            Apparel apparel = ThingMaker.MakeThing(apparelDef, stuff) as Apparel;
            if (apparel == null)
                return;

            pawn.apparel.Wear(apparel);
        }

        private void GiveWeapon(Pawn pawn, string defName)
        {
            ThingDef weaponDef = DefDatabase<ThingDef>.GetNamedSilentFail(defName);
            if (weaponDef == null)
            {
                Log.Warning("[AzamPrime] Не найдено оружие Легионера: " + defName);
                return;
            }

            if (pawn.equipment?.Primary != null)
                pawn.equipment.DestroyEquipment(pawn.equipment.Primary);

            ThingWithComps weapon = ThingMaker.MakeThing(weaponDef) as ThingWithComps;
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
