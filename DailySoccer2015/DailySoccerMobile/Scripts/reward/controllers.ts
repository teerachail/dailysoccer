module app.reward {
    'use strict';

    class RewardsController {

        static $inject = ['data', 'app.reward.BuyCouponDataService'];
        constructor(public data, private buySvc: app.reward.BuyCouponDataService) {
            buySvc.InitialData(200, 500); // HACK: Setup coupon data
        }

    }

    class BuyCouponController {

        static $inject = ['$state', 'app.reward.BuyCouponDataService'];
        constructor(private $state: angular.ui.IStateService, private buySvc: app.reward.BuyCouponDataService) {
        }

        public BuyCoupons(buyAmount: number): void {
            const MinimumBuyAmount = 1;
            var isRequestValid = buyAmount >= MinimumBuyAmount && buyAmount <= this.buySvc.BuyingPower;
            if (!isRequestValid) return;

            this.$state.go("app.processing");
        }
    }

    class BuyCouponProcessingController {
        static $inject = ['$scope', '$timeout', '$ionicModal', '$state', 'app.reward.BuyCouponDataService', 'app.shared.UserProfileService'];
        constructor(private $scope, private $timeout: ng.ITimeoutService, private $ionicModal, private $state: angular.ui.IStateService, private buySvc: app.reward.BuyCouponDataService, private userprofileSvc: app.shared.UserProfileService) {
            this.$ionicModal.fromTemplateUrl('templates/Facebook.html',
                {
                    scope: $scope,
                    animation: 'slide-in-up'
                }).then(modal=> { this.$scope.FacebookPopup = modal; });
            this.$ionicModal.fromTemplateUrl('templates/Tie.html',
                {
                    scope: $scope,
                    animation: 'slide-in-up'
                }).then(modal=> { this.$scope.TiePopup = modal; });
            this.$ionicModal.fromTemplateUrl('templates/BuyCouponComplete.html',
                {
                    scope: $scope,
                    animation: 'slide-in-up'
                }).then(modal=> { this.$scope.CompletedPopup = modal; });

            this.$scope.$on('modal.hidden', () => this.validateUserProfile());
            this.validateUserProfile();
        }

        private validateUserProfile(): void {
            if (!this.checkPreparingPopups()) return;

            if (!this.checkFacebookAuthentication()) return;
            else console.log('Facebook authentication verified');

            if (!this.checkPhoneVerification()) return;
            else console.log('Phone number verified');
        }
        private checkPreparingPopups(): boolean {
            var isSetupCompleted = this.$scope.FacebookPopup != null
                && this.$scope.TiePopup != null
                && this.$scope.CompletedPopup != null;
            if (!isSetupCompleted) {
                console.log('Preparing...');
                this.$timeout(() => this.validateUserProfile(), 333);
            }
            return isSetupCompleted;
        }
        private checkFacebookAuthentication(): boolean {
            var userprofile = this.userprofileSvc.GetUserProfile();
            if (!userprofile.IsVerifiedFacebook) {
                if (this.buySvc.CheckFirstTimeForRequestFacebookLogin()) this.$scope.FacebookPopup.show();
                else this.gobackToBuyCouponPage();
            }
            return userprofile.IsVerifiedFacebook;

        }
        private checkPhoneVerification(): boolean {
            var userprofile = this.userprofileSvc.GetUserProfile();
            var isPhoneNumberVerified = userprofile.VerifiedPhoneNumber != null;
            if (!isPhoneNumberVerified) {
                if (this.buySvc.CheckFirstTimeForRequestPhoneVerification()) this.$state.go('app.phone');
                else this.gobackToBuyCouponPage();
            }
            return isPhoneNumberVerified;
        }
        private gobackToBuyCouponPage(): void {
            this.buySvc.ResetAllRequests();
            this.$state.go('app.buy');
        }
    }

    angular
        .module('app.reward')
        .controller('app.reward.RewardsController', RewardsController)
        .controller('app.reward.BuyCouponController', BuyCouponController)
        .controller('app.reward.BuyCouponProcessingController', BuyCouponProcessingController);
}