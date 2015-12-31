module app.match {
    'use strict';

    export class MatchInformation {
        id: string;
        TeamHomeId: string;
        TeamHomeName: string;
        TeamHomeScore: number;
        TeamHomePoint: number;
        TeamAwayId: string;
        TeamAwayName: string;
        TeamAwayScore: number;
        TeamAwayPoint: number;
        DrawPoints: number;
        BeginDate: Date;
        StartedDate: Date;
        CompletedDate: Date;
        LeagueId: number;
        LeagueName: string;
        Status: string;
        IsTeamHomeWin: boolean;
        IsTeamAwayWin: boolean;
        IsGameDraw: boolean;
    }

    export class PredictionInformation {
        MatchId: string;
        IsPredictionTeamHome: boolean;
        IsPredictionTeamAway: boolean;
        IsPredictionDraw: boolean;
        PredictionPoints: number;
    }
    
}