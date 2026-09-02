using RimWorld;
using Verse;
using System.Collections.Generic;

namespace AzamPrime
{
    public enum AzamPhase
    {
        Observation,
        Trial,
        Mercy,
        Judgment
    }

    public class StorytellerCompProperties_AzamPhases : StorytellerCompProperties
    {
        public StorytellerCompProperties_AzamPhases()
        {
            compClass = typeof(StorytellerComp_AzamPhases);
        }
    }

    public class StorytellerComp_AzamPhases : StorytellerComp
    {
        private AzamPhase currentPhase = AzamPhase.Observation;
        private int nextPhaseEventTick = -1;
        private int phaseStartTick = -1;

        private const int TicksPerDay = 60000;

        public override IEnumerable<FiringIncident> MakeIntervalIncidents(
            IIncidentTarget target)
        {
            if (phaseStartTick < 0)
            {
                phaseStartTick = Find.TickManager.TicksGame;

                Log.Message(
                    "[AzamPrime] Система фаз запущена. Начальная фаза: "
                    + currentPhase
                );
            }

            UpdatePhase(target);
            UpdatePhaseEvents(target);

            yield break;
        }

        private void UpdatePhase(IIncidentTarget target)
        {
            int currentTick = Find.TickManager.TicksGame;

            int ticksPassed =
                currentTick - phaseStartTick;

            float daysPassed =
                ticksPassed / (float)TicksPerDay;

            float phaseDuration =
                GetCurrentPhaseDuration();

            if (daysPassed >= phaseDuration)
            {
                NextPhase();

                phaseStartTick = currentTick;
                ScheduleNextPhaseEvent();

                Log.Message(
                    "[AzamPrime] ПЕРЕКЛЮЧЕНИЕ ФАЗЫ -> "
                    + currentPhase
                );

                SendPhaseMessage();

                if (currentPhase == AzamPhase.Judgment)
                {
                    TriggerAzamTrial(target);
                }
            }
        }

        private float GetCurrentPhaseDuration()
        {
            // Длительность фаз в игровых днях. 8 5 4 3 1
            switch (currentPhase)
            {
                case AzamPhase.Observation:
                    return 8f;

                case AzamPhase.Trial:
                    return 5f;

                case AzamPhase.Mercy:
                    return 4f;

                case AzamPhase.Judgment:
                    return 3f;

                default:
                    return 1f;
            }
        }

        private void NextPhase()
        {
            switch (currentPhase)
            {
                case AzamPhase.Observation:
                    currentPhase = AzamPhase.Trial;
                    break;

                case AzamPhase.Trial:
                    currentPhase = AzamPhase.Mercy;
                    break;

                case AzamPhase.Mercy:
                    currentPhase = AzamPhase.Judgment;
                    break;

                case AzamPhase.Judgment:
                    currentPhase = AzamPhase.Observation;
                    break;
            }
        }

        private void TriggerAzamTrial(IIncidentTarget target)
        {
            IncidentDef azamTrial =
                DefDatabase<IncidentDef>.GetNamedSilentFail("AzamTrial");

            if (azamTrial == null)
            {
                Log.Error(
                    "[AzamPrime] Не найден IncidentDef AzamTrial."
                );

                return;
            }

            IncidentParms parms =
                StorytellerUtility.DefaultParmsNow(
                    IncidentCategoryDefOf.Misc,
                    target
                );

            bool success =
                azamTrial.Worker.TryExecute(parms);

            if (success)
            {
                Log.Message(
                    "[AzamPrime] Испытание Азама успешно запущено."
                );
            }
            else
            {
                Log.Warning(
                    "[AzamPrime] AzamTrial не удалось запустить."
                );
            }
        }

        private void SendPhaseMessage()
        {
            string message;

            switch (currentPhase)
            {
                case AzamPhase.Observation:
                    message = "Азам наблюдает.";
                    break;

                case AzamPhase.Trial:
                    message = "Испытание Азама начинается.";
                    break;

                case AzamPhase.Mercy:
                    message = "Азам проявляет милость.";
                    break;

                case AzamPhase.Judgment:
                    message = "Наступает суд Азама.";
                    break;

                default:
                    message = "Фаза Азама изменилась.";
                    break;
            }

            Messages.Message(
                message,
                MessageTypeDefOf.NeutralEvent
            );
        }

        private void UpdatePhaseEvents(IIncidentTarget target)
        {
            int currentTick = Find.TickManager.TicksGame;

            if (nextPhaseEventTick < 0)
            {
                ScheduleNextPhaseEvent();
                return;
            }

            if (currentTick < nextPhaseEventTick)
                return;

            TriggerPhaseEvent(target);

            ScheduleNextPhaseEvent();
        }

        private void TriggerPhaseEvent(IIncidentTarget target)
        {
            switch (currentPhase)
            {
                case AzamPhase.Observation:
                    TriggerObservationEvent(target);
                    break;

                case AzamPhase.Trial:
                    TriggerTrialEvent(target);
                    break;

                case AzamPhase.Mercy:
                    TriggerMercyEvent(target);
                    break;

                case AzamPhase.Judgment:
                    TriggerJudgmentEvent(target);
                    break;
            }
        }

        private void TriggerObservationEvent(IIncidentTarget target)
        {
            float roll = Rand.Value;

            if (roll < 0.40f)
            {
                TryTriggerIncident("VisitorGroup", target);
            }
            else if (roll < 0.70f)
            {
                TryTriggerIncident("TravelerGroup", target);
            }
            else
            {
                TryTriggerIncident("TraderCaravanArrival", target);
            }
        }

        private void TriggerTrialEvent(IIncidentTarget target)
        {
            float roll = Rand.Value;

            if (roll < 0.55f)
            {
                TryTriggerIncident("RaidEnemy", target);
            }
            else if (roll < 0.85f)
            {
                TryTriggerIncident("ManhunterPack", target);
            }
            else
            {
                TriggerAzamTrial(target);
            }
        }

        private void TriggerMercyEvent(IIncidentTarget target)
        {
            float roll = Rand.Value;

            if (roll < 0.55f)
            {
                TryTriggerIncident("TraderCaravanArrival", target);
            }
            else
            {
                TryTriggerIncident("VisitorGroup", target);
            }
        }

        private void TriggerJudgmentEvent(IIncidentTarget target)
        {
            float roll = Rand.Value;

            if (roll < 0.50f)
            {
                TriggerAzamTrial(target);
            }
            else
            {
                TryTriggerIncident("RaidEnemy", target);
            }
        }

        private void ScheduleNextPhaseEvent()
        {
            float days;

            switch (currentPhase)
            {
                case AzamPhase.Observation:
                    days = Rand.Range(2.5f, 4.0f);
                    break;

                case AzamPhase.Trial:
                    days = Rand.Range(1.5f, 2.5f);
                    break;

                case AzamPhase.Mercy:
                    days = Rand.Range(1.5f, 2.5f);
                    break;

                case AzamPhase.Judgment:
                    days = Rand.Range(0.8f, 1.4f);
                    break;

                default:
                    days = 3f;
                    break;
            }

            nextPhaseEventTick =
                Find.TickManager.TicksGame
                + (int)(days * TicksPerDay);
        }

        private bool TryTriggerIncident(
    string defName,
    IIncidentTarget target)
        {
            IncidentDef incident =
                DefDatabase<IncidentDef>.GetNamedSilentFail(defName);

            if (incident == null)
            {
                Log.Warning(
                    "[AzamPrime] Не найден IncidentDef: "
                    + defName
                );

                return false;
            }

            IncidentParms parms =
                StorytellerUtility.DefaultParmsNow(
                    incident.category,
                    target
                );

            bool success =
                incident.Worker.TryExecute(parms);

            if (success)
            {
                Log.Message(
                    "[AzamPrime] Фаза "
                    + currentPhase
                    + " запустила "
                    + defName
                );
            }
            else
            {
                Log.Message(
                    "[AzamPrime] Не удалось запустить "
                    + defName
                );
            }

            return success;
        }
    }

}