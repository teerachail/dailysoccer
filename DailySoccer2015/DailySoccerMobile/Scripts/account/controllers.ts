module app.account {
    'use strict';

    class VerifySecretCodeController {

        private userprofile: shared.UserProfile;

        static $inject = ['$scope', '$state', 'phoneNo', 'app.account.PhoneVerificationService', 'app.shared.UserProfileService'];
        constructor(private $scope: ng.IScope, private $state: angular.ui.IStateService, private phoneNo: string, private phoneSvc: app.account.PhoneVerificationService, private userprofileSvc: app.shared.UserProfileService) {
            this.userprofile = this.userprofileSvc.GetUserProfile();
            this.ResendSMS();
        }

        public ResendSMS(): void {
            console.log('Sending SMS to: ' + this.phoneNo);
            this.phoneSvc.UpdatePhoneNo(this.userprofile.UserId, this.phoneNo);
        }

        public SendVerificationCode(verificationCode: string): void {
            this.phoneSvc.SendVerificationCode(this.userprofile.UserId, this.phoneNo, verificationCode)
                .then((respond: VerificationCodeRespond) => {
                    console.log("Verify phone result: " + respond.IsSuccess);
                    if (respond.IsSuccess) {
                        this.userprofileSvc.UpdateVerifiedPhoneNo(this.phoneNo);
                        this.$state.go("app.processing", {}, { location: 'replace' });
                    }
                });
        }

    }

    angular
        .module('app.account')
        .controller('app.account.VerifySecretCodeController', VerifySecretCodeController);
}