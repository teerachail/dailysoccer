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
		//$stateProvider.state('default', {
		//	url: '/default',
		//	templateUrl: 'templateUrl',
		//	controller: 'ControllerFullName',
		//	controllerAs: 'cx'
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
		//});

		//$urlRouterProvider.otherwise('/home');
	}
}