﻿module app.match {
	'use strict';

    class PredictionController {

        public CurrentDate: Date = new Date();
        public PastOneDaysDate: Date = new Date();
        public PastTwoDaysDate: Date = new Date();
        public FutureOneDaysDate: Date = new Date();
        public FutureTwoDaysDate: Date = new Date();
        public Leagues: string[];

        static $inject = ['matches', 'predictions'];
        constructor(public matches: app.match.MatchInformation[], public predictions: app.match.PredictionInformation[]) {
            this.Leagues = matches.map(it => it.LeagueName).filter(function (item, i, ar) { return ar.indexOf(item) === i; });
            this.updateDisplayDate(this.CurrentDate);
        }

        public GetMatchesByLeagueName(leagueName: string): app.match.MatchInformation[] {
            return this.matches.filter(it => it.LeagueName == leagueName);
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