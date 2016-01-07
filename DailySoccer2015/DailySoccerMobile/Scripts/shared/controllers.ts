module app.shared {
    'use strict';

    class SideMenuController {

        public Pages: number;
        public CurrentDay: number;
        public team: any;

        static $inject = ['couponSummary', 'app.shared.FavoriteTeamService', 'league', '$scope', '$ionicModal'];
        constructor(public couponSummary, public teamSvc: app.shared.FavoriteTeamService, public league, private $scope, private $ionicModal) {
            var now = new Date();
            this.CurrentDay = now.getDate();
            this.$ionicModal.fromTemplateUrl('templates/Favorite.html',
                {
                    scope: $scope,
                    animation: 'slide-in-up'
                }).then(function (modal): void { $scope.Favorite = modal; });

            this.Pages = 0;

            this.$ionicModal.fromTemplateUrl('templates/Login.html',
            {
                scope: $scope,
                animation: 'slide-in-up'
            }).then(function (modal): void { $scope.Login = modal; });

            this.getTeamsByLeague(this.Pages);
        }

        private getTeamsByLeague(page: number) {
            this.team = this.teamSvc.GetTeams(this.league[this.Pages].id);
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

    angular
        .module('app.shared')
        .controller('app.shared.SideMenuController', SideMenuController);

}