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
        public PredictionRemainingCount: number;
        private userProfile: any;
        public predictions: any;
        private disableList: string[] = [];

        static $inject = [
            'matches',
            'app.shared.UserProfileService',
            'app.match.PredictionsService',
            'app.account.UserProfileService',
            '$ionicModal',
            '$scope',
            '$rootScope',
            '$stateParams',
            '$ionicPopup',
            'ads',
            '$ionicHistory',
            'MaximunPredictionCount'];
        constructor(
            public leagues: any,
            public userSvc: app.shared.UserProfileService,
            private predictSvc: app.match.PredictionsService,
            public userprofileSvc: app.account.UserProfileService,
            private $ionicModal,
            private $scope,
            private $rootScope,
            private params,
            private $ionicPopup,
            private ads,
            private $ionicHistory,
            private maximunPredictionCount: number) {

            $rootScope.refresher = () => {
                this.onPageLoad();
            };

            $ionicHistory.nextViewOptions({
                disableAnimate: true,
                disableBack: true
            });

            this.onPageLoad();
        }

        private onPageLoad() {
            if (this.userSvc.IsLogedIn()) {
                this.userprofileSvc.GetUserProfile().then(it=> {
                    this.userSvc.CurrentPoints = it.Points;
                    this.userProfile = it;
                });
                this.loadPredictions();
            }
        }

        private loadPredictions() {
            var user = this.userSvc.GetUserProfile().UserId;
            this.predictSvc.GetPredictionsByDate(user, this.params.day).then((respond) => {
                this.predictions = respond;
                // TODO:
                this.updatePredictionRemainning();
            });
        }


        private updatePredictionRemainning() {
            var now = new Date();
            var MaximunPredictionCount = this.maximunPredictionCount;
            if (this.params.day == now.getDate()) {
                for (var elementIndex = 0; elementIndex < this.predictions.length; elementIndex++) {
                    this.userSvc.MatchPredictions.push(this.predictions[elementIndex].MatchId);
                }
                this.userSvc.PredictionRemainingCount = MaximunPredictionCount - this.userSvc.MatchPredictions.data.length;
            }
        }
        
        private predict(userId: string, matchId: string, selectedTeamId: string, isCancel: boolean): void {
            const MinimumToSendPrediction = 1;
            var isAllowToPredict = this.userSvc.PredictionRemainingCount >= MinimumToSendPrediction;
            if (isAllowToPredict) {
                if (isCancel) this.userSvc.MatchPredictions.pull(matchId);
                this.predictSvc.Predict(userId, matchId, selectedTeamId, isCancel)
                    .then((respond: any): void => {
                        this.disableList = [];
                        this.$scope.$apply();
                        this.predictions = respond;
                        this.predictions.forEach((value) => value.isAnimate = false);
                        this.updatePredictionRemainning();
                    });
            } else {
                this.$ionicPopup.alert({
                    title: 'เลือกได้สูงสุดเพียง ' + this.maximunPredictionCount + ' คู่',
                    okType: 'button-royal',
                    okText: 'ปิด'
                });
            }
        }

        //#region Predictions
        public predictTeamHome(match: any): void {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            var isCancel: boolean = false;
            if (selectedPrediction != null) isCancel = selectedPrediction.IsPredictionTeamHome;

            //selectedPrediction.isAnimate = true;
            this.predict(this.userProfile.id, match.id, match.TeamHomeId, isCancel);
            this.disableList.push(match.id + 'home');
        }

        public predictTeamAway(match: any): void {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            var isCancel: boolean = false;
            if (selectedPrediction != null) isCancel = selectedPrediction.IsPredictionTeamAway;

            //selectedPrediction.isAnimate = true;
            this.predict(this.userProfile.id, match.id, match.TeamAwayId, isCancel);
            this.disableList.push(match.id + 'away');
        }

        public predictDraw(match: any): void {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            var isCancel: boolean = false;
            if (selectedPrediction != null) isCancel = selectedPrediction.IsPredictionDraw;

            //selectedPrediction.isAnimate = true;
            this.predict(this.userProfile.id, match.id, null, isCancel);
            this.disableList.push(match.id + 'draw');
        }
        //#endregion Predictions

        //#region Gamestatus
        public IsAnimate(match: any, option: string): boolean {
            var matchId = match.id + option;
            var isInTheDisableList = this.disableList.filter(it=> it == matchId).length > 0;
            return isInTheDisableList;
        }

        public IsGameStarted(match: any): boolean {
            if (match.StartedDate) return true;
        }

        public GetBeginDate(begindate: any): Date {
            return begindate;
        }

        public IsSelectedHome(match: any): boolean {
            if (this.predictions != null) {
                const firstPrediction = 0;
                var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
                if (selectedPrediction == null) return false;
                if (match.CompletedDate) return false;
                return selectedPrediction.IsPredictionTeamHome;
            }
        }

        public IsUnSelectedHome(match: any): boolean {
            if (this.predictions != null) {
                const firstPrediction = 0;
                var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
                if (selectedPrediction == null) return true;
                return !selectedPrediction.IsPredictionTeamHome;
            }
        }

        public IsPredictHomeWin(match: any): boolean {
            if (this.predictions != null) {
                const firstPrediction = 0;
                var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
                if (selectedPrediction == null) return false;
                if (!match.CompletedDate) return false;
                return match.IsTeamHomeWin && selectedPrediction.IsPredictionTeamHome;
            }
        }

        public IsPredictHomeLose(match: any): boolean {
            if (this.predictions != null) {
                const firstPrediction = 0;
                var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
                if (selectedPrediction == null) return false;
                if (!match.CompletedDate) return false;
                return !match.IsTeamHomeWin && selectedPrediction.IsPredictionTeamHome;
            }
        }

        public IsSelectedAway(match: any): boolean {
            if (this.predictions != null) {
                const firstPrediction = 0;
                var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
                if (selectedPrediction == null) return false;
                if (match.CompletedDate) return false;
                return selectedPrediction.IsPredictionTeamAway;
            }
        }

        public IsUnSelectedAway(match: any): boolean {
            if (this.predictions != null) {
                const firstPrediction = 0;
                var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
                if (selectedPrediction == null) return true;
                return !selectedPrediction.IsPredictionTeamAway;
            }
        }

        public IsPredictAwayWin(match: any): boolean {
            if (this.predictions != null) {
                const firstPrediction = 0;
                var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
                if (selectedPrediction == null) return false;
                if (!match.CompletedDate) return false;
                return match.IsTeamAwayWin && selectedPrediction.IsPredictionTeamAway;
            }
        }

        public IsPredictAwayLose(match: any): boolean {
            if (this.predictions != null) {
                const firstPrediction = 0;
                var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
                if (selectedPrediction == null) return false;
                if (!match.CompletedDate) return false;
                return !match.IsTeamAwayWin && selectedPrediction.IsPredictionTeamAway;
            }
        }

        public IsSelectedDraw(match: any): boolean {
            if (this.predictions != null) {
                const firstPrediction = 0;
                var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
                if (selectedPrediction == null) return false;
                if (match.CompletedDate) return false;
                return selectedPrediction.IsPredictionDraw;
            }
        }

        public IsUnSelectedDraw(match: any): boolean {
            if (this.predictions != null) {
                const firstPrediction = 0;
                var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
                if (selectedPrediction == null) return true;
                return !selectedPrediction.IsPredictionDraw;
            }
        }

        public IsPredictDrawWin(match: any): boolean {
            if (this.predictions != null) {
                const firstPrediction = 0;
                var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
                if (selectedPrediction == null) return false;
                if (!match.CompletedDate) return false;
                return match.IsGameDraw && selectedPrediction.IsPredictionDraw;
            }
        }

        public IsPredictDrawLose(match: any): boolean {
            if (this.predictions != null) {
                const firstPrediction = 0;
                var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
                if (selectedPrediction == null) return false;
                if (!match.CompletedDate) return false;
                return !match.IsGameDraw && selectedPrediction.IsPredictionDraw;
            }
        }

        public IsShowHomePredictionPoint(match: any): boolean {
            if (this.predictions != null) {
                const firstPrediction = 0;
                var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
                if (selectedPrediction == null) return false;
                return selectedPrediction.IsPredictionTeamHome;
            }
        }

        public IsShowAwayPredictionPoint(match: any): boolean {
            if (this.predictions != null) {
                const firstPrediction = 0;
                var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
                if (selectedPrediction == null) return false;
                return selectedPrediction.IsPredictionTeamAway;
            }
        }

        public IsShowDrawPredictionPoint(match: any): boolean {
            if (this.predictions != null) {
                const firstPrediction = 0;
                var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
                if (selectedPrediction == null) return false;
                return selectedPrediction.IsPredictionDraw;
            }
        }

        public GetPredictionPoint(match: any): number {
            if (this.predictions != null) {
                const firstPrediction = 0;
                var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
                if (selectedPrediction == null) return 0;
                return selectedPrediction.PredictionPoints;
            }
        }
        //#endregion Gamestatus

        public IsMatchAvailable(match: MatchInformation): boolean {
            var isAvailable = match.TeamHomePoint != null
                && match.TeamAwayPoint != null
                && match.DrawPoints != null;
            return isAvailable;
        }

        public CompletedMatch(match: MatchInformation): boolean {
            return match.CompletedDate != null;
        }

        public OpenAds(): void {
            window.open(this.ads.LinkUrl, '_blank', 'location=yes');
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