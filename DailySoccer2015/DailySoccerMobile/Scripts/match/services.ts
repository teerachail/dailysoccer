﻿module app.match {
	'use strict';

    export interface IMatchService {
        GetMatchesByDate(day: number): ng.IPromise<LeagueInformation[]>;
        GetPredictionsByDate(userId: string, day: number): ng.IPromise<PredictionInformation[]>;
	}

    export class MatchService implements IMatchService {

        private getMatchesByDate: ng.resource.IResourceClass<any>;
        private getPredictionsByDate: ng.resource.IResourceClass<any>;

		static $inject = ['$resource'];
		constructor(private $resource: angular.resource.IResourceService) {
            this.getMatchesByDate = $resource('http://dailysoccer-au.azurewebsites.net/api/matches/:day/:month/:year', { "day": "@day", "month": "@month", "year": "@year" });
            this.getPredictionsByDate = $resource('http://dailysoccer-au.azurewebsites.net/api/predictions/:id/:day/:month/:year', { "id": "@id", "day": "@day", "month": "@month", "year": "@year" });
		}

        public GetMatchesByDate(day: number): ng.IPromise<LeagueInformation[]> {
            return this.getMatchesByDate.query({ day: day}).$promise;
        }

        public GetPredictionsByDate(userId: string, day: number): ng.IPromise<PredictionInformation[]> {
            return this.getPredictionsByDate.query({ id: userId ,day: day}).$promise;
        }

	}

	angular
		.module('app.match')
        .service('app.match.MatchService', MatchService);
}