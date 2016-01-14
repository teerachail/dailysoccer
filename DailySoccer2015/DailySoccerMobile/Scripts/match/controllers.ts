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
            'app.match.PredictionsService',
            'predictions',
            'couponSummary',
            '$ionicModal',
            '$scope',
            '$stateParams',
            '$ionicPopup',
            'ads',
        '$ionicHistory'];
        constructor(
            public leagues: any,
            public userSvc: app.shared.UserProfileService,
            private predictSvc: app.match.PredictionsService,
            public predictions: any,
            public couponSummary,
            private $ionicModal,
            private $scope,
            private params,
            private $ionicPopup,
            private ads,
            private $ionicHistory) {

            $ionicHistory.nextViewOptions({
                disableAnimate: true,
                disableBack: true
            });

            this.userProfile = this.userSvc.GetUserProfile();
            this.updatePredictionRemainning();
        }

        private updatePredictionRemainning() {
            var now = new Date();
            const MaximunPredictionCount = 5;
            if (this.predictions == null) this.PredictionRemainingCount = MaximunPredictionCount;
            if (this.params.day == now.getDate()) {
                this.PredictionRemainingCount = MaximunPredictionCount - this.predictions.length;
            }
        }

        private checkAllowPredict(matchId: string): boolean {
            const DisabledPrediction = 0;
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == matchId)[firstPrediction];
            var isPredicted = selectedPrediction != null;
            var isAllowPredict = this.PredictionRemainingCount > DisabledPrediction;
            return isPredicted || isAllowPredict;
        }

        private predict(userId: string, matchId: string, selectedTeamId: string, isCancel: boolean): void {            
            if (this.checkAllowPredict(matchId)) {
                this.predictSvc.Predict(userId, matchId, selectedTeamId, isCancel)
                    .then((respond: any): void => {
                        this.predictions = respond;
                        this.updatePredictionRemainning();
                    });
            } else {
                this.$ionicPopup.alert({
                    title: 'เลือกได้สูงสุดเพียง 5 คู่',
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

            this.predict(this.userProfile.UserId, match.id, match.TeamHomeId, isCancel);          
        }

        public predictTeamAway(match: any): void {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            var isCancel: boolean = false;
            if (selectedPrediction != null) isCancel = selectedPrediction.IsPredictionTeamAway;

            this.predict(this.userProfile.UserId, match.id, match.TeamAwayId, isCancel);
        }

        public predictDraw(match: any): void {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            var isCancel: boolean = false;
            if (selectedPrediction != null) isCancel = selectedPrediction.IsPredictionDraw;

            this.predict(this.userProfile.UserId, match.id, null, isCancel);
        }
        //#endregion Predictions

        //#region Gamestatus
        public IsGameStarted(match: any): boolean {
            if (match.StartedDate) return true;
        }

        public IsSelectedHome(match: any): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            if (match.CompletedDate) return false;
            return selectedPrediction.IsPredictionTeamHome;
        }

        public IsUnSelectedHome(match: any): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return true;
            return !selectedPrediction.IsPredictionTeamHome;
        }

        public IsPredictHomeWin(match: any): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            if (!match.CompletedDate) return false;
            return match.IsTeamHomeWin && selectedPrediction.IsPredictionTeamHome;
        }

        public IsPredictHomeLose(match: any): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            if (!match.CompletedDate) return false;
            return !match.IsTeamHomeWin && selectedPrediction.IsPredictionTeamHome;
        }

        public IsSelectedAway(match: any): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            if (match.CompletedDate) return false;
            return selectedPrediction.IsPredictionTeamAway;
        }

        public IsUnSelectedAway(match: any): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return true;
            return !selectedPrediction.IsPredictionTeamAway;
        }

        public IsPredictAwayWin(match: any): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            if (!match.CompletedDate) return false;
            return match.IsTeamAwayWin && selectedPrediction.IsPredictionTeamAway;
        }

        public IsPredictAwayLose(match: any): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            if (!match.CompletedDate) return false;
            return !match.IsTeamAwayWin && selectedPrediction.IsPredictionTeamAway;
        }

        public IsSelectedDraw(match: any): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            if (match.CompletedDate) return false;
            return selectedPrediction.IsPredictionDraw;
        }

        public IsUnSelectedDraw(match: any): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return true;
            return !selectedPrediction.IsPredictionDraw;
        }

        public IsPredictDrawWin(match: any): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            if (!match.CompletedDate) return false;
            return match.IsGameDraw && selectedPrediction.IsPredictionDraw;
        }

        public IsPredictDrawLose(match: any): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            if (!match.CompletedDate) return false;
            return !match.IsGameDraw && selectedPrediction.IsPredictionDraw;
        }

        public IsShowHomePredictionPoint(match: any): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            return selectedPrediction.IsPredictionTeamHome;
        }

        public IsShowAwayPredictionPoint(match: any): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            return selectedPrediction.IsPredictionTeamAway;
        }

        public IsShowDrawPredictionPoint(match: any): boolean {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return false;
            return selectedPrediction.IsPredictionDraw;
        }

        public GetPredictionPoint(match: any): number {
            const firstPrediction = 0;
            var selectedPrediction = this.predictions.filter(it => it.MatchId == match.id)[firstPrediction];
            if (selectedPrediction == null) return 0;
            return selectedPrediction.PredictionPoints;
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