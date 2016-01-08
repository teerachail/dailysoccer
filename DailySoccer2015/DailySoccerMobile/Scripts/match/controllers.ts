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
        }

        private refrestPredictions(): void {
            var now = new Date();
            this.predictSvc.GetPredictionsByDate(this.userProfile.UserId, now.getDate())
                .then((respond: PredictionInformation[]): void => {
                    this.predictions = respond;
                });
        }

        public predictTeamHome(match: app.match.MatchInformation): void {
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[0];
            var isCancel: boolean = false;
            if (selectedPrediction != null) isCancel = selectedPrediction.IsPredictionTeamHome;

            this.predictSvc.Predict(this.userProfile.UserId, match.id, match.TeamHomeId, isCancel)
                .then((respond: app.match.PredictionInformation[]): void => {
                    this.predictions = respond;
            });
            
        }

        public predictTeamAway(match: app.match.MatchInformation): void {
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[0];
            var isCancel: boolean = false;
            if (selectedPrediction != null) isCancel = selectedPrediction.IsPredictionTeamAway;

            this.predictSvc.Predict(this.userProfile.UserId, match.id, match.TeamAwayId, isCancel)
                .then((respond: app.match.PredictionInformation[]): void => {
                    this.predictions = respond;
                });
        }

        public predictDraw(match: app.match.MatchInformation): void {
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[0];
            var isCancel: boolean = false;
            if (selectedPrediction != null) isCancel = selectedPrediction.IsPredictionDraw;

            this.predictSvc.Predict(this.userProfile.UserId, match.id, null, isCancel)
                .then((respond: app.match.PredictionInformation[]): void => {
                    this.predictions = respond;
                });
        }

        public IsGameStarted(match: app.match.MatchInformation): boolean {
            if (match.StartedDate) return true;
        }

        public IsUnSelectedHome(match: app.match.MatchInformation): boolean {
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[0];
            if (selectedPrediction != null) {
                return !selectedPrediction.IsPredictionTeamHome;
            } else return true;
        }

        public IsSelectedHome(match: app.match.MatchInformation): boolean {
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[0];
            if (selectedPrediction != null) {
                if (!match.CompletedDate) {
                    return selectedPrediction.IsPredictionTeamHome;
                } else return false;
            } else return false;
        }

        public IsPredictHomeWin(match: app.match.MatchInformation): boolean {
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[0];
            if (selectedPrediction != null) {
                if (match.CompletedDate) {
                    return match.IsTeamHomeWin && selectedPrediction.IsPredictionTeamHome;
                } else return false;
            } else return false;
        }

        public IsPredictHomeLose(match: app.match.MatchInformation): boolean {
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[0];
            if (selectedPrediction != null) {
                if (match.CompletedDate) {
                    return !match.IsTeamHomeWin && selectedPrediction.IsPredictionTeamHome;
                } else return false;
            } else return false;
        }

        public IsUnSelectedAway(match: app.match.MatchInformation): boolean {
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[0];
            if (selectedPrediction != null) {
                return !selectedPrediction.IsPredictionTeamAway;
            } else return true;
        }

        public IsSelectedAway(match: app.match.MatchInformation): boolean {
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[0];
            if (selectedPrediction != null) {
                if (!match.CompletedDate) {
                    return selectedPrediction.IsPredictionTeamAway;
                } else return false;
            } else return false;

        }

        public IsPredictAwayWin(match: app.match.MatchInformation): boolean {
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[0];
            if (selectedPrediction != null) {
                if (match.CompletedDate) {
                    return match.IsTeamAwayWin && selectedPrediction.IsPredictionTeamAway;
                } else return false;
            } else return false;
        }

        public IsPredictAwayLose(match: app.match.MatchInformation): boolean {
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[0];
            if (selectedPrediction != null) {
                if (match.CompletedDate) {
                    return !match.IsTeamAwayWin && selectedPrediction.IsPredictionTeamAway;
                } else return false;
            } else return false;
        }

        public IsUnSelectedDraw(match: app.match.MatchInformation): boolean {
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[0];
            if (selectedPrediction != null) {
                return !selectedPrediction.IsPredictionDraw;
            } else return true;
        }

        public IsSelectedDraw(match: app.match.MatchInformation): boolean {
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[0];
            if (selectedPrediction != null) {
                if (!match.CompletedDate) {
                    return selectedPrediction.IsPredictionDraw;
                } else return false;
            } else return false;

        }

        public IsPredictDrawWin(match: app.match.MatchInformation): boolean {
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[0];
            if (selectedPrediction != null) {
                if (match.CompletedDate) {
                    return match.IsGameDraw && selectedPrediction.IsPredictionDraw;
                } else return false;
            } else return false;
        }

        public IsPredictDrawLose(match: app.match.MatchInformation): boolean {
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[0];
            if (selectedPrediction != null) {
                if (match.CompletedDate) {
                    return !match.IsGameDraw && selectedPrediction.IsPredictionDraw;
                } else return false;
            }
        }

        public IsShowHomePredictionPoint(match: app.match.MatchInformation): boolean {
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[0];
            if (selectedPrediction != null) {
                return selectedPrediction.IsPredictionTeamHome;
            } else return false;
        }

        public IsShowAwayPredictionPoint(match: app.match.MatchInformation): boolean {
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[0];
            if (selectedPrediction != null) {
                return selectedPrediction.IsPredictionTeamAway;
            } else return false;
        }

        public IsShowDrawPredictionPoint(match: app.match.MatchInformation): boolean {
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[0];
            if (selectedPrediction != null) {
                return selectedPrediction.IsPredictionDraw;
            } else return false;
        }

        public GetPredictionPoint(match: app.match.MatchInformation): number {
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[0];
            if (selectedPrediction != null) {
                return selectedPrediction.PredictionPoints;
            } else 0;
        }
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