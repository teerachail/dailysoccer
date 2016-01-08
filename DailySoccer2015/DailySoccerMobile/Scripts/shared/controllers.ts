module app.shared {
    'use strict';

    class SideMenuController {

        public CurrentDay: number;

        static $inject = ['couponSummary', '$scope', '$ionicModal'];
        constructor(public couponSummary, private $scope, private $ionicModal) {
            var now = new Date();
            this.CurrentDay = now.getDate();
           
            this.$ionicModal.fromTemplateUrl('templates/Login.html',
            {
                scope: $scope,
                animation: 'slide-in-up'
            }).then(function (modal): void { $scope.Login = modal; });

            this.$ionicModal.fromTemplateUrl('templates/Favorite.html',
            {
                scope: $scope,
                animation: 'slide-in-up'
            }).then(function (modal): void { $scope.Favorite = modal; });
        }

        public ShowPopUp(): void {
            this.$scope.Favorite.show();
        }
        public ClosePopUp(): void {
            this.$scope.Favorite.hide();
        }
        public LoginShow(): void {
            this.$scope.Login.show();
        }
        public LoginHide(): void {
            this.$scope.Login.hide();
        }
    }

    class FavoriteController {
        public Pages: number;
        public team: any;
        public league: any;

        static $inject = ['$scope', '$ionicModal', 'app.shared.FavoriteTeamService'];
        constructor(private $scope, private $ionicModal, public svc: app.shared.FavoriteTeamService) {                 
            this.Pages = 0;
            svc.GetLeagues().then((respond): any => {
                this.league = respond;
                this.getTeamsByLeague(this.Pages);
            });

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