module app.match {
    'use strict';

    class MainController {

        public CurrentDate: Date = new Date();
        public PastOneDaysDate: Date = new Date();
        public PastTwoDaysDate: Date = new Date();
        public FutureOneDaysDate: Date = new Date();
        public FutureTwoDaysDate: Date = new Date();
        public Leagues: string[];

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
            console.log('# Update date completed.');
        }

        public SelectDay(selectedDate: Date): void {
            this.$state.go('app.main.matches', {
                id: 'u01guest',
                day: selectedDate.getDate()
            });
        }
    }

    class PredictionController {

        public Leagues: string[];

        static $inject = ['matches', 'predictions', 'point', 'couponSummary', '$ionicModal', '$scope'];
        constructor(public leagues: app.match.LeagueInformation[], public predictions: app.match.PredictionInformation[], public point, public couponSummary, private $ionicModal, private $scope) {
            
            this.$ionicModal.fromTemplateUrl('templates/MatchesPopup.html',
                {
                    scope: $scope,
                    animation: 'slide-ins-up'
                }).then(modal=> { this.$scope.MatchPopup = modal; });
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

        static $inject = ['data', 'day'];
        constructor(public data, public day) {

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
        constructor(public data) {
        }
    }

    angular
        .module('app.match')
        .controller('app.match.MainController', MainController)
        .controller('app.match.DaylyHistoryController', DaylyHistoryController)
        .controller('app.match.MonthlyHistoryController', MonthlyHistoryController)
        .controller('app.match.PredictionController', PredictionController);

}