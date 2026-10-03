using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace AzamPrime
{
    public class IncidentWorker_DanilkaArrival : IncidentWorker
    {
        protected override bool TryExecuteWorker(IncidentParms parms)
        {
            Map map = parms.target as Map;

            if (map == null)
                return false;

            PawnKindDef pawnKind =
                DefDatabase<PawnKindDef>.GetNamedSilentFail("Danilka");

            if (pawnKind == null)
            {
                Log.Error("[AzamPrime] Не найден PawnKindDef Danilka.");
                return false;
            }

            Pawn pawn =
                PawnGenerator.GeneratePawn(
                    pawnKind,
                    Faction.OfPlayer
                );

            if (pawn == null)
                return false;

            ConfigureDanilka(pawn);

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
                "Данилка прибыл",
                "Данилка появился в вашей колонии.",
                LetterDefOf.PositiveEvent,
                new LookTargets(pawn)
            );

            return true;
        }

        private void ConfigureDanilka(Pawn pawn)
        {
            pawn.Name =
                new NameSingle("Данилка");

            pawn.gender =
                Gender.Male;

            // 18 лет биологического возраста.
            pawn.ageTracker.AgeBiologicalTicks =
                18L * 3600000L;

            // 42 года хронологического возраста.
            pawn.ageTracker.AgeChronologicalTicks =
                42L * 3600000L;

            SetBackstories(pawn);
            SetXenotype(pawn);
            SetTraits(pawn);
            SetSkillsAndPassions(pawn);
            SetAppearance(pawn);

            ClearGeneratedApparel(pawn);

            // Плащ из шкуры пантеры.
            GiveApparel(
                pawn,
                "Apparel_Cape",
                "Leather_Panther"
            );

            // Рубашка из шерсти овцебыка.
            GiveApparel(
                pawn,
                "Apparel_BasicShirt",
                "WoolMuffalo"
            );

            // Штаны из шкуры пантеры.
            GiveApparel(
                pawn,
                "Apparel_Pants",
                "Leather_Panther"
            );

            // Плазменный меч.
            GiveWeapon(
                pawn,
                "MeleeWeapon_PlasmaSword"
            );

            ClearGeneratedInventory(pawn);

            // Манускрипт.
            GiveManuscript(pawn);
        }

        // ==========================================
        // БИОГРАФИИ
        // ==========================================

        private void SetBackstories(Pawn pawn)
        {
            BackstoryDef childhood =
                DefDatabase<BackstoryDef>.GetNamedSilentFail(
                    "Kekelands_DanilkaChildhood"
                );

            BackstoryDef adulthood =
                DefDatabase<BackstoryDef>.GetNamedSilentFail(
                    "Kekelands_DanilkaAdulthood"
                );

            if (childhood != null)
            {
                pawn.story.Childhood =
                    childhood;
            }
            else
            {
                Log.Error(
                    "[AzamPrime] Не найдена детская биография Данилки."
                );
            }

            if (adulthood != null)
            {
                pawn.story.Adulthood =
                    adulthood;
            }
            else
            {
                Log.Error(
                    "[AzamPrime] Не найдена взрослая биография Данилки."
                );
            }
        }

        // ==========================================
        // РАСА
        // ==========================================

        private void SetXenotype(Pawn pawn)
        {
            XenotypeDef xenotype =
                DefDatabase<XenotypeDef>.GetNamedSilentFail(
                    "Kekelands_Cuckold"
                );

            if (xenotype == null)
            {
                Log.Error(
                    "[AzamPrime] Не найден Kekelands_Cuckold для Данилки."
                );

                return;
            }

            pawn.genes?.SetXenotype(
                xenotype
            );
        }

        // ==========================================
        // ТРЕЙТЫ
        // ==========================================

        private void SetTraits(Pawn pawn)
        {
            if (pawn.story?.traits == null)
                return;

            pawn.story.traits.allTraits.Clear();

            // Беспечный стрелок.
            AddTrait(
                pawn,
                "ShootingAccuracy",
                -1
            );

            // Женоненависть.
            AddTrait(
                pawn,
                "DislikesWomen",
                0
            );

            // Нервозность.
            AddTrait(
                pawn,
                "Nerves",
                2
            );

            // Праздность.
            AddTrait(
                pawn,
                "Industriousness",
                3
            );

            // Трезвенник.
            AddTrait(
                pawn,
                "DrugDesire",
                -1
            );

            // Эффективный сон.
            AddTrait(
                pawn,
                "FastSleeper",
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
                    "[AzamPrime] Для Данилки не найден TraitDef: "
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

        // ==========================================
        // НАВЫКИ И СТРАСТИ
        // ==========================================

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
                13,
                Passion.Major
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
                5,
                Passion.Minor
            );

            SetSkill(
                pawn,
                SkillDefOf.Cooking,
                3,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Plants,
                17,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Animals,
                8,
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
                2,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Medicine,
                1,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Social,
                13,
                Passion.None
            );

            SetSkill(
                pawn,
                SkillDefOf.Intellectual,
                15,
                Passion.Major
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

        // ==========================================
        // ВНЕШНОСТЬ
        // ==========================================

        private void SetAppearance(Pawn pawn)
        {
            if (pawn.story == null)
                return;

            pawn.story.bodyType =
                BodyTypeDefOf.Male;

            HairDef hair =
                DefDatabase<HairDef>.GetNamedSilentFail(
                    "Keeper"
                );

            if (hair != null)
            {
                pawn.story.hairDef =
                    hair;
            }
            else
            {
                Log.Warning(
                    "[AzamPrime] Не найдена причёска Keeper для Данилки."
                );
            }
        }

        // ==========================================
        // ОДЕЖДА
        // ==========================================

        private void ClearGeneratedApparel(Pawn pawn)
        {
            pawn.apparel?.DestroyAll();
        }

        private void GiveApparel(
            Pawn pawn,
            string apparelDefName,
            string stuffDefName)
        {
            ThingDef apparelDef =
                DefDatabase<ThingDef>.GetNamedSilentFail(
                    apparelDefName
                );

            if (apparelDef == null)
            {
                Log.Warning(
                    "[AzamPrime] Не найдена одежда Данилки: "
                    + apparelDefName
                );

                return;
            }

            ThingDef stuff =
                DefDatabase<ThingDef>.GetNamedSilentFail(
                    stuffDefName
                );

            if (stuff == null)
            {
                Log.Warning(
                    "[AzamPrime] Не найден материал одежды Данилки: "
                    + stuffDefName
                );

                return;
            }

            Apparel apparel =
                ThingMaker.MakeThing(
                    apparelDef,
                    stuff
                ) as Apparel;

            if (apparel != null)
            {
                pawn.apparel.Wear(
                    apparel
                );
            }
        }

        // ==========================================
        // ОРУЖИЕ
        // ==========================================

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
                    "[AzamPrime] Не найдено оружие Данилки: "
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

        // ==========================================
        // МАНУСКРИПТ
        // ==========================================

        private void GiveManuscript(Pawn pawn)
        {
            ThingDef manuscriptDef =
                DefDatabase<ThingDef>.AllDefs
                    .FirstOrDefault(
                        x => x.label == "манускрипт"
                    );

            if (manuscriptDef == null)
            {
                Log.Warning(
                    "[AzamPrime] Не найден предмет с названием «манускрипт»."
                );

                return;
            }

            Thing manuscript =
                ThingMaker.MakeThing(
                    manuscriptDef
                );

            pawn.inventory.innerContainer.TryAdd(
                manuscript
            );
        }

        // ==========================================
        // ИНВЕНТАРЬ
        // ==========================================

        private void ClearGeneratedInventory(Pawn pawn)
        {
            pawn.inventory?.innerContainer
                .ClearAndDestroyContents();
        }
    }
}