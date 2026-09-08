using System.Collections.Generic;
using RimWorld;
using Verse;

namespace AzamPrime
{
    public class GameComponent_InfinTraits : GameComponent
    {
        // 60 тиков/сек на обычной скорости -> примерно одна реальная минута.
        private const int TraitCheckIntervalTicks = 3600;

        // На каждой проверке агрессивный Инфин имеет 2% шанс начать драку.
        private const float AggressionFightChance = 0.02f;

        public GameComponent_InfinTraits(Game game)
        {
        }

        public override void GameComponentTick()
        {
            if (Find.TickManager == null)
                return;

            if (Find.TickManager.TicksGame % TraitCheckIntervalTicks != 0)
                return;

            TraitDef tsundere =
                DefDatabase<TraitDef>.GetNamedSilentFail("Kekelands_Tsundere");

            TraitDef aggressive =
                DefDatabase<TraitDef>.GetNamedSilentFail("Kekelands_Aggressive");

            ThoughtDef tsundereThought =
                DefDatabase<ThoughtDef>.GetNamedSilentFail(
                    "Kekelands_InfinTsundereOpinion"
                );

            foreach (Map map in Find.Maps)
            {
                List<Pawn> pawns = map.mapPawns.AllPawnsSpawned;

                for (int i = 0; i < pawns.Count; i++)
                {
                    Pawn infin = pawns[i];

                    if (infin == null || infin.Dead || infin.story?.traits == null)
                        continue;

                    if (tsundere != null &&
                        infin.story.traits.HasTrait(tsundere))
                    {
                        ApplyTsundereOpinionLoss(
                            infin,
                            pawns,
                            tsundereThought
                        );
                    }

                    if (aggressive != null &&
                        infin.story.traits.HasTrait(aggressive) &&
                        Rand.Chance(AggressionFightChance))
                    {
                        TryStartRandomFight(infin, pawns);
                    }
                }
            }
        }

        private void ApplyTsundereOpinionLoss(
            Pawn infin,
            List<Pawn> pawns,
            ThoughtDef thoughtDef)
        {
            if (thoughtDef == null)
                return;

            for (int i = 0; i < pawns.Count; i++)
            {
                Pawn other = pawns[i];

                if (other == null ||
                    other == infin ||
                    other.Dead ||
                    !other.RaceProps.Humanlike ||
                    other.needs?.mood == null)
                {
                    continue;
                }

                other.needs.mood.thoughts.memories.TryGainMemory(
                    thoughtDef,
                    infin
                );
            }
        }

        private void TryStartRandomFight(
            Pawn infin,
            List<Pawn> pawns)
        {
            if (infin.interactions == null ||
                infin.Dead ||
                infin.Downed ||
                infin.InMentalState)
            {
                return;
            }

            List<Pawn> candidates = new List<Pawn>();

            for (int i = 0; i < pawns.Count; i++)
            {
                Pawn other = pawns[i];

                if (other == null ||
                    other == infin ||
                    other.Dead ||
                    other.Downed ||
                    !other.RaceProps.Humanlike)
                {
                    continue;
                }

                candidates.Add(other);
            }

            if (candidates.Count == 0)
                return;

            Pawn target = candidates.RandomElement();
            infin.interactions.StartSocialFight(target);
        }
    }
}
