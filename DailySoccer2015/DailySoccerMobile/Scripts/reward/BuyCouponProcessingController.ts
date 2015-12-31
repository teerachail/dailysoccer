module app.reward {
    'use strict';

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
            this.$ionicModal.fromTemplateUrl('templates/BuyCouponPopup.html',
                {
                    scope: $scope,
                    animation: 'slide-ins-up'
                }).then(modal=> { this.$scope.ErrorPopup = modal; });

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
                && this.$scope.CompletedPopup != null
                && this.$scope.ErrorPopup != null;
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
            var userprofile = this.userprofileSvc.GetUserProfile();
            this.buySvc.BuyCoupon(userprofile.UserId, this.couponDataSvc.RequestBuyAmount)
                .then((respond: BuyCouponRespond) => {
                    this.buyCouponResult = respond;
                    this.couponDataSvc.SendPurchaseOrderCompleted(respond.IsSuccess);
                    if (respond.IsSuccess) this.$scope.CompletedPopup.show();
                    else this.$scope.ErrorPopup.show();
                })
                .catch(err=> {
                    // TODO: Inform error to user
                });
        }
        private gobackToBuyCouponPage(): void {
            this.couponDataSvc.ResetAllRequests();
            this.$state.go('app.coupon.buy');
        }
    }

    angular
        .module('app.reward')
        .controller('app.reward.BuyCouponProcessingController', BuyCouponProcessingController);
}