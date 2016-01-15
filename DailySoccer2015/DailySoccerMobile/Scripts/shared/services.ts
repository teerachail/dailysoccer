module app.shared {
    'use strict';

    interface IUserProfileService {
        GetUserProfile(): UserProfile;
        Logout(): void;
        LoggedInWithGuest(userId: string): void;
        LoggedInWithFacebook(userId: string, verifiedPhoneNo: string): void;
        IsLogedIn(): boolean;
        IsLoggedFacebook(): boolean;
    }

    interface ITeamResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetTeams(data: T): T;
    }

    interface ILeagueResourceClass<T> extends ng.resource.IResourceClass<ng.resource.IResource<T>> {
        GetLeagues(): T;
    }

    export class UserProfileService implements IUserProfileService {

        public CurrentPoints: number;
        private userprofile: UserProfile;

        constructor() {           
            this.userprofile = new UserProfile();
            //this.userprofile.UserId = "u01guest";
            //this.userprofile.IsLoggedFacebook = true;
            //this.userprofile.IsLoggedIn = true;

            // HACK: Initial UserProfile (Load data from ionic user)
            var user = Ionic.User.current();
            this.userprofile.UserId = user.id;
            this.userprofile.IsLoggedIn = user.get('IsLoggedIn', false);
            this.userprofile.IsLoggedFacebook = user.get('IsLoggedFacebook', null);
        }

        public IsLogedIn(): boolean {
            return this.userprofile.IsLoggedIn;
        }

        public IsLoggedFacebook(): boolean {
            return this.userprofile.IsLoggedFacebook;
        }

        public GetUserProfile(): UserProfile {
            return this.userprofile;
        }
        public Logout(): void {
            this.userprofile.UserId = null;
            this.userprofile.IsLoggedIn = false;
            this.userprofile.IsLoggedFacebook = false;
            this.updateUserProfile();
        }
        public LoggedInWithGuest(userId: string): void {
            this.userprofile.UserId = userId;
            this.userprofile.IsLoggedIn = true;
            this.userprofile.IsLoggedFacebook = false;
            this.updateUserProfile();
        }
        public LoggedInWithFacebook(userId: string, verifiedPhoneNo: string): void {
            this.userprofile.UserId = userId;
            this.userprofile.IsLoggedIn = true;
            this.userprofile.IsLoggedFacebook = true;
            this.updateUserProfile();
        }

        private updateUserProfile(): void {
            var user = Ionic.User.current();
            user.id = this.userprofile.UserId;
            user.set('IsLoggedIn', this.userprofile.IsLoggedIn);
            user.set('IsLoggedFacebook', this.userprofile.IsLoggedFacebook);
            user.save();
        }
    }

    export class FavoriteTeamService {
        private teamSvc: ITeamResourceClass<any>;
        private leagueSvc: ILeagueResourceClass<any>;


        static $inject = ['appConfig', '$resource'];
        constructor(appConfig: IAppConfig, private $resource: angular.resource.IResourceService) {
            this.teamSvc = <ITeamResourceClass<any>>$resource(appConfig.TeamUrl + '/:id', { "id": "@id"}, {
                GetTeams: { method: 'GET', isArray: true},
            });
            this.leagueSvc = <ILeagueResourceClass<any>>$resource(appConfig.LeagueUrl, {}, {
                GetLeagues: { method: 'GET', isArray: true },
            });
        }

        public GetTeams(id: string): ng.IPromise<any> {
            return this.teamSvc.GetTeams(new TeamRequest(id)).$promise;
        }

        public GetLeagues(): ng.IPromise<any> {
            return this.leagueSvc.GetLeagues().$promise;
        }
    }

    angular
        .module('app.shared')
        .service('app.shared.UserProfileService', UserProfileService)
        .service('app.shared.FavoriteTeamService', FavoriteTeamService);
}