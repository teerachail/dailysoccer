module app.match {
	'use strict';

    class PredictionController {

        public CurrentDate: Date = new Date();
        public PastOneDaysDate: Date = new Date();
        public PastTwoDaysDate: Date = new Date();
        public FutureOneDaysDate: Date = new Date();
        public FutureTwoDaysDate: Date = new Date();

        static $inject = ['data','point'];
        constructor(public data, public point) {
            this.updateDisplayDate(this.CurrentDate);
        }

        public IsGameStarted(match: any): boolean {
            if (match.StartedDate) return true;
        }

        public IsUnSelectedHome(match: any): boolean {
                return !match.IsSelectedHome;
        }

        public IsSelectedHome(match: any): boolean {
            if (!match.CompleteDate) {
                return match.IsSelectedHome;
            } else {
                return false;
            }
                          
        }

        public IsPredictHomeWin(match: any): boolean {
            if (match.CompleteDate) {
                return match.IsTeamHomeWin && match.IsSelectedHome;
            } else {
                return false;
            }
        }

        public IsPredictHomeLose(match: any): boolean {
            if (match.CompleteDate) {
                return match.IsSelectedHome && !match.IsTeamHomeWin;
            } else {
                return false;
            }
        }

        public IsUnSelectedAway(match: any): boolean {
            return !match.IsSelectedAway;
        }

        public IsSelectedAway(match: any): boolean {
            if (!match.CompleteDate) {
                return match.IsSelectedAway;
            } else {
                return false;
            }

        }

        public IsPredictAwayWin(match: any): boolean {
            if (match.CompleteDate) {
                return match.IsTeamAwayWin && match.IsSelectedAway;
            } else {
                return false;
            }
        }

        public IsPredictAwayLose(match: any): boolean {
            if (match.CompleteDate) {
                return match.IsSelectedAway && !match.IsTeamAwayWin;
            } else {
                return false;
            }
        }

        public IsUnSelectedDraw(match: any): boolean {
            return !match.IsSelectedDraw;
        }

        public IsSelectedDraw(match: any): boolean {
            if (!match.CompleteDate) {
                return match.IsSelectedDraw;
            } else {
                return false;
            }

        }

        public IsPredictDrawWin(match: any): boolean {
            if (match.CompleteDate) {
                return match.IsGameDraw && match.IsSelectedDraw;
            } else {
                return false;
            }
        }

        public IsPredictDrawLose(match: any): boolean {
            if (match.CompleteDate) {
                return match.IsGameDraw && !match.IsSelectedDraw;
            } else {
                return false;
            }
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
    }
   
	angular
		.module('app.match')
        .controller('app.match.PredictionController', PredictionController);

}