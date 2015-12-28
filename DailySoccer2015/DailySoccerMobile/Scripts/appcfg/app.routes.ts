module app {
	'use strict';

	angular
		.module('app')
		.config(configRoutes);

	configRoutes.$inject = [
		'$stateProvider',
		'$urlRouterProvider'
	];
	function configRoutes($stateProvider: ng.ui.IStateProvider, $urlRouterProvider: ng.ui.IUrlRouterProvider) {
		$stateProvider.state('sample', {
			url: '/sample',
			templateUrl: 'templates/sample.html',
            controller: 'app.shared.SimpleDataController as cx',
            resolve: {
                "data": ["app.shared.SampleDataService", svc => { return svc.getAll(); }]
            }
		// }).state('stateName', {
		//     url: '/url/to/navigate',
		//     templateUrl: 'url/to/template/:sampleId',
		//     controller: 'ControllerFullName',
		//     controllerAs: 'cx',
		//     resolve: {
		//         'ctlrInputId': ['$stateParams', '$q', ($stateParams: ng.ui.IStateParamsService, $q: ng.IQService): ng.IPromise<number> => {
		//             var defer = $q.defer<number>();
		//             defer.resolve((<any>$stateParams).sampleId);
		//             return defer.promise;
		//         }]
		//     }
        })
            .state('app', {
             url: '/app',
             abstract: true,
             templateUrl: 'templates/SideMenu.html'
            })
            .state('app.matches', {
             url: '/matches',                
             views: {
                 'menuContent': {
                     templateUrl: 'templates/Matches.html',
                     controller: 'app.match.PredictionController as cx',
                     resolve: {
                         "data": ["app.shared.MatchService", svc => { return svc.getAll(); }]
                     }
                 }
             },

            })
            .state('app.lists', {
             url: '/lists',
             views: {
                 'menuContent': {
                     templateUrl: 'templates/Rewards.html'
                 }
             }
            })
            .state('app.winners', {
             url: '/winners',
             views: {
                 'menuContent': {
                     templateUrl: 'templates/Winners.html'
                 }
             }
            })
            .state('app.myrewards', {
             url: '/myrewards',
             views: {
                 'menuContent': {
                     templateUrl: 'templates/MyRewards.html'
                 }
             }
            })
            .state('app.buy', {
             url: '/buy',
             views: {
                 'menuContent': {
                     templateUrl: 'templates/BuyCoupon.html'
                 }
             }
            })
            .state('app.processing', {
             url: '/processing',
             views: {
                 'menuContent': {
                     templateUrl: 'templates/BuyCouponProcessing.html'
                 }
             }
            })
            .state('app.phone', {
             url: '/phone',
             views: {
                 'menuContent': {
                     templateUrl: 'templates/VerifyPhone.html'
                 }
             }
            })
            .state('app.verifycode', {
             url: '/verifycode',
             views: {
                 'menuContent': {
                     templateUrl: 'templates/VerifyCode.html'
                 }
             }
            })
            .state('app.summary', {
             url: '/summary',
             views: {
                 'menuContent': {
                     templateUrl: 'templates/YearlyHistory.html'
                 }
             }
            })
            .state('app.monthly', {
             url: '/monthly',
             views: {
                 'menuContent': {
                     templateUrl: 'templates/MonthlyHistory.html'
                 }
             }
            })
            .state('app.underconstruction', {
             url: '/underconstruction',
             views: {
                 'menuContent': {
                     templateUrl: 'templates/UnderConstruction.html'
                 }
             }
            })
        ;



        $urlRouterProvider.otherwise('/app/matches');
	}
}