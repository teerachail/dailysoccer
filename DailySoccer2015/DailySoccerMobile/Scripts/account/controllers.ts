module app.account {
	'use strict';

    class VerifySecretCodeController {

        static $inject = ['$scope', '$state', 'phoneNo', 'app.account.PhoneVerificationService'];
        constructor(private $scope: ng.IScope, private $state: angular.ui.IStateService, private phoneNo: string, private phoneSvc: app.account.PhoneVerificationService) {
            this.ResendSMS();
        }

        public ResendSMS(): void {
            console.log('Sending SMS to: ' + this.phoneNo);
            this.phoneSvc.UpdatePhoneNo("u01guest", this.phoneNo);
        }

        public SendVerificationCode(verificationCode: string): void {
            this.phoneSvc.SendVerificationCode("u01guest", this.phoneNo, verificationCode)
                .then((respond: VerificationCodeRespond) => {
                    console.log("Verify phone result: " + respond.IsSuccess);
                    if (respond.IsSuccess) this.$state.go("app.processing", {}, { location: 'replace' });
                });
        }

	}

	angular
		.module('app.account')
        .controller('app.account.VerifySecretCodeController', VerifySecretCodeController);
}