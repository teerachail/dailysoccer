module app.shared {
    'use strict';

    export class VerifyPhoneRequest {

        constructor(
            public id: string,
            public PhoneNumber: string,
            public VerificationCode: string = null) {
        }

    }

    export class VerifyCodeResponse {

        public IsSuccess: boolean;

    }

    interface IVerPhoneResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        updatePhoneNo(data: T): T;
        veriCode(data: T): T;
    }

    export class TestPhoneService {

        public svc: IVerPhoneResourceClass<any>;

        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, $resource: ng.resource.IResourceService) {

            this.svc = <IVerPhoneResourceClass<any>>$resource(appConfig.VerifyPhoneUrl, { 'id': '@id' }, {
                updatePhoneNo: { method: 'PUT', params: { 'action': 'phoneno' } },
                veriCode: { method: 'PUT', params: { 'action': 'vericode' } }
            });

        }

        public SetPhoneNo(id: string, phoneNo: string): void {
            this.svc.updatePhoneNo(new VerifyPhoneRequest(id, phoneNo));
        }

        public VerifyCode(id: string, phoneNo: string, vericode: string): ng.IPromise<VerifyCodeResponse> {
            return this.svc.veriCode(new VerifyPhoneRequest(id, phoneNo, vericode)).$promise;
        }
    }

	class SimpleDataController {

        private user: UserProfile;

        static $inject = ['data', '$scope', 'app.shared.TestPhoneService', 'app.shared.UserProfileService'];
        constructor(public data, private $scope: ng.IScope, public TestPhoneService: TestPhoneService, profSvc: app.shared.UserProfileService) {
            this.user = profSvc.GetUserProfile();
        }

        public VerPhone() {
            // update the new phone to the shared state service?
            // ...
            // send sms
            this.TestPhoneService.SetPhoneNo(this.user.UserId, '0875698745');
            // $state.go to the verification step
        }

        public VerCode() {
            this.TestPhoneService.VerifyCode(this.user.UserId, '0875698745', '3214').then((rsp: VerifyCodeResponse) => {
                console.log(JSON.stringify(rsp));
                if (rsp.IsSuccess) {
                    // get new user profile (the updated one) from the server.
                } else {
                    // inform user that the code is invalid?
                }
            }).catch(err => {
                // need to inform error to user?
            });
        }

	}

    angular
        .module('app.shared')
        .service('app.shared.TestPhoneService', TestPhoneService)
		.controller('app.shared.SimpleDataController', SimpleDataController);
}