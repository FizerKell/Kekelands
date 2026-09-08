using System;
using RimWorld;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_InfinArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;

            if (map == null)
                return false;

            PawnKindDef pawnKind =
                DefDatabase<PawnKindDef>.GetNamedSilentFail("Infin");

            if (pawnKind == null)
            {
                Log.Error("[AzamPrime] Не найден PawnKindDef Инфин.");
                return false;
            }

            Pawn pawn = PawnGenerator.GeneratePawn(
                pawnKind,
                Faction.OfPlayer
            );

            if (pawn == null)
                return false;

            ConfigureInfin(pawn);

            IntVec3 spawnCell;

            if (!CellFinder.TryFindRandomEdgeCellWith(
                c => map.reachability.CanReachColony(c),
                map,
                CellFinder.EdgeRoadChance_Neutral,
                out spawnCell))
            {
                Log.Warning("[AzamPrime] Не удалось найти точку появления Инфина.");
                return false;
            }

            GenSpawn.Spawn(pawn, spawnCell, map);

            Find.LetterStack.ReceiveLetter(
                "Инфин прибыл",
                "Инфин появился в вашей колонии.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }

        private void ConfigureInfin(Pawn pawn)
        {
            pawn.Name = new NameSingle("Инфин");
            pawn.gender = Gender.Male;

            pawn.ageTracker.AgeBiologicalTicks = 18L * 3600000L;
            pawn.ageTracker.AgeChronologicalTicks = 18L * 3600000L;

            SetInfinXenotype(pawn);
            SetInfinAdulthood(pawn);
            SetInfinTraits(pawn);
            SetInfinSkills(pawn);
            SetInfinHair(pawn);
            SetInfinApparel(pawn);
            GiveRevolver(pawn);
            SetInfinInventory(pawn);
        }

        private void SetInfinTraits(Pawn pawn)
        {
            pawn.story.traits.allTraits.Clear();

            AddTrait(pawn, "Kekelands_Tsundere");
            AddTrait(pawn, "Kekelands_Whiner");
            AddTrait(pawn, "Kekelands_Aggressive");

            AddTrait(pawn, "GreatMemory");
            AddTrait(pawn, "DislikesWomen");
            AddTrait(pawn, "PsychicSensitivity", -2);
            AddTrait(pawn, "Immunity", 1);
        }

        private void AddTrait(Pawn pawn, string defName, int degree = 0)
        {
            TraitDef traitDef =
                DefDatabase<TraitDef>.GetNamedSilentFail(defName);

            if (traitDef == null)
            {
                Log.Error("[AzamPrime] Не найден TraitDef: " + defName);
                return;
            }

            pawn.story.traits.GainTrait(new Trait(traitDef, degree));
        }

        private void SetInfinSkills(Pawn pawn)
        {
            foreach (SkillRecord skill in pawn.skills.skills)
            {
                skill.Level = 0;
                skill.passion = Passion.None;
            }

            SetSkill(pawn, SkillDefOf.Artistic, 1);
            SetSkill(pawn, SkillDefOf.Medicine, 8);
            SetSkill(pawn, SkillDefOf.Social, 5, Passion.Major);
            SetSkill(pawn, SkillDefOf.Intellectual, 13, Passion.Major);
            SetSkill(pawn, SkillDefOf.Mining, 1);
            SetSkill(pawn, SkillDefOf.Cooking, 3);
            SetSkill(pawn, SkillDefOf.Melee, 4);
            SetSkill(pawn, SkillDefOf.Shooting, 2);
            SetSkill(pawn, SkillDefOf.Animals, 1);
            SetSkill(pawn, SkillDefOf.Plants, 4);
            SetSkill(pawn, SkillDefOf.Crafting, 3);
        }

        private void SetSkill(
            Pawn pawn,
            SkillDef skillDef,
            int level,
            Passion passion = Passion.None)
        {
            SkillRecord skill = pawn.skills.GetSkill(skillDef);
            skill.Level = level;
            skill.passion = passion;
        }

        private void SetInfinApparel(Pawn pawn)
        {
            ClearGeneratedApparel(pawn);
            GiveWornShirt(pawn);
            GiveLegendaryPants(pawn);
        }

        private void ClearGeneratedApparel(Pawn pawn)
        {
            pawn.apparel?.DestroyAll();
        }

        private void GiveWornShirt(Pawn pawn)
        {
            ThingDef shirtDef =
                DefDatabase<ThingDef>.GetNamedSilentFail("Apparel_BasicShirt");

            if (shirtDef == null)
            {
                Log.Error("[AzamPrime] Не найден Apparel_BasicShirt.");
                return;
            }

            Apparel shirt = ThingMaker.MakeThing(shirtDef) as Apparel;
            if (shirt == null)
                return;

            CompQuality quality = shirt.TryGetComp<CompQuality>();
            quality?.SetQuality(QualityCategory.Poor, ArtGenerationContext.Colony);

            shirt.HitPoints = Math.Max(1, (int)(shirt.MaxHitPoints * 0.25f));
            pawn.apparel.Wear(shirt);
        }

        private void GiveLegendaryPants(Pawn pawn)
        {
            ThingDef pantsDef =
                DefDatabase<ThingDef>.GetNamedSilentFail("Apparel_Pants");

            if (pantsDef == null)
            {
                Log.Error("[AzamPrime] Не найден Apparel_Pants.");
                return;
            }

            Apparel pants = ThingMaker.MakeThing(pantsDef) as Apparel;
            if (pants == null)
                return;

            CompQuality quality = pants.TryGetComp<CompQuality>();
            quality?.SetQuality(QualityCategory.Legendary, ArtGenerationContext.Colony);

            pants.HitPoints = Math.Max(1, (int)(pants.MaxHitPoints * 0.01f));
            pawn.apparel.Wear(pants);
        }

        private void GiveRevolver(Pawn pawn)
        {
            ThingDef revolverDef =
                DefDatabase<ThingDef>.GetNamedSilentFail("Gun_Revolver");

            if (revolverDef == null)
            {
                Log.Error("[AzamPrime] Не найден Gun_Revolver.");
                return;
            }

            if (pawn.equipment.Primary != null)
                pawn.equipment.DestroyEquipment(pawn.equipment.Primary);

            ThingWithComps revolver = ThingMaker.MakeThing(revolverDef) as ThingWithComps;

            if (revolver == null)
            {
                Log.Error("[AzamPrime] Не удалось создать револьвер.");
                return;
            }

            pawn.equipment.AddEquipment(revolver);
        }

        private void SetInfinInventory(Pawn pawn)
        {
            pawn.inventory?.innerContainer.ClearAndDestroyContents();

            GiveInventoryItem(pawn, ThingDefOf.WoodLog, 10);
            GiveInventoryItem(pawn, ThingDefOf.Plasteel, 3);
            GiveInventoryItem(pawn, ThingDefOf.Penoxycyline, 1);
        }

        private void GiveInventoryItem(Pawn pawn, ThingDef def, int count)
        {
            if (pawn.inventory == null || def == null)
                return;

            Thing thing = ThingMaker.MakeThing(def);
            thing.stackCount = count;

            if (!pawn.inventory.innerContainer.TryAdd(thing))
            {
                Log.Warning(
                    "[AzamPrime] Не удалось положить в инвентарь Инфина: " + def.defName
                );
                thing.Destroy();
            }
        }

        private void SetInfinHair(Pawn pawn)
        {
            HairDef hair =
                DefDatabase<HairDef>.GetNamedSilentFail("Shaved");

            if (hair == null)
            {
                Log.Warning("[AzamPrime] Не найдена короткая причёска для Инфина.");
                return;
            }

            pawn.story.hairDef = hair;
        }

        private void SetInfinAdulthood(Pawn pawn)
        {
            BackstoryDef adulthood =
                DefDatabase<BackstoryDef>.GetNamedSilentFail(
                    "Kekelands_InfinAdulthood"
                );

            if (adulthood == null)
            {
                Log.Error(
                    "[AzamPrime] Не найдена взрослая биография Инфина: Kekelands_InfinAdulthood"
                );
                return;
            }

            pawn.story.Adulthood = adulthood;
        }

        private void SetInfinXenotype(Pawn pawn)
        {
            if (pawn.genes == null)
            {
                Log.Error("[AzamPrime] У Инфина отсутствует gene tracker. Нужен Biotech.");
                return;
            }

            XenotypeDef xenotype =
                DefDatabase<XenotypeDef>.GetNamedSilentFail("Kekelands_Old");

            if (xenotype == null)
            {
                Log.Error("[AzamPrime] Не найден ксенотип Инфина: Kekelands_Old");
                return;
            }

            pawn.genes.SetXenotype(xenotype);
        }
    }
}
