module app.match {
    'use strict';

    class MainController {

        public CurrentDate: Date = new Date();
        public PastOneDaysDate: Date = new Date();
        public PastTwoDaysDate: Date = new Date();
        public FutureOneDaysDate: Date = new Date();
        public FutureTwoDaysDate: Date = new Date();
        public Leagues: string[];

        public CurrentDay: number;
        public PastOneDay: number;
        public PastTwoDay: number;
        public FutureOneDay: number;
        public FutureTwoDay: number;


        static $inject = ['$state'];
        constructor(public $state) {
            this.updateDisplayDate(this.CurrentDate);
        }

        private updateDisplayDate(currentDate: Date): void {
            this.CurrentDate = currentDate;

            currentDate = new Date(currentDate.toString());
            this.PastOneDaysDate.setDate(currentDate.getDate() - 1);
            this.PastTwoDaysDate.setDate(currentDate.getDate() - 2);
            this.FutureOneDaysDate.setDate(currentDate.getDate() + 1);
            this.FutureTwoDaysDate.setDate(currentDate.getDate() + 2);

            this.CurrentDay = currentDate.getDate();
            this.PastOneDay = this.PastOneDaysDate.getDate();
            this.PastTwoDay = this.PastTwoDaysDate.getDate();
            this.FutureOneDay = this.FutureOneDaysDate.getDate();
            this.FutureTwoDay = this.FutureTwoDaysDate.getDate();

            console.log('# Update date completed.');
        }

        public SelectDay(selectedDate: Date): void {
            this.$state.go('app.main.matches', {
                day: selectedDate.getDate()
            });
        }
    }

    class PredictionController {

        public Leagues: string[];
        private userProfile: app.shared.UserProfile;
        public PredictionRemainingCount: number;

        static $inject = [
            'matches',
            'app.shared.UserProfileService',
            'predictions',
            'couponSummary',
            'app.match.PredictionsService',
            '$ionicModal',
            '$scope'];
        constructor(
            public leagues: app.match.LeagueInformation[],
            public userSvc: app.shared.UserProfileService,
            public predictions: app.match.PredictionInformation[],
            public couponSummary,
            private predictSvc: app.match.PredictionsService,
            private $ionicModal,
            private $scope) {

            this.userProfile = this.userSvc.GetUserProfile();
            this.$ionicModal.fromTemplateUrl('templates/MatchesPopup.html',
                {
                    scope: $scope,
                    animation: 'slide-ins-up'
                }).then(modal=> { this.$scope.MatchPopup = modal; });
            this.updatePredictionRemainning();
        }

        private updatePredictionRemainning() {
            const MaximunPredictionCount = 5;
            if (this.predictions == null) this.PredictionRemainingCount = MaximunPredictionCount;
            this.PredictionRemainingCount = MaximunPredictionCount - this.predictions.length;
        }

        private reloadPredictions(userId: string, matchId: string, selectedTeamId: string, isCancel: boolean): void {
            var now = new Date();
            this.predictSvc.Predict(userId, matchId, selectedTeamId, isCancel)
                .then((respond: app.match.PredictionInformation[]): void => {
                    this.predictions = respond;
                    this.updatePredictionRemainning();
                });
        }

        //#region Predictions
        public predictTeamHome(match: app.match.MatchInformation): void {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            var isCancel: boolean = false;
            if (selectedPrediction != null) isCancel = selectedPrediction.IsPredictionTeamHome;

            this.reloadPredictions(this.userProfile.UserId, match.id, match.TeamHomeId, isCancel);
            
        }

        public predictTeamAway(match: app.match.MatchInformation): void {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            var isCancel: boolean = false;
            if (selectedPrediction != null) isCancel = selectedPrediction.IsPredictionTeamAway;

            this.reloadPredictions(this.userProfile.UserId, match.id, match.TeamAwayId, isCancel);
        }

        public predictDraw(match: app.match.MatchInformation): void {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            var isCancel: boolean = false;
            if (selectedPrediction != null) isCancel = selectedPrediction.IsPredictionDraw;

            this.reloadPredictions(this.userProfile.UserId, match.id, null, isCancel);
        }
        //#endregion Predictions

        //#region Gamestatus
        public IsGameStarted(match: app.match.MatchInformation): boolean {
            if (match.StartedDate) return true;
        }

        public IsSelectedHome(match: app.match.MatchInformation): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            if (match.CompletedDate) return false;
            return selectedPrediction.IsPredictionTeamHome;
        }

        public IsUnSelectedHome(match: app.match.MatchInformation): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return true;
            return !selectedPrediction.IsPredictionTeamHome;
        }

        public IsPredictHomeWin(match: app.match.MatchInformation): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            if (!match.CompletedDate) return false;
            return match.IsTeamHomeWin && selectedPrediction.IsPredictionTeamHome;
        }

        public IsPredictHomeLose(match: app.match.MatchInformation): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            if (!match.CompletedDate) return false;
            return !match.IsTeamHomeWin && selectedPrediction.IsPredictionTeamHome;
        }

        public IsSelectedAway(match: app.match.MatchInformation): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            if (match.CompletedDate) return false;
            return selectedPrediction.IsPredictionTeamAway;
        }

        public IsUnSelectedAway(match: app.match.MatchInformation): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return true;
            return !selectedPrediction.IsPredictionTeamAway;
        }

        public IsPredictAwayWin(match: app.match.MatchInformation): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            if (!match.CompletedDate) return false;
            return match.IsTeamAwayWin && selectedPrediction.IsPredictionTeamAway;
        }

        public IsPredictAwayLose(match: app.match.MatchInformation): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            if (!match.CompletedDate) return false;
            return !match.IsTeamAwayWin && selectedPrediction.IsPredictionTeamAway;
        }

        public IsSelectedDraw(match: app.match.MatchInformation): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            if (match.CompletedDate) return false;
            return selectedPrediction.IsPredictionDraw;
        }

        public IsUnSelectedDraw(match: app.match.MatchInformation): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return true;
            return !selectedPrediction.IsPredictionDraw;
        }

        public IsPredictDrawWin(match: app.match.MatchInformation): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            if (!match.CompletedDate) return false;
            return match.IsGameDraw && selectedPrediction.IsPredictionDraw;
        }

        public IsPredictDrawLose(match: app.match.MatchInformation): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            if (!match.CompletedDate) return false;
            return !match.IsGameDraw && selectedPrediction.IsPredictionDraw;
        }

        public IsShowHomePredictionPoint(match: app.match.MatchInformation): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            return selectedPrediction.IsPredictionTeamHome;
        }

        public IsShowAwayPredictionPoint(match: app.match.MatchInformation): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            return selectedPrediction.IsPredictionTeamAway;
        }

        public IsShowDrawPredictionPoint(match: app.match.MatchInformation): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            return selectedPrediction.IsPredictionDraw;
        }

        public GetPredictionPoint(match: app.match.MatchInformation): number {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return 0;
            return selectedPrediction.PredictionPoints;
        }
        //#endregion Gamestatus
    }

    class DaylyHistoryController {

        public shownGroup;

        static $inject = ['data'];
        constructor(public data: app.match.PredictionDailySummary[]) {
        }

        public filterDay(day: Date): string {
            var value = day.toString();
            value = value.substring(0, value.indexOf('T'));
            return value;
        }

        public toggleGroup(group): void {
            if (this.isGroupShown(group)) {
                this.shownGroup = null;
            } else {
                this.shownGroup = group;
            }
        };

        public isGroupShown(group): boolean {
            return this.shownGroup == group;
        };
    }

    class MonthlyHistoryController {

        static $inject = ['data'];
        constructor(public data: app.match.PredictionMonthlySummary[]) {
        }
    }

	angular
        .module('app.match')
        .controller('app.match.MainController', MainController)
        .controller('app.match.DaylyHistoryController', DaylyHistoryController)
        .controller('app.match.MonthlyHistoryController', MonthlyHistoryController)
        .controller('app.match.PredictionController', PredictionController);

}