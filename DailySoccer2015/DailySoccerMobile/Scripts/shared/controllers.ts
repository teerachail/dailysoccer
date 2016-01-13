module app.shared {
    'use strict';

    class SideMenuController {

        public Favorite: any;
        public Login: any;
        public Pages: number;
        public team: any;
        public league: any;
        public SelectedTeam: string;

        static $inject = ['couponSummary', '$scope', '$ionicModal', 'app.shared.FavoriteTeamService', 'app.account.UserProfileService', 'app.shared.UserProfileService'];
        constructor(public couponSummary, private $scope, private $ionicModal, public svc: app.shared.FavoriteTeamService, public userSvc: app.account.UserProfileService, public userInfo: app.shared.UserProfileService) {          
            this.$ionicModal.fromTemplateUrl('templates/Login.html',
            {
                scope: $scope,
                animation: 'slide-in-up',
                backdropClickToClose: false
                }).then((modal) => {  this.Login = modal; });

            this.$ionicModal.fromTemplateUrl('templates/Favorite.html',
            {
                scope: $scope,
                animation: 'slide-in-up',
                backdropClickToClose: false
                }).then((modal) => { this.Favorite = modal; });

            this.Pages = 0;
            this.svc.GetLeagues().then((respond): any => {
                this.league = respond;
                this.getTeamsByLeague(this.Pages);
            });
        }

        public ShowPopUp(): void {
            this.Favorite.show();
        }
        public ConfirmFavoriteTeam(): void {
            var userId = this.userInfo.GetUserProfile().UserId;
            this.userSvc.SetFavoriteTeam(userId, this.SelectedTeam);
            this.Favorite.hide();
        }
        public LoginShow(): void {
            this.Login.show();
        }
        public LoginHide(): void {
            this.Login.hide();
        }

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

    class FavoriteController {
        public Pages: number;
        public team: any;
        public league: any;
        public SelectedTeam: string;

        static $inject = ['$scope', '$ionicModal', 'app.shared.FavoriteTeamService'];
        constructor(private $scope, private $ionicModal, public svc: app.shared.FavoriteTeamService) {                 
            this.Pages = 0;
            svc.GetLeagues().then((respond): any => {
                this.league = respond;
                this.getTeamsByLeague(this.Pages);
            });

        }

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
    }

    class LoginController {
        static $inject = ['app.shared.UserProfileService'];
        constructor(public svc: app.shared.UserProfileService) {
        }
    }

    angular
        .module('app.shared')
        .controller('app.shared.SideMenuController', SideMenuController)
        .controller('app.shared.FavoriteController', FavoriteController)
        .controller('app.shared.LoginController', LoginController);

}