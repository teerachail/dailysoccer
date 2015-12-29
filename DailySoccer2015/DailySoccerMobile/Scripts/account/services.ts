module app.account {
	'use strict';

    export interface IPhoneVerificationService {
        UpdatePhoneNo(userId: string, phoneNo: string): void;
        SendVerificationCode(userId: string, phoneNo: string, verificationCode: string): ng.IPromise<VerificationCodeRespond>;
    }

    export class PhoneVerificationService implements IPhoneVerificationService {

        private updatePhoneSvc: ng.resource.IResourceClass<any>;
        private sendVerifierSvc: ng.resource.IResourceClass<any>;

        static $inject = ['$resource'];
        constructor(private $resource: angular.resource.IResourceService) {
            var saveAction: ng.resource.IActionDescriptor = { method: 'PUT' };
            this.updatePhoneSvc = $resource('http://localhost:2394/api/profiles/:id/phoneno', { "id": "@id" }, { save: saveAction });
            this.sendVerifierSvc = $resource<any>('http://localhost:2394/api/profiles/:id/vericode', { "id": "@id" }, { save: saveAction });
        }

        public UpdatePhoneNo(userId: string, phoneNo: string): void {
            var body = new VerificationPhonenoRequest();
            body.PhoneNumber = phoneNo;
            this.updatePhoneSvc.save({ id: userId }, body);
        }

        public SendVerificationCode(userId: string, phoneNo: string, verificationCode: string): ng.IPromise<VerificationCodeRespond> {
            var body = new VerificationCodeRequest();
            body.PhoneNumber = phoneNo;
            body.VerificationCode = verificationCode;
            return (<ng.resource.IResource<VerificationCodeRespond>>this.sendVerifierSvc.save({ id: userId }, body)).$promise;
        }

    }

	angular
		.module('app.account')
        .service('app.account.PhoneVerificationService', PhoneVerificationService);
}