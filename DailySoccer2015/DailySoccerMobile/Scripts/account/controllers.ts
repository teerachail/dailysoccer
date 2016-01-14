module app.account {
    'use strict';

    class VerifySecretCodeController {

        private userprofile: shared.UserProfile;

        static $inject = ['$scope', '$ionicPopup', '$state', 'phoneNo', 'app.account.PhoneVerificationService', 'app.shared.UserProfileService', '$ionicHistory'];
        constructor(private $scope: ng.IScope, private $ionicPopup, private $state: angular.ui.IStateService, private phoneNo: string, private phoneSvc: app.account.PhoneVerificationService, private userprofileSvc: app.shared.UserProfileService, private $ionicHistory: ionic.navigation.IonicHistoryService) {
            this.userprofile = this.userprofileSvc.GetUserProfile();
            this.ResendSMS();
        }

        public ResendSMS(): void {
            console.log('Sending SMS to: ' + this.phoneNo);
            this.phoneSvc.UpdatePhoneNo(this.userprofile.UserId, this.phoneNo);
        }

        public SendVerificationCode(verificationCode: string): void {
            this.phoneSvc.SendVerificationCode(this.userprofile.UserId, this.phoneNo, verificationCode)
                .then((respond: VerifyCodeRespond) => {
                    console.log("Verify phone result: " + respond.IsSuccess);
                    if (respond.IsSuccess) this.$ionicHistory.goBack(-2);
                    else {
                        this.$ionicPopup.alert({
                            title: 'รหัสยืนยันที่ระบุไม่ถูกต้อง กรุณาลองใหม่อีกครั้ง',
                            okType: 'button-royal',
                            okText: 'ระบุใหม่'
                        });
                    }
                })
                .catch(err=> { 
                    // TODO: Inform error to user
                });
        }

    }

    class SettingController {
        static $inject = ['$scope', 'userprofile'];
        constructor(private $scope: ng.IScope, private userprofile) {
        }
    }

    angular
        .module('app.account')
        .controller('app.account.VerifySecretCodeController', VerifySecretCodeController)
        .controller('app.account.SettingController', SettingController);
}