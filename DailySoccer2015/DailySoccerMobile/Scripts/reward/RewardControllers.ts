module app.reward {
    'use strict';

    class RewardsController {

        static $inject = ['data', 'point', 'app.reward.BuyCouponDataService'];
        constructor(public data, public point, private buySvc: app.reward.BuyCouponDataService) {
            buySvc.InitialData(200, 500); // HACK: Setup coupon data
        }

    }

    class BuyCouponController {

        static $inject = ['$ionicModal', '$scope','$state', 'app.reward.BuyCouponDataService'];
        constructor(private $ionicModal, private $scope,private $state: angular.ui.IStateService, private buySvc: app.reward.BuyCouponDataService) {
            this.$ionicModal.fromTemplateUrl('templates/BuyCouponPopup.html',
                {
                    scope: $scope,
                    animation: 'slide-ins-up'
                }).then(modal=> { this.$scope.ErrorPopup = modal; });
        }

        public BuyCoupons(buyAmount: number): void {
            const MinimumBuyAmount = 1;
            var isRequestValid = buyAmount >= MinimumBuyAmount && buyAmount <= this.buySvc.BuyingPower;
            if (!isRequestValid) return;

            this.buySvc.RequestBuyAmount = buyAmount;
            this.$state.go("app.coupon.processing");
        }
    }

    class BuyCouponProcessingController {

        private buyCouponResult: BuyCouponRespond;

        static $inject = ['$scope', '$timeout', '$ionicModal', '$state', 'app.reward.BuyCouponDataService', 'app.shared.UserProfileService', 'app.reward.BuyCouponService'];
        constructor(private $scope, private $timeout: ng.ITimeoutService, private $ionicModal, private $state: angular.ui.IStateService, private couponDataSvc: app.reward.BuyCouponDataService, private userprofileSvc: app.shared.UserProfileService, private buySvc: app.reward.BuyCouponService) {
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

            if (this.couponDataSvc.IsBuyingCompleted) {
                this.gobackToBuyCouponPage();
                return;
            }

            if (!this.checkPreparingPopups()) return;

            if (!this.checkFacebookAuthentication()) return;
            else console.log('Facebook authentication verified');

            if (!this.checkPhoneVerification()) return;
            else console.log('Phone number verified');

            this.buyCoupons();
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
                if (this.couponDataSvc.CheckFirstTimeForRequestFacebookLogin()) this.$scope.FacebookPopup.show();
                else this.gobackToBuyCouponPage();
            }
            return userprofile.IsVerifiedFacebook;

        }
        private checkPhoneVerification(): boolean {
            var userprofile = this.userprofileSvc.GetUserProfile();
            var isPhoneNumberVerified = userprofile.VerifiedPhoneNumber != null;
            if (!isPhoneNumberVerified) {
                if (this.couponDataSvc.CheckFirstTimeForRequestPhoneVerification()) this.$state.go('app.verify.phone');
                else this.gobackToBuyCouponPage();
            }
            return isPhoneNumberVerified;
        }
        private buyCoupons(): void {
            this.couponDataSvc.SendPurchaseOrderCompleted();
            var userprofile = this.userprofileSvc.GetUserProfile();
            var body = new BuyCouponRequest();
            body.UserId = userprofile.UserId;
            body.BuyAmount = this.couponDataSvc.RequestBuyAmount;
            this.buySvc.BuyCoupon(body)
                .then((respond: BuyCouponRespond) => {
                    this.buyCouponResult = respond;
                    if (respond.IsSuccess) this.$scope.CompletedPopup.show();
                    else {
                        // HACK: Show the error message
                        alert('Buy failed: ' + respond.ErrorMessage);
                    }
                });
        }
        private gobackToBuyCouponPage(): void {
            this.couponDataSvc.ResetAllRequests();
            this.$state.go('app.coupon.buy');
        }
    }

    angular
        .module('app.reward')
        .controller('app.reward.RewardsController', RewardsController)
        .controller('app.reward.BuyCouponController', BuyCouponController)
        .controller('app.reward.BuyCouponProcessingController', BuyCouponProcessingController);
}