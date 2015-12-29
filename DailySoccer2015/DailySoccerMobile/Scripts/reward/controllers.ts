module app.reward {
	'use strict';

	class RewardsController {

		//public model: any = null;

        static $inject = ['data'];
        constructor(public data) {
		}

		// public myMethod(): void {
		// }
        BuyCouponController

	}
    class BuyCouponController {

        //public model: any = null;

        static $inject = ['$scope', '$ionicModal'];
        constructor(private $scope, private $ionicModal) {
            this.$ionicModal.fromTemplateUrl('templates/Facebook.html',
                {
                    scope: $scope,
                    animation: 'slide-in-up'
                }).then(function (modal): void { $scope.FacebookPopup = modal; });
            this.$ionicModal.fromTemplateUrl('templates/Tie.html',
                {
                    scope: $scope,
                    animation: 'slide-in-up'
                }).then(function (modal): void { $scope.TiePopup = modal; });
            this.$ionicModal.fromTemplateUrl('templates/BuyCouponComplete.html',
                {
                    scope: $scope,
                    animation: 'slide-in-up'
                }).then(function (modal): void { $scope.CompletedPopup = modal; });
        }
       
        
    }
	angular
		.module('app.reward')
        .controller('app.reward.RewardsController', RewardsController)
        .controller('app.reward.BuyCouponController', BuyCouponController);
}