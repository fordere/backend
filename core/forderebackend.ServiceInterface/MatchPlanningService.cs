using forderebackend.ServiceInterface.Entities;
using ServiceStack;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Linq;

namespace forderebackend.ServiceInterface
{
    public class Confrontation
    {
        // Cup ~= 2020, Garlando Ligue 1, Group 1 ?
        public Cup Cup;
        public Team Team1;
        public Team Team2;
    }

    public class Slot
    {
        public Table Table;
        public DateTime PlayDate;
    }

    public class MatchProposition
    {
        public Confrontation Confrontation;
        public Slot Slot;
        // FIXME: public PropositionStatus Status; // Duplicate from MatchPropositionConsent.Status ?
    }

    public class MatchPropositionConsent
    {
        public enum ConsentStatus
        {
            Pending,
            Approved,
            Declined,
        }

        // User to ask for consent.
        public User User;
        // Prerequisities before asking this consent.
        public List<MatchPropositionConsent> Dependencies;
        // Proposition to approve.
        public MatchProposition Proposition;
        // Status.
        public ConsentStatus Status;
    }

    interface IConfrontationProvider
    {
        // Return all remaining confrontations for a cup (for example, used by system to plan matches).
        List<Confrontation> GetConfrontations(Cup cup);

        // Return all remaining confrontations for a cup and a specific team (for example, when a team want to play a match).
        List<Confrontation> GetConfrontations(Cup cup, Team team);
    }

    interface ISlotProvider
    {
        // Return all remaining slots according Cup TableType.
        List<Slot> GetSlots(Cup cup, DateTime from, DateTime to);

        // Return all remaining slots according Cup TableType and a specific bar.
        List<Slot> GetSlots(Cup cup, DateTime from, DateTime to, Bar bar);
    }

    interface IMatchPropositionProvider
    {
        // Return a list of MatchProposition with distinct confrontations, in relevance order.
        // Note: Implementation could favor solution based on Team.WhishPlayDay, Team.BarId, planned Matches...
        // Important: Implementation must take care to not propose impossible situation (team play on 2 tables at same time, ...).
        List<MatchProposition> GetPropositions(
            // List of all confrontations to consider, in importance order.
            // Examples:
            // - If Team1 want to plan match(es): listOf(Team1 vs Team2, Team1 vs Team3, ..., Team2 vs Team1, ...).
            // - If Team1 want to plan match(es) against Team2: listOf(Team1 vs Team2, Team2 vs Team1).
            // - If System want to plan match(es): listOf(Team1 vs Team2, Team3 vs Team4, Team5 vs Team6, ...).
            // - If Team1 have less matches than all other teams, add some Team1 confrontations at the begining.
            // - If Team1 declined a confrontation vs Team2 but still need to play this match at some point, add (Team1 vs Team2) at the end.
            // Notes:
            // - Usual number of confrontations for N teams: N * (N-1) / 2.
            List<Confrontation> confrontations,
            // List of all slots to consider, in importance order.
            // Examples:
            // - Add slots in upcoming order to force filling those slots before.
            // - Add only the next 2 weeks available slots to avoid planning the full season.
            List<Slot> slots
        );
    }  // FIXME: Add a sorting layer on top of IMatchPropositionProvider, with Decorator Pattern?

    interface IMatchPropositionManager
    {
        // Select one or more Propositions to keep/consider/persist.
        List<MatchProposition> Select(List<MatchProposition> propositions);

        // Implementation note:

        // This step is crutial and will depend and impact other modules!!
        // For now, we could simply take only the first proposition.
    }

    // Build the mandatory MatchConsent for a proposition, according a specific use case (initiator).
    interface IMatchPropositionConsentProvider
    {
        // Return all needed consents for propositions (initiator is the system).
        List<MatchPropositionConsent> GetConsents(List<MatchProposition> propostitions);

        // Return all needed consents for propositions (initiator is a user).
        List<MatchPropositionConsent> GetConsents(List<MatchProposition> propostitions, int initiatorId);
    }

    // Resolve/Transform/dispatch propositions consents to user(s), usually through a notification mechanism.
    interface IMatchPropositionConsentResolver
    {
        // Dispatch Consents according dependencies through NotificationManager.
        void Dispatch(List<MatchPropositionConsent> consents);
    }

    class Notification
    {
        public User User;
        public String Title;
        public String Body;
    }

    class PushNotification: Notification
    {
        public int CollapseKey;
        public String Type;
        public String JsonData;
    }

    // Base service for notifications (email or/and push).
    interface INotificationManager {
        void Dispatch(List<Notification> notifications);
    }

    // Base service for updating Consents.
    interface IMatchPropositionConsentService
    {
        // If approved -> Send remaining Consent, if needed, or validate MatchProposition.
        // If declined -> Invalidate MatchProposition.
        void Update(MatchPropositionConsent constent);

        // Implementation note:

        // To send remaining consents (triggered with a MatchPropositionConsent.Status is updated to Approved),
        // it could be better to have another manager/service that observe MatchPropositionConsent DB table changes
        // instead of putting this specific logic here.

        // Validating a MatchProposition will Create/Enter a Match Appointment (MatchEvent?).
        // Updating a Match Appointment will trigger a notification to the players ("You have a match on ... against ... by ...").
        // Invalidating a MatchProposition will trigger a notification to the players that previously gave a consent ("Unfortunately the proposition ... has been declined by ....").
        // Invalidating a MatchProposition could also trigger another available MatchProposition to be asked for consent, ...
    }

    // Base service for requesting MatchProposition.
    public class MatchPlanningService : BaseService
    {
        // Those implementations should be injected ?
        IConfrontationProvider confrontationProvider;
        ISlotProvider slotProvider;
        IMatchPropositionProvider propositionProvider;
        IMatchPropositionManager propositionManager;
        IMatchPropositionConsentProvider consentProvider;
        IMatchPropositionConsentResolver consentManager;

        // Example when current user want to play a match, without specifying anything else.
        public bool RequestMatch(Cup cup)
        {
            // Look for the next 7 days only.
            return RequestMatch(SessionUserId, cup, DateTime.Now, DateTime.Now.AddDays(7));
        }

        public bool RequestMatch(int userId, Cup cup, DateTime from, DateTime to)
        {
            // Team GetTeam(int userId)
            var initiatorTeam = Db.Select(
                Db.From<Team>().Where(team => team.Player1Id == userId || team.Player2Id == userId
            )).First();

            // 1) Get current remaining confrontations & slots.
            var confrontations = confrontationProvider.GetConfrontations(cup, initiatorTeam);
            var slots = slotProvider.GetSlots(cup, from, to);

            // 2) Get propositions from the current proposition provider implementation.
            var propositions = propositionProvider.GetPropositions(confrontations, slots);

            // 3) Filter/select one or more propositions.
            propositions = propositionManager.Select(propositions);

            // 4) Get Consents for one or more propositions.
            var neededConsents = consentProvider.GetConsents(propositions, userId);

            // 5) Dispatch Consents.
            consentManager.Dispatch(neededConsents);

            // Then, MatchPropositionConsentService will update MatchProposition|Consent and then continue/finish the planning process.
            return true;
        }

        /* Example:

        Teams    : T1, T2, T3, T4.
        Players  : P11, P12, P21, P22, P31, P32, P41, P42.
        Bars     : B1 (T11,T12,T13), B2 (T21). 
        Slots    : 19:00 -> 20:30, defined as: S1, S2, S3 (only 3 slots per table per day).
        Planned  : 2vs3->Mon-T11-S1.

        P11 (T1) want to play in next 2 days (first game in season for T1):
        1) Confrontations -> 1vs2, 1vs3, 1vs4.
           Slots          ->            Mon-T11-S2,Mon-T11-S3,
                             Mon-T12-S1,Mon-T12-S2,Mon-T12-S3,
                             Mon-T13-S1,Mon-T13-S2,Mon-T13-S3,
                             Mon-T21-S1,Mon-T21-S2,Mon-T21-S3,

                             Tue-T11-S1,Tue-T11-S2,Tue-T11-S3,
                             Tue-T12-S1,Tue-T12-S2,Tue-T12-S3,
                             Tue-T13-S1,Tue-T13-S2,Tue-T13-S3,
                             Tue-T21-S1,Tue-T21-S2,Tue-T21-S3.
                             
        2) Propositions   -> 1vs2->Mon-T11-S2,
                             1vs3->Mon-T11-S3,
                             1vs4->Mon-T12-S1.

           Notes:
           - ConsentProvider will first only consider the first proposition.
             Future implementation may want to ask consent for more than 1 propositions as same time.

        3) Consents       -> C1:P11->1vs2->Mon-T11-S2 (already Approved as initiator is P11),
                             C2:P12->1vs2->Mon-T11-S2 (first step, depends on C1),
                             C3:P21->1vs2->Mon-T11-S2 (depends on C2),
                             C4:P22->1vs2->Mon-T11-S2 (depends on C3).

        4) Dispatch Consent -> 3 Notifications will be sent:
                            -> C2:N2 (directly sent)
                               C3:N3 (wait on C2 status)
                               C4:N4 (wait on C3 status)          
        */
    }
}
