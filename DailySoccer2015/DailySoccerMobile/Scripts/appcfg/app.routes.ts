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
		$stateProvider/*.state('sample', {
			url: '/sample',
			templateUrl: 'templates/sample.html',
            controller: 'app.shared.SimpleDataController as cx',
            resolve: {
                "data": ["app.shared.SampleDataService", svc => { return svc.getAll(); }]
            } */
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
        /*})*/
            .state('app', {
             url: '/app',
             abstract: true,
             templateUrl: 'templates/SideMenu.html'
            })
            .state('app.main', {
                url: '/main',
                abstract: true,              
             views: {
                 'menuContent': {
                     templateUrl: 'templates/Tabs.html',
                     controller: 'app.match.MainController as cx',
                 }
             }

            })

            .state('app.main.matches', {
             url: '/matches/:id/:day/:month/:year',                
             views: {
                 'matchContent': {
                     templateUrl: 'templates/Matches.html',
                     controller: 'app.match.PredictionController as cx',
                     resolve: {
                         "matches": ["$stateParams", "app.match.MatchService", (params, svc: app.match.MatchService) => {
                             return svc.GetMatchesByDate(params.day, params.month, params.year);
                         }],
                         "predictions": ["$stateParams", "app.match.MatchService", (params,svc: app.match.MatchService) => {
                             return svc.GetPredictionsByDate(params.id, params.day, params.month, params.year);
                         }],
                         "point": ["app.shared.CouponPointsService", svc => { return svc.getAll(); }]
                     }
                 }
             }
            })

            .state('app.reward', {
                url: '/reward',
                abstract: true,
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
                         "data": ["app.shared.RewardsService", svc => { return svc.getAll(); }],
                         "couponSummary": ["app.reward.CouponSummaryService", svc=> { return svc.GetCouponSummary(); }]
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

            .state('app.coupon', {
                url: '/coupon',
                abstract: true,
                views: {
                    'menuContent': {
                        templateUrl: 'templates/BlankLayout.html'
                    }
                }
            })
                .state('app.coupon.sample', {
                    url: '/sample',
                    views: {
                        'menuContent': {
                            templateUrl: 'templates/sample.html'
                        }
                    }
                })

            .state('app.coupon.buy', {
             url: '/buy',
             views: {
                 'menuContent': {
                     templateUrl: 'templates/BuyCoupon.html',
                     controller: 'app.reward.BuyCouponController as cx'
                 }
             }
            })

            .state('app.coupon.processing', {
             url: '/processing',
             views: {
                 'menuContent': {
                     templateUrl: 'templates/BuyCouponProcessing.html',
                     controller: 'app.reward.BuyCouponProcessingController as cx'
                 }
             }
            })
            .state('app.verify', {
                url: '/verify',
                abstract: true,
                views: {
                'menuContent': {
                    templateUrl: 'templates/BlankLayout.html'
                }
            }
            })
            .state('app.verify.phone', {
             url: '/phone',
             views: {
                 'menuContent': {
                     templateUrl: 'templates/VerifyPhone.html'
                 }
             }
            })

            .state('app.verify.verifycode', {
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
            .state('app.history', {
                url: '/history',
                abstract: true,
                views: {
                    'menuContent': {
                        templateUrl: 'templates/BlankLayout.html'
                    }
                }
            })
            .state('app.history.summary', {
             url: '/summary',
             views: {
                 'menuContent': {
                     templateUrl: 'templates/YearlyHistory.html'
                 }
             }
            })

            .state('app.history.monthly', {
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

        $urlRouterProvider.otherwise(()=>
        {
            var now = new Date;
            var userId = 'u01guest';
            var month = now.getMonth() + 1;
            return '/app/main/matches/' + userId + '/' + now.getDate() + '/' + month + '/' + now.getFullYear();
        });
        //$urlRouterProvider.otherwise('/sample');
	}
}