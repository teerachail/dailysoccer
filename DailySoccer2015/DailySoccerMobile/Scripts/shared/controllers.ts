module app.shared {
    'use strict';

    class SideMenuController {

        public CurrentDay: number;
        public Favorite: any;
        public Login: any;
        public FacebookPopup: any;
        public TiePopup: any;
        public Pages: number;
        public team: any;
        public league: any;
        public SelectedTeam: string;
        public localPoint: number;
        public facebookPoint: number;

        static $inject = [
            '$scope',
            '$rootScope',
            '$state',
            '$location',
            '$timeout',
            '$ionicModal',
            'app.shared.FavoriteTeamService',
            'app.account.UserProfileService',
            'app.shared.UserProfileService'];
        constructor(
            private $scope,
            private $rootScope,
            private $state,
            private $location,
            private $timeout,
            private $ionicModal,
            public svc: app.shared.FavoriteTeamService,
            public userSvc: app.account.UserProfileService,
            public userInfo: app.shared.UserProfileService) {          

            this.Pages = 0;
            var now = new Date();
            this.CurrentDay = now.getDate();

            $rootScope.refresher = () => { alert('site menu'); };

            this.$ionicModal.fromTemplateUrl('templates/Login.html',
            {
                scope: $scope,
                animation: 'slide-in-up',
                backdropClickToClose: false,
                hardwareBackButtonClose: false
                }).then((modal) => {this.Login = modal;});

            this.$ionicModal.fromTemplateUrl('templates/Favorite.html',
            {
                scope: $scope,
                animation: 'slide-in-up',
                backdropClickToClose: false,
                hardwareBackButtonClose: false
                }).then((modal) => { this.Favorite = modal; });

            this.$ionicModal.fromTemplateUrl('templates/Facebook.html',
                {
                    scope: $scope,
                    animation: 'slide-in-up'
                }).then((modal) => { this.FacebookPopup = modal; });

            this.$ionicModal.fromTemplateUrl('templates/Tie.html',
                {
                    scope: $scope,
                    animation: 'slide-in-up'
                }).then(modal=> { this.TiePopup = modal; });

            this.svc.GetLeagues().then((respond): any => {
                this.league = respond;
                this.getTeamsByLeague(this.Pages);
            });

            this.checkLoginStatus();
        }

        public ShowPopUp(): void {
            this.Favorite.show();
        }
        public ConfirmFavoriteTeam(): void {
            var userId = this.userInfo.GetUserProfile().UserId;
            this.userSvc.SetFavoriteTeam(userId, this.SelectedTeam);
            this.$rootScope.refresher();
            this.Favorite.hide();
        }
        public LoginShow(): void {
            this.Login.show();
        }

        public LoginWithFacebook(): void {
            this.FacebookPopup.show();
        }

        public LoggedInWithGuest(): void {
            this.userSvc.CreateNewGuest().then((respond) => {
                this.userInfo.LoggedInWithGuest(respond.id);
                this.Login.hide();
                this.Favorite.show();
            });
        }

        ////#Login#
        public ShowTiePopup() {
            this.FacebookPopup.hide();
            this.TiePopup.show();
        }

        public IsShowNewGuestButton() {
            return this.userInfo.GetUserProfile().UserId == null ;
        }

        public Logout() {
            this.userInfo.Logout();
            this.Login.show();
        }

        public IsLogedIn(): boolean {
            return this.userInfo.IsLogedIn();
        }

        public IsLogedInFacebook(): boolean {
            return this.userInfo.IsLoggedFacebook();
        }

        public checkLoginStatus(): void {
            if (!this.userInfo.IsLogedIn()) {
                this.$timeout(() => {
                    this.Login.show();
                }, 1000);                
            }
        }

        public LoginWithFacebookAccount(): void {
            var facebookId = this.userInfo.FacebookId;
            this.userSvc.TieFacebook(facebookId, '', false).then((respond) => {
                this.userInfo.LoggedInWithFacebook(respond.id);
                this.Login.hide();
                if (respond.FavouriteTeamId == null) {
                    this.Favorite.show();
                }
            });
        }

        public TieFacebook(): void {
            var facebookId = this.userInfo.FacebookId;
            this.userSvc.TieFacebook(facebookId, '', false).then((respond) => {
                this.facebookPoint = respond.Points;
                this.localPoint = this.userInfo.CurrentPoints;
            });
        }

        public TieWithFacebookData(): void {
            var facebookId = this.userInfo.FacebookId;
            this.userSvc.TieFacebook(facebookId, '', false).then((respond) => {
                this.userInfo.LoggedInWithFacebook(respond.id);
                this.TiePopup.hide();
            });
            
        }

        public TieWithLocalData(): void {
            var facebookId = this.userInfo.FacebookId;
            var userId = this.userInfo.GetUserProfile().UserId;
            this.userSvc.TieFacebook(facebookId, userId, false).then((respond) => {
                this.userInfo.LoggedInWithFacebook(respond.id);
                this.TiePopup.hide();
            });
        }
        ////#End Login#

        ////#FavoriteTeam#
        public IsSelectedTeam(teamId: string) {
            return this.SelectedTeam == teamId;
        }

        public SelectTeam(teamId: string) {
            this.SelectedTeam = teamId;
        }

        private getTeamsByLeague(page: number) {
            this.svc.GetTeams(this.league[this.Pages].id).then((respond): any => {
                this.team = respond;
            });
        }

        public NextPages(): void {
            if (this.Pages < this.league.length - 1)
                this.Pages++;
            else
                this.Pages = 0;

            this.getTeamsByLeague(this.Pages);
        }

        public PreviousPages(): void {
            if (this.Pages > 0)
                this.Pages--;
            else
                this.Pages = this.league.length - 1;
            this.getTeamsByLeague(this.Pages);
        }
        ////#End FavoriteTeam#
    }

    class LoginController {
        static $inject = ['app.shared.UserProfileService'];
        constructor(public svc: app.shared.UserProfileService) {
        }
    }

    angular
        .module('app.shared')
        .controller('app.shared.SideMenuController', SideMenuController)
        .controller('app.shared.LoginController', LoginController);

}