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
                         "matches": ["app.match.MatchService", (svc: app.match.MatchService) => {
                             var now = new Date();
                             return svc.GetMatchesByDate(28, 12, 2015);
                         }],
                         "predictions": ["app.match.MatchService", (svc: app.match.MatchService) => {
                             var now = new Date();
                             return svc.GetPredictionsByDate("u01guest", 28, 12, 2015);
                         }],
                     }
                 }
             }
            })

            .state('app.reward', {
                url: '/reward',
                views: {
                    'menuContent': {
                        templateUrl: 'templates/RewardTab.html'
                    }
                }
            })

            .state('app.reward.lists', {
             url: '/lists',
             views: {
                 'tab-rewards': {
                     templateUrl: 'templates/Rewards.html',
                     controller: 'app.reward.RewardsController as cx',
                     resolve: {
                         "data": ["app.shared.RewardsService", svc => { return svc.getAll(); }]
                     }
                 }
             }
            })

            .state('app.reward.winners', {
             url: '/winners',
             views: {
                 'tab-winners': {
                     templateUrl: 'templates/Winners.html',
                     controller: 'app.reward.WinnersController as cx',
                     resolve: {
                         "data": ["app.shared.WinnersRewardService", svc => { return svc.getAll(); }],
                         "nameData": ["app.shared.WinnersListService", svc => { return svc.getAll(); }]
                     }
                 }
             }
            })

            .state('app.reward.myrewards', {
             url: '/myrewards',
             views: {
                 'tab-myrewards': {
                     templateUrl: 'templates/MyRewards.html',
                     controller: 'app.reward.MyRewardsController as cx',
                     resolve: {
                         "data": ["app.shared.MyRewardsService", svc => { return svc.getAll(); }],
                         "oldData": ["app.shared.MyOldRewardsService", svc => { return svc.getAll(); }]
                     }
                 }
             }
            })

            .state('app.buy', {
             url: '/buy',
             views: {
                 'menuContent': {
                     templateUrl: 'templates/BuyCoupon.html',
                     controller: 'app.reward.BuyCouponController as cx'
                 }
             }
            })

            .state('app.processing', {
             url: '/processing',
             views: {
                 'menuContent': {
                     templateUrl: 'templates/BuyCouponProcessing.html',
                     controller: 'app.reward.BuyCouponProcessingController as cx'
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
             url: '/verifycode/:phoneNo',
             views: {
                 'menuContent': {
                     templateUrl: 'templates/VerifyCode.html',
                     controller: 'app.account.VerifySecretCodeController as cx',
                     resolve: {
                         "phoneNo": ["$stateParams", stateParams=> { return stateParams.phoneNo; }]
                     }
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