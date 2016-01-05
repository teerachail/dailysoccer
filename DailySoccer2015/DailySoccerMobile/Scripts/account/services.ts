module app.account {
    'use strict';

    interface IVerifyPhoneResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        UpdatePhoneNo(data: T): T;
        VerifyCode(data: T): T;
    }

    export class PhoneVerificationService {

        private svc: IVerifyPhoneResourceClass<any>;

        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService) {
            this.svc = <IVerifyPhoneResourceClass<any>>$resource(appConfig.VerifyPhoneUrl, { 'id': '@id' }, {
                UpdatePhoneNo: { method: 'PUT', params: { 'action': 'phoneno' } },
                VerifyCode: { method: 'PUT', params: { 'action': 'vericode' } }
            });
        }

        public UpdatePhoneNo(userId: string, phoneNo: string): void {
            this.svc.UpdatePhoneNo(new VerifyPhoneRequest(userId, phoneNo));
        }

        public SendVerificationCode(userId: string, phoneNo: string, verificationCode: string): ng.IPromise<VerifyCodeRespond> {
            return this.svc.VerifyCode(new VerifyPhoneRequest(userId, phoneNo, verificationCode)).$promise;
        }

    }

    angular
        .module('app.account')
        .service('app.account.PhoneVerificationService', PhoneVerificationService);
}