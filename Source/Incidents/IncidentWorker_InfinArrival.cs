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

            private void ConfigureInfin(Pawn pawn)
        {
            // Имя
            pawn.Name = new NameSingle("Инфин");

            // Пол
            pawn.gender = Gender.Male;

            // Возраст 18 лет
            pawn.ageTracker.AgeBiologicalTicks = 18L * 3600000L;
            pawn.ageTracker.AgeChronologicalTicks = 18L * 3600000L;

            // Удаляем случайные трейты
            pawn.story.traits.allTraits.Clear();

            AddTrait(pawn, "Tsundere");
            AddTrait(pawn, "Whiner");
            AddTrait(pawn, "Aggressive");

            // Искусство = 1
            SkillRecord artistic =
                pawn.skills.GetSkill(SkillDefOf.Artistic);

            artistic.Level = 1;

            // Страсть к общению
            SkillRecord social =
                pawn.skills.GetSkill(SkillDefOf.Social);

            social.passion = Passion.Major;

            // Страсть к исследованию
            SkillRecord intellectual =
                pawn.skills.GetSkill(SkillDefOf.Intellectual);

            intellectual.passion = Passion.Major;
        }

        private void AddTrait(Pawn pawn, string defName)
        {
            TraitDef traitDef =
                DefDatabase<TraitDef>.GetNamedSilentFail(defName);

            if (traitDef == null)
            {
                Log.Error(
                    "[AzamPrime] Не найден TraitDef: "
                    + defName
                );

                return;
            }

            pawn.story.traits.GainTrait(
                new Trait(traitDef, 0)
            );
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

            // Убираем случайное оружие
            if (pawn.equipment.Primary != null)
            {
                pawn.equipment.DestroyEquipment(pawn.equipment.Primary);
            }

            ThingWithComps revolver =
                ThingMaker.MakeThing(revolverDef) as ThingWithComps;

            if (revolver == null)
            {
                Log.Error("[AzamPrime] Не удалось создать револьвер.");
                return;
            }

            pawn.equipment.AddEquipment(revolver);
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

            Apparel pants =
                ThingMaker.MakeThing(pantsDef) as Apparel;

            if (pants == null)
                return;

            CompQuality quality =
                pants.TryGetComp<CompQuality>();

            if (quality != null)
            {
                quality.SetQuality(
                    QualityCategory.Legendary,
                    ArtGenerationContext.Colony
                );
            }

            pants.HitPoints =
                System.Math.Max(
                    1,
                    (int)(pants.MaxHitPoints * 1f)
                );

            pawn.apparel.Wear(pants);
        }

        private void GiveWornShirt(Pawn pawn)
        {
            ThingDef shirtDef =
                DefDatabase<ThingDef>.GetNamedSilentFail("Apparel_BasicShirt");

            if (shirtDef == null)
                return;

            Apparel shirt =
                ThingMaker.MakeThing(shirtDef) as Apparel;

            if (shirt == null)
                return;

            CompQuality quality =
                shirt.TryGetComp<CompQuality>();

            if (quality != null)
            {
                quality.SetQuality(
                    QualityCategory.Poor,
                    ArtGenerationContext.Colony
                );
            }

            shirt.HitPoints =
                System.Math.Max(
                    1,
                    (int)(shirt.MaxHitPoints * 0.25f)
                );

            pawn.apparel.Wear(shirt);
        }

        private void ClearGeneratedApparel(Pawn pawn)
        {
            if (pawn.apparel == null)
                return;

            pawn.apparel.DestroyAll();
        }

        ClearGeneratedApparel(pawn);

        GiveWornShirt(pawn);
        GiveLegendaryPants(pawn);
        GiveRevolver(pawn);

        private void SetInfinHair(Pawn pawn)
        {
            HairDef hair =
                DefDatabase<HairDef>.GetNamedSilentFail("Shaved");

            if (hair == null)
            {
                Log.Warning("[AzamPrime] Не найдена причёска для Инфина.");
                return;
            }

            pawn.story.HairDef = hair;
        }

        SetInfinHair(pawn);

        IntVec3 spawnCell;

            if (!CellFinder.TryFindRandomEdgeCellWith(
                c => map.reachability.CanReachColony(c),
                map,
                CellFinder.EdgeRoadChance_Neutral,
                out spawnCell))
            {
                Log.Warning(
                    "[AzamPrime] Не удалось найти точку появления Инфина."
                );

                return false;
            }

            GenSpawn.Spawn(
                pawn,
                spawnCell,
                map
            );

            Find.LetterStack.ReceiveLetter(
                "Инфин прибыл",
                "Инфин появился в вашей колонии.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }
    }
}