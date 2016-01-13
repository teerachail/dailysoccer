module app.account {
    'use strict';

    interface IVerifyPhoneResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        UpdatePhoneNo(data: T): T;
        VerifyCode(data: T): T;
    }
    interface IProfilesResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        CreateNewGuest(): T;
    }
    interface IUserFavoriteTeamResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        SetFavoriteTeam(data: T): T;
    }
    export class PhoneVerificationService {

        private svc: IVerifyPhoneResourceClass<any>;
        private profileSvc: IProfilesResourceClass<any>;

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

    export class UserProfileService {
        private profileSvc: IProfilesResourceClass<any>;
        private favoriteSvc: IUserFavoriteTeamResourceClass<any>;

        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService) {
            this.profileSvc = <IProfilesResourceClass<any>>$resource(appConfig.ProfileUrl, {}, {
                CreateNewGuest: { method: 'POST' }
            });
            this.favoriteSvc = <IUserFavoriteTeamResourceClass<any>>$resource(appConfig.ProfileUrl + '/:id/favteam', { 'id': '@id'}, {
                SetFavoriteTeam: { method: 'POST'},
            });
        }

        public CreateNewGuest(): ng.IPromise<any> {
            return this.profileSvc.CreateNewGuest().$promise;
        }
        public SetFavoriteTeam(id: string, teamId: string): void {
            this.favoriteSvc.SetFavoriteTeam(new app.account.SetFavoriteTeamRequest(id, teamId));
        }
    }

    angular
        .module('app.account')
        .service('app.account.PhoneVerificationService', PhoneVerificationService)
        .service('app.account.UserProfileService', UserProfileService);
}